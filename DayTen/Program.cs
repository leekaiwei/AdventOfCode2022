var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayTen\\input.txt");

PartTwo(lines);

void PartOne(string[] lines)
{
    var instructions = GetInstructions(lines);

    var cyclesToCheck = new int[] { 20, 60, 100, 140, 180, 220 };
    var totalSignalStrength = 0;
    var x = 1;
    var cycle = 1;
    var index = 0;
    while (index < instructions.Count)
    {
        if (cyclesToCheck.Contains(cycle))
        {
            var signalStrength = x * cycle;
            totalSignalStrength += signalStrength;

        }
        var instruction = instructions[index];
        instruction.Item2--;
        instructions[index] = instruction;
        if (instruction.Item2 == 0)
        {
            x += instruction.Item1;
            index++;
        }

        cycle++;
    }

    Console.WriteLine(totalSignalStrength);
}

void PartTwo(string[] lines)
{
    var instructions = GetInstructions(lines);

    var x = 1;
    var cycle = 1;
    var index = 0;
    var drawingPosition = 0;
    while (index < instructions.Count)
    {
        char character;
        var spritePositions = new int[] { x - 1, x, x + 1 };
        if (spritePositions.Contains(drawingPosition))
        {
            character = '#';
        }
        else
        {
            character = '.';
        }

        if (drawingPosition == 39)
        {
            Console.WriteLine(character);
            drawingPosition = 0;
        }
        else
        {
            Console.Write(character);
            drawingPosition++;
        }

        var instruction = instructions[index];
        instruction.Item2--;
        instructions[index] = instruction;
        if (instruction.Item2 == 0)
        {
            x += instruction.Item1;
            index++;
        }

        cycle++;
    }
}

List<(int, int)> GetInstructions(string[] lines)
{
    var instructions = new List<(int, int)>();
    foreach (var line in lines)
    {
        if (line == "noop")
        {
            instructions.Add((0, 1));
        }
        else
        {
            var lineParts = line.Split(' ');
            var numberToAdd = int.Parse(lineParts[1]);
            instructions.Add((numberToAdd, 2));
        }
    }

    return instructions;
}