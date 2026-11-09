using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace ChestCounter;

/// <summary>
/// Ajustes con el mismo aspecto que la ventana principal: dos columnas de tarjetas, títulos pequeños en gris y
/// cifras grandes de color. Cada cambio se aplica y se guarda al instante.
/// </summary>
[SupportedOSPlatform("windows")]
public sealed class SettingsForm : Form
{
    private static readonly string[] UpdateLabels = { L.T("Preguntar", "Ask"), L.T("Automáticas", "Automatic"), L.T("Nunca", "Never") };
    private static readonly string[] UpdateModes = { "prompt", "auto", "off" };

    // Misma rejilla que la ventana principal: columna izquierda y derecha.
    private const int LeftX = 15, LeftW = 267, RightX = 297, RightW = 280;
    private const int MaxInitial = 1_000_000;

    private readonly Monitor _monitor;
    private readonly ToggleSwitch _fortnite;
    private readonly ToggleSwitch _report;
    private readonly PillButton[] _updates = new PillButton[UpdateModes.Length];
    private readonly TextBox _folder;
    private readonly TextBox _initial;
    private bool _loading;

    public SettingsForm(Monitor monitor, Store store)
    {
        _monitor = monitor;
        var config = monitor.Config;

        Text = L.T("Ajustes", "Settings");
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
        ClientSize = new Size(592, 530);
        Icon = AppIcon.Load();

        // Cabecera, como en la ventana principal.
        Lbl(this, L.T("AJUSTES", "SETTINGS"), 8.5f, FontStyle.Bold, Theme.Cyan, new Point(LeftX, 14), new Size(200, 18));
        Lbl(this, L.T($"Versión {AppInfo.VersionText}", $"Version {AppInfo.VersionText}"), 8.0f, FontStyle.Regular, Theme.Muted,
            new Point(RightX, 13), new Size(RightW, 18), ContentAlignment.TopRight);

        // ---- Columna izquierda ---------------------------------------------------------------

        // Inicio: dos interruptores.
        var start = AddCard(LeftX, 40, LeftW, 100, 16);
        CardTitle(start, L.T("Inicio", "Startup"));
        _fortnite = AddToggleRow(start, L.T("Abrir con Fortnite", "Open with Fortnite"),
            L.T("Cuenta tus partidas sin pensar en ello", "Counts your matches without thinking about it"), 24, config.StartWithFortnite);
        _fortnite.CheckedChanged += (_, _) =>
        {
            if (_loading)
            {
                return;
            }
            if (!_monitor.SetStartWithFortnite(_fortnite.Checked))
            {
                _loading = true;
                _fortnite.Checked = !_fortnite.Checked;
                _loading = false;
                MessageBox.Show(this,
                    L.T("No se puede desde aquí: abre el contador con ChestCounter.exe.", "Not possible from here: open the counter with ChestCounter.exe."),
                    L.T("Ajustes", "Settings"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };
        _report = AddToggleRow(start, L.T("Abrir el informe al iniciar", "Open the report at startup"), null, 64, config.OpenReportOnStart);
        _report.CheckedChanged += (_, _) => { if (!_loading) _monitor.Change(c => c.OpenReportOnStart = _report.Checked); };

        // Actualizaciones: tres píldoras.
        var updates = AddCard(LeftX, 149, LeftW, 74, 14);
        CardTitle(updates, L.T("Actualizaciones", "Updates"));
        var pillW = (LeftW - 26 - 2 * 6) / 3;
        for (var i = 0; i < UpdateModes.Length; i++)
        {
            var index = i;
            var pill = new PillButton { Text = UpdateLabels[i], Location = new Point(13 + i * (pillW + 6), 30), Size = new Size(pillW, 32), Radius = 10, Font = Theme.Text(8.5f, FontStyle.Bold) };
            pill.Click += (_, _) => SelectUpdateMode(index, save: true);
            updates.Controls.Add(pill);
            _updates[i] = pill;
        }
        var mode = Array.IndexOf(UpdateModes, config.UpdateMode);
        SelectUpdateMode(mode < 0 ? 0 : mode, save: false);

        // Carpeta de replays y datos.
        var folderCard = AddCard(LeftX, 232, LeftW, 94, 14);
        CardTitle(folderCard, L.T("Carpeta de replays", "Replays folder"));
        _folder = AddField(folderCard, new Point(13, 26), LeftW - 26 - 72, config.DemosFolder);
        var browse = new PillButton { Text = L.T("Cambiar", "Change"), Location = new Point(LeftW - 13 - 66, 26), Size = new Size(66, 30), Radius = 10, Font = Theme.Text(8.5f, FontStyle.Bold) };
        browse.Click += (_, _) => ChooseFolder();
        folderCard.Controls.Add(browse);
        Lbl(folderCard, L.T("Tus datos se guardan en ", "Your data is saved in ") + store.Folder,
            7.5f, FontStyle.Regular, Theme.Muted, new Point(13, 62), new Size(LeftW - 26, 28));

        // ---- Columna derecha -----------------------------------------------------------------

        // Cofres de antes del contador: cifra grande en ámbar, editable, con − / +.
        var initial = AddCard(RightX, 40, RightW, 100, 16);
        CardTitle(initial, L.T("Cofres antes del contador", "Chests before the counter"));
        _initial = new TextBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Theme.Card,
            ForeColor = Theme.Amber,
            Font = Theme.Text(24f, FontStyle.Bold),
            Location = new Point(15, 26),
            Width = 160,
            MaxLength = 7,
            Text = Math.Min(MaxInitial, monitor.InitialChests).ToString(L.Culture),
        };
        initial.Controls.Add(_initial);
        var plus = new PillButton { Text = "+", Location = new Point(RightW - 13 - 34, 22), Size = new Size(34, 28), Radius = 9, Font = Theme.Text(11f, FontStyle.Bold) };
        var minus = new PillButton { Text = "−", Location = new Point(RightW - 13 - 34, 54), Size = new Size(34, 28), Radius = 9, Font = Theme.Text(11f, FontStyle.Bold) };
        plus.Click += (_, _) => StepInitial(10);
        minus.Click += (_, _) => StepInitial(-10);
        initial.Controls.Add(plus);
        initial.Controls.Add(minus);
        _initial.KeyPress += (_, e) => e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        _initial.TextChanged += (_, _) =>
        {
            if (!_loading)
            {
                _monitor.SetInitialChests(ParseInitial());
            }
        };
        var profileName = monitor.ShownProfileName;
        Lbl(initial, profileName != null
                ? L.T($"estimado · se suma al total de {profileName}", $"estimate · added to {profileName}'s total")
                : L.T("estimado · se suma a tu total", "estimate · added to your total"),
            8.0f, FontStyle.Regular, Theme.Muted, new Point(16, 74), new Size(RightW - 70, 16));

        // OBS: dirección y estado.
        var obsCard = AddCard(RightX, 149, RightW, 74, 14);
        CardTitle(obsCard, L.T("Overlay para OBS", "Overlay for OBS"));
        var obsUrl = ObsServer.Url;
        var stateCard = new Card { Location = new Point(RightW - 13 - 70, 6), Size = new Size(70, 18), Radius = 9, BackColor = Theme.Panel };
        Lbl(stateCard, obsUrl != null ? L.T("● ACTIVO", "● ON") : L.T("● NO", "● OFF"), 7.0f, FontStyle.Bold,
            obsUrl != null ? Theme.Mint : Theme.Red, new Point(0, 0), new Size(70, 18), ContentAlignment.MiddleCenter);
        obsCard.Controls.Add(stateCard);
        var obs = AddField(obsCard, new Point(13, 30), RightW - 26 - 72, obsUrl ?? L.T("Puerto ocupado", "Port in use"));
        var copy = new PillButton { Text = L.T("Copiar", "Copy"), Location = new Point(RightW - 13 - 66, 30), Size = new Size(66, 30), Radius = 10, Font = Theme.Text(8.5f, FontStyle.Bold), Enabled = obsUrl != null };
        copy.Click += (_, _) =>
        {
            try
            {
                Clipboard.SetText(obsUrl!);
                copy.Text = "✓";
            }
            catch
            {
                obs.Focus();
                obs.SelectAll();
            }
        };
        obsCard.Controls.Add(copy);

        // Atajos del overlay.
        var keys = AddCard(RightX, 232, RightW, 45, 12);
        Lbl(keys, L.T("Atajos del overlay", "Overlay shortcuts"), 7.5f, FontStyle.Regular, Theme.Muted, new Point(13, 6), new Size(RightW - 26, 13));
        Lbl(keys, L.T("Ctrl+Mayús+O mostrar  ·  Ctrl+Mayús+L fijar", "Ctrl+Shift+O show  ·  Ctrl+Shift+L lock"),
            8.0f, FontStyle.Bold, Theme.Ink, new Point(13, 21), new Size(RightW - 26, 18));

        var team = AddCard(LeftX, 335, LeftW, 74, 14);
        CardTitle(team, L.T("Equipo", "Team"));
        var teamToggle = AddToggleRow(team, L.T("Equipo en overlay", "Team in overlay"), null, 30, config.OverlayTeam);
        teamToggle.CheckedChanged += (_, _) => _monitor.Change(c => c.OverlayTeam = teamToggle.Checked);

        var archive = AddCard(RightX, 335, RightW, 180, 14);
        CardTitle(archive, L.T("Archivo de replays", "Replay archive"));
        var archiveToggle = AddToggleRow(archive, L.T("Archivar replays", "Archive replays"), null, 27, config.ArchiveReplays);
        archiveToggle.CheckedChanged += (_, _) => _monitor.Change(c => c.ArchiveReplays = archiveToggle.Checked);
        var archivePath = AddField(archive, new Point(13, 65), RightW - 100, config.ArchiveFolder);
        var chooseArchive = new PillButton { Text = L.T("Cambiar", "Change"), Location = new Point(RightW - 80, 65), Size = new Size(67, 30), Radius = 10 };
        chooseArchive.Click += (_, _) =>
        {
            using var dialog = new FolderBrowserDialog { SelectedPath = config.ArchiveFolder };
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var path = Path.GetFullPath(dialog.SelectedPath);
                var demos = Path.GetFullPath(config.DemosFolder).TrimEnd(Path.DirectorySeparatorChar);
                if (path.Equals(demos, StringComparison.OrdinalIgnoreCase) || path.StartsWith(demos + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) return;
                _monitor.Change(c => c.ArchiveFolder = path);
                archivePath.Text = path;
            }
        };
        archive.Controls.Add(chooseArchive);
        Lbl(archive, L.T("Límite (GB)", "Limit (GB)"), 8, FontStyle.Regular, Theme.Muted, new Point(13, 108), new Size(95, 20));
        var cap = new NumericUpDown { Minimum = 1, Maximum = 10000, Value = Math.Clamp(config.ArchiveLimitGb, 1, 10000), Location = new Point(125, 105), Width = 90 };
        cap.ValueChanged += (_, _) => _monitor.Change(c => c.ArchiveLimitGb = (int)cap.Value);
        archive.Controls.Add(cap);
        long archiveBytes = 0;
        try { archiveBytes = ReplayArchive.Size(config.ArchiveFolder); } catch (IOException) { }
        Lbl(archive, L.T("Tamaño", "Size") + $": {archiveBytes / 1073741824.0:F2} GB", 8, FontStyle.Regular, Theme.Muted, new Point(13, 145), new Size(RightW - 26, 20));

        // Cerrar, en el sitio de los botones de la ventana principal.
        var close = new PillButton
        {
            Text = L.T("Cerrar", "Close"),
            Location = new Point(LeftX, 475),
            Size = new Size(LeftW, 40),
            Radius = 12,
            BackColor = Theme.Accent,
            HoverColor = Theme.AccentHover,
            ForeColor = Theme.AccentInk,
            DialogResult = DialogResult.OK,
        };
        Controls.Add(close);
        AcceptButton = close;
        CancelButton = close;
        ActiveControl = close; // sin anillo de foco en el primer interruptor al abrir
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);
        Theme.Chrome(Handle);
    }

    private void SelectUpdateMode(int index, bool save)
    {
        for (var i = 0; i < _updates.Length; i++)
        {
            var on = i == index;
            _updates[i].BackColor = on ? Theme.Accent : Theme.Panel;
            _updates[i].HoverColor = on ? Theme.AccentHover : Theme.PanelHover;
            _updates[i].ForeColor = on ? Theme.AccentInk : Theme.Ink;
            _updates[i].Invalidate();
        }
        if (save)
        {
            var chosen = UpdateModes[index];
            _monitor.Change(c => c.UpdateMode = chosen);
        }
    }

    private int ParseInitial() =>
        int.TryParse(_initial.Text, System.Globalization.NumberStyles.None, L.Culture, out var n) ? Math.Clamp(n, 0, MaxInitial) : 0;

    private void StepInitial(int delta)
    {
        var value = Math.Clamp(ParseInitial() + delta, 0, MaxInitial);
        _initial.Text = value.ToString(L.Culture); // TextChanged guarda el valor
    }

    private void ChooseFolder()
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = L.T("Carpeta de replays de Fortnite", "Fortnite replays folder"),
            UseDescriptionForTitle = true,
            SelectedPath = Directory.Exists(_folder.Text) ? _folder.Text : "",
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            _monitor.ChangeDemosFolder(dialog.SelectedPath);
            _folder.Text = dialog.SelectedPath;
        }
    }

    // ---- Piezas de la ventana ----------------------------------------------------------------

    private Card AddCard(int x, int y, int w, int h, int radius)
    {
        var card = new Card { Location = new Point(x, y), Size = new Size(w, h), Radius = radius, BackColor = Theme.Card };
        Controls.Add(card);
        return card;
    }

    /// <summary>Título pequeño en gris, como "Hoy" o "Mejor día" en la ventana principal.</summary>
    private static void CardTitle(Card card, string text) =>
        Lbl(card, text, 8.0f, FontStyle.Regular, Theme.Muted, new Point(13, 7), new Size(card.Width - 100, 15));

    /// <summary>Texto (y una línea gris opcional) a la izquierda y un interruptor a la derecha; clicar el texto también lo cambia.</summary>
    private static ToggleSwitch AddToggleRow(Card card, string text, string? hint, int top, bool value)
    {
        var toggle = new ToggleSwitch { Location = new Point(card.Width - 13 - 40, top + 5), Checked = value };
        var label = Lbl(card, text, 9.5f, FontStyle.Bold, Theme.Ink, new Point(13, top), new Size(card.Width - 26 - 48, 18));
        label.Cursor = Cursors.Hand;
        label.Click += (_, _) => toggle.Checked = !toggle.Checked;
        if (hint != null)
        {
            var small = Lbl(card, hint, 7.5f, FontStyle.Regular, Theme.Muted, new Point(13, top + 18), new Size(card.Width - 26 - 48, 14));
            small.Cursor = Cursors.Hand;
            small.Click += (_, _) => toggle.Checked = !toggle.Checked;
        }
        card.Controls.Add(toggle);
        return toggle;
    }

    /// <summary>Campo de solo lectura sin borde dentro de un fondo redondeado.</summary>
    private static TextBox AddField(Card card, Point at, int width, string text)
    {
        var holder = new Card { Location = at, Size = new Size(width, 30), Radius = 10, BackColor = Theme.Panel };
        var box = new TextBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Theme.Panel,
            ForeColor = Theme.Ink,
            Font = Theme.Text(9f),
            ReadOnly = true,
            Text = text,
            Width = width - 20,
        };
        box.Location = new Point(10, (holder.Height - box.Height) / 2);
        holder.Controls.Add(box);
        card.Controls.Add(holder);
        return box;
    }

    private static Label Lbl(Control parent, string text, float size, FontStyle style, Color color, Point at, Size sz, ContentAlignment align = ContentAlignment.TopLeft)
    {
        var label = new Label
        {
            Text = text,
            Font = Theme.Text(size, style),
            ForeColor = color,
            BackColor = parent.BackColor,
            Location = at,
            Size = sz,
            TextAlign = align,
            AutoEllipsis = true,
        };
        parent.Controls.Add(label);
        return label;
    }
}
