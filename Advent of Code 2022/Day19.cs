namespace Advent_of_Code_2022;

[TestClass]
public class Day19
{
    public enum Resource { Ore, Clay, Obsidian, Geode };
    public record Robot(int OreCost, int ClayCost, int ObsidianCost);
    
    public class BluePrint
    {
        public int Id { get; init; }
        public Dictionary<Resource, Robot>? Robots { get; init; }

        public static BluePrint FromString(string str)
        {
            var split = str.Split(' ');
            var bluePrint = new BluePrint()
            {
                Id = int.Parse(split[1].Substring(0, split[1].Length - 1)),
                Robots = new Dictionary<Resource, Robot>()
                {
                    { Resource.Ore,  new Robot(int.Parse(split[6]), 0, 0) },
                    { Resource.Clay, new Robot(int.Parse(split[12]), 0, 0) },
                    { Resource.Obsidian, new Robot(int.Parse(split[18]), int.Parse(split[21]), 0) },
                    { Resource.Geode, new Robot(int.Parse(split[27]), 0, int.Parse(split[30])) }
                }
            };
            return bluePrint;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine($"Id:{Id} ");
            if (Robots is not null)
            {
                foreach (var (resource, robot) in Robots)
                {
                    result.Append($"   Resource: {resource} ");
                    result.AppendLine($"Robot: {robot}");
                }
            }
            return result.ToString();
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        foreach (var line in input)
        {
            var bluePrint = BluePrint.FromString(line);
            result.AppendLine(bluePrint.ToString());
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        foreach (var line in input)
        {
        }
        return result.ToString();
    }
    
    [TestMethod]
    public void Day19_Part1_Example01()
    {
        var input = """
            Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
            Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day19)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day19_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day19)));
        Assert.AreEqual("", result);
    }
    
}
