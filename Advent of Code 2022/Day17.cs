using Advent_of_Code_2022.Commons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
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

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Solids
    {
        static long LastHighestSolid = N - 1;

        private long Width { get; } = 7;
        private long Height => N;
        private long _compressedTopY => _compressedBottomY - SolidsRows;

        private readonly List<List<char>> _solids = new();
        private readonly Box<long> _solidArea;
        private static long N = 2500000000000L;
        private const int SolidsRows = 500; // 5000
        private long _compressedBottomY = N;
        private int _rockCount = 0;

        public Solids(long N)
        {
            Solids.N = N;
            for (int y = 0; y < SolidsRows; y++)
            {
                _solids.Add(Enumerable.Repeat('.', (int)Width).ToList());
            }
            LastHighestSolid = N;
            _solidArea = new Box<long>(Width, Height);
        }

        public char this[Pos<long>  p]
        {
            get
            {
                if (p.y < _compressedTopY || p.y >= _compressedBottomY) throw new ArgumentException(p.ToString(), nameof(p));
                int x = (int)p.x;
                int y = (int)(p.y - _compressedTopY);
                return _solids[y][x];
            }
        }

        public bool Contains(Pos<long> p)
        {
            return _solidArea.Contains(p) && p.y < _compressedBottomY && p.y >= _compressedTopY;
        }

        static List<(int, long, int, long)> rows = new();
        public bool TryCompress()
        {
            var isCompressed = false;

            var highestSolidIndex = (int)(HighestSolid() - _compressedTopY);
            if (highestSolidIndex <= SolidsRows / 2)
            {
                isCompressed = true;
                // Try to compress!
                //Console.WriteLine($"Compress since highestSolidIndex={highestSolidIndex}");

                var nextBottomIndex = HighestRowIndex(new Pos<int>(0, highestSolidIndex - 1)) + 1;
                //Console.WriteLine($"   nextBottomIndex={nextBottomIndex}");
                if (nextBottomIndex < SolidsRows)
                {
                    for (int i = nextBottomIndex; i < SolidsRows; i++)
                    {
                        var x = Convert.ToInt32(string.Concat(_solids[i].Select(x => x != '.' ? '1' : '0')), 2);
                        rows.Add((x, _compressedTopY + i, _rockCount, _compressedTopY + highestSolidIndex));
                    }

                    var diff = SolidsRows - nextBottomIndex;
                    int j = SolidsRows - 1;
                    var from = j - diff;
                    for (; j >= 0 && from != highestSolidIndex; j--)
                    {
                        from = j - diff;
                        _solids[j] = _solids[from];
                    }
                    for (; j >= highestSolidIndex; j--)
                    {
                        _solids[j] = Enumerable.Repeat('.', (int)Width).ToList();
                    }

                    // new compressed bottom!
                    _compressedBottomY -= diff;

                    Assert.IsTrue(_compressedTopY >= 0);
                }
                //Console.WriteLine($"   _compressedBottomY={_compressedBottomY}");


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
                    //Console.WriteLine($"HighestRowIndex: Start at {pos}");
                    
                    var directions = new List<Pos<int>> { new(1, 0), new(0, 1), new(-1, 0) };
                    directions.Reverse(); // since we are using a stack
                    
                    var stack = new Stack<Pos<int>>();
                    var visited = new HashSet<Pos<int>>();
                    int highestRowIndex = pos.y;
                    stack.Push(pos);
                    while (stack.TryPeek(out _))
                    {
                        var p = stack.Pop();
                        //Console.Write($"\t{p}");
                        highestRowIndex = int.Max(highestRowIndex, p.y);
                        visited.Add(p);

                        foreach (var dp in directions)
                        {
                            var next = p + dp;
                            if (!visited.Contains(next)
                                && next.x >= 0 
                                && next.x < Width 
                                && next.y < SolidsRows
                                && _solids[next.y][next.x] == '.')
                            {
                                stack.Push(next);
                            }
                        }
                    }
                    //Console.WriteLine();
                    //Console.WriteLine($" End at: {highestRowIndex}");
                    return highestRowIndex;
                }
            }

            return isCompressed;
        }

        public long HighestSolid()
        {
            var highestSolidIndex = (int)(LastHighestSolid - _compressedTopY);
            for (var i = highestSolidIndex; i >= 0; i--)
            {
                if (i < _solids.Count && !_solids[i].Any(c => c != '.'))
                {
                    highestSolidIndex = i + 1;
                    LastHighestSolid = highestSolidIndex + _compressedTopY;
                    break;
                }
            }

            return LastHighestSolid;
        }

        public void Add(Rock rock)
        {
            _rockCount++;
            for (long y = rock.SpriteArea.Min.y; y <= rock.SpriteArea.Max.y; y++)
            {
                for (long x = rock.SpriteArea.Min.x; x <= rock.SpriteArea.Max.x; x++)
                {
                    var p = new Pos<long>(x, y);
                    if (rock[p] != '.')
                    {
                        _solids[(int)(y - _compressedTopY)][(int)x] = rock.Index.ToString()[0];
                    }
                }
            }
        }

        override public string ToString()
        {
            var result = new StringBuilder();
            var yStart = (int)(HighestSolid() - _compressedTopY);
            Assert.IsTrue(yStart >= 0);
            for (int y = yStart; y < _solids.Count; y++)
            {
                if (result.Length > 0) result.AppendLine();
                result.Append(string.Concat(_solids[(int)y]));
            }
            return result.ToString();
        }

        private string DebuggerDisplay
        {
            get
            {
                var result = new StringBuilder();
                var yStart = _solids.Count - 20;
                Assert.IsTrue(yStart >= 0);
                for (int y = yStart; y < _solids.Count; y++)
                {
                    if (result.Length > 0) result.AppendLine();
                    result.Append(string.Concat(_solids[(int)y]));
                }
                return result.ToString();
            }
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
                    //Console.WriteLine(intersection);
                    if (rock[p] == '#' && _solids[(int)(y - _compressedTopY)][(int)x] != '.')
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
                               
        private const long N = 2500000000000L;
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
                var isCompressed = _solids.TryCompress();
                NextRock();
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            //TODO think about compressed range of solids
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
        //var expectedCount = 2668;
        var expectedCount = 2022;
        var stopCount = 0;
        for (int i = 0; i < 50000 && stopCount != expectedCount; i++)
        {
            chamber.Push();
            if (!chamber.Fall())
            {
                stopCount++;
                if (stopCount == expectedCount)
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
        var expectedCount = 1000000000000L;
        var stopCount = 0L;
        for (long i = 0; i < expectedCount * 10 && stopCount != expectedCount; i++)
        {
            chamber.Push();
            if (!chamber.Fall())
            {
                stopCount++;
                if (stopCount == 1000000000000L)
                {
                    Console.WriteLine("Count: " + stopCount);
                    //Console.WriteLine(chamber.ToString());
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
            >>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day17_Part2()
    {
        var result = "1504093567249";
        Assert.AreNotEqual("1504093568003", result); // Too high, guessed by analyzing rows converted to numbers in excel...
        Assert.AreNotEqual("1504093567251", result); // Too high!... also guessed with excel...
        //                  1504093567249 <== Correct Answer!!
        Assert.AreNotEqual("1504093566129", result); // Not the right answer 6:05
        Assert.AreNotEqual("1504093565946", result); // Too low!

        // After 2668 rocks, the pattern repeats every 1570 rocks. At 2668 rocks the bottom i.e. repeated pattern bottom, is at 3788 and top at 4026
        // Every 1570 rocks after 2668 rocks the top increases 2572 rows (also checked seen if doing autocorrelation)
        // Repeat the pattern 584795320 times (999999997200 rocks)
        // Still need to drop 132 rocks to come up to 1000000000000
        // Run part one with 2668 + 132 = 2800 rocks, top now at 4209
        // 3788 + 584795320 * 2572 + 238 + 183 = 1504093567249

        Assert.AreEqual("1504093567249", result);
    }
    
}
