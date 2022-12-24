namespace Advent_of_Code_2022;

[TestClass]
public class Day24
{
    class Blizard
    {
        internal Pos<int> Pos { get; set; } = new(0, 0);
        internal Pos<int> Dir { get; set; } = new(0, 0);
    }

    class Valley
    {
        internal Box<int> Area { get; set; } = new(new Pos<int>(0, 0));
        internal Box<int> InnerArea { get; set; } = new(new Pos<int>(1, 1));

        internal Pos<int> Start { get; set;} = new(1, 0);
        internal Pos<int> End { get; set; } = new(0, 1);
        internal List<Blizard> Blizards { get; set; } = new List<Blizard>();

        internal void Tick()
        {
            foreach (var blizard in Blizards)
            {
                var next = blizard.Pos + blizard.Dir;
                if (InnerArea.IsInside(next))
                {
                    blizard.Pos = next;
                }
                else
                {
                    var q = next - InnerArea.Min; // move for modulus operation
                    q = (q + InnerArea.Size) % InnerArea.Size;
                    blizard.Pos = q + InnerArea.Min;
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int x = Area.Min.x; x <= Area.Max.x; x++)
            {
                var p = new Pos<int>(x, Area.Min.y);
                if (p == Start)
                {
                    result.Append('.');
                }
                else
                {
                    result.Append('#');
                }
            }
            result.AppendLine();
            for (int y = InnerArea.Min.y; y <= InnerArea.Max.y; y++)
            {
                if (y != InnerArea.Min.y)
                {
                    result.AppendLine();
                }
                result.Append('#');
                for (int x = InnerArea.Min.x; x <= InnerArea.Max.x; x++)
                {
                    var p = new Pos<int>(x, y);
                    var blizards = Blizards.Where(b => b.Pos == p);
                    var count = blizards.Count();
                    if (count <= 0)
                    {
                        result.Append('.');
                    }
                    else if (count == 1)
                    {
                        result.Append(DirectionChars[blizards.First().Dir]);
                    }
                    else
                    {
                        result.Append(count > 9 ? '∞' : count);
                    }
                }
                result.Append('#');
            }
            result.AppendLine();
            for (int x = Area.Min.x; x <= Area.Max.x; x++)
            {
                var p = new Pos<int>(x, Area.Min.y);
                if (p == End)
                {
                    result.Append('.');
                }
                else
                {
                    result.Append('#');
                }
            }
            return result.ToString();
        }
    }

    static internal Dictionary<char, Pos<int>> Directions = new()
    {
        { '>', new Pos<int>( 1,  0) },
        { '<', new Pos<int>(-1,  0) },
        { '^', new Pos<int>( 0, -1) },
        { 'v', new Pos<int>( 0,  1) },
    };

    static internal Dictionary<Pos<int>, char> DirectionChars = Directions.ToDictionary(x => x.Value, x => x.Key);

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        
        var valley = new Valley();

        {
            int y = 0;
            foreach (var line in input)
            {
                int x = 0;
                foreach (char c in line)
                {
                    var p = new Pos<int>(x, y);
                    if (Directions.TryGetValue(c, out var dir))
                    {
                        valley.Blizards.Add(new Blizard()
                        {
                            Pos = p,
                            Dir = dir,
                        });
                    }
                    else if (c == '#')
                    {
                        valley.Area.IncreaseToPoint(p);
                    }
                    x++;
                }
                y++;
            }
            valley.End = new(valley.Area.Max.x - 1, valley.Area.Max.y);
            valley.InnerArea.IncreaseToPoint(new(valley.Area.Max - Pos<int>.One));
        }

        for (int i = 0; i < 19; i++)
        {
            Console.WriteLine("Minute: " + i);
            Console.WriteLine(valley.ToString());
            Console.WriteLine();
            valley.Tick();
        }


        return "mmmop".ToString();
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
    public void Day24_Part1_Example01()
    {
        var input = """
            #.#####
            #.....#
            #>....#
            #.....#
            #...v.#
            #.....#
            #####.#
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day24_Part1_Example02()
    {
        var input = """
            #.######
            #>>.<^<#
            #.<..<<#
            #>v.><>#
            #<^v^^>#
            ######.#
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day24_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day24)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day24_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day24_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day24_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day24)));
        Assert.AreEqual("", result);
    }
    
}
