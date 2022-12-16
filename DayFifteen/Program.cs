using System.Diagnostics.CodeAnalysis;

var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayFifteen\\input.txt");
var data = new Dictionary<int, Dictionary<int, Item?>>();
var sensorsToBeacons = new List<((int, int), (int, int), int)>();

var minimumX = int.MaxValue;
var minimumY = int.MaxValue;
var maximumX = int.MinValue;
var maximumY = int.MinValue;

foreach (var line in lines)
{
    var lineParts = line.Split(':');

    var sensorParts = lineParts[0].Split(' ');
    var sensorX = int.Parse(sensorParts[2].Split("x=")[1].Split(',')[0]);
    var sensorY = int.Parse(sensorParts[3].Split("y=")[1]);

    Dictionary<int, Item?> ySensorsForX;
    if (data.TryGetValue(sensorX, out var ySensors))
    {
        ySensorsForX = ySensors;
    }
    else
    {
        ySensorsForX = new Dictionary<int, Item?>();
        data.Add(sensorX, ySensorsForX);
    }
    
    ySensorsForX.Add(sensorY, Item.Sensor);

    var beaconParts = lineParts[1].Split(' ');
    var beaconX = int.Parse(beaconParts[5].Split("x=")[1].Split(',')[0]);
    var beaconY = int.Parse(beaconParts[6].Split("y=")[1]);

    Dictionary<int, Item?> yBeaconsForX;
    if (data.TryGetValue(beaconX, out var yBeacons))
    {
        yBeaconsForX = yBeacons;
    }
    else
    {
        yBeaconsForX = new Dictionary<int, Item?>();
        data.Add(beaconX, yBeaconsForX);
    }

    if (!yBeaconsForX.ContainsKey(beaconY))
    {
        yBeaconsForX.Add(beaconY, Item.Beacon);
    }

    var xDelta = beaconX - sensorX;
    var yDelta = beaconY - sensorY;

    var delta = Math.Abs(xDelta) + Math.Abs(yDelta);

    minimumX = Math.Min(sensorX - delta, minimumX);
    minimumY = Math.Min(sensorY - delta, minimumY);
    maximumX = Math.Max(sensorX + delta, maximumX);
    maximumY = Math.Max(sensorY + delta, maximumY);

    sensorsToBeacons.Add(((sensorX, sensorY), (beaconX, beaconY), delta));
}

//PartOne(data, sensorsToBeacons, minimumX, maximumX);

//PartTwo(data, sensorsToBeacons);

var source = new CancellationTokenSource();
var tasks = new List<Task>();
const int max = 20;
var start = DateTime.Now;
for (var y = 0; y <= max + 1; y++)
{
    var t = y;
    //tasks.Add(Task.Run(() =>
    //{
    //    var stop = PartTwoAgain(data, sensorsToBeacons, 0, max, t, start);
    //    if (stop)
    //    {
    //        source.Cancel();
    //    }
    //}, source.Token));

    var stop = PartTwoAgain(data, sensorsToBeacons, 0, max, t, start);
    if (stop)
    {
        break;
    }
}

try
{
    await Task.WhenAll(tasks);
}
catch { }

void PartOne(Dictionary<int, Dictionary<int, Item?>> data, List<((int, int), (int, int), int)> sensorsToBeacons, int minimumX, int maximumX)
{
    const int y = 10;
    var filledY = new HashSet<int>();
    var index = 1;
    foreach (var combinations in sensorsToBeacons)
    {
        var sensorX = combinations.Item1.Item1;
        var sensorY = combinations.Item1.Item2;
        var sensorMaxRange = combinations.Item3;

        for (var x = minimumX; x <= maximumX; x++)
        {
            //if (x % 1000000 == 0)
            //{
            //    Console.WriteLine($"{index}: {x} || {minimumX} - {maximumX}");
            //}

            var xDelta = x - sensorX;
            var yDelta = y - sensorY;
            var delta = Math.Abs(xDelta) + Math.Abs(yDelta);

            var pointTaken = false;
            if (data.TryGetValue(x, out var xData))
            {
                if (xData.ContainsKey(y))
                {
                    pointTaken = true;
                }
            }

            if (delta <= sensorMaxRange && !pointTaken)
            {
                filledY.Add(x);
            }
        }

        Console.WriteLine(index);
        index++;
    }

    Console.WriteLine(filledY.Count);
    //Print(data, minimumX, minimumY, maximumX, maximumY);
}

void PartTwo(Dictionary<int, Dictionary<int, Item?>> data, List<((int, int), (int, int), int)> sensorsToBeacons)
{
    const int max = 20;
    var index = 1;
    //var emptyPoints = new char[max + 1, max + 1];
    //for (var x = 0; x <= max; x++)
    //{
    //    for (var y = 0; y <= max; y++)
    //    {
    //        emptyPoints[x, y] = 'X';
    //    }
    //}

    var result = new HashSet<(int, int)>();
    var confirmed = new HashSet<(int, int)>(new Comparer());
    
    foreach (var combinations in sensorsToBeacons)
    {
        var sensorX = combinations.Item1.Item1;
        var sensorY = combinations.Item1.Item2;
        var beaconX = combinations.Item2.Item1;
        var beaconY = combinations.Item2.Item2;
        var sensorMaxRange = combinations.Item3;

        var hashSet = new HashSet<(int, int)>(new Comparer());
        var subList = new HashSet<(int, int)>(new Comparer());
        var y = 0;
        while (y <= max)
        {
            var xList = new HashSet<int>();
            var x = 0;
            
            while (x <= max)
            {
                //Console.WriteLine(x);
                //if (x % 1000000 == 0)
                //{
                //    Console.WriteLine($"{index}: {x} || {minimumX} - {maximumX}");
                //}
            
                var xDelta = x - sensorX;
                var yDelta = y - sensorY;
                var delta = Math.Abs(xDelta) + Math.Abs(yDelta);

                var pointTaken = false;
                if (data.TryGetValue(x, out var xData))
                {
                    if (xData.ContainsKey(y))
                    {
                        pointTaken = true;
                    }
                }

                if (delta > sensorMaxRange && !pointTaken && !confirmed.Contains((x, y)))
                {
                    result.Add((x, y));
                    x++;
                }
                else
                {
                    result.Remove((x, y));
                    confirmed.Add((x, y));

                    var skip = 0;
                    
                    if (sensorY == y)
                    {
                        skip = (delta * 2) + 2;
                    }
                    else if (y < sensorY)
                    {
                        skip = ((delta - sensorY + y) * 2) + 1;
                    }
                    else
                    {
                        skip = ((delta + sensorY - y) * 2) + 1;
                    }

                    var oldX = x;
                    x += skip;

                    for (var t = oldX; t <= x; t++)
                    {
                        result.Remove((t, y));
                    }
                }
                //x++;
            }

            y++;
        }

        Console.WriteLine(index);
        index++;
    }

    //for (var i = 0; i < max + 1; i++)
    //{
    //    for (var j = 0; j < max + 1; j++)
    //    {
    //        Console.Write(emptyPoints[i, j]);
    //    }

    //    Console.WriteLine();
    //}
    var final = result.Single();
    Console.WriteLine((final.Item1 * 40000000) + final.Item2);
    //Print(data, minimumX, minimumY, maximumX, maximumY);
}

bool PartTwoAgain(Dictionary<int, Dictionary<int, Item?>> data, List<((int, int), (int, int), int)> sensorsToBeacons, int minimumX, int maximumX, int y, DateTime start)
{
    if (y % 10 == 0)
    {
        var now = DateTime.Now;
        Console.WriteLine($"{now}: {y} {now - start}");
        //Console.WriteLine(y);
    }

    //var now = DateTime.Now;
    //Console.WriteLine($"{now}: {y} {now - start}");

    var skips = new HashSet<(int, int)>();

    var filledY = new HashSet<int>();
    var index = 1;
    foreach (var combinations in sensorsToBeacons)
    {
        var sensorX = combinations.Item1.Item1;
        var sensorY = combinations.Item1.Item2;
        var beaconX = combinations.Item2.Item1;
        var beaconY = combinations.Item2.Item2;
        var sensorMaxRange = combinations.Item3;

        int? skipFrom = null;
        int? skipTo = null;

        var x = minimumX;
        while (x <= maximumX)
        {
            if (skipFrom.HasValue && skipTo.HasValue && x >= skipFrom && x <= skipTo)
            {
                x = skipTo.Value + 1;
            }

            //if (x % 1000000 == 0)
            //{
            //    Console.WriteLine($"{index}: {x} || {minimumX} - {maximumX}");
            //}

            var xDelta = x - sensorX;
            var yDelta = y - sensorY;
            var delta = Math.Abs(xDelta) + Math.Abs(yDelta);

            var pointTaken = false;
            if (data.TryGetValue(x, out var xData))
            {
                if (xData.ContainsKey(y))
                {
                    pointTaken = true;
                }
            }

            if (pointTaken && delta > sensorMaxRange)
            {
                filledY.Add(x);
            }

            if (delta > sensorMaxRange)
            {
                x++;
            }

            else if (delta <= sensorMaxRange)
            {
                //filledY.Add(x);

                var skip = 0;
                if (sensorY == y)
                {
                    skip = (delta * 2) + 2;
                }
                else if (y < sensorY)
                {
                    skip = ((delta - sensorY + y) * 2) + 1;
                }
                else
                {
                    skip = ((delta + sensorY - y) * 2) + 1;
                }

                var oldX = x;
                x += skip;

                skipFrom = !skipFrom.HasValue ? oldX : Math.Min(oldX, skipFrom.Value);
                skipTo = !skipTo.HasValue ? x : Math.Max(x, skipTo.Value);
                skipTo = skipTo > max ? max : skipTo;
                skipTo--;
            }

            if (skipFrom.HasValue && skipTo.HasValue)
            {
                skips.Add((skipFrom.Value, skipTo.Value));
            }
        }

        //Console.WriteLine($"{y} {index}");
        index++;
    }

    //Console.WriteLine();

    var distinctSkips = new HashSet<(int, int)>(new Comparer());
    var skipsArray = skips.ToArray();
    var queue = new Queue<(int, int)>();
    foreach (var skip in skips)
    {
        queue.Enqueue(skip);
    }

    while (queue.TryDequeue(out var skip))
    {
        if (!distinctSkips.Any())
        {
            distinctSkips.Add(skip);
        }
        else
        {
            var temporarySkips = distinctSkips.ToHashSet(new Comparer());
            foreach (var distinctSkip in distinctSkips)
            {
                (int?, int?) toSkip = (null, null);
                if (skip.Item1 < distinctSkip.Item1 && skip.Item2 < distinctSkip.Item1)
                {
                    toSkip = skip;
                }
                else if (skip.Item1 < distinctSkip.Item1 && skip.Item2 == distinctSkip.Item1)
                {
                    temporarySkips.Remove(distinctSkip);
                    toSkip = (skip.Item1, distinctSkip.Item2);
                }
                else if (skip.Item1 < distinctSkip.Item1 && skip.Item2 > distinctSkip.Item2)
                {
                    temporarySkips.Remove(distinctSkip);
                    toSkip = skip;
                }
                else if (skip.Item1 == distinctSkip.Item1 && skip.Item2 > distinctSkip.Item2)
                {
                    temporarySkips.Remove(distinctSkip);
                    toSkip = skip;
                }
                else if (skip.Item1 > distinctSkip.Item1 && skip.Item1 < distinctSkip.Item2 & skip.Item2 > distinctSkip.Item2)
                {
                    temporarySkips.Remove(distinctSkip);
                    toSkip = (distinctSkip.Item1, skip.Item2);
                }
                else if (skip.Item1 < distinctSkip.Item1 && skip.Item2 > distinctSkip.Item1 && skip.Item2 < distinctSkip.Item2)
                {
                    temporarySkips.Remove(distinctSkip);
                    toSkip = (skip.Item1, distinctSkip.Item2);
                }
                else if (skip.Item1 > distinctSkip.Item2)
                {
                    toSkip = skip;
                }
                else if (skip.Item1 == distinctSkip.Item2)
                {
                    temporarySkips.Remove(distinctSkip);
                    toSkip = (distinctSkip.Item1, skip.Item2);
                }
                else if (skip.Item1 < distinctSkip.Item1 && skip.Item1 < distinctSkip.Item2 && skip.Item2 == distinctSkip.Item2)
                {
                    temporarySkips.Remove(distinctSkip);
                    toSkip = skip;
                }
                else if (skip.Item1 >= distinctSkip.Item1 && skip.Item2 < distinctSkip.Item2)
                {
                    temporarySkips.Remove(skip);
                    if (queue.Count == 1 && queue.Contains(skip))
                    {
                        queue.Clear();
                    }
                    break;
                }
                else if (skip.Item1 > distinctSkip.Item1 && skip.Item2 <= distinctSkip.Item2)
                {
                    temporarySkips.Remove(skip);
                    if (queue.Count == 1 && queue.Contains(skip))
                    {
                        queue.Clear();
                    }
                    break;
                }
                else
                {
                    break;
                }

                if (toSkip.Item1.HasValue && toSkip.Item2.HasValue)
                {
                    var properSkip = (toSkip.Item1.Value, toSkip.Item2.Value);
                    var addedToList = temporarySkips.Add(properSkip);
                    if (toSkip.Item1 == minimumX && toSkip.Item2 == maximumX)
                    {
                        queue.Clear();
                    }
                    else if (addedToList)
                    {
                        queue.Enqueue(properSkip);
                    }
                }
            }

            distinctSkips = temporarySkips;
        }
    }

    var finalFilled = new HashSet<int>();
    foreach (var f in filledY)
    {
        foreach (var skip in distinctSkips)
        {
            if (f < skip.Item1 && f > skip.Item2)
            {
                finalFilled.Add(f);
            }
        }
    }

    var sum = distinctSkips.Sum(s => s.Item2 - s.Item1);
    sum = sum > maximumX ? maximumX : sum;
    var filled = finalFilled.Count + sum;
    var answer = maximumX - filled;
    if (answer == 1)
    {
        var range = new HashSet<int>(Enumerable.Range(minimumX, maximumX));
        range.ExceptWith(filledY);
        Console.WriteLine($"Answer: {range.First()}, {y}");

        return true;
    }

    return false;

    //Print(data, minimumX, minimumY, maximumX, maximumY);
}
static void Print(Dictionary<int, Dictionary<int, Item?>> grid, int fromX, int fromY, int toX, int toY)
{
    for (var y = fromY - 10; y <= toY + 10; y++)
    {
        for (var x = fromX - 10; x <= toX + 10; x++)
        {
            char character;
            Item? point;
            try
            {
                point = grid[x][y];

                switch (point)
                {
                    case null:
                        character = '.';
                        break;
                    case Item.NoBeacon:
                        character = '#';
                        break;
                    case Item.Sensor:
                        character = 'S';
                        break;
                    case Item.Beacon:
                        character = 'B';
                        break;
                    default:
                        character = '.';
                        break;
                }
            }
            catch
            {
                character = '.';
            }

            Console.Write(character);
        }

        Console.WriteLine();
    }
}

enum Item
{
    NoBeacon,
    Sensor,
    Beacon
}

class Comparer : IEqualityComparer<(int, int)>
{
    public bool Equals((int, int) x, (int, int) y)
    {
        return x.Item1 == y.Item1 && x.Item2 == y.Item2;
    }

    public int GetHashCode([DisallowNull] (int, int) obj)
    {
        var tmp = (obj.Item2 + ((obj.Item1 + 1) / 2));
        return obj.Item1 + (tmp * tmp);
    }
}