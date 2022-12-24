namespace Advent_of_Code_2022;

[TestClass]
public class Day23
{
    private static string Simulate(IEnumerable<string> input, int check)
    {
        var result = 0;
        var elfs = new List<Pos<int>>();
        {
            int y = 0;
            foreach (var line in input)
            {
                int x = 0;
                foreach (var c in line)
                {
                    if (c == '#')
                    {
                        elfs.Add(new Pos<int>(x, y));
                    }
                    x++;
                }
                y++;
            }
        }

        Print(elfs);
        for (int i = 0; Scan(elfs, i); i++)
        {
            //Console.WriteLine($"== End of Round {i + 1} ==");
            result = CountEmpty(elfs);
            if (i > 0 && i + 1 == check) break;
            //Console.WriteLine();
        }

        bool Scan(IEnumerable<Pos<int>> elfs, int offset)
        {
            Pos<int> N = new Pos<int>(0, -1);
            Pos<int> NE = new Pos<int>(1, -1);
            Pos<int> NW = new Pos<int>(-1, -1);

            Pos<int> S = new Pos<int>(0, 1);
            Pos<int> SE = new Pos<int>(1, 1);
            Pos<int> SW = new Pos<int>(-1, 1);

            Pos<int> W = new Pos<int>(-1, 0);
            Pos<int> E = new Pos<int>(1, 0);

            Dictionary<string, List<Pos<int>>> considers = new Dictionary<string, List<Pos<int>>>
            {
                { "A",  new List<Pos<int>> {N, NE, NW, S, SE, SW, W, E } },
                { "N",  new List<Pos<int>> {N, NE, NW} },
                { "S",  new List<Pos<int>> {S, SE, SW} },
                { "W",  new List<Pos<int>> {W, NW, SW} },
                { "E",  new List<Pos<int>> {E, NE, SE} },
            };
            Dictionary<string, Pos<int>> directions = new Dictionary<string, Pos<int>>
            {
                { "N",  N },
                { "S",  S },
                { "W",  W },
                { "E",  E },
            };
            List<string> order = new List<string> { "N", "S", "W", "E" };

            var proposals = new Dictionary<Pos<int>, List<Pos<int>>>();

            int Count(Pos<int> elf, IEnumerable<Pos<int>> elfs, IEnumerable<Pos<int>> considers)
            {
                return considers.Count(p => elfs.Contains(elf + p));
            }

            foreach (var elf in elfs)
            {
                for (int i = 0; i < order.Count; i++)
                {
                    var index = (i + offset) % order.Count;
                    var dir = directions[order[index]];

                    if (Count(elf, elfs, considers["A"]) == 0)
                    {
                        // No other elfs...stay put
                        //Console.WriteLine($"Elf{elf} stays");
                        break;
                    }
                    else if (Count(elf, elfs, considers[order[index]]) == 0)
                    {
                        var proposal = elf + dir;
                        //Console.WriteLine($"Elf{elf} propose {proposal}");
                        if (proposals.TryGetValue(proposal, out var list))
                        {
                            list.Add(elf);
                        }
                        else
                        {
                            proposals[proposal] = new List<Pos<int>> { elf };
                        }
                        break;
                    }
                }
            }

            foreach (var kv in proposals.Where(kv => kv.Value.Count == 1))
            {
                var e = kv.Value[0];
                //Console.WriteLine($"Move elf{e} to {kv.Key}");
                e.Set(kv.Key);
            }

            return proposals.Count > 0;
        }


        return result.ToString();
    }
    private static int CountEmpty(IEnumerable<Pos<int>> elfs)
    {
        //Console.WriteLine("Elfs: " + string.Join(",", elfs));
        var box = new Box<int>(elfs.ToArray());
        //Console.WriteLine($"box={box} box.Width={box.Width} box.Height={box.Height} box.Area={box.Area}");
        return box.Area - elfs.Count();
    }

    private static int Print(IEnumerable<Pos<int>> elfs)
    {
        var result = new StringBuilder();
        var box = new Box<int>(elfs.ToArray());
        //var unit = new Pos<int>(1, 1);
        //box.IncreaseToPoint(box.Min - unit);
        //box.IncreaseToPoint(box.Max + unit);
        var sum = 0;
        for (int y = box.Min.y; y<= box.Max.y; y++)
        {
            if (result.Length != 0) result.AppendLine();
            for (int x = box.Min.x; x <= box.Max.x; x++)
            {
                if (elfs.Contains(new Pos<int>(x, y)))
                {
                    result.Append('#');
                }
                else
                {
                    sum++;
                    result.Append('.');
                }
            }
        }
        Console.WriteLine(result.ToString());
        return sum;
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
    public void Day23_Part1_Example01()
    {
        var input = """
            .....
            ..##.
            ..#..
            .....
            ..##.
            .....
            """;
        var result = Simulate(Common.GetLines(input), -1);
        Assert.AreEqual("25", result);
    }
    
    [TestMethod]
    public void Day23_Part1_Example02()
    {
        var input = """
            ....#..
            ..###.#
            #...#.#
            .#...##
            #.###..
            ##.#.##
            .#..#..
            """;
        var result = Simulate(Common.GetLines(input), 10);
        Assert.AreEqual("110", result);
    }
    
    [TestMethod]
    public void Day23_Part1()
    {
        var result = Simulate(Common.DayInput(nameof(Day23)), 10);
        Assert.AreEqual("4005", result);
    }
    
    [TestMethod]
    public void Day23_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day23_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day23_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day23)));
        Assert.AreEqual("", result);
    }
    
}
