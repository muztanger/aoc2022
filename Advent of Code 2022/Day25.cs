using System.Globalization;

namespace Advent_of_Code_2022;

[TestClass]
public class Day25
{
    class Snafu<T> where T : INumber<T>
    {
        private static readonly T Base = T.One + T.One + T.One + T.One + T.One; // 5...

        public string NumStr { get; set; } = "0";
        internal T GetValue()
        {
            T result = T.Zero;
            int i = 0;
            while (i < NumStr.Length)
            {
                T x = T.Zero;
                switch (NumStr[i])
                {
                    case '=':
                        x = -T.One - T.One;
                        break;
                    case '-':
                        x = -T.One;
                        break;
                    default:
                        x = T.Parse(NumStr[i].ToString(), System.Globalization.NumberStyles.Integer, NumberFormatInfo.CurrentInfo);
                        break;
                }
                result += x;
                i++;
                if (i < NumStr.Length) result *= Base;
            }
            return result;
        }

        static internal Snafu<T> FromDec(T dec)
        {
            var snafu = new StringBuilder();
            if (dec < Base)
            {
                snafu.Append(dec);
            }
            var result = new Snafu<T>()
            {
                NumStr = snafu.ToString(),
            };
            return result;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = BigInteger.Zero;
        var re = new Regex(@"\s+");
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var sna = new Snafu<BigInteger> { NumStr = line };
            result += sna.GetValue();
        }
        return result.ToString();
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
    public void Day25_Part1_Example01()
    {
        var input = """
                        1         1
                        2         2
                       1=         3
                       1-         4
                       10         5
                       11         6
                       12         7
                       2=         8
                       2-         9
                       20        10
                      1=0        15
                      1-0        20
                   1=11-2      2022
                  1-0---0     12345
            1121-1110-1=0 314159265
            1=-0-2     1747
            12111      906
             2=0=      198
               21       11
             2=01      201
              111       31
            20012     1257
              112       32
            1=-1=      353
             1-12      107
               12        7
               1=        3
              122       37
            """;
        var result = new StringBuilder();
        var re = new Regex(@"\s+");
        foreach (var line in Common.GetLines(input))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var split = re.Split(line.Trim()).Select(s => s.Trim()).ToArray();
            var sna = new Snafu<int> { NumStr = split[0] };
            var dec = int.Parse(split[1]);
            Assert.AreEqual(dec, sna.GetValue());
        }
    }
    
    [TestMethod]
    public void Day25_Part1_Example02()
    {
        var input = """
            1=-0-2
            12111
            2=0=
            21
            2=01
            111
            20012
            112
            1=-1=
            1-12
            12
            1=
            122
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("4890", result);
    }
    
    [TestMethod]
    public void Day25_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day25)));
        Assert.AreNotEqual("31069194366050", result);
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day25_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day25_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day25_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day25)));
        Assert.AreEqual("", result);
    }
    
}
