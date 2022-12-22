using System.ComponentModel.DataAnnotations;
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

        int width = map[0].Count;
        int height = map.Count;
        var mapSize = new Box<int>(new Pos<int>(0,0), new Pos<int>(width, height));

        var pos = new Pos<int>(0, 0);
        {
            var x = map[0].IndexOf('.');
            pos.x = x;
        }
        var dirs = new List<Pos<int>> { new(1, 0), new(0, -1), new(-1, 0), new(0, -1) };
        var facing = 0;
        var dir = dirs[facing];

        foreach (var instruction in path)
        {
            if (instruction.Turn == Turn.Right)
            {
                facing = (facing + 1) % dirs.Count;
            }
            else if (instruction.Turn == Turn.Left)
            {
                facing = (facing + dirs.Count - 1) % dirs.Count;
            }
            else
            {
                for (int i = 0; i < instruction.Count; i++)
                {
                    var next = pos + dir;
                    if (mapSize.IsInside(next))
                    {
                        var c = map[pos.y][pos.x];
                        if (c == '.')
                        {
                            pos = next;
                        }
                        else if (c == '#')
                        {
                            // just stay
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
                                c = map[pos.y][pos.x];
                            }
                            if (c == '#')
                            {
                                // just stay
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
                    }
                }
            }
        }


        return (pos.y * 1000 + 4 * pos.x + facing).ToString();
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
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day22_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day22_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day22)));
        Assert.AreEqual("", result);
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
