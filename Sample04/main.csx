using System.IO;
using System.Threading.Tasks;
private static string FindDiskWithName(string diskName)
{
    var allDrives = DriveInfo.GetDrives();

    foreach (var drive in allDrives)
    {
        if (drive.IsReady && drive.VolumeLabel.ToUpper() == diskName.ToUpper())
        {
            return drive.Name;
        }
    }

    return string.Empty;
}

private static void DeleteFilesInSourceDirectoryIfRequired(string sourceDirectory)
{
    Console.WriteLine("Do you want to delete source files? (y/n)");
    var answer = Console.ReadLine();
    if (answer.ToUpper() == "Y")
    {
        var filesInSourceDirectory = new DirectoryInfo(sourceDirectory).GetFiles();
        foreach (FileInfo file in filesInSourceDirectory)
        {
            file.Delete();
            Console.WriteLine($"{DateTime.Now}: File {file.FullName} deleted.");
        }

        Console.WriteLine("Files deleted.");
    }
}

private static void CopyFiles(string sourceDirectory, string targetDirectory)
{
    var filesInSourceDirectory = new DirectoryInfo(sourceDirectory).EnumerateFiles();
    foreach (var file in filesInSourceDirectory)
    {
        var lastWriteTime = file.LastWriteTime;
        // var creationDate = file.CreationTime;
        var year = lastWriteTime.Year;
        var month = lastWriteTime.Month;
        var day = lastWriteTime.Day;

        var targetYearDir = Path.Combine(targetDirectory, year.ToString());
        var targetDir = Path.Combine(targetYearDir, $"{year.ToString():0000}-{month:00}-{day:00}");

        var targetDirectoryInfo = new DirectoryInfo(targetDir);
        if (!targetDirectoryInfo.Exists)
        {
            targetDirectoryInfo.Create();
        }
        var targetFile = Path.Combine(targetDir, file.Name);

        Console.WriteLine($"{DateTime.Now}: Copying file {file.FullName} to {targetFile}.");

        var fileName = Path.Combine(targetDir, file.Name);
        if (!File.Exists(fileName))
        {
            file.CopyTo(fileName, true);
        }

        Console.WriteLine($"{DateTime.Now}: Done.");
    }
}

var sourceDirectory = @"C:\Videos\Videos Import\";
var sourceDrive = FindDiskWithName("System");
var mainTargetDirectory = @"C:\Videos\Footage\";
if (sourceDrive != string.Empty)
{
    sourceDirectory = Path.Combine(sourceDrive, @"Videos\Import\");
    mainTargetDirectory = Path.Combine(sourceDrive, @"Videos\Footage\");
}

var backupTargetDirectory = @"h:\Videos\Footage\";
var backupTargetDrive = FindDiskWithName("Backup");
if (backupTargetDrive != string.Empty)
{
    backupTargetDirectory = Path.Combine(backupTargetDrive, @"Videos\Footage\");
}

CopyFiles(sourceDirectory, mainTargetDirectory);
//CopyFiles(sourceDirectory, backupTargetDirectory);
DeleteFilesInSourceDirectoryIfRequired(sourceDirectory);