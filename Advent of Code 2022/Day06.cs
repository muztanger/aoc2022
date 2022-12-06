namespace Advent_of_Code_2022;

[TestClass]
public class Day06
{
    private static string Part1(IEnumerable<string> input)
    {
        var result = new List<int>();
        var list = new List<char>();
        bool AllDifferent()
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i] == list[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        foreach (var line in input)
        {
            var i = 0;
            foreach (var c in line)
            {
                if (list.Count < 4)
                {
                    list.Add(c);
                }
                else
                {
                    list.Add(c);
                    if (list.Count > 4)
                    {
                        list.RemoveAt(0);
                    }

                }

                if (list.Count == 4 && AllDifferent())
                {
                    Console.WriteLine(i + 1);
                    break;
                }

                i++;
            }
            list.Clear();
        }
        return result.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var result = new List<int>();
        var list = new List<char>();
        bool AllDifferent()
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i] == list[j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        foreach (var line in input)
        {
            var i = 0;
            var n = 14;
            foreach (var c in line)
            {
                if (list.Count < n)
                {
                    list.Add(c);
                }
                else
                {
                    list.Add(c);
                    if (list.Count > n)
                    {
                        list.RemoveAt(0);
                    }

                }

                if (list.Count == n  && AllDifferent())
                {
                    Console.WriteLine(i + 1);
                    break;
                }

                i++;
            }
            list.Clear();
        }
        return result.ToString();
    }

    [TestMethod]
    public void Day06_Part1_Example01()
    {
        var input = """
            mjqjpqmgbljsphdztnvjfqwrcgsmlb
            bvwbjplbgvbhsrlpgdmjqwftvncz
            nppdvjthqldpwncqszvftbrmjlhg
            nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg
            zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day06_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day06_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day06)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day06_Part2_Example01()
    {
        var input = """
            mjqjpqmgbljsphdztnvjfqwrcgsmlb
            bvwbjplbgvbhsrlpgdmjqwftvncz
            nppdvjthqldpwncqszvftbrmjlhg
            nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg
            zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day06_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day06_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day06)));
        Assert.AreEqual("", result);
    }
    
}
