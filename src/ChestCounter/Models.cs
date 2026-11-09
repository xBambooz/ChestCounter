using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChestCounter;

public sealed class ChestRecord
{
    public float TimeSeconds { get; set; }
    public string Kind { get; set; } = "";
    public string Blueprint { get; set; } = "";
    /// <summary>True when the chest was destroyed by the player instead of being opened.</summary>
    public bool Destroyed { get; set; }
    public List<string> Items { get; set; } = new();
    public List<string> TreasureItems { get; set; } = new();
}

public sealed class TreasureFind
{
    /// <summary>Name to display (re-resolved from treasures.json so renaming updates old matches).</summary>
    public string DisplayName => PrizeId != null ? TreasureNames.Friendly(PrizeId) : L.Name(Item);

    public string Item { get; set; } = "";
    public string RawItem { get; set; } = "";
    /// <summary>Prize id sent by the server with the golden chest, e.g. "DelilahPrize:dpid_item4".</summary>
    public string? PrizeId { get; set; }
    /// <summary>True when detected from the golden treasure chest itself.</summary>
    public bool FromGoldenChest { get; set; }
    /// <summary>True when detected only from the prize handler (exact time, item unknown unless sent at that moment).</summary>
    public bool FromPrizeHandler { get; set; }
    /// <summary>True when the time is exact (golden chest or prize handler), false when estimated.</summary>
    public bool TimeExact { get; set; }
    /// <summary>True when the source chest was destroyed rather than opened.</summary>
    public bool SourceDestroyed { get; set; }
    public string SourceLabel => SourceDestroyed ? $"{L.Name(SourceKind)} ({L.T("destruido", "destroyed")})" : L.Name(SourceKind);
    /// <summary>When it was obtained (replay seconds). Exact for golden chests, estimated otherwise.</summary>
    public float? ObtainedAtSeconds { get; set; }
    /// <summary>Kind of container / interaction it came from (Chest, Loot cannon...).</summary>
    public string SourceKind { get; set; } = "";
    public string SourceBlueprint { get; set; } = "";
    public bool SourceIsChest { get; set; } = true;
    /// <summary>True when the source is known for sure (golden chest), false when it is the last interaction before use.</summary>
    public bool SourceCertain { get; set; }
    /// <summary>For estimated sources: other (non-chest) interactions between the source and the first use.</summary>
    public List<string> AlsoPossible { get; set; } = new();
}

public sealed class TeammateRecord
{
    public string? AccountId { get; set; }
    public bool Observed { get; set; }
    public string Name { get; set; } = "";
    public List<ChestRecord> Chests { get; set; } = new();
}

public sealed class MatchRecord
{
    public string SessionId { get; set; } = "";
    public string ReplayFile { get; set; } = "";
    public DateTime AnalyzedAtUtc { get; set; }
    public DateTime? MatchStartUtc { get; set; }
    public int? DurationSeconds { get; set; }
    public string? Playlist { get; set; }
    public string? GameVersion { get; set; }
    public string? PlayerName { get; set; }
    public string? EpicId { get; set; }
    public bool TrackedByUsername { get; set; }
    /// <summary>Set when none of the configured names played this match (name of the replay owner).</summary>
    public string? NotTrackedOwner { get; set; }
    public bool IsReplayOwner { get; set; }
    public int? Placement { get; set; }
    public int? TotalPlayers { get; set; }
    public int? Eliminations { get; set; }
    public List<TeammateRecord>? Teammates { get; set; }
    public int TeamChests => ChestsOpened + (Teammates?.Sum(t => t.Chests.Count) ?? 0);
    public int ChestsOpened { get; set; }
    public int AmmoBoxesOpened { get; set; }
    public int OtherContainersOpened { get; set; }
    /// <summary>Non-chest interactions by kind (loot cannons, ammo boxes, food boxes...).</summary>
    public Dictionary<string, int> OtherInteractions { get; set; } = new();
    public int AnalyzerVersion { get; set; } = 1;
    public bool IsBattleRoyale { get; set; } = true;
    public List<ChestRecord> Chests { get; set; } = new();
    public List<TreasureFind> TreasureFinds { get; set; } = new();
}

public sealed class Config
{
    /// <summary>Idioma de la interfaz. La aplicación solo está en español: se guarda por compatibilidad.</summary>
    public string? Language { get; set; } = "es";

    /// <summary>Selected profile (Epic id or name of the account the player plays on); null = most recent one.</summary>
    public string? Profile { get; set; }
    public bool OverlayTeam { get; set; }
    public bool ArchiveReplays { get; set; }
    public string ArchiveFolder { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ChestCounter", "ReplayArchive");
    public int ArchiveLimitGb { get; set; } = 10;
    public string DemosFolder { get; set; } = "";

    /// <summary>Open the tool automatically when Fortnite starts (invisible standby process started with Windows).</summary>
    public bool StartWithFortnite { get; set; }

    /// <summary>"prompt" (offer new versions), "auto" (install them) or "off" (never check GitHub).</summary>
    public string UpdateMode { get; set; } = "prompt";
    public bool OpenReportOnStart { get; set; } = true;
    public int? OverlayX { get; set; }
    public int? OverlayY { get; set; }
    /// <summary>Locked overlay = click-through, cannot be moved (Ctrl+Shift+L to toggle).</summary>
    public bool OverlayLocked { get; set; }

    /// <summary>Overlay shown on screen. Off by default: the player turns it on with the Overlay button or Ctrl+Shift+O.</summary>
    public bool OverlayVisible { get; set; }

    /// <summary>Count the chests of the match in progress (re-reads the replay being recorded).</summary>
    public bool LiveUpdate { get; set; } = true;

    /// <summary>Minimum delay between two readings of the replay in progress (seconds, 10 minimum).</summary>
    public int LiveIntervalSeconds { get; set; } = 30;

    /// <summary>Port of the local OBS page (http://localhost:port/), served while the tool is open.</summary>
    public int ObsPort { get; set; } = ObsServer.DefaultPort;

    /// <summary>
    /// Estimate of the chests opened before using the tool, per profile key ("" = no profile yet).
    /// Added to the total only (not to averages, records or per-day figures).
    /// </summary>
    public Dictionary<string, int> InitialChests { get; set; } = new();
}

public sealed class TrackerData
{
    /// <summary>
    /// Converts the internal names written by versions before 2.0.9 (in French) to the current ones.
    /// Returns true when something changed (the history must be saved).
    /// </summary>
    public bool UpgradeNames()
    {
        var changed = false;
        string Up(string value)
        {
            var current = L.Upgrade(value);
            changed |= !ReferenceEquals(current, value) && current != value;
            return current;
        }
        void UpList(List<string> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = Up(list[i]);
            }
        }

        foreach (var m in Matches)
        {
            if (m.NotTrackedOwner != null)
            {
                m.NotTrackedOwner = Up(m.NotTrackedOwner);
            }
            foreach (var c in m.Chests)
            {
                c.Kind = Up(c.Kind);
                UpList(c.Items);
                UpList(c.TreasureItems);
            }
            foreach (var f in m.TreasureFinds)
            {
                f.Item = Up(f.Item);
                f.SourceKind = Up(f.SourceKind);
                UpList(f.AlsoPossible);
            }
            if (m.OtherInteractions.Keys.Any(k => L.Upgrade(k) != k))
            {
                var merged = new Dictionary<string, int>();
                foreach (var kv in m.OtherInteractions)
                {
                    var key = Up(kv.Key);
                    merged[key] = merged.GetValueOrDefault(key) + kv.Value;
                }
                m.OtherInteractions = merged;
            }
        }
        foreach (var key in ProcessedFiles.Where(kv => kv.Value == "autre mode").Select(kv => kv.Key).ToList())
        {
            ProcessedFiles[key] = "other mode";
            changed = true;
        }
        return changed;
    }

    public int Version { get; set; } = 1;
    public int AnalyzerVersion { get; set; } = 1;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public List<MatchRecord> Matches { get; set; } = new();

    /// <summary>Key = file name|size|last write ticks. Value = session id, "skipped" or "error: ...".</summary>
    public Dictionary<string, string> ProcessedFiles { get; set; } = new();
}

/// <summary>Reads/writes JSON files atomically in the data folder.</summary>
public sealed class Store
{
    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public string Folder { get; }

    /// <summary>
    /// %APPDATA%\ChestCounter. The tool was called ChestTracker before version 1.1: its folder is moved
    /// (history, settings and treasure names are kept).
    /// </summary>
    public static string DefaultFolder()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var folder = Path.Combine(appData, "ChestCounter");
        var old = Path.Combine(appData, "ChestTracker");
        try
        {
            if (!Directory.Exists(folder) && File.Exists(Path.Combine(old, "data.json")))
            {
                Directory.Move(old, folder);
            }
        }
        catch
        {
            // the old folder is in use (old version still running): start fresh, nothing is lost
        }
        return folder;
    }

    public string ConfigPath => Path.Combine(Folder, "config.json");

    public string DataPath => Path.Combine(Folder, "data.json");
    public string ReportPath => Path.Combine(Folder, "report.html");
    public string OverlayPath => Path.Combine(Folder, "overlay.html");
    public string CsvPath => Path.Combine(Folder, "matches.csv");
    public string PrizeNamesPath => Path.Combine(Folder, "treasures.json");
    public string ErrorLogPath => Path.Combine(Folder, "errors.log");

    /// <summary>Premios conocidos de la caza del tesoro (semana 1). Los ids nuevos se añaden solos con un nombre vacío.</summary>
    public static Dictionary<string, string> DefaultPrizeNames => L.En
        ? new()
        {
            ["DelilahPrize:dpid_item1"] = "A$AP Rocky solid gold gauntlets (pickaxe) ?",
            ["DelilahPrize:dpid_item2"] = "Plunder in Paradise (loading screen)",
            ["DelilahPrize:dpid_item3"] = "Mercedes-AMG G 63 (vehicle) ?",
            ["DelilahPrize:dpid_item4"] = "No Trespassing (music track)",
        }
        : new()
        {
            ["DelilahPrize:dpid_item1"] = "Guanteletes de A$AP Rocky de oro macizo (pico) ?",
            ["DelilahPrize:dpid_item2"] = "Plunder in Paradise (pantalla de carga)",
            ["DelilahPrize:dpid_item3"] = "Mercedes-AMG G 63 (vehículo) ?",
            ["DelilahPrize:dpid_item4"] = "No Trespassing (pista musical)",
        };

    public void LoadPrizeNames()
    {
        var names = Load<Dictionary<string, string>>(PrizeNamesPath);
        if (names == null)
        {
            names = DefaultPrizeNames;
            Save(PrizeNamesPath, names);
        }

        TreasureNames.PrizeNames.Clear();
        foreach (var kv in names)
        {
            TreasureNames.PrizeNames[kv.Key] = kv.Value;
        }
    }

    /// <summary>Adds unknown prize ids to treasures.json so the user can name them. Returns the new ids.</summary>
    public List<string> RegisterPrizeIds(IEnumerable<string> ids)
    {
        var added = ids.Where(id => !TreasureNames.PrizeNames.ContainsKey(id)).Distinct().ToList();
        if (added.Count == 0)
        {
            return added;
        }

        var names = Load<Dictionary<string, string>>(PrizeNamesPath) ?? DefaultPrizeNames;
        foreach (var id in added)
        {
            names[id] = "";
            TreasureNames.PrizeNames[id] = "";
        }
        Save(PrizeNamesPath, names);
        return added;
    }

    public Store(string folder)
    {
        Folder = folder;
        Directory.CreateDirectory(folder);
        MigrateFileNames();
    }

    /// <summary>Versions before 1.1 used other file names: keep the user's treasure names, drop the old generated pages.</summary>
    private void MigrateFileNames()
    {
        try
        {
            var oldPrizes = Path.Combine(Folder, "tresors.json");
            if (File.Exists(oldPrizes) && !File.Exists(PrizeNamesPath))
            {
                File.Move(oldPrizes, PrizeNamesPath);
            }
            foreach (var generated in new[] { "rapport.html", "parties.csv" })
            {
                var path = Path.Combine(Folder, generated);
                if (File.Exists(path))
                {
                    File.Delete(path); // regenerated as report.html / matches.csv
                }
            }
        }
        catch
        {
            // not critical: the old files simply stay there
        }
    }

    public Config? LoadConfig() => Load<Config>(ConfigPath);
    public void SaveConfig(Config config) => Save(ConfigPath, config);
    /// <summary>A new history starts at the current analysis version (nothing to re-analyze).</summary>
    public TrackerData LoadData() => Load<TrackerData>(DataPath) ?? new TrackerData { AnalyzerVersion = ReplayAnalyzer.CurrentVersion };
    public void SaveData(TrackerData data) => Save(DataPath, data);

    private static T? Load<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(path), Json);
        }
        catch (Exception ex)
        {
            // Keep a copy of the unreadable file rather than silently losing the history.
            var backup = path + $".corrupt-{DateTime.Now:yyyyMMdd-HHmmss}";
            File.Copy(path, backup, overwrite: true);
            ErrorLog.Write($"Archivo ilegible: {path}", ex);

            return null;
        }
    }

    private static void Save<T>(string path, T value)
    {
        var tmp = path + ".tmp";
        File.WriteAllText(tmp, JsonSerializer.Serialize(value, Json));
        File.Move(tmp, path, overwrite: true);
    }
}
