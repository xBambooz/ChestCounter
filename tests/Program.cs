using ChestCounter;
using System.Text.Json;

void Check(bool value) { if (!value) throw new Exception("Check failed"); }
Check(JsonSerializer.Deserialize<MatchRecord>("{\"ChestsOpened\":7}") is { Teammates: null, TeamChests: 7 });
var history = Enumerable.Range(0, 4).Select(i => new MatchRecord
{
    EpicId = "self", PlayerName = "Recorder", ChestsOpened = 10,
    Teammates = new() {
        new() { AccountId = "regular", Name = i == 0 ? "Old name" : "Regular", Observed = true, Chests = new() { new(), new() } },
        new() { AccountId = "missing", Name = "Unobserved" },
    },
}).ToList();
history[0].Teammates!.Add(new() { Name = "Newcomer", Observed = true, Chests = Enumerable.Range(0, 20).Select(_ => new ChestRecord()).ToList() });
var rows = Squad.Build(history);
Check(rows.Count == 3 && rows[^1].Name == "Newcomer");
Check(rows.Single(r => r.Name == "Regular") is { Matches: 4, Total: 8 });
Check(Math.Abs(rows.Single(r => r.Self).Score - (40 + 28.0 / 5 * 5) / 9) < .00001);
var root = Path.Combine(Path.GetTempPath(), "ChestCounter-check-" + Guid.NewGuid());
try
{
    Directory.CreateDirectory(Path.Combine(root, "demos"));
    var source = Path.Combine(root, "demos", "test.replay");
    File.WriteAllText(source, "synthetic replay");
    var config = new Config { DemosFolder = Path.GetDirectoryName(source)!, ArchiveFolder = Path.Combine(root, "archive") };
    ReplayArchive.Copy(new FileInfo(source), config);
    ReplayArchive.Copy(new FileInfo(source), config);
    Check(ReplayArchive.Size(config.ArchiveFolder) == new FileInfo(source).Length);
    config.ArchiveFolder = config.DemosFolder;
    try { ReplayArchive.Copy(new FileInfo(source), config); throw new Exception("Unsafe archive accepted"); }
    catch (IOException) { }
    Check(File.ReadAllText(source) == "synthetic replay");
}
finally { Directory.Delete(root, true); }
Console.WriteLine("History, squad ranking and archive checks passed.");
