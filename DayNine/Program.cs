var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayNine\\input.txt");

// part 1
Execute(lines, 2);

// part 2
Execute(lines, 10);

void Execute(string[] lines, int numberOfKnots)
{
    var tailVisitedPositions = new List<ValueTuple<int, int>>
    {
        (0 , 0)
    };

    var knots = new List<ValueTuple<int, int>>();
    for (var i = 0; i < numberOfKnots; i++)
    {
        knots.Add((0, 0));
    }

    foreach (var line in lines)
    {
        var lineParts = line.Split(' ');
        var direction = lineParts[0];
        var steps = int.Parse(lineParts[1]);

        for (var step = 1; step <= steps; step++)
        {
            var head = knots[0];
            if (direction == "L")
            {
                head.Item1--;
            }
            else if (direction == "R")
            {
                head.Item1++;
            }
            else if (direction == "U")
            {
                head.Item2++;
            }
            else if (direction == "D")
            {
                head.Item2--;
            }

            knots[0] = head;

            for (var j = 1; j < knots.Count; j++)
            {
                var knot = knots[j];
                var previousKnot = knots[j - 1];
                var previousKnotX = previousKnot.Item1;
                var previousKnotY = previousKnot.Item2;
                var currentKnotX = knot.Item1;
                var currentKnotY = knot.Item2;

                var xDelta = previousKnotX - currentKnotX;
                var yDelta = previousKnotY - currentKnotY;

                var xWrong = Math.Abs(xDelta) > 1;
                var yWrong = Math.Abs(yDelta) > 1;

                if (!xWrong && !yWrong)
                {
                    continue;
                }

                if (xWrong)
                {
                    knot.Item1 = MoveTailX(xDelta, currentKnotX);

                    if (previousKnotY != currentKnotY)
                    {
                        knot.Item2 = MoveTailY(yDelta, currentKnotY);
                    }

                }
                else if (yWrong)
                {
                    knot.Item2 = MoveTailY(yDelta, currentKnotY);

                    if (previousKnotX != currentKnotX)
                    {
                        knot.Item1 = MoveTailX(xDelta, currentKnotX);
                    }
                }

                knots[j] = knot;

                var tail = knots.Last();
                var tailX = tail.Item1;
                var tailY = tail.Item2;

                var isVisited = tailVisitedPositions.Any(position => position.Item1 == tailX && position.Item2 == tailY);
                if (!isVisited)
                {
                    tailVisitedPositions.Add((tailX, tailY));
                }
            }
        }
    }

    Console.WriteLine(tailVisitedPositions.Count);
}

int MoveTailX(int xDelta, int x)
{
    if (xDelta > 0)
    {
        return x + 1;
    }
    
    return x - 1;
}

int MoveTailY(int yDelta, int y)
{
    if (yDelta > 0)
    {
        return y + 1;
    }

    return y - 1;
}