namespace Advent_of_Code_2022;

[TestClass]
public class Day03
{
    private readonly Dictionary<char, int> _priority;

    public Day03()
    {
        _priority = new Dictionary<char, int>();
        var lower = "abcdefghijklmnopqrstuvwxyz";
        for (int i = 0; i < lower.Length; i++)
        {
            _priority[lower[i]] = i + 1;
            _priority[Char.ToUpper(lower[i])] = i + 27;
        }
    }

    private string Part1(IEnumerable<string> input)
    {
        var result = 0;
        foreach (var line in input)
        {
            var c = line.Take(line.Length / 2).Where(c => line.Skip(line.Length /2).Contains(c)).First();
            result += _priority[c];
        }
        return result.ToString();
    }
    
    private string Part2(IEnumerable<string> input)
    {
        var result = 0;
        foreach (var group in input.Chunk(3))
        {
            var c = group[0].Where(c => group[1].Contains(c) && group[2].Contains(c)).First();
            result += _priority[c];
        }
        return result.ToString();
    }
    
    readonly string _example = """
        vJrwpWtwJgWrhcsFMMfFFhFp
        jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
        PmmdzqPrVvPwwTWBwg
        wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
        ttgJtRGJQctTZtZT
        CrZsJsPPZsGzwwsLwLmpwMDw
        """;

    [TestMethod]
    public void Day03_Part1_Example01()
    {
        var result = Part1(Common.GetLines(_example));
        Assert.AreEqual("157", result);
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
        var result = Part2(Common.GetLines(_example));
        Assert.AreEqual("70", result);
    }
    
    [TestMethod]
    public void Day03_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day03)));
        Assert.AreEqual("2425", result);
    }

    [TestMethod]
    public void Day03_TestPriority()
    {
        Assert.AreEqual(26 * 2, _priority.Count);
        Assert.AreEqual(1, _priority['a']);
        Assert.AreEqual(26, _priority['z']);
        Assert.AreEqual(27, _priority['A']);
        Assert.AreEqual(52, _priority['Z']);
    }
}
