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
        public int Index {get; set;} = 0;
        public Pos<int> Pos = new(0, 0);
        public Box<int> Box => _box;
        public int Width => _box.Width;
        public int Height => _box.Height;
        public Box<int> SpriteArea => _box.Translate(Pos);
        private List<List<char>> Form => _form;

        Box<int> _box = new Box<int>(new Pos<int>(0, 0));
        readonly List<List<char>> _form = new List<List<char>>();

        public Rock() { }

        public Rock(Rock other)
        {
            Pos = new Pos<int>(other.Pos);
            _box = new Box<int>(other._box);
            _form = new List<List<char>>(other._form);
            Index = other.Index;
        }

        public char this[Pos<int> p]
        {
            get
            {
                var q = p - Pos;
                return _form[q.y][q.x];
            }
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
        private int Width { get; } = 7;
        private int Height => _solids.Count;
        private static int N = 5000;
        private Box<int> SolidArea => new Box<int>(Width, Height);

        public Solids(int N)
        {
            Solids.N = N;
            for (int y = 0; y < N; y++)
            {
                _solids.Add(Enumerable.Repeat('.', Width).ToList());
            }
            LastHighestSolid = N - 1;
        }

        public char this[Pos<int>  p]
        {
            get => _solids[p.y][p.x];
        }

        public bool Contains(Pos<int> p)
        {
            var box = new Box<int>(Width, Height);
            return box.Contains(p);
        }

        static int LastHighestSolid = N - 1;
        public int HighestSolid()
        {
            for (int i = LastHighestSolid; i >= 0; i--)
            {
                if (i < _solids.Count && !_solids[i].Any(c => c != '.'))
                {
                    LastHighestSolid = i + 1;
                    return i + 1;
                }
            }
            throw new NotImplementedException();
        }

        public void Add(Rock rock)
        {
            for (int y = rock.SpriteArea.Min.y; y <= rock.SpriteArea.Max.y; y++)
            {
                for (int x = rock.SpriteArea.Min.x; x <= rock.SpriteArea.Max.x; x++)
                {
                    var p = new Pos<int>(x, y);
                    if (rock[p] != '.')
                    {
                        _solids[y][x] = rock.Index.ToString()[0];
                    }
                }
            }
        }

        override public string ToString()
        {
            var result = new StringBuilder();
            var yStart = HighestSolid();
            for (int y = yStart; y < _solids.Count; y++)
            {
                if (result.Length > 0) result.AppendLine();
                result.Append(string.Concat(_solids[y]));
            }
            return result.ToString();
        }

        internal bool IsInto(Rock rock)
        {
            var intersection = SolidArea.Intersection(rock.SpriteArea);
            if (intersection is null) return false;

            for (int y = intersection.Min.y; y <= intersection.Max.y; y++)
            {
                for (int x = intersection.Min.x; x <= intersection.Max.x; x++)
                {
                    var p = new Pos<int>(x, y);
                    if (rock[p] == '#' && _solids[y][x] != '.')
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    [TestMethod]
    public void Day17_TestSolids()
    {
        var solids = new Solids(11);
        Rock rock1 = Rock.FromString("""
            ..#.
            ###.
            ..##
            """);
        rock1.Pos = new Pos<int>(2, 0);
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
        
        private const int N = 5000;
        private readonly string _jetStream;
        private readonly Box<int> _walls = new(7, N);
        private readonly Solids _solids;
        private int _rockIndex = -1;
        private int _jetIndex = -1;
        private Rock? _sprite;

        public Chamber(string jetStream)
        {
            _jetStream = jetStream.Trim();
            _solids = new Solids(N);
            for (int i = 0; i < _rockForms.Count; i++)
            {
                _rockForms[i].Index = i + 1;
            }
        }
        
        public void NextRock()
        {
            //Console.WriteLine("NextRock");
            _rockIndex = (_rockIndex + 1) % _rockForms.Count;
            // left edge is two units away from the left wall
            // its bottom edge is three units above the highest rock in the room (or the floor, if there isn't one).
            _sprite = new Rock(_rockForms[_rockIndex]);
            int nextY = _solids.HighestSolid() - _sprite.Height - 3;
            _sprite.Pos = new Pos<int>(2, nextY);
            
            _walls.IncreaseToPoint(_sprite.Pos);
        }

        public void Push()
        {
            Assert.IsNotNull(_sprite);

            //  being pushed by a jet of hot gas one unit (in the direction indicated by the next symbol in the jet pattern)
            // If any movement would cause any part of the rock to move into the walls, floor, or a stopped rock, the movement instead does not occur.
            _jetIndex = (_jetIndex + 1) % _jetStream.Length;
            //string msg = $"Push: {_jetStream[_jetIndex]}";
            //Console.WriteLine(msg);
            //System.Diagnostics.Debug.WriteLine(msg);

            _sprite.Pos += Direction();

            if (!_walls.Contains(_sprite.SpriteArea))
            {
                _sprite.Pos -= Direction(); // restore
                return;
            }
            if (_solids.IsInto(_sprite))
            {
                _sprite.Pos -= Direction(); // restore
                return;
            }

            Pos<int> Direction() => _jetStream[_jetIndex] switch
            {
                '>' => new Pos<int>(1, 0),
                '<' => new Pos<int>(-1, 0),
                _ => new Pos<int>(0, 0)
            };
        }

        public bool Fall()
        {
            //Console.WriteLine("Fall");
            if (_sprite is null) return true;
            
            // falling one unit down.
            // If any movement would cause any part of the rock to move into the walls, floor, or a stopped rock, the movement instead does not occur.
            // If a downward movement would have caused a falling rock to move into the floor or an already-fallen rock,
            // the falling rock stops where it is (having landed on something) and a new rock immediately begins falling.

            bool CanMove()
            {
                if (_sprite is null) return false;

                Pos<int> down = new Pos<int>(0, 1);
                
                _sprite.Pos += down;
                
                if (!_walls.Contains(_sprite.SpriteArea))
                {
                    _sprite.Pos -= down; // restore
                    return false;
                }

                if (_solids.IsInto(_sprite))
                {
                    _sprite.Pos -= down; // restore
                    return false;
                }

                return true;
            }

            if (!CanMove())
            {
                _solids.Add(_sprite);
                NextRock();
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            for (int y = Math.Min(_sprite?.Pos.y ?? N - 1, _solids.HighestSolid()); y <= _walls.Max.y + 1; y++)
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

                        if (_sprite is not null && _sprite.Contains(p) && _sprite[p] == '#')
                        {
                            result.Append(_sprite.Index);
                        }
                        else if (_solids.Contains(p) && _solids[p] != '.')
                        {
                            result.Append(_solids[p]);
                            //result.Append('■');
                        }
                        else
                        {
                            result.Append('·');
                        }
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

        internal int HighestRock()
        {
            return N - _solids.HighestSolid();
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var chamber = new Chamber(input.First().Trim());
        chamber.NextRock();
        var expectedCount = 2022;
        var stopCount = 0;
        for (int i = 0; i < 50000 && stopCount != expectedCount; i++)
        {
            chamber.Push();
            if (!chamber.Fall())
            {
                stopCount++;
                if (stopCount == 2022)
                {
                    Console.WriteLine("Count: " + stopCount);
                    Console.WriteLine(chamber.ToString());
                }
            }
        }
        Assert.AreEqual(expectedCount, stopCount);
        int y = chamber.HighestRock();
        return y.ToString();
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
    public void Day17_TestChamber_01()
    {
        var chamber = new Chamber(">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>");

        chamber.NextRock();
        Console.WriteLine(chamber.ToString());
        for (int i = 0; i < 100; i++)
        {
            chamber.Push();
            //Console.WriteLine(chamber.ToString());
            if(!chamber.Fall()) {
                Console.WriteLine(chamber.ToString());
            }
        }
        Assert.AreEqual("moop", chamber.ToString());
        var rock = Rock.FromString("""
                ..#
                ..#
                ###
                """);
    }


    [TestMethod]
    public void Day17_TestChamber_02()
    {
        var chamber = new Chamber("<");

        chamber.NextRock();
        Console.WriteLine(chamber.ToString());
        for (int i = 0; i < 100; i++)
        {
            chamber.Push();
            //Console.WriteLine(chamber.ToString());
            if (!chamber.Fall())
            {
                Console.WriteLine(chamber.ToString());
            }
        }
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
            >>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("3068", result);
    }
    
    [TestMethod]
    public void Day17_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day17)));
        Assert.AreNotEqual("3023", result); // too low
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
