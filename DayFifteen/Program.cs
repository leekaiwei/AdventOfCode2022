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

var tasks = new List<Task>();

const int y = 2000000;
var filledY = new HashSet<int>();
var index = 1;
foreach (var combinations in sensorsToBeacons)
{
    var sensorX = combinations.Item1.Item1;
    var sensorY = combinations.Item1.Item2;
    var beaconX = combinations.Item2.Item1;
    var beaconY = combinations.Item2.Item2;
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

await Task.WhenAll(tasks);

Console.WriteLine(filledY.Count);
//Print(data, minimumX, minimumY, maximumX, maximumY);

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