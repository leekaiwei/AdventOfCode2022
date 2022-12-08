var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayFive\\input.txt");
const int endingLine = 7;
const int startingStepsLine = endingLine + 3;

// part 1
var stacks = ReadStacks(lines);
for (var lineNumber = startingStepsLine; lineNumber < lines.Length; lineNumber++)
{
    var line = lines[lineNumber];
    var steps = line.Split(" ");
    var numberToMove = int.Parse(steps[1]);
    var from = int.Parse(steps[3]);
    var to = int.Parse(steps[5]);

    for (var number = 0; number < numberToMove; number++)
    {
        var crate = stacks[from - 1].Pop();
        stacks[to - 1].Push(crate);
    }
}

Print(stacks);

// part 2
stacks = ReadStacks(lines);
for (var lineNumber = startingStepsLine; lineNumber < lines.Length; lineNumber++)
{
    var line = lines[lineNumber];
    var steps = line.Split(" ");
    var numberToMove = int.Parse(steps[1]);
    var from = int.Parse(steps[3]);
    var to = int.Parse(steps[5]);

    var temporaryStack = new Stack<char>();
    for (var number = 0; number < numberToMove; number++)
    {
        var crate = stacks[from - 1].Pop();
        temporaryStack.Push(crate);
    }

    while (temporaryStack.Any())
    {
        var crate = temporaryStack.Pop();
        stacks[to - 1].Push(crate);
    }
}

Print(stacks);

static List<Stack<char>> ReadStacks(string[] lines)
{
    var result = new List<Stack<char>>();
    for (var lineNumber = endingLine; lineNumber >= 0; lineNumber--)
    {
        var line = lines[lineNumber];
        var crates = line.Split(' ');
        var isCrate = true;
        var stackIndex = 0;
        var numberOfSpaces = 0;
        foreach (var crate in crates)
        {
            if (crate == "")
            {
                if (isCrate && numberOfSpaces == 3)
                {
                    stackIndex++;
                    numberOfSpaces = 0;
                }
                else
                {
                    isCrate = true;
                }

                numberOfSpaces++;
                continue;
            }

            var letter = crate[1];
            var stack = result.ElementAtOrDefault(stackIndex);
            if (stack is null)
            {
                stack = new Stack<char>();
                result.Add(stack);
            }

            stack.Push(letter);

            isCrate = false;
            stackIndex++;
        }
    }

    return result;
}

static void Print(IEnumerable<Stack<char>> stacks)
{
    var result = string.Empty;
    foreach (var stack in stacks)
    {
        result += stack.Peek();
    }

    Console.WriteLine(result);
}