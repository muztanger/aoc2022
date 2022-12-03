using System.Security.Cryptography.X509Certificates;

namespace Advent_of_Code_2022;

[TestClass]
public class Day03
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = 0;
        foreach (var line in input)
        {
            // rucksack two compartments
            var c1 = string.Concat(line.Take(line.Length / 2));
            var c2 = string.Concat(line.Skip(line.Length / 2));
            Console.WriteLine($"line={line} c1={c1} c2={c2}");
            
            var priority = new Dictionary<char, int>();
            var lower = "abcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < lower.Length; i++)
            {
                priority[lower[i]] = i + 1;
                priority[Char.ToUpper(lower[i])] = i + 27;
            }
            Assert.AreEqual(26 * 2, priority.Count);
            Assert.AreEqual(1, priority['a']);
            Assert.AreEqual(26, priority['z']);
            Assert.AreEqual(27, priority['A']);
            Assert.AreEqual(52, priority['Z']);

            foreach (char c in c1)
            {
                if (c2.Contains(c))
                {
                    Console.WriteLine($"Found: {c} {priority[c]}");
                    result += priority[c];
                    break;
                }
            }
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var priority = new Dictionary<char, int>();
        var lower = "abcdefghijklmnopqrstuvwxyz";
        for (int i = 0; i < lower.Length; i++)
        {
            priority[lower[i]] = i + 1;
            priority[Char.ToUpper(lower[i])] = i + 27;
        }
        Assert.AreEqual(26 * 2, priority.Count);
        Assert.AreEqual(1, priority['a']);
        Assert.AreEqual(26, priority['z']);
        Assert.AreEqual(27, priority['A']);
        Assert.AreEqual(52, priority['Z']);

        var result = 0;
        var group = new List<string>();
        foreach (var line in input)
        {
            group.Add(line);
            if (group.Count == 3)
            {
                foreach (char c in group[0])
                {
                    if (group[1].Contains(c) && group[2].Contains(c))
                    {
                        Console.WriteLine($"Found: {c} {priority[c]}");
                        result += priority[c];
                        break;
                    }
                }
                group.Clear();
            }

        }
        return result.ToString();
    }
    
    [TestMethod]
    public void Day03_Part1_Example01()
    {
        var input = """
            vJrwpWtwJgWrhcsFMMfFFhFp
            jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
            PmmdzqPrVvPwwTWBwg
            wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
            ttgJtRGJQctTZtZT
            CrZsJsPPZsGzwwsLwLmpwMDw
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("157", result);
    }
    
    [TestMethod]
    public void Day03_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day03_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day03)));
        Assert.AreEqual("8053", result);
    }
    
    [TestMethod]
    public void Day03_Part2_Example01()
    {
        var input = """
            vJrwpWtwJgWrhcsFMMfFFhFp
            jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
            PmmdzqPrVvPwwTWBwg
            wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
            ttgJtRGJQctTZtZT
            CrZsJsPPZsGzwwsLwLmpwMDw
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("70", result);
    }
    
    [TestMethod]
    public void Day03_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day03_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day03)));
        Assert.AreEqual("", result);
    }
    
}
