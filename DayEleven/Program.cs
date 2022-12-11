using System.Numerics;

var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayEleven\\input.txt");

// part 1
Execute(lines, 20, true);

// part 2
//Execute(lines, 1000, false);

void Execute(string[] lines, int rounds, bool divideByThree)
{
    var inspectionRounds = new int[] { 1, 20, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 };
    var monkeys = GetMonkeys(lines);
    for (var i = 0; i < rounds; i++)
    {
        Console.WriteLine($"Calculating Round: {i}...");
        Console.WriteLine();
        foreach (var m in monkeys)
        {
            while (m.StartingItems.TryDequeue(out var oldWorryLevel))
            {
                var operations = m.Operations;
                var newWorryLevel = Calculate(operations[1], operations[0], operations[2], oldWorryLevel);
                newWorryLevel = divideByThree ? newWorryLevel / 3 : newWorryLevel;
                if (newWorryLevel % m.DivideBy == 0)
                {
                    monkeys[m.TrueMonkey].StartingItems.Enqueue(newWorryLevel);
                }
                else
                {
                    monkeys[m.FalseMonkey].StartingItems.Enqueue(newWorryLevel);
                }

                m.TimesInspected++;
            }

            if (inspectionRounds.Contains(i + 1))
            {
                Console.WriteLine(m.TimesInspected);

                if (m == monkeys.Last())
                {
                    Console.WriteLine();
                }
            }
        }
    }

    var orderedMonkeys = monkeys.OrderByDescending(m => m.TimesInspected).ToArray();
    Console.WriteLine(orderedMonkeys[0].TimesInspected * orderedMonkeys[1].TimesInspected);
}

List<Monkey> GetMonkeys(string[] lines)
{
    var monkeys = new List<Monkey>();
    var monkey = new Monkey();
    foreach (var line in lines)
    {
        var trimmedLine = line.Trim();
        var lineSplitBySpace = line.Split(' ');
        var lineSplitByColon = line.Split(':');
        var lineAfterColon = lineSplitByColon.Length > 1 ? lineSplitByColon[1] : null;
        if (trimmedLine.StartsWith("Monkey"))
        {
            monkey = new Monkey();
        }
        else if (trimmedLine.StartsWith("Starting"))
        {
            var startingItems = lineAfterColon!.Split(", ");
            foreach (var startingItem in startingItems)
            {
                monkey.StartingItems.Enqueue(int.Parse(startingItem));
            }
        }
        else if (trimmedLine.StartsWith("Operation"))
        {
            monkey.Operation = lineAfterColon!.Split("= ")[1];
        }
        else if (trimmedLine.StartsWith("Test"))
        {
            monkey.DivideBy = int.Parse(lineAfterColon!.Split(' ').Last());
        }
        else if (trimmedLine.StartsWith("If true"))
        {
            monkey.TrueMonkey = int.Parse(lineAfterColon!.Split(' ').Last());
        }
        else if (trimmedLine.StartsWith("If false"))
        {
            monkey.FalseMonkey = int.Parse(lineAfterColon!.Split(' ').Last());
            monkeys.Add(monkey);
        }
    }

    return monkeys;
}

static int Calculate(string operation, string leftSide, string rightSide, int old)
{
    var left = GetValue(leftSide, old);
    var right = GetValue(rightSide, old);
    int result = 0;
    switch (operation)
    {
        case "+":
            result = left + right;
            break;
        case "-":
            result = left - right;
            break;
        case "*":
            result = Multiply(left, right);
            break;
        default:
            throw new Exception();
    }

    return result;
}

static int Multiply(int a, int b)
{
    return a * b;
}

static int GetValue(string input, int old) => input == "old" ? old : int.Parse(input);

record Monkey
{
    public Queue<int> StartingItems { get; set; } = new();

    public int DivideBy { get; set; }

    public string Operation { get; set; } = null!;

    public int TrueMonkey { get; set; }

    public int FalseMonkey { get; set; }

    public string[] Operations => Operation.Split(' ');

    public int TimesInspected { get; set; } = 0;
}