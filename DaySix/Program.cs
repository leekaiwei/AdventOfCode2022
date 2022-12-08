var input = await File.ReadAllTextAsync("H:\\Repositories\\AdventOfCode2022\\DaySix\\input.txt");

// part 1
var index = 4;
var characters = input[..4];
Output(index, characters, input);

// part 2
index = 14;
characters = input[..14];
Output(index, characters, input);

void Output(int index, string characters, string input)
{
    while (!IsMarker(characters))
    {
        characters = characters[1..];
        characters += input[index];
        index++;
    }

    Console.WriteLine(index);
}


static bool IsMarker(string input)
{
    var set = new HashSet<char>();
    for (int i = 0; i < input.Length; i++)
    {
        if (!set.Add(input[i]))
        {
            return false;
        }
    }
        
    return true;
}