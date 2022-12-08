var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayThree\\input.txt");
const int lowerStartingPriorityDifference = (int)'a' - 1;
const int upperStartingPriorityDifference = (int)'A' - 27;

// part 1
var totalPriority = 0;
char matchingCharacter;
foreach (var line in lines)
{
    var halfLength = line.Length / 2;
    var firstHalf = line[..halfLength];
    var secondHalf = line[halfLength..];
    var intersect = firstHalf.Intersect(secondHalf);
    matchingCharacter = intersect.First();
    if (char.IsLower(matchingCharacter))
    {
        totalPriority += matchingCharacter - lowerStartingPriorityDifference;
    }
    else
    {
        totalPriority += matchingCharacter - upperStartingPriorityDifference;
    }
}

Console.WriteLine(totalPriority);

// part 2
totalPriority = 0;
for (var line = 3; line <= lines.Length; line += 3)
{
    var lineOne = lines[line - 3];
    var lineTwo = lines[line - 2];
    var lineThree = lines[line - 1];

    var firstIntersect = lineOne.Intersect(lineTwo);
    var secondIntersect = firstIntersect.Intersect(lineThree);
    matchingCharacter = secondIntersect.First();

    if (char.IsLower(matchingCharacter))
    {
        totalPriority += matchingCharacter - lowerStartingPriorityDifference;
    }
    else
    {
        totalPriority += matchingCharacter - upperStartingPriorityDifference;
    }
}

Console.WriteLine(totalPriority);