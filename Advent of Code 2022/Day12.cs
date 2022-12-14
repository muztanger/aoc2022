using System.Security.AccessControl;

namespace Advent_of_Code_2022;

[TestClass]
public class Day12
{
    public class Grid: IEnumerator, IEnumerable
    {
        readonly List<List<int>> _gridList;
        readonly Box<int> _gridBorders;
        Pos<int> _index = new(-1, 0);
        public Grid(List<List<int>> grid)
        {
            _gridList = grid;
            int width = grid.First().Count;
            int height = grid.Count;
            _gridBorders = new Box<int>(width, height);
        }

        public int this[Pos<int> p]
        {
            get => _gridList[p.y][p.x];
            set { _gridList[p.y][p.x] = value; }
        }

        public object Current => this[_index];

        public IEnumerator GetEnumerator()
        {
            return (IEnumerator) this;
        }

        public bool IsInside(Pos<int> p)
        {
            return _gridBorders.Contains(p);
        }

        public bool MoveNext()
        {
            _index.x++;
            if (!_gridBorders.Contains(_index))
            {
                _index.x = 0;
                _index.y++;
            }
            if (!_gridBorders.Contains(_index))
            {
                return false;
            }
            return true;
        }

        public void Reset()
        {
            _index = new(-1, 0);
        }
    }

    private static string Part1(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var gridList = new List<List<int>>();
        var start = new Pos<int>(0, 0);
        var end = new Pos<int>(0, 0);
        var y = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            List<int> row = new List<int>();
            gridList.Add(row);
            var x = 0;
            foreach (var c in line)
            {
                if (c == 'S') // startPos
                {
                    start = new Pos<int>(x, y);
                    row.Add(0);
                }
                else if (c == 'E') // endPos
                {
                    end = new Pos<int>(x, y);
                    row.Add('z' - 'a');
                }
                else
                {
                    row.Add(c - 'a');
                }
                x++;
            }
            y++;
        }
        var grid = new Grid(gridList);

        PrintGrid(gridList, start, end);
        var adjecent = new Dictionary<Pos<int>, List<Pos<int>>>();
        for (y = 0; y < gridList.Count; y++)
        {
            for (int x = 0; x < gridList.First().Count; x++)
            {
                var p = new Pos<int>(x, y);
                foreach (var dp in walks)
                {
                    var next = p + dp;
                    if (grid.IsInside(next) && grid[next] - 1 <= grid[p])
                    {
                        if (adjecent.TryGetValue(p, out var values))
                        {
                            adjecent[p].Add(next);
                        }
                        else
                        {
                            adjecent[p] = new() { next };
                        }
                    }
                }
            }
        }

        return Dijkstra(gridList, start, end, adjecent).ToString();
    }

    // Dijkstra's algorithm
    private static int Dijkstra(List<List<int>> gridList, Pos<int> start, Pos<int> end, Dictionary<Pos<int>, List<Pos<int>>> adjecent)
    {
        var distList = new List<List<int>>();
        for (int y = 0; y < gridList.Count; y++)
        {
            List<int> row = new();
            for (int x = 0; x < gridList.First().Count; x++)
            {
                row.Add(int.MaxValue / 2);
            }
            distList.Add(row);
        }

        var dist = new Grid(distList);
        var q = new PriorityQueue<Pos<int>, int>();
        var processed = new HashSet<Pos<int>>();
        dist[start] = 0;
        q.Enqueue(start, 0);
        while (q.TryDequeue(out var a, out var priority))
        {
            if (processed.Contains(a)) continue;
            processed.Add(a);
            if (adjecent.ContainsKey(a))
            {
                foreach (var b in adjecent[a])
                {
                    if (dist[a] + 1 < dist[b])
                    {
                        dist[b] = dist[a] + 1;
                        q.Enqueue(b, dist[b]);
                    }
                }
            }
        }

        return dist[end];
    }

    private static readonly List<Pos<int>> walks = new List<Pos<int>> { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
    private static void PrintGrid(List<List<int>> grid, Pos<int> start, Pos<int> end)
    {
        int y;
        var tostr = new StringBuilder();
        y = 0;
        foreach (var row in grid)
        {
            var x = 0;
            if (y != 0)
            {
                tostr.AppendLine();
            }
            foreach (var c in row)
            {
                Pos<int> p = new Pos<int>(x, y);
                if (start == p)
                {
                    tostr.Append('S');
                }
                else if (end == p)
                {
                    tostr.Append('E');
                }
                else
                {
                    tostr.Append(Convert.ToChar('a' + c));
                }
                x++;
            }
            y++;
        }
        Console.WriteLine(tostr.ToString());
    }

    private static string Part2(IEnumerable<string> input)
    {
        var result = new StringBuilder();
        var gridList = new List<List<int>>();
        var startList = new List<Pos<int>>();
        var end = new Pos<int>(0, 0);
        var y = 0;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            List<int> row = new List<int>();
            gridList.Add(row);
            var x = 0;
            foreach (var c in line)
            {
                if (c == 'S' || c == 'a') // startPos
                {

                    startList.Add(new Pos<int>(x, y));
                    row.Add(0);
                }
                else if (c == 'E') // endPos
                {
                    end = new Pos<int>(x, y);
                    row.Add('z' - 'a');
                }
                else
                {
                    row.Add(c - 'a');
                }
                x++;
            }
            y++;
        }
        var grid = new Grid(gridList);

        //PrintGrid(gridList, start, end);
        var adjecent = new Dictionary<Pos<int>, List<Pos<int>>>();
        for (y = 0; y < gridList.Count; y++)
        {
            for (int x = 0; x < gridList.First().Count; x++)
            {
                var p = new Pos<int>(x, y);
                foreach (var dp in walks)
                {
                    var next = p + dp;
                    if (grid.IsInside(next) && grid[next] - 1 <= grid[p])
                    {
                        if (adjecent.TryGetValue(p, out var values))
                        {
                            adjecent[p].Add(next);
                        }
                        else
                        {
                            adjecent[p] = new() { next };
                        }
                    }
                }
            }
        }

        var min = int.MaxValue;
        foreach (var start in startList)
        {
            min = Math.Min(min, Dijkstra(gridList, start, end, adjecent));
        }
        return min.ToString();
    }

    [TestMethod]
    public void Day12_Part1_Example01()
    {
        var input = """
            Sabqponm
            abcryxxl
            accszExk
            acctuvwj
            abdefghi
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("31", result);
    }

    [TestMethod]
    public void Day12_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day12)));
        Assert.AreEqual("383", result);
    }
    
    [TestMethod]
    public void Day12_Part2_Example01()
    {
        var input = """
            Sabqponm
            abcryxxl
            accszExk
            acctuvwj
            abdefghi
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("29", result);
    }
    
    [TestMethod]
    public void Day12_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day12)));
        Assert.AreEqual("377", result);
    }
    
}
