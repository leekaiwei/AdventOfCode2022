const char a = 'A';
const char b = 'B';
const char c = 'C';

const char x = 'X';
const char y = 'Y';
const char z = 'Z';

var shapePoints = new Dictionary<char, int>
{
    { x, 1 },
    { y, 2 },
    { z, 3 },
};

const int lostPoints = 0;
const int drawPoints = 3;
const int winPoints = 6;

var drawResults = new Dictionary<char, char>
{
    { a, x },
    { b, y },
    { c, z },
};

var opponentWinResults = new Dictionary<char, char>
{
    { a, z },
    { b, x },
    { c, y },
};

// part 1
var totalPoints = 0;
var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayTwo\\input.txt");
foreach (var line in lines)
{
    var opponentShape = line[0];
    var myShape = line[2];

    if (drawResults[opponentShape] == myShape)
    {
        totalPoints += drawPoints;
    }
    else if (opponentWinResults[opponentShape] == myShape)
    {
        totalPoints += lostPoints;
    }
    else
    {
        totalPoints += winPoints;
    }

    totalPoints += shapePoints[myShape];
}

Console.WriteLine(totalPoints);

// part 2
const char lose = x;
const char draw = y;
const char win = z;

var opponentLoseResults = new Dictionary<char, char>
{
    { a, y },
    { b, z },
    { c, x },
};
char myRequiredShape;
totalPoints = 0;
foreach (var line in lines)
{
    var opponentShape = line[0];
    var requiredResult = line[2];

    if (requiredResult == lose)
    {
        myRequiredShape = opponentWinResults[opponentShape];
        totalPoints += lostPoints;
    }
    else if (requiredResult == draw)
    {
        myRequiredShape = drawResults[opponentShape];
        totalPoints += drawPoints;
    }
    else
    {
        myRequiredShape = opponentLoseResults[opponentShape];
        totalPoints += winPoints;
    }

    totalPoints += shapePoints[myRequiredShape];
}

Console.WriteLine(totalPoints);