namespace Advent_of_Code_2022;

file enum Play { Rock = 1, Paper = 2, Scissors = 3, None = 4}

file class Game
{
    public int TotalScore { get; private set; } = 0;

    public void Round(Play oponent, Play me)
    {
        // shape you selected (1 for Rock, 2 for Paper, and 3 for Scissors)
        // plus the score for the outcome of the round (0 if you lost, 3 if the round was a draw, and 6 if you won)

        int shape = (int)me;
        int outcome = 3;

        if (oponent == me)
        {
            outcome = 3;
        }
        else if (oponent == Play.Rock)
        {
            outcome = me == Play.Paper ? 6 : 0;
        }
        else if (oponent == Play.Paper)
        {
            outcome = me == Play.Rock ? 0 : 6;
        }
        else if (oponent == Play.Scissors)
        {
            outcome = me == Play.Rock ? 6 : 0;
        }
        else
        {
            Assert.Fail();
        }
        TotalScore += shape + outcome;
    }

    public Play Tactic(Play oponent, string tactic)
    {
        //X means you need to lose, Y means you need to end the round in a draw, and Z means you need to win. Good luck!"
        if (tactic == "Y")
        {
            return oponent;
        }

        if (oponent == Play.Rock)
        {
            return tactic == "X" ? Play.Scissors : Play.Paper;
        }
        else if (oponent == Play.Paper)
        {
            return tactic == "X" ? Play.Rock : Play.Scissors;
        }
        else if (oponent == Play.Scissors)
        {
            return tactic == "X" ? Play.Paper : Play.Rock;
        }
        Assert.Fail();
        return Play.None;
    }
    
    public static Play PlayFromString(string str)
    {
        var result = Play.None;
        switch (str)
        {
            case "R":
            case "A":
            case "X":
                result = Play.Rock;
                break;
            case "P":
            case "B":
            case "Y":
                result = Play.Paper;
                break;
            case "S":
            case "C":
            case "Z":
                result = Play.Scissors;
                break;
        }
        return result;
    }
}

[TestClass]
public class Day02
{
    private static int Calc(IEnumerable<string> input, bool useTactics)
    {
        var game = new Game();
        foreach (var line in input)
        {
            var split = line.Split();
            var oponent = Game.PlayFromString(split[0]);
            Play me;
            if (useTactics)
            {
                me = game.Tactic(oponent, split[1]);
            }
            else
            {
                me = Game.PlayFromString(split[1]);
            }
            game.Round(oponent, me);
            Console.WriteLine($"opponent={oponent} me={me} TotalScore={game.TotalScore}");
        }
        return game.TotalScore;
    }
    
    private readonly string _example = """
        A Y
        B X
        C Z
        """;

    [TestMethod]
    public void Day02_Part1_Example01()
    {
        var result = Calc(Common.GetLines(_example), useTactics: false);
        Assert.AreEqual(15, result);
    }
    
    [TestMethod]
    public void Day02_Part1()
    {
        var result = Calc(Common.DayInput(nameof(Day02)), useTactics: false);
        Assert.AreEqual(11475, result);
    }
    
    [TestMethod]
    public void Day02_Part2_Example01()
    {
        var result = Calc(Common.GetLines(_example), useTactics: true);
        Assert.AreEqual(12, result);
    }
    
    [TestMethod]
    public void Day02_Part2()
    {
        var result = Calc(Common.DayInput(nameof(Day02)), useTactics: true);
        Assert.AreEqual(16862, result);
    }
    
}
