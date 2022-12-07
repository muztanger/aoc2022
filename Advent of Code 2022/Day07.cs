using System.ComponentModel;
using System.IO;
using System.Xml.Linq;

namespace Advent_of_Code_2022;

[TestClass]
public class Day07
{
        enum State { WaitCommand, Listing };
    private static string Part1(IEnumerable<string> input)
    {
        var directory = new List<string>();
        var sizes = new Dictionary<string, int>();
        var state = State.WaitCommand;
        var result = new StringBuilder();
        foreach (var line in input)
        {
            Console.WriteLine(line);
            if (line.StartsWith("$"))
            {
                state = State.WaitCommand;
            }

            switch (state)
            {
                case State.Listing:
                    Console.WriteLine($"Listing line={line}");
                    if (!line.StartsWith("dir"))
                    {
                        var (size, _) = line.Split(' ');
                        sizes.TryGetValue(DirectoryToString(directory), out var x);
                        x += int.Parse(size);
                        sizes[DirectoryToString(directory)] = x;
                    }
                    break;
                default:
                    break;
            }

            if (line.StartsWith("$ cd"))
            {
                //cd means change directory. This changes which directory is the current directory, but the specific result depends on the argument:
                // cd x moves in one level: it looks in the current directory for the directory named x and makes it the current directory.
                // cd..moves out one level: it finds the directory that contains the current directory, then makes that directory the current directory.
                var (_, _, path) = line.Split();
                Console.WriteLine("cd: " + path);
                if (path == "/")
                {
                    directory.Clear();
                }
                else if (path == "..")
                {
                    directory.RemoveAt(directory.Count - 1);
                }
                else
                {
                    directory.Add(path);
                }
                Console.WriteLine("directory: " + DirectoryToString(directory));
            }
            else if (line.StartsWith("$ ls"))
            {
                state = State.Listing;
            }
        }
        foreach (var kv in sizes)
        {
            // TODO calculate total sum per path
            if (kv.Value < 100000)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }
        }
        return result.ToString();
    }

    private static string DirectoryToString(List<string> directory)
    {
        return "/" + string.Join("/", directory);
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
    public void Day07_Part1_Example01()
    {
        var input = """
            $ cd /
            $ ls
            dir a
            14848514 b.txt
            8504156 c.dat
            dir d
            $ cd a
            $ ls
            dir e
            29116 f
            2557 g
            62596 h.lst
            $ cd e
            $ ls
            584 i
            $ cd ..
            $ cd ..
            $ cd d
            $ ls
            4060174 j
            8033020 d.log
            5626152 d.ext
            7214296 k
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day07)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day07_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day07)));
        Assert.AreEqual("", result);
    }
    
}
