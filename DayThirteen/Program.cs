using System.Text.Json;

var lines = await File.ReadAllLinesAsync("H:\\Repositories\\AdventOfCode2022\\DayThirteen\\input.txt");

var list = new List<string>();
foreach (var line in lines)
{
    if (line.StartsWith('['))
    {
        list.Add(line);
    }
}

list.Add("[2]");
list.Add("[6]");

// One(result);

Two(list);

void One(List<string> result)
{
    var correctIndices = new List<int>();
    var index = 1;

    for (var i = 0; i < result.Count - 1; i++)
    {
        Console.Write($"Index {index}: ");

        var left = result[i];
        var right = result[i + 1];

        var leftList = JsonSerializer.Deserialize<List<dynamic>>(left);
        var rightList = JsonSerializer.Deserialize<List<dynamic>>(right);

        var isRight = Calculate(leftList, rightList);
        if (isRight is null || isRight == true)
        {
            if (isRight is null)
            {
                Console.WriteLine("Left ran out so right order");
            }

            correctIndices.Add(index);
        }

        index++;
    }

    Console.WriteLine(correctIndices.Sum());
}
static bool? Calculate(List<object> leftList, List<object> rightList)
{
    if (!leftList.Any() && rightList.Any())
    {
        Console.WriteLine("Left got nothing but right has so right order");
        return true;
    }

    bool? result = null;
    for (var leftValueIndex = 0; leftValueIndex < leftList.Count; leftValueIndex++)
    {
        var leftValue = leftList[leftValueIndex];
        var rightValue = rightList.ElementAtOrDefault(leftValueIndex);
        if (rightValue is null)
        {
            Console.WriteLine("Right ran out so wrong order");
            result = false;
        }
        else
        {
            var leftIsInteger = TryParseInteger(leftValue, out var leftInteger);
            var rightIsInteger = TryParseInteger(rightValue, out var rightInteger);
            if (leftIsInteger && rightIsInteger)
            {
                if (leftInteger < rightInteger)
                {
                    Console.WriteLine($"{leftInteger} < {rightInteger} right order");
                    result = true;
                }
                else if (leftInteger > rightInteger)
                {
                    Console.WriteLine($"{leftInteger} > {rightInteger} wrong order");
                    result = false;
                }
                else
                {
                    result = null;
                }
            }
            else if (leftIsInteger)
            {
                result = Calculate(new List<object> { leftInteger }, ToList(rightValue));
            }
            else if (rightIsInteger)
            {
                result = Calculate(ToList(leftValue), new List<object> { rightValue });
            }
            else
            {
                result = Calculate(ToList(leftValue), ToList(rightValue));
            }
        }

        if (result is not null)
        {
            return result;
        }
    }

    return result;
}

void Two(List<string> result)
{
    var index = 1;

    var sorted = new List<bool>();
    for (var i = 0; i < result.Count - 1; i++)
    {
        sorted.Add(false);
    }

    while (sorted.Any(a => !a))
    {
        index = 1;
        for (var i = 0; i < result.Count - 1; i++)
        {
            Console.Write($"Index {index}: ");

            var left = result[i];
            var right = result[i + 1];

            var leftList = JsonSerializer.Deserialize<List<dynamic>>(left);
            var rightList = JsonSerializer.Deserialize<List<dynamic>>(right);

            var isRight = Sort(leftList, rightList);
            if (isRight is null || isRight == true)
            {
                if (isRight is null)
                {
                    Console.WriteLine("Left ran out so right order");
                }

                sorted[i] = true;
            }
            else
            {
                result[i] = right;
                result[i + 1] = left;

                sorted[i] = false;
            }

            index++;
        }
    }

    Console.WriteLine((result.IndexOf("[2]") + 1) * (result.IndexOf("[6]") + 1));
}

bool? Sort(List<object> leftList, List<object> rightList)
{
    if (!leftList.Any() && rightList.Any())
    {
        Console.WriteLine("Left got nothing but right has so right order");
        return true;
    }

    bool? result = null;
    for (var leftValueIndex = 0; leftValueIndex < leftList.Count; leftValueIndex++)
    {
        var leftValue = leftList[leftValueIndex];
        var rightValue = rightList.ElementAtOrDefault(leftValueIndex);
        if (rightValue is null)
        {
            Console.WriteLine("Right ran out so wrong order");
            result = false;
        }
        else
        {
            var leftIsInteger = TryParseInteger(leftValue, out var leftInteger);
            var rightIsInteger = TryParseInteger(rightValue, out var rightInteger);
            if (leftIsInteger && rightIsInteger)
            {
                if (leftInteger < rightInteger)
                {
                    Console.WriteLine($"{leftInteger} < {rightInteger} right order");
                    result = true;
                }
                else if (leftInteger > rightInteger)
                {
                    Console.WriteLine($"{leftInteger} > {rightInteger} wrong order");
                    result = false;
                }
                else if (leftValueIndex == leftList.Count - 1 && leftList.Count < rightList.Count)
                {
                    Console.WriteLine("Left ran out first so right order");
                    result = true;
                }
                else
                {
                    result = null;
                }
            }
            else if (leftIsInteger)
            {
                result = Sort(new List<object> { leftInteger }, ToList(rightValue));
            }
            else if (rightIsInteger)
            {
                result = Sort(ToList(leftValue), new List<object> { rightValue });
            }
            else
            {
                result = Sort(ToList(leftValue), ToList(rightValue));
            }
        }

        if (result is not null)
        {
            return result;
        }
    }

    return result;
}

static bool TryParseInteger(dynamic input, out int result)
{
    if (input is int)
    {
        result = (int)input;

        return true;
    }
    else if (input.ValueKind == JsonValueKind.Number)
    {
        result = int.Parse(input.ToString());

        return true;
    }

    result = 0;
    return false;
}

static List<object> ToList(dynamic input)
{
    var list = new List<object>();
    foreach (var thing in ((JsonElement)input).EnumerateArray())
    {
        list.Add(thing);
    }

    return list;
}