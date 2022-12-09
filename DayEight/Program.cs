var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayEight\\input.txt");
var maxX = lines[0].Length - 1;
var maxY = lines.Length - 1;

var numberOfVisibleTrees = (2 * lines.Length) + (2 * lines[0].Length) - 4;
var trees = new List<Tree>();
for (var y = 0; y < lines.Length; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        trees.Add(new Tree
        {
            X = x,
            Y = y,
            Height = int.Parse(line[x].ToString())
        });
    }
}

//PartOne(lines, trees);

// part 2
var maximumViewScore = 0;
foreach (var tree in trees)
{
    var treesForRow = trees.Where(t => t.Y == tree.Y);
    var treesForColumn = trees.Where(t => t.X == tree.X);

    var treesAbove = treesForColumn.Where(t => t.Y < tree.Y);
    var treesBelow = treesForColumn.Where(t => t.Y > tree.Y);
    var treesToTheLeft = treesForRow.Where(t => t.X < tree.X);
    var treesToTheRight = treesForRow.Where(t => t.X > tree.X);

    var viewBlocked = false;

    var y = tree.Y - 1;
    var viewAboveScore = 0;
    while (!viewBlocked && y >= 0)
    {
        var treeToCheck = treesAbove.Single(t => t.Y == y);
        if (treeToCheck.Height < tree.Height)
        {
            viewAboveScore++;
        }
        else
        {
            viewAboveScore++;
            viewBlocked = true;
        }

        y--;
    }

    viewBlocked = false;

    y = tree.Y + 1;
    var viewBelowScore = 0;
    while (!viewBlocked && y <= maxY)
    {
        var treeToCheck = treesBelow.Single(t => t.Y == y);
        if (treeToCheck.Height < tree.Height)
        {
            viewBelowScore++;
        }
        else
        {
            viewBelowScore++;
            viewBlocked = true;
        }

        y++;
    }

    viewBlocked = false;

    var x = tree.X - 1;
    var viewToTheLeftScore = 0;
    while (!viewBlocked && x >= 0)
    {
        var treeToCheck = treesToTheLeft.Single(t => t.X == x);
        if (treeToCheck.Height < tree.Height)
        {
            viewToTheLeftScore++;
        }
        else
        {
            viewToTheLeftScore++;
            viewBlocked = true;
        }

        x--;
    }

    viewBlocked = false;

    x = tree.X + 1;
    var viewToTheRightScore = 0;
    while (!viewBlocked && x <= maxX)
    {
        var treeToCheck = treesToTheRight.Single(t => t.X == x);
        if (treeToCheck.Height < tree.Height)
        {
            viewToTheRightScore++;
        }
        else
        {
            viewToTheRightScore++;
            viewBlocked = true;
        }

        x++;
    }

    var viewScore = viewAboveScore * viewBelowScore * viewToTheLeftScore * viewToTheRightScore;
    if (viewScore > maximumViewScore)
    {
        maximumViewScore = viewScore;
    }
}

Console.WriteLine(maximumViewScore);

void PartOne(string[] lines, IEnumerable<Tree> trees)
{
    foreach (var tree in trees)
    {
        if (tree.IsEdge(maxX, maxY))
        {
            continue;
        }

        var tallerTreesForRow = trees.Where(t => t.Y == tree.Y && t.Height >= tree.Height);
        var tallerTreesForColumn = trees.Where(t => t.X == tree.X && t.Height >= tree.Height);

        var numberOfTreesTallerForRow = tallerTreesForRow.Count();
        var numberOfTreesTallerForColumn = tallerTreesForColumn.Count();
        if (numberOfTreesTallerForRow < 2 || numberOfTreesTallerForColumn < 2)
        {
            numberOfVisibleTrees++;
            continue;
        }

        if (numberOfTreesTallerForRow >= 2)
        {
            var treesToTheLeft = tallerTreesForRow.Where(t => t.X < tree.X);
            var treesToTheRight = tallerTreesForRow.Where(t => t.X > tree.X);
            if (!treesToTheLeft.Any() || !treesToTheRight.Any())
            {
                numberOfVisibleTrees++;
                continue;
            }
        }

        if (numberOfTreesTallerForColumn >= 2)
        {
            var treesAbove = tallerTreesForColumn.Where(t => t.Y < tree.Y);
            var treesBelow = tallerTreesForColumn.Where(t => t.Y > tree.Y);
            if (!treesAbove.Any() || !treesBelow.Any())
            {
                numberOfVisibleTrees++;
            }
        }
    }

    Console.WriteLine(numberOfVisibleTrees);
}

record Tree
{
    public int X { get; set; }

    public int Y { get; set; }

    public int Height { get; set; }

    public bool IsEdge(int maxX, int maxY)
    {
        if (X == 0 ||
            X == maxX ||
            Y == 0 ||
            Y == maxY)
        {
            return true;
        }

        return false;
    }
}