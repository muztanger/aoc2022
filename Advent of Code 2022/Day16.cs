using System.Data.Common;
using System.Xml.XPath;

namespace Advent_of_Code_2022;

[TestClass]
public class Day16
{
    public class Valve: IEquatable<Valve>
    {
        public static int MaxFlow { get; set; } = 0;
        public string Name { get; init; } = string.Empty;
        public int FlowRate { get; init; }
        
        private List<Valve> _output = new List<Valve>();

        public void AddOutput(Valve valve)
        {
            _output.Add(valve);
        }

        public override string ToString()
        {
            return $"{Name}, FlowRate={FlowRate}";
        }

        bool IEquatable<Valve>.Equals(Valve? other)
        {
            if (other is not null)
            {
                return Name.Equals(other.Name);
            }
            return false;
        }

        public override bool Equals(object? obj)
        {
            return ((IEquatable<Valve>)this).Equals(obj as Valve);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        static int count = 0;
        public int MaxReleasedPressure(int time, ref List<Valve> openValves, ref Dictionary<(Valve, bool, int), int> cache)
        {
            count++;
            var release = openValves.Sum(v => v.FlowRate);
            //Console.WriteLine($"{Name} time={time} release={release}");

            if (time < 0 ) { return 0; }
            if (time == 0) { return release; }
            
            if (release == MaxFlow)
            {
                return time * MaxFlow;
            }

            var isOpen = openValves.Contains(this);
            if (cache.TryGetValue((this, isOpen, time), out var cachedMax))
            {
                return cachedMax;
            }

            int max = 0;

            if (FlowRate > 0)
            {
                if (isOpen)
                {
                    var testMax = 0;
                    foreach (Valve valve in _output)
                    {
                        testMax = Math.Max(testMax, valve.MaxReleasedPressure(time - 1, ref openValves, ref cache));
                    }
                    cache[(this, true, time)] = testMax;
                    max = Math.Max(max, testMax);
                }
                else
                {
                    {
                        var testMax = 0;
                        foreach (Valve valve in _output)
                        {
                            testMax = Math.Max(testMax, valve.MaxReleasedPressure(time - 1, ref openValves, ref cache));
                        }
                        cache[(this, false, time)] = testMax;
                        max = Math.Max(max, testMax);
                    }
                    {
                        var testMax = 0;
                        openValves.Add(this);
                        foreach (Valve valve in _output)
                        {
                            var pressure = valve.MaxReleasedPressure(time - 2, ref openValves, ref cache);
                            testMax = Math.Max(testMax, pressure);
                        }
                        openValves.RemoveAt(openValves.Count - 1);
                        cache[(this, true, time)] = testMax;
                        max = Math.Max(max, testMax);
                    }
                }
            }
            else
            {
                var testMax = 0;
                foreach (Valve valve in _output)
                {
                    testMax = Math.Max(testMax, valve.MaxReleasedPressure(time - 1, ref openValves, ref cache));
                }
                cache[(this, false, time)] = testMax;
                max = Math.Max(max, testMax);
            }
            return release + max;
        }

        record StackItem(Valve valve, int time, int total, List<Valve> openValves);

        internal int SearchMaxReleasePressure(int time, int maxValveCount)
        {
            int Release(List<Valve> openValves) => openValves.Sum(v => v.FlowRate);
            var stack = new Stack<StackItem>();
            var maxTot = 0;
            stack.Push(new StackItem(this, time, 0, new List<Valve>()));
            while (stack.Any())
            {
                var item = stack.Pop();
                (var v, var t, var tot, var openValves) = item;
                Assert.IsTrue(t >= 0, $"stack.Count={stack.Count} item={item}");
                if (t == 0)
                {
                    //Console.WriteLine($"item={item} stack.Count={stack.Count}");
                    maxTot = Math.Max(tot, maxTot);
                }
                else if (openValves.Count == maxValveCount)
                {
                    t--;
                    tot += t * Release(openValves);
                    maxTot = Math.Max(tot, maxTot);
                }
                else
                {
                    tot += Release(openValves);
                    if (v.FlowRate > 0 && !openValves.Contains(v))
                    {
                        t--;
                        var valves = new List<Valve>(openValves)
                        {
                            v
                        };
                        var next = new StackItem(v, t, tot, valves);
                        stack.Push(next);
                    }
                    if (t > 0)
                    {
                        t--;
                        foreach (var w in v._output.Where(x => !openValves.Contains(x)))
                        {
                            StackItem next = new (w, t, tot, openValves);
                            stack.Push(next);
                        }
                    }
                }
            }

            return maxTot;
        }

        record Steps(Valve v, int x);

        internal int ShortestPath(Valve other)
        {

            var result = int.MaxValue;
            var stack = new Stack<Steps>();
            stack.Push(new Steps(this, 0));
            while (stack.Any())
            {
                (var v, var x) = stack.Pop();
                if (v == other)
                {
                    result = Math.Min(result, x);
                }
                else
                {
                    x++;
                    if (x < result && x < 30)
                    {
                        foreach (var w in v._output)
                        {
                            if (!stack.Any(x => x.v.Name == w.Name))
                            {
                                stack.Push(new Steps(w, x));
                            }
                        }
                    }
                }
            }
            return result;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        // open name 1 min
        // walk name 1 min
        // flow rate = pressure/min
        // Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
        var re = new Regex(@"Valve (?<valve>[^ ]+) has flow rate=(?<flowrate>\d+);[a-z ]*(?<output>[A-Z, ]+$)+");
        var valves = new HashSet<Valve>();
        foreach (var line in input)
        {
            var match = re.Match(line);
            Assert.IsTrue(match.Success, line);
            Assert.AreEqual(4, match.Groups.Count);
      
            var valve = match.Groups["valve"].Value;
            
            var flowRate = int.Parse(match.Groups["flowrate"].Value);
            Valve.MaxFlow += flowRate;
            
            Valve v = new Valve() { FlowRate = flowRate, Name = valve };
            valves.Add(v);
        }
        Assert.IsTrue(valves.TryGetValue(new Valve() { Name = "AA" }, out var root));
        foreach (var line in input)
        {
            var match = re.Match(line);
            Assert.IsTrue(match.Success, line);
            Assert.AreEqual(4, match.Groups.Count);
            
            var name = match.Groups["valve"].Value;
            var outputs = match.Groups["output"].Value.Split(", ");

            Assert.IsTrue(valves.TryGetValue(new Valve() { Name = name }, out var valve));
            foreach (var outputName in outputs)
            {
                Assert.IsTrue(valves.TryGetValue(new Valve() { Name = outputName }, out var output));
                valve.AddOutput(output);
            }
        }

        var distances = new Dictionary<(Valve, Valve), int>();
        foreach (var v1 in valves)
        {
            foreach (var v2 in valves)
            {
                if (v1 == v2) continue;
                if (v1 == root || v2 == root || (v1.FlowRate > 0 && v2.FlowRate > 0) )
                {
                    var key = string.Join(",", (new List<string> { v1.Name, v2.Name }).OrderBy(x => x));
                    if (!distances.TryGetValue((v1, v2), out var x))
                    {
                        distances[(v1, v2)] = v1.ShortestPath(v2);
                        distances[(v2, v1)] = distances[(v1, v2)];
                    }
                }
            }
        }

        var time = 30;
        var maxValveCount = valves.Count(v => v.FlowRate > 0);
        //var cache = new Dictionary<(Valve, bool, int), int>();
        //var openValves = new List<Valve>();
        //var result = root.MaxReleasedPressure(time, ref openValves, ref cache);
        int result = root.SearchMaxReleasePressure(time, maxValveCount);
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
    public void Day16_Part1_Example01()
    {
        var input = """
            Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
            Valve BB has flow rate=13; tunnels lead to valves CC, AA
            Valve CC has flow rate=2; tunnels lead to valves DD, BB
            Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
            Valve EE has flow rate=3; tunnels lead to valves FF, DD
            Valve FF has flow rate=0; tunnels lead to valves EE, GG
            Valve GG has flow rate=0; tunnels lead to valves FF, HH
            Valve HH has flow rate=22; tunnel leads to valve GG
            Valve II has flow rate=0; tunnels lead to valves AA, JJ
            Valve JJ has flow rate=21; tunnel leads to valve II
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("1651", result);
    }
    
    [TestMethod]
    public void Day16_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day16)));
        Assert.AreNotEqual("164", result);
        Assert.AreNotEqual("3530", result);
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day16_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day16_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day16_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day16)));
        Assert.AreEqual("", result);
    }
    
}
