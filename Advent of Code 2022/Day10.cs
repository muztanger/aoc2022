namespace Advent_of_Code_2022;

enum Command { Addx, Noop };

[TestClass]
public class Day10
{
    public class Crt
    {
        const int Width = 40;
        const int Height = 6;
        readonly List<int> _screen = Enumerable.Repeat(0, Width * Height).ToList();

        public void Draw(Cpu cpu)
        {
            int cycleIndex = (int)(cpu.Cycle - 1);
            int i =  cycleIndex % Width;
            int pos = (int) cpu.X;
            if (i >= pos - 1  && i <= pos + 1)
            {
                _screen[cycleIndex] = 1;
            }
        }

        override public string ToString()
        {
            var result = new StringBuilder();
            for (int j = 0; j < Height; j++)
            {
                var line = new StringBuilder();
                for (int i = 0; i < Width; i++)
                {
                    if (_screen[j * Width + i] > 0)
                    {
                        line.Append('#');
                    }
                    else
                    {
                        line.Append('.');
                    }
                }
                result.AppendLine(line.ToString());
            }
            return result.ToString();
        }
    }

    public class Cpu
    {
        public long Cycle { get; private set; } = 0L;
        public long X { get; private set; } = 1L;
        public long SignalStrength => Cycle * X;

        private List<(Command, long)> _program = new();
        private int _cyclesLeft = 0;
        private int _programPointer = -1;

        public bool StartCycle()
        {
            if (_cyclesLeft == 0)
            {
                _programPointer++;
                if (_programPointer >= _program.Count)
                {
                    return false;
                }
                if (_program[_programPointer].Item1 == Command.Addx)
                {
                    _cyclesLeft = 1;
                }
                else if (_program[_programPointer].Item1 == Command.Noop)
                {
                    _cyclesLeft = 0;
                }
            }
            else
            {
                _cyclesLeft--;
            }
            Cycle++;
            return true;
        }

        public void EndCycle()
        {
            if (_program[_programPointer].Item1 == Command.Addx && _cyclesLeft == 0)
            {
                X += _program[_programPointer].Item2;
            }
        }

        public static Cpu Load(IEnumerable<string> input)
        {
            var cpu = new Cpu();
            foreach (var line in input)
            {
                var split = line.Split();
                if (split.Length == 2)
                {
                    cpu._program.Add((Command.Addx, long.Parse(split[1])));
                }
                else
                {
                    cpu._program.Add((Command.Noop, long.MinValue));
                }
            }
            return cpu;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var checks = new List<long> { 20, 60, 100, 140, 180, 220 };
        var signals = new List<long>();
        var cpu = Cpu.Load(input);
        while (cpu.StartCycle())
        {
            if (checks.Contains(cpu.Cycle))
            {
                signals.Add(cpu.SignalStrength);
                if (signals.Count == checks.Count)
                {
                    break;
                }
            }
            cpu.EndCycle();
        }
        return signals.Sum().ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var crt = new Crt();
        var cpu = Cpu.Load(input);
        while (cpu.StartCycle())
        {
            crt.Draw(cpu);
            cpu.EndCycle();
        }
        return crt.ToString();
    }


    [TestMethod]
    public void Day10_Part1_Example01()
    {
        var input = """
            noop
            addx 3
            addx -5
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("0", result); // Not really a test...
    }
    
    [TestMethod]
    public void Day10_Part1_Example02()
    {
        var input = """
            addx 15
            addx -11
            addx 6
            addx -3
            addx 5
            addx -1
            addx -8
            addx 13
            addx 4
            noop
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx -35
            addx 1
            addx 24
            addx -19
            addx 1
            addx 16
            addx -11
            noop
            noop
            addx 21
            addx -15
            noop
            noop
            addx -3
            addx 9
            addx 1
            addx -3
            addx 8
            addx 1
            addx 5
            noop
            noop
            noop
            noop
            noop
            addx -36
            noop
            addx 1
            addx 7
            noop
            noop
            noop
            addx 2
            addx 6
            noop
            noop
            noop
            noop
            noop
            addx 1
            noop
            noop
            addx 7
            addx 1
            noop
            addx -13
            addx 13
            addx 7
            noop
            addx 1
            addx -33
            noop
            noop
            noop
            addx 2
            noop
            noop
            noop
            addx 8
            noop
            addx -1
            addx 2
            addx 1
            noop
            addx 17
            addx -9
            addx 1
            addx 1
            addx -3
            addx 11
            noop
            noop
            addx 1
            noop
            addx 1
            noop
            noop
            addx -13
            addx -19
            addx 1
            addx 3
            addx 26
            addx -30
            addx 12
            addx -1
            addx 3
            addx 1
            noop
            noop
            noop
            addx -9
            addx 18
            addx 1
            addx 2
            noop
            noop
            addx 9
            noop
            noop
            noop
            addx -1
            addx 2
            addx -37
            addx 1
            addx 3
            noop
            addx 15
            addx -21
            addx 22
            addx -6
            addx 1
            noop
            addx 2
            addx 1
            noop
            addx -10
            noop
            noop
            addx 20
            addx 1
            addx 2
            addx 2
            addx -6
            addx -11
            noop
            noop
            noop
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("13140", result);
    }
    
    [TestMethod]
    public void Day10_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day10)));
        Assert.AreEqual("14340", result);
    }
    
    [TestMethod]
    public void Day10_Part2_Example01()
    {
        var input = """
            addx 15
            addx -11
            addx 6
            addx -3
            addx 5
            addx -1
            addx -8
            addx 13
            addx 4
            noop
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx 5
            addx -1
            addx -35
            addx 1
            addx 24
            addx -19
            addx 1
            addx 16
            addx -11
            noop
            noop
            addx 21
            addx -15
            noop
            noop
            addx -3
            addx 9
            addx 1
            addx -3
            addx 8
            addx 1
            addx 5
            noop
            noop
            noop
            noop
            noop
            addx -36
            noop
            addx 1
            addx 7
            noop
            noop
            noop
            addx 2
            addx 6
            noop
            noop
            noop
            noop
            noop
            addx 1
            noop
            noop
            addx 7
            addx 1
            noop
            addx -13
            addx 13
            addx 7
            noop
            addx 1
            addx -33
            noop
            noop
            noop
            addx 2
            noop
            noop
            noop
            addx 8
            noop
            addx -1
            addx 2
            addx 1
            noop
            addx 17
            addx -9
            addx 1
            addx 1
            addx -3
            addx 11
            noop
            noop
            addx 1
            noop
            addx 1
            noop
            noop
            addx -13
            addx -19
            addx 1
            addx 3
            addx 26
            addx -30
            addx 12
            addx -1
            addx 3
            addx 1
            noop
            noop
            noop
            addx -9
            addx 18
            addx 1
            addx 2
            noop
            noop
            addx 9
            noop
            noop
            noop
            addx -1
            addx 2
            addx -37
            addx 1
            addx 3
            noop
            addx 15
            addx -21
            addx 22
            addx -6
            addx 1
            noop
            addx 2
            addx 1
            noop
            addx -10
            noop
            noop
            addx 20
            addx 1
            addx 2
            addx 2
            addx -6
            addx -11
            noop
            noop
            noop
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("""
            ##..##..##..##..##..##..##..##..##..##..
            ###...###...###...###...###...###...###.
            ####....####....####....####....####....
            #####.....#####.....#####.....#####.....
            ######......######......######......####
            #######.......#######.......#######.....

            """, result);
    }
    
    [TestMethod]
    public void Day10_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day10)));
        Assert.AreEqual("""
            ###...##..###....##..##..###..#..#.###..
            #..#.#..#.#..#....#.#..#.#..#.#..#.#..#.
            #..#.#..#.#..#....#.#....###..####.#..#.
            ###..####.###.....#.#....#..#.#..#.###..
            #....#..#.#....#..#.#..#.#..#.#..#.#....
            #....#..#.#.....##...##..###..#..#.#....
            
            """, result);
    }
    
}
