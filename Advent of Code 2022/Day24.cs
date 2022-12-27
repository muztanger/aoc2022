namespace Advent_of_Code_2022;

[TestClass]
public class Day24
{
    class Blizard
    {
        internal Pos<int> Pos { get; set; } = new(0, 0);
        internal Pos<int> Dir { get; set; } = new(0, 0);
        internal Pos<int> Next { get; set; } = new(0, 0);
    }

    class Valley
    {
        internal Box<int> Area { get; set; } = new(new Pos<int>(0, 0));
        internal Box<int> InnerArea { get; set; } = new(new Pos<int>(1, 1));

        internal static Pos<int> Start { get; } = new(1, 0);
        internal Pos<int> End { get; set; } = new(0, 1);
        internal List<Blizard> Blizards { get; set; } = new List<Blizard>();

        internal void ForwardTick()
        {
            Step(1);
        }

        internal void BackwardTick()
        {
            Step(-1);
        }

        private void Step(int direction)
        {
            foreach (var blizard in Blizards)
            {
                var next = blizard.Pos + blizard.Dir * Math.Sign(direction);
                if (InnerArea.Contains(next))
                {
                    blizard.Next = next;
                }
                else
                {
                    var q = next - InnerArea.Min; // move for modulus operation
                    q = (q + InnerArea.Size) % InnerArea.Size;
                    blizard.Next = q + InnerArea.Min;
                }
            }
        }

        internal void Tock()
        {
            foreach (var blizard in Blizards)
            {
                blizard.Pos = blizard.Next;
            }
        }

        public string ToString(Pos<int> expidition)
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
                    if (p == expidition)
                    {
                        result.Append('E');
                    }
                    else
                    {
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
                }
                result.Append('#');
            }
            result.AppendLine();
            for (int x = Area.Min.x; x <= Area.Max.x; x++)
            {
                var p = new Pos<int>(x, Area.Max.y);
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

        internal static Valley FromInput(IEnumerable<string> input)
        {
            var valley = new Valley();

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

            return valley;
        }

    }

    static internal List<char> Order = new List<char> { '>', 'v', 'O', '<', '^'};

    static internal Dictionary<char, Pos<int>> Directions = new()
    {
        { '>', new Pos<int>( 1,  0) },
        { '<', new Pos<int>(-1,  0) },
        { '^', new Pos<int>( 0, -1) },
        { 'v', new Pos<int>( 0,  1) },
        { 'O', new Pos<int>( 0,  0) },
    };

    static internal Dictionary<Pos<int>, char> DirectionChars = Directions.ToDictionary(x => x.Value, x => x.Key);

    //TODO! Something tree... in combination with someting move forward and backward in time and keep track of options and what has been tested... something detect loops and something remember dead ends.. 😉
    public class Option
    {
        public int Minute = 0;
        public Pos<int> Expidition = new Pos<int>(0, 0);
        public Option? Parent = null;
        public List<Option> Options = new();

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is not Option optionObj)
                return false;
            else
                return Equals(optionObj);
        }

        public bool Equals(Option? other)
        {
            return other is not null &&
                   Expidition == other.Expidition &&
                   Minute == other.Minute;
        }

        public override int GetHashCode()
        {
            return Minute.GetHashCode() * 5009 + Expidition.GetHashCode();
        }

    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var valley = Valley.FromInput(input);

        var expidition = new Pos<int>(Valley.Start);
        var isFinished = false;
        var root = new Option
        {
            Expidition = new Pos<int>(expidition),
            Minute = 0,
        };
        var current = root;
        for (int i = 0; i < 19 && !isFinished; i++)
        {
            Console.WriteLine("Minute: " + i);
            Console.WriteLine(valley.ToString(expidition));
            Console.WriteLine();
            valley.ForwardTick();
            
            var options = new List<Pos<int>>();
            foreach (var dir in Order.Select(c => Directions[c]))
            {
                var next = expidition + dir;
                if (next == valley.End)
                {
                    //WOHOO! how many steps did this take??
                    options = new List<Pos<int>> { valley.End };
                    isFinished = true;
                    break;
                }
                else if (valley.InnerArea.Contains(next) && !valley.Blizards.Any(b => b.Next == next))
                {
                    options.Add(next);
                }
            }
            if (options.Count == 0)
            {
                Console.WriteLine($"Expidition {expidition} is stuck!");
                isFinished = true;
                break;
            }
            else
            {
                Console.WriteLine($"Expidition at {expidition} options={string.Join(",", options)} choosing={options[0]}");
                expidition = options[0];
            }
            valley.Tock();
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
