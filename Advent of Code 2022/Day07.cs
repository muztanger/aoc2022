using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Xml.Linq;

namespace Advent_of_Code_2022;

[TestClass]
public class Day07
{
    private record File(string Name, long Size);
    
    private record Node
    {
        public string Name { get; init; } = "";
        public string Path { get; init; } = "";
        public string FullName => $"{Path}/{Name}";
        public long Size => Files.Sum(f => f.Size);
        public List<File> Files { get; init; } = new List<File>();
        public Node? Parent { get; init; } = null;
        
        private List<Node> _children = new List<Node>();

        public void AddChild(Node node)
        {
            _children.Add(node);
        }

        public void AddFile(File file)
        {
            Files.Add(file);
        }

        public bool TryGetChild(string name, out Node? child)
        {
            foreach (var c in _children)
            {
                if (c.Name == name)
                {
                    child = c;
                    return true;
                }
            }
            child = null;
            return false;
        }

        public long TotalSize()
        {
            var sum = Size;
            foreach (var node in _children)
            {
                sum += node.TotalSize();
            }
            return sum;
        }

        public void Part1(ref List<Node> nodes)
        {
            nodes ??= new List<Node>();

            if (TotalSize() <= 100000)
            {
                nodes.Add(this);
            }

            foreach (var child in _children)
            {
                child.Part1(ref nodes);
            }
        }

        public void Part2(ref List<Node> nodes, long threshold)
        {
            nodes ??= new List<Node>();

            if (TotalSize() >= threshold)
            {
                nodes.Add(this);
            }

            foreach (var child in _children)
            {
                child.Part2(ref nodes, threshold);
            }
        }
    }

    private enum State { WaitCommand, Listing };
    
    private static string Part1(IEnumerable<string> input)
    {
        var directory = new List<string>();
        var sizes = new Dictionary<string, int>();
        var state = State.WaitCommand;
        var result = new StringBuilder();
        Node root = new Node { Name = "", Path = "" };
        Node current = root;
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
                    //  - 123 abc means that the current directory contains a file named abc with size 123.
                    //  - dir xyz means that the current directory contains a directory named xyz.

                    Console.WriteLine($"Listing line={line}");
                    var (size, name) = line.Split(' ');
                    if (!line.StartsWith("dir"))
                    {
                        sizes.TryGetValue(DirectoryToString(directory), out var x);
                        x += int.Parse(size);
                        sizes[DirectoryToString(directory)] = x;
                        current.AddFile(new File(name, long.Parse(size)));
                    }
                    else
                    {
                        current.AddChild(new Node { Name = name, Parent = current});
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
                    root = new Node{ Name = "", Path = "" };
                    current = root;
                }
                else if (path == "..")
                {
                    if (current.Parent != null)
                    {
                        current= current.Parent;
                    }
                    directory.RemoveAt(directory.Count - 1);
                }
                else
                {
                    
                    if (current.TryGetChild(path, out var child) )
                    {
                        current = child;
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    directory.Add(path);
                }
                Console.WriteLine("directory: " + DirectoryToString(directory));
            }
            else if (line.StartsWith("$ ls"))
            {
                //ls means list.It prints out all of the files and directories immediately contained by the current directory:
                //
                state = State.Listing;
            }
        }
        var nodes = new List<Node>();
        root.Part1(ref nodes);
        Console.WriteLine("Nodes: " + string.Join(",", nodes));
        var sum = 0L;
        foreach (var node in nodes)
        {
            if (node.TotalSize() <= 100000)
            {
                if (result.Length > 0)
                {
                    result.AppendLine();
                }
                result.Append($"{node.FullName}: {node.TotalSize()}");
                sum += node.TotalSize();
            }
        }
        return sum.ToString();
    }

    private static string DirectoryToString(List<string> directory)
    {
        return "/" + string.Join("/", directory);
    }

    private static string Part2(IEnumerable<string> input)
    {
        var directory = new List<string>();
        var sizes = new Dictionary<string, int>();
        var state = State.WaitCommand;
        var result = new StringBuilder();
        Node root = new Node { Name = "", Path = "" };
        Node current = root;
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
                    //  - 123 abc means that the current directory contains a file named abc with size 123.
                    //  - dir xyz means that the current directory contains a directory named xyz.

                    Console.WriteLine($"Listing line={line}");
                    var (size, name) = line.Split(' ');
                    if (!line.StartsWith("dir"))
                    {
                        sizes.TryGetValue(DirectoryToString(directory), out var x);
                        x += int.Parse(size);
                        sizes[DirectoryToString(directory)] = x;
                        current.AddFile(new File(name, long.Parse(size)));
                    }
                    else
                    {
                        current.AddChild(new Node { Name = name, Parent = current });
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
                    root = new Node { Name = "", Path = "" };
                    current = root;
                }
                else if (path == "..")
                {
                    if (current.Parent != null)
                    {
                        current = current.Parent;
                    }
                    directory.RemoveAt(directory.Count - 1);
                }
                else
                {

                    if (current.TryGetChild(path, out var child))
                    {
                        current = child;
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    directory.Add(path);
                }
                Console.WriteLine("directory: " + DirectoryToString(directory));
            }
            else if (line.StartsWith("$ ls"))
            {
                //ls means list.It prints out all of the files and directories immediately contained by the current directory:
                //
                state = State.Listing;
            }
        }
        var nodes = new List<Node>();
        //root.Part1(ref nodes);
        root.Part2(ref nodes, 30000000L - ( 70000000L - root.TotalSize()));
        Console.WriteLine("Nodes: " + string.Join(",", nodes));
        foreach (var node in nodes)
        {
            if (result.Length > 0)
            {
                result.AppendLine();
            }
            result.Append($"{node.FullName}: {node.TotalSize()}");
        }
        return nodes.OrderBy(x => x.TotalSize()).First().TotalSize().ToString();
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
        Assert.AreEqual("95437", result);
    }
    
    [TestMethod]
    public void Day07_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day07)));
        Assert.AreEqual("1582412", result);
    }
    
    [TestMethod]
    public void Day07_Part2_Example01()
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
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("24933642", result);
    }
    
    [TestMethod]
    public void Day07_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day07)));
        Assert.AreEqual("3696336", result);
    }
    
}
