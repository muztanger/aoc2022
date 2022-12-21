using System.Security;

namespace Advent_of_Code_2022;

[TestClass]
public class Day20
{
    public class Node : IEquatable<Node>
    {
        public long Id { get; init; }
        public long X { get; init; }
        public Node(long x, long id)
        {
            this.X = x;
            this.Id = id;
        }

        public Node? Next { get; set; } = null;
        public Node? Last { get; set; } = null;

        public override string ToString()
        {
            return $"Node(X:{X}, Last={Last?.X} Next={Next?.X} Id={Id})";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj is not Node nodeObj)
                return false;
            else
                return Equals(nodeObj);
        }

        public bool Equals(Node? other)
        {
            return other is not null &&
                   X == other.X &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * 7919 + Id.GetHashCode();
        }

        public static bool operator ==(Node n1, Node n2)
        {
            if (((object)n1) == null || ((object)n2) == null)
                return Object.Equals(n1, n2);

            return n1.Equals(n2);
        }

        public static bool operator !=(Node n1, Node n2)
        {
            if (((object)n1) == null || ((object)n2) == null)
                return !Object.Equals(n1, n2);

            return !(n1.Equals(n2));
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var initial = new List<Node>();
        var result = new StringBuilder();

        long id = 0;
        foreach (var x in input.Select(line => long.Parse(line)))
        {
            var node = new Node(x, id);
            initial.Add(node);
            if (initial.Count >= 2)
            {
                Node last = initial[^2];
                last.Next = node;
                node.Last = last;
            }
            id++;
        }
        initial[^1].Next = initial[0];
        initial[0].Last = initial[^1];

        var current = initial[0];

        Print(current, initial.Count);

        void Print(Node first, int n)
        {
            var result = new StringBuilder();
            var current = first;
            for (int i = 0; i < n; i++)
            {
                if (i != 0) result.Append(", ");
                Assert.IsNotNull(current);
                result.Append(current.X);
                current = current.Next;
            }
            Console.WriteLine(result);
        }

        foreach (var node in initial)
        {
            current = node;
            var next = current;
            int i = 0;
            while (i < long.Abs(current.X))
            {
                Assert.IsNotNull(next);
                if (current.X > 0)
                {
                    next = next.Next;
                }
                else if (current.X < 0)
                {
                    next = next.Last;
                }
                i++;
            }
            if (current.X < 0)
            {
                next = next.Last;
            }

            Console.WriteLine($"Move {current} to {next}");
            Assert.IsNotNull(next);
            //if (current.X != 0)
            {
                Move(current, next);
            }
            Assert.IsTrue(CheckList(current), $"{current} {next}");
            //Print(initial[0], initial.Count);

            void Move(Node from, Node to) {
                if (from == to || from.Last == to)
                {
                    return;
                }

                // from Node(X:9999,  Last=-5413 Next=-304 Id=4747)
                // to   Node(X:-5413, Last=2470  Next=9999 Id=939)

                // Move Node(X:9999, Last=-5413 Next=-304 Id=4747)
                // to Node(X:-5413, Last=2470 Next=9999 Id=939)

                var toNextLast = from;
                var fromLastNext = from.Next;
                var fromNextLast = from.Last;
                var fromLast = to;
                var fromNext = to.Next;
                var toNext = from;

                to.Next.Last = toNextLast;
                from.Last.Next = fromLastNext;
                from.Next.Last = fromNextLast;
                from.Last = fromLast;
                from.Next = fromNext;
                to.Next = toNext;
            }

            //result.AppendLine(node.ToString());
        }

        bool CheckList(Node start) {
            var n = start;
            var set = new HashSet<Node>();
            while (set.Add(n))
            {
                n = n.Next;
            }
            return set.Count == initial.Count;
        }

        var stack = new HashSet<(long, long, long)>();
        while (current.X != 0)
        {
            Assert.IsNotNull(current.Last);
            Assert.IsNotNull(current.Next);
            Assert.IsTrue(stack.Add((current.X, current.Last.X, current.Next.X)), stack.Count.ToString());
            current = current.Next;
            Assert.IsNotNull(current);
        }
        Console.WriteLine("current: " + current);

        var start = initial.First(x => x.X == 0);
        Console.WriteLine("start: " + start);

        var find = new int[] { 1000, 2000, 3000 };
        var n = start;
        var sum = 0L;
        for (int i = 0; i < 3000; i++)
        {
            n = n.Next;
            if (find.Contains(i + 1))
            {
                Console.WriteLine($"this={n.X} last={n.Last.X} next={n.Next.X}");
                sum += n.X;
            }
        }

        return sum.ToString();
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
    public void Day20_Part1_Example01()
    {
        var input = """
            1
            2
            -3
            3
            -2
            0
            4
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("3", result);
    }
    
    [TestMethod]
    public void Day20_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day20)));
        Assert.AreNotEqual("13733", result); // too low
        Assert.AreNotEqual("-18746", result); // well... also to low
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day20_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day20)));
        Assert.AreEqual("", result);
    }
    
}
