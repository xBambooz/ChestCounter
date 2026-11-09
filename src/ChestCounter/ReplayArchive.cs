namespace ChestCounter;

public static class ReplayArchive
{
    public static long Size(string folder) => Directory.Exists(folder)
        ? new DirectoryInfo(folder).EnumerateFiles("*.replay").Sum(f => f.Length) : 0;

    public static void Copy(FileInfo source, Config config)
    {
        var folder = Path.GetFullPath(config.ArchiveFolder);
        var demos = Path.GetFullPath(config.DemosFolder);
        if (folder.Equals(demos, StringComparison.OrdinalIgnoreCase) || folder.StartsWith(demos.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new IOException(L.T("El archivo debe estar fuera de Demos.", "The archive must be outside Demos."));
        Directory.CreateDirectory(folder);
        var destination = Path.Combine(folder, source.Name);
        if (!source.FullName.Equals(destination, StringComparison.OrdinalIgnoreCase) && !File.Exists(destination))
        {
            var pending = destination + ".partial";
            try
            {
                File.Copy(source.FullName, pending, true);
                File.SetLastWriteTimeUtc(pending, source.LastWriteTimeUtc);
                File.Move(pending, destination);
            }
            finally { if (File.Exists(pending)) File.Delete(pending); }
        }
        var files = new DirectoryInfo(folder).EnumerateFiles("*.replay").OrderBy(f => f.LastWriteTimeUtc).ToList();
        var size = files.Sum(f => f.Length);
        var limit = Math.Clamp(config.ArchiveLimitGb, 1, 10000) * 1024L * 1024 * 1024;
        foreach (var file in files)
        {
            if (size <= limit) break;
            var length = file.Length;
            file.Delete();
            size -= length;
        }
    }
}
