using Advent_of_Code_2022.Commons;

namespace Advent_of_Code_2022;

[TestClass]
public class Day17
{
    public class Rock
    {
        public Box<int> Box => _box;
        public int Width => _box.Width;
        public int Height => _box.Height;

        Box<int> _box = new Box<int>(new Pos<int>(0, 0));
        List<List<char>> _form = new List<List<char>>();

        public char this[Pos<int> p]
        {
            get {
                Assert.IsTrue(_box.IsInside(p));
                return _form[p.y][p.x];
            }
            set { _form[p.y][p.x] = value; }
        }

        public static Rock FromString(string str)
        {
            Rock rock = new Rock();
            foreach (var line in Common.GetLines(str))
            {
                var row = new List<char>();
                foreach (var c in line)
                {
                    row.Add(c);
                }
                rock._form.Add(row);
            }
            int width = rock._form[0].Count;
            int height = rock._form.Count;
            rock._box.IncreaseToPoint(new Pos<int>(width, height));
            return rock;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            foreach (var line in _form)
            {
                if (result.Length> 0)
                {
                    result.AppendLine();
                }
                foreach (var c in line)
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }

    public class SolidRock
    {
        private List<List<char>> _stoppedRocks = new List<List<char>>();
        
        public void Paint(Box<int> walls, Rock rock, Pos<int> pos)
        {
            Assert.IsTrue(pos.y <= 0);
            while (_stoppedRocks.Count < walls.Height)
            {
                _stoppedRocks.Add(Enumerable.Repeat('.', rock.Width).ToList());
            }
            for (int y = rock.Box.Min.y; y <= rock.Box.Max.y; y++)
            {
                for (int x = rock.Box.Min.x; x <= rock.Box.Max.x; x++)
                {
                    var p1 = new Pos<int>(x, y);
                    var p2 = new Pos<int>(x + pos.x, rock.Height - y - pos.y);
                    _stoppedRocks[p2.y][p2.x] = rock[p1];
                }
            }
        }
    }

    public class Chamber
    {
        private readonly static List<Rock> _rockForms = new List<Rock>
        {
            Rock.FromString("""
                ####
                """),
            Rock.FromString("""
                .#.
                ###
                .#.
                """),
            Rock.FromString("""
                ..#
                ..#
                ###
                """),
            Rock.FromString("""
                #
                #
                #
                #
                """),
            Rock.FromString("""
                ##
                ##
                """),
        };
        
        private readonly string _jetStream;
        private int _rockIndex = 0;
        private Box<int> _walls = new(new Pos<int>(0,0), new Pos<int>(7,0));
        private SolidRock _solidRock = new SolidRock();

        public Chamber(string jetStream)
        {
            _jetStream = jetStream;
            var start = new Pos<int>(2, -3);
            _walls.IncreaseToPoint(start);
        }
        
        private void NextRock()
        {
            _rockIndex = (_rockIndex + 1) % _rockForms.Count;
            // left edge is two units away from the left wall
            // its bottom edge is three units above the highest rock in the room (or the floor, if there isn't one).
        }

        public void Push()
        {
            //  being pushed by a jet of hot gas one unit (in the direction indicated by the next symbol in the jet pattern)
            // If any movement would cause any part of the rock to move into the walls, floor, or a stopped rock, the movement instead does not occur.
        }

        public void Fall()
        {
            // falling one unit down.
            // If any movement would cause any part of the rock to move into the walls, floor, or a stopped rock, the movement instead does not occur.
            // If a downward movement would have caused a falling rock to move into the floor or an already-fallen rock, the falling rock stops where it is (having landed on something) and a new rock immediately begins falling.
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int y = _walls.Min.y; y <= _walls.Max.y + 1; y++)
            {
                if (result.Length > 0)
                {
                    result.AppendLine();
                }
                if (y <= _walls.Max.y)
                {
                    result.Append('|');
                    for (int x = _walls.Min.x; x <= _walls.Max.x; x++)
                    {
                        // check sprite
                        // check grid
                        result.Append('.');
                    }
                    result.Append('|');
                }
                else
                {
                    result.Append('+');
                    for (int x = _walls.Min.x; x <= _walls.Max.x; x++)
                    {
                        result.Append('-');
                    }
                    result.Append('+');
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
    public void Day17_Rock()
    {
        var rock = Rock.FromString("""
                .#.
                ###
                .#.
                """);
        Assert.AreEqual(new Box<int>(new Pos<int>(0, 0), new Pos<int>(3, 3)), rock.Box);
        Assert.AreEqual("""
                .#.
                ###
                .#.
                """, rock.ToString());

        
    }

    [TestMethod]
    public void Day17_Chamber()
    {
        var chamber = new Chamber(">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>");
        Assert.AreEqual("moop", chamber.ToString());
    }

    [TestMethod]
    public void Day17_Part1_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day17)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day17)));
        Assert.AreEqual("", result);
    }
    
}
