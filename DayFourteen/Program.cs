var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayFourteen\\input.txt");
const int sandStartingX = 500;

var numberOfSand = 0;

//numberOfSand = PartOne(lines);
numberOfSand = PartTwo(lines);

Console.WriteLine(numberOfSand);

int PartOne(string[] lines)
{
    var grid = GetStartingGrid(lines);
    var isFilled = false;
    var mostBottomFilledY = grid.Max(p => p.Y);
    var numberOfSand = 0;
    while (!isFilled)
    {
        Point sandRestingPoint;
        var stack = new Stack<Point>();
        var mostBottomFreePointYInX = grid.Where(p => p.X == sandStartingX).Min(p => p.Y) - 1;
        var startPoint = new Point(sandStartingX, mostBottomFreePointYInX, Status.Sand);
        stack.Push(startPoint);
        while (stack.TryPop(out var point))
        {
            var nextY = point.Y + 1;
            if (nextY > mostBottomFilledY)
            {
                isFilled = true;
                break;
            }

            var pointBelow = grid.SingleOrDefault(p => p.X == point.X && p.Y == nextY);
            var pointToTheLeftDown = grid.SingleOrDefault(p => p.X == point.X - 1 && p.Y == nextY);
            var pointToTheRightDown = grid.SingleOrDefault(p => p.X == point.X + 1 && p.Y == nextY);
            if (pointBelow is null)
            {
                stack.Push(new Point(point.X, nextY, Status.Sand));
                continue;
            }
            else if (pointToTheLeftDown is null)
            {
                stack.Push(new Point(point.X - 1, nextY, Status.Sand));
                continue;
            }
            else if (pointToTheRightDown is null)
            {
                stack.Push(new Point(point.X + 1, nextY, Status.Sand));
                continue;
            }

            grid.Add(point);
            numberOfSand++;
            Console.WriteLine(numberOfSand);
            break;
        }
    }

    return numberOfSand;
}

int PartTwo(string[] lines)
{
    var grid = GetStartingGrid(lines);
    var mostBottomFilledY = grid.Max(p => p.Y);
    var floorY = mostBottomFilledY + 2;
    var numberOfSand = 0;
    var isFilled = false;
    while (!isFilled)
    {
        Point sandRestingPoint;
        var stack = new Stack<Point>();
        var mostBottomFreePointYInX = grid.Where(p => p.X == sandStartingX).Min(p => p.Y) - 1;
        var startPoint = new Point(sandStartingX, mostBottomFreePointYInX, Status.Sand);
        stack.Push(startPoint);
        while (stack.TryPop(out var point))
        {
            //Console.Clear();
            //for (var y = 0; y <= floorY; y++)
            //{
            //    for (var x = 450; x < 550; x++)
            //    {
            //        var printer = grid.FirstOrDefault(p => p.X == x && p.Y == y);
            //        if (y == point.Y && x == point.X)
            //        {
            //            Console.Write('~');
            //        }
            //        else if (y == floorY)
            //        {
            //            Console.Write('#');
            //        }
            //        else if (printer == null)
            //        {
            //            Console.Write('.');
            //        }
            //        else if (printer.Status == Status.Rock)
            //        {
            //            Console.Write('#');
            //        }
            //        else if (printer.Status == Status.Sand)
            //        {
            //            Console.Write('O');
            //        }
            //    }

            //    Console.WriteLine();
            //}
            if (point.X == sandStartingX && point.Y < 0)
            {
                isFilled = true;
                break;
            }
            var nextY = point.Y + 1;
            if (nextY == floorY)
            {
                grid.Add(point);
                numberOfSand++;
                Console.WriteLine(numberOfSand);
                break;
            }

            var pointBelow = grid.SingleOrDefault(p => p.X == point.X && p.Y == nextY);
            var pointToTheLeftDown = grid.SingleOrDefault(p => p.X == point.X - 1 && p.Y == nextY);
            var pointToTheRightDown = grid.SingleOrDefault(p => p.X == point.X + 1 && p.Y == nextY);

            if (pointBelow is null)
            {
                stack.Push(new Point(point.X, nextY, Status.Sand));
                continue;
            }
            else if (pointToTheLeftDown is null)
            {
                stack.Push(new Point(point.X - 1, nextY, Status.Sand));
                continue;
            }
            else if (pointToTheRightDown is null)
            {
                stack.Push(new Point(point.X + 1, nextY, Status.Sand));
                continue;
            }

            grid.Add(point);
            numberOfSand++;
            Console.WriteLine(numberOfSand);
            break;
        }

        //Console.Clear();
        //for (var y = 0; y <= floorY; y++)
        //{
        //    for (var x = 450; x < 550; x++)
        //    {
        //        var printer = grid.FirstOrDefault(p => p.X == x && p.Y == y);
        //        if (y == floorY)
        //        {
        //            Console.Write('#');
        //        }
        //        else if (printer == null)
        //        {
        //            Console.Write('.');
        //        }
        //        else if (printer.Status == Status.Rock)
        //        {
        //            Console.Write('#');
        //        }
        //        else if (printer.Status == Status.Sand)
        //        {
        //            Console.Write('O');
        //        }
        //    }

        //    Console.WriteLine();
        //}
    }

    return numberOfSand;
}

HashSet<Point> GetStartingGrid(string[] lines)
{
    var grid = new HashSet<Point>();

    foreach (var line in lines)
    {
        var rocks = line.Split(" -> ");
        for (var i = 0; i < rocks.Length - 1; i++)
        {
            var fromRock = new Point(rocks[i], Status.Rock);
            var toRock = new Point(rocks[i + 1], Status.Rock);

            var instruction = fromRock.InstructionTo(toRock);
            if (instruction is null)
            {
                continue;
            }

            if (instruction.Axis == Axis.X)
            {
                for (var x = fromRock.X; x != instruction.PointToStop; x += instruction.Direction)
                {
                    grid = Add(grid, new Point(x, fromRock.Y, Status.Rock));
                }
            }

            if (instruction.Axis == Axis.Y)
            {
                for (var y = fromRock.Y; y != instruction.PointToStop; y += instruction.Direction)
                {
                    grid = Add(grid, new Point(fromRock.X, y, Status.Rock));
                }
            }
        }
    }

    return grid;
}

HashSet<Point> Add(HashSet<Point> grid, Point point)
{
    if (!grid.Any(p => p.EqualsPoint(point)))
    {
        grid.Add(point);
    }

    return grid;
}

record Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Status Status { get; set; }

    public Point(string coordinate, Status status)
    {
        var coordinates = coordinate.Split(',');
        X = int.Parse(coordinates[0]);
        Y = int.Parse(coordinates[1]);
        Status = status;
    }

    public Point(int X, int Y, Status status)
    {
        this.X = X;
        this.Y = Y;
        Status = status;
    }

    public Instruction? InstructionTo(Point point)
    {
        var xDelta = point.X - X;
        var xStop = X + xDelta;
        if (xDelta > 0)
        {
            return new Instruction(xStop, Axis.X, 1);
        }

        if (xDelta < 0)
        {
            return new Instruction(xStop, Axis.X, -1);
        }

        var yDelta = point.Y - Y;
        var yStop = Y + yDelta;
        if (yDelta > 0)
        {
            return new Instruction(yStop, Axis.Y, 1);
        }

        if (yDelta < 0)
        {
            return new Instruction(yStop, Axis.Y, -1);
        }

        return null;
    }

    public bool EqualsPoint(Point point)
    {
        return X == point.X && Y == point.Y;
    }
}

public enum Status
{
    Empty,
    Sand,
    Rock
}

public record Instruction
{
    public int PointToStop { get; set; }

    public Axis Axis { get; set; }

    public int Direction { get; set; }

    public Instruction(int pointToStop, Axis axis, int direction)
    {
        PointToStop = pointToStop + direction;
        Axis = axis;
        Direction = direction;
    }
}

public enum Axis
{
    X,
    Y
}