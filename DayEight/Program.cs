var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayEight\\input.txt");
var linesToCheck = lines[1..(lines.Length - 1)];

var tallestPerRow = new int[lines[0].Length];
var tallestPerColumn = new int[lines.Length];

for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    var tallestForRow = 0;
    foreach (var tree in line)
    {
        var treeHeight = int.Parse(tree.ToString());
        if (treeHeight > tallestForRow)
        {
            tallestForRow = treeHeight;
        }
    }

    tallestPerRow[i] = tallestForRow;
}

for (var i = 0; i < lines[0].Length; i++)
{
    var tallestForColumn = 0;
    foreach (var line in lines)
    {
        var tree = line[i];
        var treeHeight = int.Parse(tree.ToString());
        if (treeHeight > tallestForColumn)
        {
            tallestForColumn = treeHeight;
        }
    }

    tallestPerColumn[i] = tallestForColumn;
}

for (var i = 0; i < lines.Length; i++)
{
    var line = lines[i];
    var lineToCheck = line[1..(lines.Length - 1)];
    for (var j = 0; j < lineToCheck.Length; j++)
    {
        CheckTree(lineToCheck[j], j + 1, i + 1);
    }
}

void CheckTree(int height, int x, int y)
{
    var tallestForRow = tallestPerRow[y];
    var tallestForColumn = tallestPerColumn[x];
    if (height > t)
}