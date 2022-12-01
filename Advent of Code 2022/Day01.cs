namespace Advent_of_Code_2022;

[TestClass]
public class Day01
{
    private static string Calc(IEnumerable<string> input, int n)
    {
        var elfs = new List<long>();
        var sum = 0L;
        foreach (var line in input)
        {
            if (string.IsNullOrEmpty(line))
            {
                elfs.Add(sum);
                sum = 0L;
            }
            else
            {
                sum += long.Parse(line);
            }
        }
        elfs.Add(sum);
        return elfs.OrderByDescending(x => x).Take(n).Sum().ToString();
    }
    
    [TestMethod]
    public void Day01_Part1_Example01()
    {
        var input = """
            1000
            2000
            3000

            4000

            5000
            6000

            7000
            8000
            9000

            10000
            """;
        var result = Calc(Common.GetLines(input), 1);
        Assert.AreEqual("24000", result);
    }
    
    [TestMethod]
    public void Day01_Part1()
    {
        var result = Calc(Common.DayInput(nameof(Day01)), 1);
        Assert.AreEqual("72017", result);
    }
    
    [TestMethod]
    public void Day01_Part2_Example01()
    {
        var input = """
            1000
            2000
            3000
            
            4000
            
            5000
            6000
            
            7000
            8000
            9000
            
            10000
            """;
        var result = Calc(Common.GetLines(input), 3);
        Assert.AreEqual("45000", result);
    }
    
    [TestMethod]
    public void Day01_Part2()
    {
        var result = Calc(Common.DayInput(nameof(Day01)), 3);
        Assert.AreEqual("212520", result);
    }
    
}
