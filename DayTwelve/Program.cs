var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayTwelve\\input.txt");
var height = lines.Length;
var length = lines[0].Length;

var matrix = new int[lines[0].Length, lines.Length];
var start = (0, 0);
var end = (0, 0);
var starts = new List<(int, int)>();
for (var y = 0; y < lines.Length; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        char currentNodeHeight;
        var character = line[x];
        if (character == 'S')
        {
            currentNodeHeight = 'a';
            start = (x, y);
        }
        else if (character == 'E')
        {
            currentNodeHeight = 'z';
            end = (x, y);
        }
        else
        {
            if (character == 'a')
            {
                starts.Add((x, y));
            }

            currentNodeHeight = character;
        }

        matrix[x, y] = currentNodeHeight;
    }
}

var graph = new List<Node>();
for (var y = 0; y < lines.Length; y++)
{
    var line = lines[y];
    for (var x = 0; x < line.Length; x++)
    {
        char currentNodeHeight;
        var character = line[x];
        if (character == 'S')
        {
            currentNodeHeight = 'a';
            start = (x, y);
        }
        else if (character == 'E')
        {
            currentNodeHeight = 'z';
            end = (x, y);
        }
        else
        {
            currentNodeHeight = character;
        }

        var node = graph.SingleOrDefault(n => n.X == x && n.Y == y);
        if (node is null)
        {
            node = new Node { X = x, Y = y };
            graph.Add(node);
        }

        var linesToCheck = new List<(int, int)>();
        if (x < length - 1)
        {
            linesToCheck.Add((x + 1, y));
        }

        if (x > 0)
        {
            linesToCheck.Add((x - 1, y));
        }

        if (y < height - 1)
        {
            linesToCheck.Add((x, y + 1));
        }

        if (y > 0)
        {
            linesToCheck.Add((x, y - 1));
        }

        foreach (var lineToCheck in linesToCheck)
        {
            if (TryGetNeighbourNode(currentNodeHeight, lineToCheck.Item1, lineToCheck.Item2, matrix, out var neighbourNode))
            {
                node.Neighbours.Add(neighbourNode);
            }
        }
    }
}

var shortestPath = int.MaxValue;

// part 1
//shortestPath = Execute(graph, start);
//Console.WriteLine(shortestPath);

// part 2
shortestPath = int.MaxValue;
foreach (var s in starts)
{
    var path = Execute(graph, s);
    if (path < shortestPath)
    {
        shortestPath = path;
    }
}

Console.WriteLine(shortestPath);

int Execute(List<Node> graph, (int, int) start)
{
    Console.WriteLine($"{start.Item1}, {start.Item2}");
    var firstNode = graph.Single(n => n.X == start.Item1 && n.Y == start.Item2);
    var queue = new Queue<Node>();
    queue.Enqueue(firstNode);

    var paths = new Dictionary<Node, List<Node>>();
    var visitedNodes = new Dictionary<Node, int>() { { firstNode, 0 } };
    while (queue.Any())
    {
        var node = queue.Dequeue();
        var nodeData = visitedNodes.First(n => n.Key.X == node.X && n.Key.Y == node.Y);
        foreach (var neighbourNode in node.Neighbours)
        {
            var isNodeVisited = visitedNodes.Any(n => n.Key.X == neighbourNode.X && n.Key.Y == neighbourNode.Y);
            var actualNeighbourNode = graph.Single(n => n.X == neighbourNode.X && n.Y == neighbourNode.Y);
            if (!isNodeVisited)
            {
                queue.Enqueue(actualNeighbourNode);
                visitedNodes.Add(neighbourNode, nodeData.Value + 1);

                if (neighbourNode.X == end.Item1 && neighbourNode.Y == end.Item2)
                {
                    break;
                }
            }
        }
    }

    var destinationNode = visitedNodes.SingleOrDefault(n => n.Key.X == end.Item1 && n.Key.Y == end.Item2);

    return destinationNode.Key == null ? int.MaxValue : destinationNode.Value;
}

static bool TryGetNeighbourNode(int currentNodeHeight, int x, int y, int[,] matrix, out Node node)
{
    var neighbourNodeHeight = matrix[x, y];
    var heightDelta = neighbourNodeHeight - currentNodeHeight;
    if (heightDelta <= 1)
    {
        node = new Node { X = x, Y = y };

        return true;
    }

    node = new Node();
    
    return false;
}

record Node
{
    public int X { get; set; }

    public int Y { get; set; }

    public ICollection<Node> Neighbours { get; set; } = new List<Node>();
}