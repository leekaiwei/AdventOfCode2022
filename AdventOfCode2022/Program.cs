var lines = File.ReadAllLines("H:\\Repositories\\AdventOfCode2022\\AdventOfCode2022\\input.txt");
var maximumTotal = 0;
var currentTotal = 0;

// part 1
foreach (var line in lines)
{
    var isNumber = int.TryParse(line, out var lineTotal);
    if (isNumber)
    {
        currentTotal+= lineTotal;
    }
    else
    {
        if (currentTotal > maximumTotal)
        {
            maximumTotal = currentTotal;
        }

        currentTotal = 0;
    }
}

Console.WriteLine(maximumTotal);

// part 2
currentTotal = 0;
var totals = new List<int>();
foreach (var line in lines)
{
    var isNumber = int.TryParse(line, out var lineTotal);
    if (isNumber)
    {
        currentTotal += lineTotal;
    }
    else
    {
        totals.Add(currentTotal);

        currentTotal = 0;
    }
}

maximumTotal = 0;
var orderedTotals = totals.OrderByDescending(total => total);
for (var i = 0; i <= 2; i++)
{
    maximumTotal += orderedTotals.ElementAt(i);
}

Console.WriteLine(maximumTotal);