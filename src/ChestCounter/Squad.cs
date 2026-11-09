namespace ChestCounter;

public sealed record SquadRow(string Name, bool Self, int Matches, int Total, double Average, double Score);

public static class Squad
{
    public static string Help => L.T("La clasificación combina la media de cada compañero con cinco partidas de la media general. Tu fila usa tu media real.", "Teammates are ranked by their average blended with five matches at the overall teammate average. Your row uses your real average.");
    public static string Caveat => L.T("Los replays solo incluyen acciones de compañeros cercanos o espectados; los recuentos pueden ser incompletos.", "Replays only include nearby or spectated teammates; counts can be incomplete.");

    public static List<SquadRow> Build(IEnumerable<MatchRecord> history)
    {
        var matches = history.Where(m => m.Teammates != null).ToList();
        var observed = matches.SelectMany(m => m.Teammates!.Where(t => t.Observed)).ToList();
        var mean = observed.Count > 0 ? observed.Average(t => t.Chests.Count) : 0;
        var entries = matches.SelectMany(m => m.Teammates!.Where(t => t.Observed).Select(t =>
            (Key: t.AccountId ?? t.Name, t.Name, Self: false, Count: t.Chests.Count)))
            .Concat(matches.Select(m => (Key: m.EpicId ?? m.PlayerName ?? "self", Name: m.PlayerName ?? L.T("Tú", "You"), Self: true, Count: m.ChestsOpened)));
        return entries.GroupBy(e => (e.Key, e.Self)).Select(g =>
        {
            var total = g.Sum(e => e.Count);
            return new SquadRow(g.Last().Name, g.Key.Self, g.Count(), total, (double)total / g.Count(), g.Key.Self ? (double)total / g.Count() : (total + mean * 5) / (g.Count() + 5));
        }).OrderByDescending(r => r.Matches >= 3).ThenByDescending(r => r.Score).ThenBy(r => r.Name).ToList();
    }
}
