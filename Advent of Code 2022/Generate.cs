namespace Advent_of_Code_2022;

[TestClass]
public class Generate
{
    static readonly HttpClient client = new HttpClient();

    [TestMethod]
    public void GenerateDay()
    {
        //int day = DateTime.Now.Day;
        var day = 1;
        string dayStr = $"Day{day:D2}";
        var baseDir  = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
        {
            var file = Path.Combine(baseDir, "input", $"{dayStr}.input");
            if (!File.Exists(file))
            {
                File.WriteAllText(file, GetDayInput(day).Result);
            }
        }
        {
            var file = Path.Combine(baseDir, $"{dayStr}.cs");
            if (!File.Exists(file))
            {
                var tests = new List<string>()
                {
                    $"{dayStr}_Part1_Example01",
                    $"{dayStr}_Part1_Example02",
                    $"{dayStr}_Part1",
                    $"{dayStr}_Part2_Example01",
                    $"{dayStr}_Part2_Example02",
                    $"{dayStr}_Part2"
                };

                using var fs = File.Create(file);
                using var writer = new StreamWriter(fs);

                int pad = 0;
                WriteLine("namespace Advent_of_Code_2022;");
                WriteLine();
                WriteLine("[TestClass]");
                WriteLine($"public class {dayStr}");
                WriteLine("{");
                pad++;

                WriteLine("private static string Part1(IEnumerable<string> input)");
                WriteLine("{");
                pad++;
                WriteLine("var result = new StringBuilder();");
                WriteLine("foreach (var line in input)");
                WriteLine("{");
                WriteLine("}");
                WriteLine("return result.ToString();");
                pad--;
                WriteLine("}");
                WriteLine();

                WriteLine("private static string Part2(IEnumerable<string> input)");
                WriteLine("{");
                pad++;
                WriteLine("var result = new StringBuilder();");
                WriteLine("foreach (var line in input)");
                WriteLine("{");
                WriteLine("}");
                WriteLine("return result.ToString();");
                pad--;
                WriteLine("}");
                WriteLine();

                foreach (var test in tests)
                {
                    WriteLine("[TestMethod]");
                    WriteLine($"public void {test}()");
                    WriteLine("{");
                    pad++;
                    var fun = "Part1";
                    if (test.Contains("Part2"))
                    {
                        fun = "Part2";
                    }
                    if (test.Contains("Example"))
                    {
                        WriteLine("var input = \"\"\"");
                        pad++;
                        WriteLine("<TODO>");
                        WriteLine("\"\"\";");
                        pad--;
                        WriteLine($"var result = {fun}(Common.GetLines(input));");
                    }
                    else
                    {
                        WriteLine($"var result = {fun}(Common.DayInput(nameof({dayStr})));");
                    }
                    WriteLine("""Assert.AreEqual("", result);""");
                    pad--;
                    WriteLine("}");
                    WriteLine();
                }
                pad--;
                WriteLine("}");

                string Padding() => new String(' ', 4 * pad);
                void WriteLine(string str="") => writer.WriteLine($"{Padding()}{str}");
            }
        }
    }
    public static async Task<string> GetDayInput(int day)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://adventofcode.com/2016/day/{day}/input");
        var cookieFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Cookie.dat");
        if (!File.Exists(cookieFile))
        {
            Console.WriteLine($"Create a cookie file and call it: {cookieFile}");
        }
        var lines = File.ReadAllLines(cookieFile);
        request.Headers.Add("Cookie", lines[0]);
        HttpResponseMessage response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}