var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayFour\\input.txt");

// part 1
var numberOfPairs = 0;
foreach (var line in lines)
{
    var elves = line.Split(',');
    
    var firstElf = elves[0];
    var secondElf = elves[1];
    
    var firstElfRooms = firstElf.Split("-");
    var secondElfRooms = secondElf.Split("-");

    var firstElfFirstRoom = Parse(firstElfRooms.First());
    var firstElfLastRoom = Parse(firstElfRooms.Last());
    var secondElfFirstRoom = Parse(secondElfRooms.First());
    var secondElfLastRoom = Parse(secondElfRooms.Last());

    if ((firstElfFirstRoom <= secondElfFirstRoom && firstElfLastRoom >= secondElfLastRoom) ||
        (secondElfFirstRoom <= firstElfFirstRoom && secondElfLastRoom >= firstElfLastRoom))
    {
        numberOfPairs++;
    }
}

Console.WriteLine(numberOfPairs);

// part 2;
numberOfPairs = 0;
foreach (var line in lines)
{
    var elves = line.Split(',');

    var firstElf = elves[0];
    var secondElf = elves[1];

    var firstElfRoomRanges = firstElf.Split("-");
    var secondElfRoomsRanges = secondElf.Split("-");

    var firstElfFirstRoom = Parse(firstElfRoomRanges.First());
    var firstElfLastRoom = Parse(firstElfRoomRanges.Last());
    var secondElfFirstRoom = Parse(secondElfRoomsRanges.First());
    var secondElfLastRoom = Parse(secondElfRoomsRanges.Last());

    var firstElfRooms = Enumerable.Range(firstElfFirstRoom, firstElfLastRoom - firstElfFirstRoom + 1);
    var secondElfRooms = Enumerable.Range(secondElfFirstRoom, secondElfLastRoom - secondElfFirstRoom + 1);

    var intersect = firstElfRooms.Intersect(secondElfRooms);
    if (intersect.Any())
    {
        numberOfPairs++;
    }
}

Console.WriteLine(numberOfPairs);

static int Parse(string value)
{
    return int.Parse(value);
}