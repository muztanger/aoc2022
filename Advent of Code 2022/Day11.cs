using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Advent_of_Code_2022;

[TestClass]
public class Day11
{
    private static string Part1(IEnumerable<string> input)
    {
        var monkeys = Monkey.Parse(input);
        var game = new Game(monkeys);
        Console.WriteLine(game);
        Console.WriteLine();
        for (int round = 1; round <= 20; round++)
        {
            game.Round();
            if (round <= 10 || round == 15 || round == 20)
            {
                Console.WriteLine($"After round: {round}");
                Console.WriteLine(game);
                Console.WriteLine();
            }
        }

        var monkeyBusiness = new List<long>();
        for (int i = 0; i < monkeys.Count; i++)
        {
            //Monkey 0 inspected items 101 times.
            Console.WriteLine($"Monkey {i} inspected items {monkeys[i].Inspections} times.");
            monkeyBusiness.Add(monkeys[i].Inspections);
        }

        return monkeyBusiness.OrderByDescending(x => x).Take(2).Aggregate(1L, (a, b) => a * b).ToString();
    }

    private static string Part2(IEnumerable<string> input)
    {
        return "meep";
    }

    public class Monkey
    {
        private long _testVal;
        private int _trueAction;
        private int _falseAction;

        public List<long> StartingItems { get; private set; } = new List<long>();
        public char Op { get; private set; }
        public long? OpLeft { get; private set; } = null;
        public long? OpRight { get; private set; } = null;
        public long Inspections { get; private set; } = 0L;

        public List<(long, int)> Turn()
        {
            var throws = new List<(long, int)>();
            for (int i = 0; i < StartingItems.Count; i++)
            {
                Inspections++;

                // inspect
                long left = StartingItems[i];
                long right = StartingItems[i];
                if (OpLeft.HasValue)
                {
                    left = OpLeft.Value;
                }
                if (OpRight.HasValue)
                {
                    right = OpRight.Value;
                }
                if (Op == '+')
                {
                    StartingItems[i] = left + right;
                }
                else if (Op == '*')
                {
                    StartingItems[i] = left * right;
                }
                else
                {
                    Assert.Fail(Op.ToString());
                }

                // relax
                StartingItems[i] /= 3L;

                // throw
                var throwTo = StartingItems[i] % _testVal == 0 ? _trueAction : _falseAction;
                throws.Add((StartingItems[i], throwTo));
            }
            StartingItems = new();
            return throws;
        }

        public void Receive(long item)
        {
            StartingItems.Add(item);
        }

        public static List<Monkey> Parse(IEnumerable<string> input)
        {
            var result = new List<Monkey>();
            Monkey? monkey = null;
            foreach (var line in input)
            {
                if (line.StartsWith("Monkey"))
                {
                    monkey = new Monkey();
                }
                else if (monkey != null)
                {
                    var trimmedLine = line.Trim();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        result.Add(monkey);
                    }
                    else if (trimmedLine.StartsWith("Starting items:"))
                    {
                        var items = trimmedLine["Starting items: ".Length..].Split(",").Select(x => long.Parse(x)).ToList();
                        monkey.StartingItems = items;
                    }
                    else if (trimmedLine.StartsWith("Operation:"))
                    {
                        var (left, op, right) = trimmedLine.Split()[^3..];
                        monkey.Op = op[0];
                        if (long.TryParse(left, out var leftValue))
                        {
                            monkey.OpLeft = leftValue;
                        }
                        if (long.TryParse(right, out var rightValue))
                        {
                            monkey.OpRight = rightValue;
                        }
                    }
                    else if (trimmedLine.StartsWith("Test:"))
                    {
                        monkey._testVal = long.Parse(trimmedLine.Split().Last());
                    }
                    else if (trimmedLine.StartsWith("If true:"))
                    {
                        monkey._trueAction = int.Parse(trimmedLine.Split().Last());
                    }
                    else if (trimmedLine.StartsWith("If false:"))
                    {
                        monkey._falseAction = int.Parse(trimmedLine.Split().Last());
                    }
                }
            }
            if (monkey != null)
            {
                result.Add(monkey);
            }
            return result;
        }
    }

    class Game
    {
        private readonly List<Monkey> _monkeys;

        public Game(List<Monkey> monkeys)
        {
            this._monkeys = monkeys;
        }

        public void Round()
        {
            for (int monkey = 0; monkey < _monkeys.Count; monkey++)
            {
                var throws = _monkeys[monkey].Turn();
                foreach (var t in throws)
                {
                    _monkeys[t.Item2].Receive(t.Item1);
                }
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();
            var index = 0;
            foreach (var monkey in _monkeys)
            {
                if (index != 0)
                {
                    result.AppendLine();
                }
                result.Append($"Monkey {index}: {string.Join(", ", monkey.StartingItems)}");
                index++;
            }
            return result.ToString();
        }
    }
    
    [TestMethod]
    public void Day11_Part1_Example01()
    {
        var input = """
            Monkey 0:
              Starting items: 79, 98
              Operation: new = old * 19
              Test: divisible by 23
                If true: throw to monkey 2
                If false: throw to monkey 3

            Monkey 1:
              Starting items: 54, 65, 75, 74
              Operation: new = old + 6
              Test: divisible by 19
                If true: throw to monkey 2
                If false: throw to monkey 0

            Monkey 2:
              Starting items: 79, 60, 97
              Operation: new = old * old
              Test: divisible by 13
                If true: throw to monkey 1
                If false: throw to monkey 3

            Monkey 3:
              Starting items: 74
              Operation: new = old + 3
              Test: divisible by 17
                If true: throw to monkey 0
                If false: throw to monkey 1
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day11)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part2_Example01()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day11_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day11)));
        Assert.AreEqual("", result);
    }
    
}
