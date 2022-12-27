using Advent_of_Code_2022.Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Advent_of_Code_2022.Day17;

namespace Advent_of_Code_2022;

[TestClass]
public class Day17
{
    public class Rock
    {
        public Pos<int> Pos = new(0, 0);
        public Box<int> Box => _box;
        public int Width => _box.Width;
        public int Height => _box.Height;
        public Box<int> SpriteArea => _box.Translate(Pos);

        Box<int> _box = new Box<int>(new Pos<int>(0, 0));
        readonly List<List<char>> _form = new List<List<char>>();

        public Rock() { }

        public Rock(Rock other)
        {
            Pos = new Pos<int>(other.Pos);
            _box = new Box<int>(other._box);
            _form = new List<List<char>>(other._form);
        }

        public char this[Pos<int> p]
        {
            get {
                Assert.IsTrue(_box.Contains(p), $"{p} is not inside box={_box}");
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
            rock._box = new Box<int>(width, height);
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

        internal bool Contains(Pos<int> p)
        {
            return SpriteArea.Contains(p);
        }
    }

    public class Solids
    {
        private readonly List<List<char>> _solids = new();
        private int Width { get; init; } = 7;
        private int Height => _solids.Count;

        public Solids(Box<int> init)
        {
            Width = init.Width;
            for (int y = 0; y < init.Height; y++)
            {
                _solids.Add(Enumerable.Repeat('.', Width).ToList());
            }
        }
        public char this[Pos<int> p]
        {
            get
            {
                Assert.IsTrue(Contains(p));
                return _solids[-p.y][p.x];
            }
        }

        public bool Contains(Pos<int> p)
        {
            var box = new Box<int>(Width, Height).Translate(new Pos<int>(0, -Height + 1));
            return box.Contains(p);
        }

        public int HighestSolid()
        {
            int y = _solids.Count - 1;
            while (y >= 0)
            {
                if (_solids[y].Any(c => c == '#'))
                {
                    return y;
                }
                y--;
            }
            return 0;
        }

        public void Add(Rock rock)
        {
            Assert.IsTrue(rock.Pos.y <= 0);
            while (_solids.Count < Math.Abs(rock.Pos.y) + 1)
            {
                _solids.Add(Enumerable.Repeat('.', Width).ToList());
            }
            var rockPos = new Pos<int>(rock.Pos.x, -rock.Pos.y); // inverted y-axis
            for (int y = rock.Box.Min.y; y <= rock.Box.Max.y; y++)
            {
                for (int x = rock.Box.Min.x; x <= rock.Box.Max.x; x++)
                {
                    var p1 = new Pos<int>(x, y);
                    var p2 = new Pos<int>(x + rockPos.x, rockPos.y - y);
                    _solids[p2.y][p2.x] = rock[p1];
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int y = _solids.Count - 1; y >= 0; y--)
            {
                if (result.Length > 0) result.AppendLine();
                result.Append(string.Concat(_solids[y]));
            }
            return result.ToString();
        }
    }

    [TestMethod]
    public void Day17_TestSolids()
    {
        var solids = new Solids(new Box<int>(7, 1));
        Rock rock1 = Rock.FromString("""
            ..#.
            ###.
            ..##
            """);
        rock1.Pos = new Pos<int>(2, -10);
        solids.Add(rock1);
        Assert.AreEqual("""
            ....#..
            ..###..
            ....##.
            .......
            .......
            .......
            .......
            .......
            .......
            .......
            .......
            """, solids.ToString());
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
        private readonly Box<int> _walls = new(7, 3);
        private readonly Solids _solids;
        private int _rockIndex = -1;
        private int _jetIndex = -1;
        private Rock? _sprite;

        public Chamber(string jetStream)
        {
            _jetStream = jetStream;
            var start = new Pos<int>(2, -3);
            _walls.IncreaseToPoint(start);
            _solids = new Solids(_walls);
        }
        
        public void NextRock()
        {
            Console.WriteLine("NextRock");
            _rockIndex = (_rockIndex + 1) % _rockForms.Count;
            // left edge is two units away from the left wall
            // its bottom edge is three units above the highest rock in the room (or the floor, if there isn't one).
            _sprite = new Rock(_rockForms[_rockIndex]);
            _sprite.Pos = new Pos<int>(2,  -_solids.HighestSolid() - (_sprite.Height - 1) - 3);
            
            _walls.IncreaseToPoint(_sprite.Pos);
        }

        public void Push()
        {
            Console.WriteLine("Push");
            Assert.IsNotNull(_sprite);

            //  being pushed by a jet of hot gas one unit (in the direction indicated by the next symbol in the jet pattern)
            // If any movement would cause any part of the rock to move into the walls, floor, or a stopped rock, the movement instead does not occur.
            _jetIndex = (_jetIndex + 1) % _jetStream.Length;

            _sprite.Pos += Direction();

            if (!_walls.Contains(_sprite.SpriteArea))
            {
                _sprite.Pos -= Direction(); // restore
                return;
            }

            Box<int> overlap = _walls.Intersection(_sprite.SpriteArea);

            Pos<int> Direction() => _jetStream[_jetIndex] switch
            {
                '>' => new Pos<int>(1, 0),
                '<' => new Pos<int>(0, 1),
                _ => new Pos<int>(0, 0)
            };
        }

        public void Fall()
        {
            Console.WriteLine("Fall");
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
                        var p = new Pos<int>(x, y);

                        var isSquare = false;
                        if (_sprite is not null && _sprite.Contains(p))
                        {
                            var q = p - _sprite.SpriteArea.Min;
                            if (_sprite[q] == '#')
                            {
                                isSquare = true;
                            }
                        }
                        else if (_solids.Contains(p) && _solids[p] == '#')
                        {
                            isSquare = true;
                        }
                        result.Append(isSquare ? '■' : '·');
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

        chamber.NextRock();
        Console.WriteLine(chamber.ToString());
        chamber.Push();
        Console.WriteLine(chamber.ToString());
        chamber.Fall();
        Console.WriteLine(chamber.ToString());
        chamber.NextRock();
        Console.WriteLine(chamber.ToString());
        chamber.NextRock();
        Assert.AreEqual("moop", chamber.ToString());
        var rock = Rock.FromString("""
                ..#
                ..#
                ###
                """);

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
