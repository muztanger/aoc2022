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
        public Pos<long> Pos = new(0, 0);
        public Box<long> Box => _box;
        public long Width => _box.Width;
        public long Height => _box.Height;
        public Box<long> SpriteArea => _box.Translate(Pos);
        private List<List<char>> Form => _form;

        Box<long> _box = new Box<long>(new Pos<long>(0, 0));
        readonly List<List<char>> _form = new List<List<char>>();

        public Rock() { }

        public Rock(Rock other)
        {
            Pos = new Pos<long>(other.Pos);
            _box = new Box<long>(other._box);
            _form = new List<List<char>>(other._form);
            Index = other.Index;
        }

        public char this[Pos<long> p]
        {
            get
            {
                var q = p - Pos;
                return _form[(int)q.y][(int)q.x];
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
            rock._box = new Box<long>(width, height);
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

        internal bool Contains(Pos<long> p)
        {
            return SpriteArea.Contains(p);
        }
    }

    public class Solids
    {
        private readonly List<List<char>> _solids = new();
        private readonly Box<long> _solidArea;
        private long Width { get; } = 7;
        private long Height => N;
        private static long N = 5000;
        private const int SolidsRows = 5000;

        public Solids(long N)
        {
            Solids.N = N;
            for (int y = 0; y < SolidsRows; y++)
            {
                _solids.Add(Enumerable.Repeat('.', (int)Width).ToList());
            }
            LastHighestSolid = N - 1;
            _solidArea = new Box<long>(Width, Height);
        }

        public char this[Pos<long>  p]
        {
            // if p.y >= _compressedBottomY return #
            get => _solids[(int)p.y][(int)p.x];
        }

        public bool Contains(Pos<long> p)
        {
            return _solidArea.Contains(p);
        }

        static long LastHighestSolid = N - 1;
        private long _compressedBottomY = N;
        private long _compressedTopY => (_compressedBottomY - SolidsRows);
        public long HighestSolid()
        {
            var highestSolidIndex = (int)(LastHighestSolid - _compressedTopY);
            for (var i = highestSolidIndex; i >= 0; i--)
            {
                if (i < _solids.Count && !_solids[i].Any(c => c != '.'))
                {
                    highestSolidIndex = i + 1;
                    LastHighestSolid = highestSolidIndex + _compressedBottomY;
                    break;
                }
            }

            if (highestSolidIndex <= SolidsRows / 2)
            {
                // Try to compress!
                Console.WriteLine($"Compress since highestSolidIndex={highestSolidIndex}");

                var nextBottomIndex = HighestRowIndex(new Pos<int>(0, highestSolidIndex - 1)) + 1;
                if (nextBottomIndex < SolidsRows)
                {
                    // compress!

                    for (int i = highestSolidIndex; i < nextBottomIndex; i++)
                    {
                        _solids[nextBottomIndex + i] = new List<char>(_solids[i]);
                        _solids[i] = Enumerable.Repeat('.', (int)Width).ToList();
                    }

                    //for (int i = highestSolidIndex; i < nextBottomIndex)
                
                    // new compressed bottom!

                }


                // N = 10
                // SolidsRows = 4
                // 
                // Y-pos|solidY
                // -----+------+    +-------+                                               +-------+
                //     0|      |    |·······|                                               |·······|
                //     1|      |    |·······|                                               |·······|
                //     2|      |    |·······|                                               |·······|
                //     3|     0|  3 |·······| _compressedTopY = 3                           |·······|
                //     4|     1|    |·······|                                               |·······| 
                //     5|     2|    |····#··| highestSolidIndex = 2                         |····#··| 0 highestSolidIndex = 0, compressedTopY = 5                       
                //     6|     3|    |#··####| LowestRowIndex() = 3                          |#··####| 1 LowestRowIndex = 1
                //     7|      |  7 |###·#··| nextBottomIndex = 4, _compressedBottomY = 7   |###·#··| 2 nextBottomIndex = 2
                //     8|      |    |··####·|                                               |··####·| 3
                //     9|      |    |·####··|                                               |·####··|   compressedBottomY = 9
                // -----+------+ 10 +-------+                                               +-------+

                int HighestRowIndex(Pos<int> pos)
                {
                    var directions = new List<Pos<int>> { new(1, 0), new(0, 1), new(-1, 0) };
                    int highestRowIndex = pos.y;
                    foreach (var dp in directions)
                    {
                        var next = pos + dp;
                        
                        if (next.x >= 0 && next.x < Width && next.y < SolidsRows && _solids[next.y][next.x] == '.')
                        {
                            var nextY = HighestRowIndex(next);
                            if (nextY is int rowIndex)
                            {
                                highestRowIndex = int.Max(highestRowIndex, rowIndex); // higher value => closer to bottom
                            }
                        }
                    }
                    return highestRowIndex;
                }
            }



            throw new NotImplementedException();
        }

        public void Add(Rock rock)
        {
            for (long y = rock.SpriteArea.Min.y; y <= rock.SpriteArea.Max.y; y++)
            {
                for (long x = rock.SpriteArea.Min.x; x <= rock.SpriteArea.Max.x; x++)
                {
                    var p = new Pos<long>(x, y);
                    if (rock[p] != '.')
                    {
                        _solids[(int)y][(int)x] = rock.Index.ToString()[0];
                    }
                }
            }
        }

        override public string ToString()
        {
            var result = new StringBuilder();
            var yStart = HighestSolid();
            for (long y = yStart; y < _solids.Count; y++)
            {
                if (result.Length > 0) result.AppendLine();
                result.Append(string.Concat(_solids[(int)y]));
            }
            return result.ToString();
        }

        internal bool IsInto(Rock rock)
        {
            var intersection = _solidArea.Intersection(rock.SpriteArea);
            if (intersection is null) return false;

            for (long y = intersection.Min.y; y <= intersection.Max.y; y++)
            {
                for (long x = intersection.Min.x; x <= intersection.Max.x; x++)
                {
                    var p = new Pos<long>(x, y);
                    if (rock[p] == '#' && _solids[(int)y][(int)x] != '.')
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
        rock1.Pos = new Pos<long>(2, 0);
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
        
        private const long N = 5000;
        private readonly string _jetStream;
        private readonly Box<long> _walls = new(7, N);
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
            long nextY = _solids.HighestSolid() - _sprite.Height - 3;
            _sprite.Pos = new Pos<long>(2, nextY);
            
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

            Pos<long> Direction() => _jetStream[_jetIndex] switch
            {
                '>' => new Pos<long>(1, 0),
                '<' => new Pos<long>(-1, 0),
                _ => new Pos<long>(0, 0)
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

                Pos<long> down = new Pos<long>(0, 1);
                
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
            for (long y = Math.Min(_sprite?.Pos.y ?? N - 1, _solids.HighestSolid()); y <= _walls.Max.y + 1; y++)
            {
                if (result.Length > 0)
                {
                    result.AppendLine();
                }
                if (y <= _walls.Max.y)
                {
                    result.Append('|');
                    for (long x = _walls.Min.x; x <= _walls.Max.x; x++)
                    {
                        var p = new Pos<long>(x, y);

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
                    for (long x = _walls.Min.x; x <= _walls.Max.x; x++)
                    {
                        result.Append('-');
                    }
                    result.Append('+');
                }
            }
            return result.ToString();
        }

        internal long HighestRock()
        {
            return N - _solids.HighestSolid();
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
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
        var y = chamber.HighestRock();
        return y.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        // N = 1000000000000... impossible.
        // Need to compact the solids and rembember the size.
        var chamber = new Chamber(input.First().Trim());
        chamber.NextRock();
        var expectedCount = 1000000000000;
        var stopCount = 0L;
        for (long i = 0; i < expectedCount * 10 && stopCount != expectedCount; i++)
        {
            chamber.Push();
            if (!chamber.Fall())
            {
                stopCount++;
                if (stopCount == 1000000000000)
                {
                    Console.WriteLine("Count: " + stopCount);
                    Console.WriteLine(chamber.ToString());
                }
            }
        }
        Assert.AreEqual(expectedCount, stopCount);
        var y = chamber.HighestRock();
        return y.ToString();
    }

    [TestMethod]
    public void Day17_Rock()
    {
        var rock = Rock.FromString("""
                .#.
                ###
                .#.
                """);
        Assert.AreEqual(new Box<long>(new Pos<long>(0, 0), new Pos<long>(3, 3)), rock.Box);
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
        Assert.AreEqual("3048", result);
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
