using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;

namespace Advent_of_Code_2022;

[TestClass]
public class Day25
{
    class Snafu<T> where T : INumber<T>
    {
        private static readonly T Two = T.One + T.One;
        private static readonly T Three = Two + T.One;
        private static readonly T Four = Three + T.One;
        private static readonly T Five = Four + T.One;
        private static readonly T Base = Five; // 5...

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
            T x = dec;
            var dict = new Dictionary<T, char>
            {
                { T.Zero, '=' },
                { T.One, '-' },
                { Two, '0' },
                { Three, '1' },
                { Four, '2' },
            };
            var snafu = new StringBuilder();

                //        1              1
                //        2              2
                //        3             1=
                //        4             1-
                //        5             10
                //        6             11
                //        7             12
                //        8             2=
                //        9             2-
                //       10             20
                //       11             21
                //       12             22
                //       13            1==
                //       14            1=-
                //       15            1=0
                //       20            1-0
                //     2022         1=11-2
                //    12345        1-0---0
                //314159265  1121-1110-1=0

            if (x < Three)
            {
                snafu.Append(x);
            }
            else
            {
                var y = x;
                while (y > T.Zero)
                {
                    y += Two;
                    var z = y % Base;
                    snafu.Append(dict[z]);
                    y /= Base;
                }
            }
            var skipLeadingZeros = new StringBuilder();
            var isLead = true;
            foreach (var c in snafu.ToString().Reverse())
            {
                if (c != '0')
                {
                    isLead = false;
                    skipLeadingZeros.Append(c);
                }
                else
                {
                    if (!isLead) skipLeadingZeros.Append(c);
                }
            }
            var result = new Snafu<T>()
            {
                NumStr = string.Concat(skipLeadingZeros.ToString()),
            };
            return result;
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = 0L;
        var re = new Regex(@"\s+");
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            var sna = new Snafu<long> { NumStr = line };
            result += sna.GetValue();
        }
        return Snafu<long>.FromDec(result).NumStr;
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
    public void Dec2Snafu2Dec()
    {
        for (long i = 1; i < 50; i++)
        {
            var snafu = Snafu<long>.FromDec(i);
            Assert.AreEqual(i, snafu.GetValue(), $"snafu={snafu.NumStr}");
        }
    }

    [TestMethod]
    public void Day25_SnafuToDecimal()
    {
        var input = """
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
                        1         1
                        2         2
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
