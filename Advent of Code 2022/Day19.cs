using System.Collections.Generic;
using System.Linq;

namespace Advent_of_Code_2022;

[TestClass]
public class Day19
{
    public enum Resource { Ore, Clay, Obsidian, Geode };
    public record Robot(Resource Resource, int OreCost, int ClayCost, int ObsidianCost);
    
    public class BluePrint
    {
        public BluePrint(int id, Dictionary<Resource, Robot> robots)
        {
            Id = id;
            Robots = robots;
        }

        public int Id { get; init; }
        public Dictionary<Resource, Robot> Robots { get; init; }

        public static BluePrint FromString(string str)
        {
            var split = str.Split(' ');
            var robots = new Dictionary<Resource, Robot>()
                {
                    { Resource.Ore,  new Robot(Resource.Ore, int.Parse(split[6]), 0, 0) },
                    { Resource.Clay, new Robot(Resource.Clay, int.Parse(split[12]), 0, 0) },
                    { Resource.Obsidian, new Robot(Resource.Obsidian, int.Parse(split[18]), int.Parse(split[21]), 0) },
                    { Resource.Geode, new Robot(Resource.Geode, int.Parse(split[27]), 0, int.Parse(split[30])) }
                };
            return new BluePrint(id: int.Parse(split[1][..^1]), robots: robots);
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine($"Id:{Id} ");
            if (Robots is not null)
            {
                foreach (var robot in Robots.Values)
                {
                    result.AppendLine($"{robot}");
                }
            }
            return result.ToString();
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var qualities = new List<int>();
        foreach (var line in input)
        {
            var bluePrint = BluePrint.FromString(line);
            Console.WriteLine(bluePrint);
            var maxGeodes = MaxGeodes(bluePrint);
            qualities.Add(bluePrint.Id * maxGeodes);
        }

        var result = new StringBuilder();
        return result.ToString();
    }

    private static int MaxGeodes(BluePrint bluePrint)
    {
        var minutes = 24;
        var robots = new Dictionary<Robot, int>()
        {
            { bluePrint.Robots[Resource.Ore], 1 },
            { bluePrint.Robots[Resource.Clay], 0 },
            { bluePrint.Robots[Resource.Obsidian], 0 },
            { bluePrint.Robots[Resource.Geode], 0 },
        };
        var resources = new Dictionary<Resource, int>()
        {
            {Resource.Ore, 0 },
            {Resource.Clay, 0 },
            {Resource.Obsidian, 0 },
            {Resource.Geode, 0 },
        };
        return MaximizeGeodes(bluePrint, minutes, robots, resources);
    }

    private static int MaximizeGeodes(BluePrint bluePrint, int minutes, Dictionary<Robot, int> robots, Dictionary<Resource, int> resources)
    {
        Console.WriteLine($"Minutes {24 - minutes + 1} resources: {string.Join(",", resources.Select(kv => $"{kv.Key}={kv.Value}"))} robots:{string.Join(",", robots.Select(kv => $"{kv.Key}:{kv.Value}"))}");
        
        if (minutes <= 0) return resources[Resource.Geode];

        bool CanPay(Robot robot)
        {
            return resources[Resource.Ore] >= robot.OreCost
                && resources[Resource.Clay] >= robot.ClayCost
                && resources[Resource.Obsidian] >= robot.ObsidianCost;
        }

        var choices = bluePrint.Robots.Values.Where(r => CanPay(r)).ToList();

        Dictionary<Robot, int> AddRobot(Robot robot, Dictionary<Robot, int> robots)
        {
            var result = new Dictionary<Robot, int>(robots);
            result.TryGetValue(robot, out var count);
            count++;
            result[robot] = count;
            return result;
        }

        Dictionary<Resource, int> ResourcesAfterPay(Robot robot)
        {
            var result = new Dictionary<Resource, int>(resources);
            result[Resource.Ore] -= robot.OreCost;
            result[Resource.Clay] -= robot.ClayCost;
            result[Resource.Obsidian] -= robot.ObsidianCost;
            return result;
        }

        var maxGeode = 0;
        Robot? maxChoice = null;
        if (choices.Count > 0)
        {
            foreach (var choice in choices)
            {
                Dictionary<Resource, int> resAfterPayAndProduce = ResourcesAfterPay(choice);
                Dictionary<Robot, int> moreRobots = AddRobot(choice, robots);
                foreach (var robot in moreRobots)
                {
                    var resource = robot.Key.Resource;
                    resAfterPayAndProduce.TryGetValue(resource, out var x);
                    x += robot.Value;
                    resAfterPayAndProduce[resource] = x;
                }
                var geode = MaximizeGeodes(bluePrint, minutes - 1, moreRobots, resAfterPayAndProduce);
                if (geode > maxGeode)
                {
                    maxGeode = geode;
                    maxChoice = choice;
                }
            }
        }
        { // test choose none as well
            foreach (var robot in robots)
            {
                var resource = robot.Key.Resource;
                resources.TryGetValue(resource, out var x);
                x += robot.Value;
                resources[resource] = x;
            }

            var geode = MaximizeGeodes(bluePrint, minutes - 1, robots, resources);
            if (geode > maxGeode)
            {
                maxGeode = geode;
                maxChoice = null;
            }
        }

        return maxGeode;
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
