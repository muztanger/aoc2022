namespace Advent_of_Code_2022;

[TestClass]
public class Day02
{
    class Player
    {
        public int Id { get; set; }
        public int Score { get; set; }
    }

    public class Game
    {
        private Dictionary<string, int> shapeScore = new Dictionary<string, int>
        {
            {"R", 1 },
            {"P", 2 },
            {"S", 3 },
        };
        public int Round(string oponent, string me)
        {
            // shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
            // plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won)
            int shape = shapeScore[me];
            int outcome = 0;
            if (oponent == "R" && me == "R")
            {
                outcome = 3;
            }
            else if (oponent == "R" && me == "P")
            {
                outcome = 6;
            }
            else if (oponent == "R" && me == "S")
            {
                outcome = 0;
            }
            else if (oponent == "P" && me == "R")
            {
                outcome = 0;
            }
            else if (oponent == "P" && me == "P")
            {
                outcome = 3;
            }
            else if (oponent == "P" && me == "S")
            {
                outcome = 6;
            }
            else if (oponent == "S" && me == "R")
            {
                outcome = 6;
            }
            else if (oponent == "S" && me == "P")
            {
                outcome = 0;
            }
            else if (oponent == "S" && me == "S")
            {
                outcome = 3;
            }
            return shape + outcome;
        }

        public string Tactic(string oponent, string tactic)
        {
            //X means you need to lose, Y means you need to end the round in a draw, and Z means you need to win. Good luck!"
            if (tactic == "Y")
            {
                return oponent;
            }

            if (oponent == "R" && tactic == "X")
            {
                return "S";
            }
            else if (oponent == "R" && tactic == "Z")
            {
                return "P";
            }
            else if (oponent == "P" && tactic == "X")
            {
                return "R";
            }
            else if (oponent == "P" && tactic == "Z")
            {
                return "S";
            }
            else if (oponent == "S" && tactic == "X")
            {
                return "P";
            }
            else if (oponent == "S" && tactic == "Z")
            {
                return "R";
            }
            Assert.Fail();
            return "FAIL";
        }
    }
    private static string Part1(IEnumerable<string> input)
    {
        var dict = new Dictionary<string, string>();
        dict.Add("A", "R");
        dict.Add("B", "P");
        dict.Add("C", "S");
        dict.Add("X", "R");
        dict.Add("Y", "P");
        dict.Add("Z", "S");
        var p1 = new Player { Id = 1, Score = 0 };
        var p2 = new Player { Id = 2, Score = 0 };
        var result = new StringBuilder();
        var game = new Game();
        var total = 0;
        foreach (var line in input)
        {
            var split = line.Split();
            var x = dict[split[0]];
            var y = dict[split[1]];
            var round = game.Round(x, y);
            Console.WriteLine($"opponent={x} me={y} round={round}");
            total += round;

        }
        return total.ToString();
    }
    
    private static string Part2(IEnumerable<string> input)
    {
        var dict = new Dictionary<string, string>();
        dict.Add("A", "R");
        dict.Add("B", "P");
        dict.Add("C", "S");
        //dict.Add("X", "R");
        //dict.Add("Y", "P");
        //dict.Add("Z", "S");
        var p1 = new Player { Id = 1, Score = 0 };
        var p2 = new Player { Id = 2, Score = 0 };
        var result = new StringBuilder();
        var game = new Game();
        var total = 0;
        foreach (var line in input)
        {
            var split = line.Split();
            var x = dict[split[0]];
            var y = game.Tactic(x, split[1]);
            var round = game.Round(x, y);
            Console.WriteLine($"opponent={x} me={y} round={round}");
            total += round;

        }
        return total.ToString();
    }
    
    [TestMethod]
    public void Day02_Part1_Example01()
    {
        var input = """
            A Y
            B X
            C Z
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day02_Part1_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part1(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day02_Part1()
    {
        var result = Part1(Common.DayInput(nameof(Day02)));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day02_Part2_Example01()
    {
        var input = """
            A Y
            B X
            C Z
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day02_Part2_Example02()
    {
        var input = """
            <TODO>
            """;
        var result = Part2(Common.GetLines(input));
        Assert.AreEqual("", result);
    }
    
    [TestMethod]
    public void Day02_Part2()
    {
        var result = Part2(Common.DayInput(nameof(Day02)));
        Assert.AreEqual("", result);
    }
    
}
