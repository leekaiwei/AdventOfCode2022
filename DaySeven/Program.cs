const string changeDirectory = "cd";
const string listDirectory = "ls";
const string rootDirectory = "/";

var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DaySeven\\input.txt");
var currentDirectory = string.Empty;
var directorySizes = new Dictionary<string, int>();
var totalDirectorySize = 0;

foreach (var line in lines)
{
    if (line.StartsWith('$'))
    {
        Command(line);
    }
    else
    {
        var lineParts = line.Split(' ');
        if (char.IsDigit(lineParts[0][0]) && char.IsLetter(lineParts[1][0]))
        {
            ProcessDirectorySize(int.Parse(lineParts[0]));
        }
    }
}

foreach (var directorySize in directorySizes)
{
    if (directorySize.Value <= 100000)
    {
        totalDirectorySize += directorySize.Value;
    }
}

Console.WriteLine(totalDirectorySize);

void Command(string input)
{
    var commandParts = input.Split(' ');
    var command = commandParts[1];
    if (command == changeDirectory)
    {
        ChangeDirectory(commandParts[2]);
    }
}

void ChangeDirectory(string directory)
{
    if (directory == rootDirectory)
    {
        currentDirectory = rootDirectory;
    }
    else if (directory == "..")
    {
        currentDirectory = currentDirectory[1..(currentDirectory.Length - 1)];
        var directories = currentDirectory.Split('/').ToList();
        directories.RemoveAt(directories.Count - 1);
        if (!directories.Any())
        {
            currentDirectory = rootDirectory;
        }
        else
        {
            currentDirectory = "/" + string.Join('/', directories) + "/";
        }
        
    }
    else
    {
        currentDirectory += $"{directory}/";
    }
}

void ProcessDirectorySize(int directorySize)
{
    var directories = currentDirectory.Split('/');
    for (var i = 0; i < directories.Length; i++)
    {
        var directory = string.Join("/", directories[0..i]);
        var directoryExists = directorySizes!.TryGetValue(directory, out var size);
        if (!directoryExists)
        {
            directorySizes.Add(directory, directorySize);
        }
        else
        {
            size += directorySize;
            directorySizes[directory] = size;
        }
    }
}