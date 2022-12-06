namespace Advent_of_Code_2022;

[TestClass]
public class Day06
{
   
    private static string Calc(IEnumerable<string> input, int n)
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
                    result.Add(i + 1);
                    break;
                }

                i++;
            }
            list.Clear();
        }
        return string.Join(",", result);
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
        var result = Calc(Common.GetLines(input), 4);
        Assert.AreEqual("7,5,6,10,11", result);
    }
    
    [TestMethod]
    public void Day06_Part1()
    {
        var result = Calc(Common.DayInput(nameof(Day06)), 4);
        Assert.AreEqual("1140", result);
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
        var result = Calc(Common.GetLines(input), 14);
        Assert.AreEqual("19,23,23,29,26", result);
    }
    
    [TestMethod]
    public void Day06_Part2()
    {
        var result = Calc(Common.DayInput(nameof(Day06)), 14);
        Assert.AreEqual("3495", result);
    }
    
}
