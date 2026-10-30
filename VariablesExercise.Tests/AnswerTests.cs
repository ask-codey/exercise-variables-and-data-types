using System.Globalization;
using System.Reflection;

namespace VariablesExercise.Tests;

/// <summary>
/// Runs your program once and checks each answer it prints. A test fails until its problem prints a line like
/// "13) shotsTaken: 10", so read a failing test's message: it says which problem and what it expected.
/// </summary>
[TestClass]
public sealed class AnswerTests
{
    private static Dictionary<string, string> answers = [];

    [ClassInitialize]
    public static void RunTheProgram(TestContext _)
    {
        var output = new StringWriter();
        var console = Console.Out;
        Console.SetOut(output);
        try
        {
            Assembly.Load("VariablesExercise").EntryPoint!.Invoke(null, [Array.Empty<string>()]);
        }
        finally
        {
            Console.SetOut(console);
        }

        // "13) shotsTaken: 10" -> answers["13"] = "10"
        foreach (var line in output.ToString().Split('\n'))
        {
            var close = line.IndexOf(')');
            var colon = line.LastIndexOf(": ", StringComparison.Ordinal);
            if (close > 0 && colon > close) answers[line[..close].Trim()] = line[(colon + 2)..].Trim();
        }
    }

    private static string Answer(int problem)
    {
        Assert.IsTrue(answers.TryGetValue(problem.ToString(), out var value),
            $"Problem {problem} printed nothing. Print a line like: {problem}) yourVariable: value");
        return value!;
    }

    private static decimal Number(int problem)
    {
        var text = Answer(problem).TrimEnd('%');
        Assert.IsTrue(decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var number),
            $"Problem {problem} printed \"{text}\", which isn't a number.");
        return number;
    }

    // Section 1: declaring variables

    [TestMethod]
    public void Problem01_Name_IsPrinted() => Assert.IsFalse(string.IsNullOrWhiteSpace(Answer(1)), "Problem 1: print your name.");

    [TestMethod]
    public void Problem02_Age_IsAWholeNumber() =>
        Assert.IsTrue(int.TryParse(Answer(2), out _), "Problem 2: an age is a whole number, so store it in an int.");

    [TestMethod]
    public void Problem03_Pi_IsTheFirstFiveDigits() => Assert.AreEqual(3.1415m, Number(3), "Problem 3: the first 5 digits of pi are 3.1415.");

    [TestMethod]
    public void Problem04_BuffetPrice_IsAPrice() => Assert.IsTrue(Number(4) > 0, "Problem 4: print the price of a meal.");

    // Section 2: choosing data types

    [TestMethod]
    public void Problem05_Superhero_IsPrinted() => Assert.IsFalse(string.IsNullOrWhiteSpace(Answer(5)), "Problem 5: print your superhero's name.");

    [TestMethod]
    public void Problem06_TestScore_IsAWholeNumberUpTo100()
    {
        Assert.IsTrue(int.TryParse(Answer(6), out var score), "Problem 6: a score out of 100 questions is a whole number.");
        Assert.IsTrue(score is >= 0 and <= 100, "Problem 6: the score is between 0 and 100.");
    }

    [TestMethod]
    public void Problem07_UsPopulation_IsAWholeNumber() =>
        Assert.IsTrue(long.TryParse(Answer(7), out var people) && people > 300_000_000, "Problem 7: the US has more than 300 million people.");

    [TestMethod]
    public void Problem08_WorldPopulation_NeedsALong() =>
        Assert.IsTrue(long.TryParse(Answer(8), out var people) && people > int.MaxValue,
            "Problem 8: the world has more than 8 billion people, too many for an int: use a long.");

    [TestMethod]
    public void Problem09_IsMouseWireless_IsTrueOrFalse() =>
        Assert.IsTrue(bool.TryParse(Answer(9), out _), "Problem 9: a yes-or-no value is a bool: True or False.");

    [TestMethod]
    public void Problem10_Latitude_IsBetweenMinus90And90() =>
        Assert.IsTrue(Number(10) is >= -90 and <= 90, "Problem 10: a latitude is between -90 and 90.");

    [TestMethod]
    public void Problem11_Longitude_IsBetweenMinus180And180() =>
        Assert.IsTrue(Number(11) is >= -180 and <= 180, "Problem 11: a longitude is between -180 and 180.");

    [TestMethod]
    public void Problem12_SharePrice_IsAPrice() => Assert.IsTrue(Number(12) > 0, "Problem 12: print the share price.");

    // Section 3: Backyard Basketball

    [TestMethod]
    public void Problem13_ShotsTaken() => Assert.AreEqual(10m, Number(13));

    [TestMethod]
    public void Problem14_TwoPointShotsMade() => Assert.AreEqual(11m, Number(14));

    [TestMethod]
    public void Problem15_ShotsMadeTogether() => Assert.AreEqual(18m, Number(15));

    [TestMethod]
    public void Problem16_ShotsMadeWithOneMoreEach() => Assert.AreEqual(24m, Number(16));

    [TestMethod]
    public void Problem17_PercentageMade() =>
        Assert.AreEqual(73.33m, Math.Round(Number(17), 2), "Problem 17: 11 made out of 15 taken. Divide with a double or decimal, not two ints.");

    [TestMethod]
    public void Problem18_ShotsNeededToWin() => Assert.AreEqual(16m, Number(18));

    // Section 4: Claire's Cookies

    [TestMethod]
    public void Problem19_CookiesEach() => Assert.AreEqual(2m, Number(19));

    [TestMethod]
    public void Problem20_CookiesLeftOver() => Assert.AreEqual(8m, Number(20));

    [TestMethod]
    public void Problem21_DozensForTheParty() => Assert.AreEqual(4m, Number(21));

    [TestMethod]
    public void Problem22_DozensForTheTestParty() => Assert.AreEqual(3m, Number(22));

    [TestMethod]
    public void Problem23_CostOfFourDozen() => Assert.AreEqual(51.96m, Number(23), "Problem 23: money is a decimal (12.99m).");

    [TestMethod]
    public void Problem24_TotalDozens() => Assert.AreEqual(5m, Number(24));

    [TestMethod]
    public void Problem25_TotalCost() => Assert.AreEqual(72.95m, Number(25), "Problem 25: add up the cost of each kind. Money is a decimal, so the cents come out exact.");

    [TestMethod]
    public void Problem26_SnickerdoodlesLeftOver() => Assert.AreEqual(3m, Number(26));

    [TestMethod]
    public void Problem27_ChocolateChipLeftOver() => Assert.AreEqual(9m, Number(27));

    [TestMethod]
    public void Problem28_FrostedChocolateChipLeftOver() => Assert.AreEqual(6m, Number(28));
}
