namespace Advent_of_Code_2022;

[TestClass]
public class Day07
{
    private record File(string Name, long Size);
    
    private record Node
    {
        public string Name { get; init; } = "";
        public string Path { get; init; } = "";
        public long Size => Files.Sum(f => f.Size);
        public List<File> Files { get; init; } = new List<File>();
        public Node? Parent { get; init; } = null;
        
        private readonly List<Node> _children = new();

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

        public long TotalSize() => Size + _children.Select(n => n.TotalSize()).Sum();

        public void Filter(ref List<Node> nodes, Func<long, long, bool> filter, long threshold)
        {
            nodes ??= new List<Node>();

            if (filter(TotalSize(), threshold))
            {
                nodes.Add(this);
            }

            foreach (var child in _children)
            {
                child.Filter(ref nodes, filter, threshold);
            }
        }

        public static Node Parse(IEnumerable<string> input)
        {
            Node root = new() { Name = "", Path = "" };
            var directory = new List<string>();
            var state = State.WaitCommand;
            Node current = root;
            var isDebug = false;

            string DirectoryToString(List<string> directory)
            {
                return "/" + string.Join("/", directory);
            }

            foreach (var line in input)
            {
                if (isDebug) Console.WriteLine(line);
                if (line.StartsWith("$"))
                {
                    state = State.WaitCommand;
                }

                switch (state)
                {
                    case State.Listing:
                        //  - 123 abc means that the current directory contains a file named abc with size 123.
                        //  - dir xyz means that the current directory contains a directory named xyz.

                        if (isDebug) Console.WriteLine($"Listing line={line}");
                        var (size, name) = line.Split(' ');
                        if (!line.StartsWith("dir"))
                        {
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
                    if (isDebug) Console.WriteLine("cd: " + path);
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
                    if (isDebug) Console.WriteLine("directory: " + DirectoryToString(directory));
                }
                else if (line.StartsWith("$ ls"))
                {
                    //ls means list.It prints out all of the files and directories immediately contained by the current directory:
                    //
                    state = State.Listing;
                }
            }

            return root;
        }
    }

    private enum State { WaitCommand, Listing };
    
    private static string Part1(IEnumerable<string> input)
    {
        Node root = Node.Parse(input);
        var nodes = new List<Node>();
        root.Filter(ref nodes, (a, b) => a <= b, 100000L);
        return nodes.Select(x => x.TotalSize()).Sum().ToString();
    }


    private static string Part2(IEnumerable<string> input)
    {
        Node root = Node.Parse(input);
        var nodes = new List<Node>();
        root.Filter(ref nodes, (a, b) => a >= b, root.TotalSize() - 40000000L);
        return nodes.OrderBy(x => x.TotalSize()).First().TotalSize().ToString();
    }

    string _example = """
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

    [TestMethod]
    public void Day07_Part1_Example01()
    {
        var result = Part1(Common.GetLines(_example));
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
        var result = Part2(Common.GetLines(_example));
        Assert.AreEqual("24933642", result);
    }
    
    [TestMethod]
    public void Day07_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day07)));
        Assert.AreEqual("3696336", result);
    }
}
