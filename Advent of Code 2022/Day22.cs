using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace Advent_of_Code_2022;

enum Turn { None, Left, Right };

[TestClass]
public class Day22
{
    class Instruction
    {
        public int Count { get; set; } = 0;
        public Turn Turn { get; set; } = Turn.None;

        public override string ToString()
        {
            return $"({Turn} {Count})";
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var map = new List<List<char>>();
        var result = new StringBuilder();
        var isReadInstruction = false;
        var path = new List<Instruction>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                isReadInstruction = true;
            }
            else if (!isReadInstruction)
            {
               map.Add(line.ToList());
            }
            else if (isReadInstruction)
            {
                var digit = new StringBuilder();
                foreach (var c in line)
                {
                    if (char.IsLetter(c))
                    {
                        if (digit.Length > 0)
                        {
                            path.Add(new Instruction { Count = int.Parse(digit.ToString()) });
                            digit.Clear();
                        }

                        Assert.IsTrue(c == 'L' || c == 'R');
                        path.Add(new Instruction { Turn = c == 'L' ? Turn.Left : Turn.Right });
                    }
                    else if (char.IsDigit(c))
                    {
                        digit.Append(c);
                    }
                }
                if (digit.Length > 0)
                {
                    path.Add(new Instruction { Count = int.Parse(digit.ToString()) });
                    digit.Clear();
                }
                break; // finished
            }
        }

        int width = map.Max(m => m.Count);
        int height = map.Count;
        var mapSize = new Box<int>(new Pos<int>(0,0), new Pos<int>(width - 1, height - 1));
        foreach (var row in map)
        {
            while (row.Count < width)
            {
                row.Add(' ');
            }
        }

        var pos = new Pos<int>(0, 0);
        {
            var x = map[0].IndexOf('.');
            pos.x = x;
        }
        var dirs = new List<Pos<int>> { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
        var facing = 0;
        var dir = dirs[facing];

        Console.WriteLine("---------");
        foreach (var instruction in path)
        {
            Console.WriteLine($"Instruction={instruction} Pos={pos} dir={dir} facing={facing}");
            if (instruction.Turn == Turn.Right)
            {
                facing = (facing + 1) % dirs.Count;
                dir = dirs[facing];
            }
            else if (instruction.Turn == Turn.Left)
            {
                facing = (facing + dirs.Count - 1) % dirs.Count;
                dir = dirs[facing];
            }
            else
            {
                for (int i = 0; i < instruction.Count; i++)
                {
                    var next = pos + dir;
                    if (mapSize.IsInside(next))
                    {
                        var c = map[next.y][next.x];
                        if (c == '.')
                        {
                            pos = next;
                        }
                        else if (c == '#')
                        {
                            break;
                        }
                        else if (c == ' ')
                        {
                            while (c == ' ')
                            {
                                next += dir;
                                if (!mapSize.IsInside(next))
                                {
                                    next.x = (next.x + width) % width;
                                    next.y = (next.y + height) % height;
                                    Assert.IsTrue(mapSize.IsInside(next));
                                }
                                c = map[next.y][next.x];
                            }
                            if (c == '#')
                            {
                                break;
                            }
                            else if (c == '.')
                            {
                                pos = next;
                            }
                        }
                    }
                    else
                    {
                        //wrap and go!
                        next.x = (next.x + width) % width;
                        next.y = (next.y + height) % height;
                        var c = map[next.y][next.x];
                        while (c == ' ')
                        {
                            next += dir;
                            if (!mapSize.IsInside(next))
                            {
                                next.x = (next.x + width) % width;
                                next.y = (next.y + height) % height;
                                Assert.IsTrue(mapSize.IsInside(next));
                            }
                            c = map[next.y][next.x];
                        }
                        if (c == '#')
                        {
                            break;
                        }
                        else if (c == '.')
                        {
                            pos = next;
                        }
                    }
                }
            }
        }


        return ((pos.y + 1) * 1000 + 4 * (pos.x + 1) + facing).ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var map = new List<List<char>>();
        var result = new StringBuilder();
        var isReadInstruction = false;
        var path = new List<Instruction>();
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                isReadInstruction = true;
            }
            else if (!isReadInstruction)
            {
                map.Add(line.ToList());
            }
            else if (isReadInstruction)
            {
                var digit = new StringBuilder();
                foreach (var c in line)
                {
                    if (char.IsLetter(c))
                    {
                        if (digit.Length > 0)
                        {
                            path.Add(new Instruction { Count = int.Parse(digit.ToString()) });
                            digit.Clear();
                        }

                        Assert.IsTrue(c == 'L' || c == 'R');
                        path.Add(new Instruction { Turn = c == 'L' ? Turn.Left : Turn.Right });
                    }
                    else if (char.IsDigit(c))
                    {
                        digit.Append(c);
                    }
                }
                if (digit.Length > 0)
                {
                    path.Add(new Instruction { Count = int.Parse(digit.ToString()) });
                    digit.Clear();
                }
                break; // finished
            }
        }

        int width = map.Max(m => m.Count);
        int height = map.Count;
        var mapSize = new Box<int>(new Pos<int>(0, 0), new Pos<int>(width - 1, height - 1));
        foreach (var row in map)
        {
            while (row.Count < width)
            {
                row.Add(' ');
            }
        }

        // <=>
        // =>
        // <=
        // >=

        //    111222
        //    111222
        //    111222
        //    333
        //    333
        //    333
        // 444555
        // 444555
        // 444555
        // 666
        // 666
        // 666

        // ^1 => >6
        // <1 => >4
        // ^2 => 
        // >6 => ^5


        //var _ = """
        //            1111
        //            1111
        //            1111
        //            1111
        //    222233334444
        //    222233334444
        //    222233334444
        //    222233334444
        //            55556666
        //            55556666
        //            55556666
        //            55556666
        //    """;

        // Above 1
        // CCW Rot 90
        // dp (-4, -4)


        // Left of 1
        // Right of 1
        // Above 2
        // Left of 2
        // Below 2
        // Above 3
        // Below 3
        // Right of 4
        // Left of 5
        // Below 5
        // Above 6
        // Right of 6
        // Below 6

        var pos = new Pos<int>(0, 0);
        {
            var x = map[0].IndexOf('.');
            pos.x = x;
        }
        var dirs = new List<Pos<int>> { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
        var facing = 0;
        var dir = dirs[facing];

        Console.WriteLine("---------");
        foreach (var instruction in path)
        {
            Console.WriteLine($"Instruction={instruction} Pos={pos} dir={dir} facing={facing}");
            if (instruction.Turn == Turn.Right)
            {
                facing = (facing + 1) % dirs.Count;
                dir = dirs[facing];
            }
            else if (instruction.Turn == Turn.Left)
            {
                facing = (facing + dirs.Count - 1) % dirs.Count;
                dir = dirs[facing];
            }
            else
            {
                for (int i = 0; i < instruction.Count; i++)
                {
                    var next = pos + dir;
                    if (mapSize.IsInside(next))
                    {
                        var c = map[next.y][next.x];
                        if (c == '.')
                        {
                            pos = next;
                        }
                        else if (c == '#')
                        {
                            break;
                        }
                        else if (c == ' ')
                        {
                            while (c == ' ')
                            {
                                next += dir;
                                if (!mapSize.IsInside(next))
                                {
                                    next.x = (next.x + width) % width;
                                    next.y = (next.y + height) % height;
                                    Assert.IsTrue(mapSize.IsInside(next));
                                }
                                c = map[next.y][next.x];
                            }
                            if (c == '#')
                            {
                                break;
                            }
                            else if (c == '.')
                            {
                                pos = next;
                            }
                        }
                    }
                    else
                    {
                        //wrap and go!
                        next.x = (next.x + width) % width;
                        next.y = (next.y + height) % height;
                        var c = map[next.y][next.x];
                        while (c == ' ')
                        {
                            next += dir;
                            if (!mapSize.IsInside(next))
                            {
                                next.x = (next.x + width) % width;
                                next.y = (next.y + height) % height;
                                Assert.IsTrue(mapSize.IsInside(next));
                            }
                            c = map[next.y][next.x];
                        }
                        if (c == '#')
                        {
                            break;
                        }
                        else if (c == '.')
                        {
                            pos = next;
                        }
                    }
                }
            }
        }


        return ((pos.y + 1) * 1000 + 4 * (pos.x + 1) + facing).ToString();
    }

    public class AffineMatrix<T>
        where T : INumber<T>
    {
        private List<T> _rows = new List<T>();
        internal Pos<T> Mult(Pos<T> other)
        {
            //T x = Row1.x * other.x + Row2.x * other.y;
            //T y = Row2.y * other.x * Row2.y * other.y;
            //return new(x, y);
            return new(T.Zero, T.Zero);
        }
    }

    [TestMethod]
    public void TestRotation()
    {

        int y = 0;
        for (int x = 0; x < 3; x++)
        {

        }

    }


    [TestMethod]
    public void Day22_Part1_Example01()
    {
        var input = """
                    ...#
                    .#..
                    #...
                    ....
            ...#.......#
            ........#...
            ..#....#....
            ..........#.
                    ...#....
                    .....#..
                    .#......
                    ......#.

            10R5L5R10L4R5L5
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("6032", result);
    }
    
    [TestMethod]
    public void Day22_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day22)));
        Assert.AreEqual("29408", result);
    }
    
    [TestMethod]
    public void Day22_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day22_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day22_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day22)));
        Assert.AreEqual("", result);
    }
    
}
