using System.ComponentModel.DataAnnotations;

namespace Advent_of_Code_2022;

[TestClass]
public class Day05
{
    private static string Part1(IEnumerable<string> input)
    {
        var isFillCrates = true;
        var result = new StringBuilder();
        var crates = new Dictionary<string, Stack<string>>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line) && isFillCrates)
            {
                foreach (var kv in crates)
                {
                    var tmp = new Stack<string>();
                    foreach (var c in kv.Value)
                    {
                        tmp.Push(c);
                    }
                    crates[kv.Key] = tmp;
                }
                isFillCrates = false;
                continue;
            }
            if (isFillCrates)
            {
                var i = 0;
                foreach (var c in line)
                {
                    if (Char.IsUpper(c))
                    {
                        var crate = ((i - 1) / 4 + 1).ToString();
                        if (crates.TryGetValue(crate, out Stack<string>? stack))
                        {
                            Assert.IsNotNull(stack);
                            stack.Push(c.ToString());
                            crates[crate] = stack;
                        }
                        else
                        {
                            stack = new Stack<string>();
                            stack.Push(c.ToString());
                            crates[crate] = stack;
                        }
                    }

                    i++;
                }
            }
            else
            {
                var (_, n, _, from, _, to) = line.Split();
                //Console.WriteLine($"- move {crate} from {from} to {to}");
                for (int i = 0; i < int.Parse(n); i++)
                {
                    crates[to].Push(crates[from].Pop());
                }
            }
        }
        PrintCrates(crates);
        return result.ToString();
    }

    private static void PrintCrates(Dictionary<string, Stack<string>> crates)
    {
        foreach (var crate in crates.Keys.OrderBy(x => x))
        {
            Console.WriteLine(crate.ToString() + " " + string.Join(",", crates[crate]));
        }
    }

    private static string Part2(IEnumerable<string> input)
    {
        var isFillCrates = true;
        var result = new StringBuilder();
        var crates = new Dictionary<string, Stack<string>>();
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line) && isFillCrates)
            {
                foreach (var kv in crates)
                {
                    var tmp = new Stack<string>();
                    foreach (var c in kv.Value)
                    {
                        tmp.Push(c);
                    }
                    crates[kv.Key] = tmp;
                }
                isFillCrates = false;
                continue;
            }
            if (isFillCrates)
            {
                var i = 0;
                foreach (var c in line)
                {
                    if (Char.IsUpper(c))
                    {
                        var crate = ((i - 1) / 4 + 1).ToString();
                        if (crates.TryGetValue(crate, out Stack<string>? stack))
                        {
                            Assert.IsNotNull(stack);
                            stack.Push(c.ToString());
                            crates[crate] = stack;
                        }
                        else
                        {
                            stack = new Stack<string>();
                            stack.Push(c.ToString());
                            crates[crate] = stack;
                        }
                    }

                    i++;
                }
            }
            else
            {
                var (_, n, _, from, _, to) = line.Split();
                var cratesToMove = new Stack<string>();
                //Console.WriteLine($"- move {crate} from {from} to {to}");
                for (int i = 0; i < int.Parse(n); i++)
                {
                    cratesToMove.Push(crates[from].Pop());
                }
                for (int i = 0; i < int.Parse(n); i++)
                {
                    crates[to].Push(cratesToMove.Pop());
                }
            }
        }
        PrintCrates(crates);
        return result.ToString();
    }


    [TestMethod]
    public void Day05_Part1_Example01()
    {
        var input = """
                [D]    
            [N] [C]    
            [Z] [M] [P]
             1   2   3 

            move 1 from 2 to 1
            move 3 from 1 to 3
            move 2 from 2 to 1
            move 1 from 1 to 2
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day05)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part2_Example01()
    {
        var input = """
                [D]    
            [N] [C]    
            [Z] [M] [P]
             1   2   3 
            
            move 1 from 2 to 1
            move 3 from 1 to 3
            move 2 from 2 to 1
            move 1 from 1 to 2
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day05_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day05)));
        Assert.AreEqual("", result);
    }
    
}
