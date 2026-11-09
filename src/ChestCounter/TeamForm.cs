using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace ChestCounter;

/// <summary>
/// Squad leaderboard and per-match teammate chests, styled like the main window and settings.
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class TeamForm : Form
{
    private const int Pad = 15, Width_ = 640, GridTop = 44, GridHeight = 420;

    private readonly PillButton[] _tabs = new PillButton[2];
    private readonly DataGridView _squad;
    private readonly DataGridView _matches;
    private readonly Card _card;

    public TeamForm(List<MatchRecord> history)
    {
        Text = L.T("Escuadra", "Squad");
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoScaleDimensions = new SizeF(96, 96);
        Font = Theme.Text();
        BackColor = Theme.Bg;
        ForeColor = Theme.Ink;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(Width_, GridTop + GridHeight + 34);
        Icon = AppIcon.Load();

        Controls.Add(new Label
        {
            Text = L.T("ESCUADRA", "SQUAD"), Font = Theme.Text(8.5f, FontStyle.Bold), ForeColor = Theme.Cyan,
            Location = new Point(Pad, 14), Size = new Size(200, 18), BackColor = Color.Transparent,
        });

        string[] names = { L.T("Clasificación", "Leaderboard"), L.T("Partidas", "Matches") };
        for (var i = 0; i < _tabs.Length; i++)
        {
            var index = i;
            var pill = new PillButton { Text = names[i], Size = new Size(110, 28), Radius = 9, Location = new Point(Width_ - Pad - 110 * (2 - i) - 6 * (1 - i), 8) };
            pill.Click += (_, _) => Select(index);
            _tabs[i] = pill;
            Controls.Add(pill);
        }

        _card = new Card { Location = new Point(Pad, GridTop), Size = new Size(Width_ - Pad * 2, GridHeight), Radius = 14, BackColor = Theme.Card };
        Controls.Add(_card);

        _squad = Grid();
        AddColumn(_squad, "#", 34, right: true);
        AddColumn(_squad, L.T("Jugador", "Player"), 0, fill: true);
        AddColumn(_squad, L.T("Partidas", "Matches"), 72, right: true);
        AddColumn(_squad, L.T("Cofres", "Chests"), 66, right: true);
        AddColumn(_squad, L.T("Por partida", "Per match"), 82, right: true);
        AddColumn(_squad, L.T("Puntuación", "Score"), 78, right: true);
        _squad.Columns[5].ToolTipText = Squad.Help;
        _squad.Columns[5].DefaultCellStyle.Font = Theme.Text(9f, FontStyle.Bold);
        var rank = 0;
        foreach (var row in Squad.Build(history))
        {
            var few = !row.Self && row.Matches < 3;
            var i = _squad.Rows.Add(few ? "" : (++rank).ToString(L.Culture), row.Name, L.N(row.Matches), L.N(row.Total),
                row.Average.ToString("F1", L.Culture), row.Score.ToString("F1", L.Culture));
            var style = _squad.Rows[i].DefaultCellStyle;
            if (row.Self) style.ForeColor = style.SelectionForeColor = Theme.Amber;
            else if (few) style.ForeColor = style.SelectionForeColor = Theme.Muted;
        }

        _matches = Grid();
        AddColumn(_matches, L.T("Partida", "Match"), 120);
        AddColumn(_matches, L.T("Tú", "You"), 50, right: true);
        AddColumn(_matches, L.T("Equipo", "Team"), 58, right: true);
        AddColumn(_matches, L.T("Compañeros", "Teammates"), 0, fill: true);
        foreach (var match in history.OrderByDescending(m => m.MatchStartUtc ?? m.AnalyzedAtUtc))
        {
            var mates = match.Teammates == null ? L.T("Sin datos", "No data")
                : match.Teammates.Count == 0 ? "" : string.Join("   ", match.Teammates.Select(t => $"{t.Name} {(t.Observed ? t.Chests.Count.ToString(L.Culture) : "?")}"));
            var i = _matches.Rows.Add(L.DateTimeShort((match.MatchStartUtc ?? match.AnalyzedAtUtc).ToLocalTime()),
                L.N(match.ChestsOpened), L.N(match.TeamChests), mates);
            _matches.Rows[i].Cells[1].Style.ForeColor = _matches.Rows[i].Cells[1].Style.SelectionForeColor = Theme.Amber;
            if (match.Teammates == null) _matches.Rows[i].Cells[3].Style.ForeColor = _matches.Rows[i].Cells[3].Style.SelectionForeColor = Theme.Muted;
        }

        _card.Controls.Add(_squad);
        _card.Controls.Add(_matches);

        Controls.Add(new Label
        {
            Text = Squad.Caveat, Font = Theme.Text(8f), ForeColor = Theme.Muted, BackColor = Color.Transparent,
            Location = new Point(Pad, GridTop + GridHeight + 8), Size = new Size(Width_ - Pad * 2, 18),
        });

        Select(0);
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Theme.Chrome(Handle);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        _squad.ClearSelection();
        _matches.ClearSelection();
    }

    private void Select(int index)
    {
        for (var i = 0; i < _tabs.Length; i++)
        {
            var on = i == index;
            _tabs[i].BackColor = on ? Theme.Accent : Theme.Panel;
            _tabs[i].HoverColor = on ? Theme.AccentHover : Theme.PanelHover;
            _tabs[i].ForeColor = on ? Theme.AccentInk : Theme.Ink;
            _tabs[i].Invalidate();
        }
        _squad.Visible = index == 0;
        _matches.Visible = index == 1;
    }

    private DataGridView Grid()
    {
        var grid = new DataGridView
        {
            Location = new Point(8, 8), Size = new Size(_card.Width - 16, _card.Height - 16),
            ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false,
            AllowUserToResizeColumns = false, AllowUserToOrderColumns = false, RowHeadersVisible = false, MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect, BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal, GridColor = Theme.CardEdge,
            BackgroundColor = Theme.Card, EnableHeadersVisualStyles = false,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing, ColumnHeadersHeight = 30,
            ScrollBars = ScrollBars.Vertical, ShowCellToolTips = true, StandardTab = true,
        };
        grid.RowTemplate.Height = 28;
        var header = grid.ColumnHeadersDefaultCellStyle;
        header.BackColor = header.SelectionBackColor = Theme.Card;
        header.ForeColor = header.SelectionForeColor = Theme.Muted;
        header.Font = Theme.Text(8f);
        header.Padding = new Padding(4, 0, 4, 0);
        var cell = grid.DefaultCellStyle;
        cell.BackColor = Theme.Card;
        cell.ForeColor = Theme.Ink;
        cell.SelectionBackColor = Theme.Panel;
        cell.SelectionForeColor = Theme.Ink;
        cell.Font = Theme.Text(9f);
        cell.Padding = new Padding(4, 0, 4, 0);
        grid.DataBindingComplete += (_, _) => grid.ClearSelection();
        grid.HandleCreated += (_, _) => grid.ClearSelection();
        return grid;
    }

    private static void AddColumn(DataGridView grid, string title, int width, bool right = false, bool fill = false)
    {
        var col = new DataGridViewTextBoxColumn { HeaderText = title, SortMode = DataGridViewColumnSortMode.NotSortable };
        if (fill) col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        else col.Width = width;
        if (right)
        {
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        grid.Columns.Add(col);
    }
}
