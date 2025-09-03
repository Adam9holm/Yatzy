

public class ComputerPlayer : IPlayer
{
    public string Name { get; set; }
    public int Score { get; set; }
    public Categories AvailableCategories { get; set; }
    public bool HasAvailableCategories()
            {
                return AvailableCategories.Any();
            }
public bool RollDiceAgain(Dice localDice)
{
    Console.WriteLine("I wonder if I should roll again...");
    Thread.Sleep(1000);

    var calc = new Calculator();
    var available = AvailableCategories.ToList(); 
    var values = localDice.ToArray();
    var counts = values.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());

    // 1. Starka kombinationer
    if ((calc.FindOfAKind(localDice, 5) > 0 && available.Contains("Yatzy")) ||
        (calc.FindOfAKind(localDice, 4) > 0 && available.Contains("Four of a kind")) ||
        (calc.IsFullHouse(localDice) && available.Contains("Full house")) ||
        (calc.IsSmallStraight(localDice) && available.Contains("Small straight")) ||
        (calc.IsLargeStraight(localDice) && available.Contains("Large straight")))
    {
        return false; // stanna
    }

    // 2. Triss
    int triss = calc.FindOfAKind(localDice, 3);
    if (triss > 0)
    {
        int num = counts.First(kvp => kvp.Value >= 3).Key;
        if (triss >= 12 && 
            (available.Contains("Three of a kind") || available.Contains(num + "s")))
            return false;
    }

    // 3. Par
    int pair = calc.FindPair(localDice);
    if (pair > 0)
    {
        int num = counts.Where(kvp => kvp.Value >= 2).OrderByDescending(kvp => kvp.Key).First().Key;
        if (pair >= 10 && 
            (available.Contains("Pair") || available.Contains("Two pairs") || available.Contains(num + "s")))
            return false;
    }

    // 4. Chans
    int chance = calc.Chance(localDice);
    if (chance >= 22 && available.Contains("Chance"))
        return false;

    // 5. Nästan-straight → rulla vidare bara de som inte passar
    var sorted = values.Distinct().OrderBy(v => v).ToList();
    var currentRun = new List<int>();
    var longestRun = new List<int>();

    for (int i = 0; i < sorted.Count; i++)
    {
        if (i == 0 || sorted[i] == sorted[i - 1] + 1)
            currentRun.Add(sorted[i]);
        else
        {
            if (currentRun.Count > longestRun.Count)
                longestRun = new List<int>(currentRun);
            currentRun = new List<int> { sorted[i] };
        }
    }
    if (currentRun.Count > longestRun.Count)
        longestRun = currentRun;

    if (longestRun.Count >= 4 && 
        (available.Contains("Small straight") || available.Contains("Large straight")))
    {
        return false; // håll sekvensen
    }

    // 6. Default → rulla om
    return true;
}

public string[] SelectDiceToHold(Dice localDice)
{
    Console.WriteLine("What would be the best dice to hold...");
    Thread.Sleep(1000);

    var holdIndexes = new List<string>();
    var values = localDice.ToArray();
    var calc = new Calculator();

    // Hjälpvariabler
    var counts = values.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
var available = AvailableCategories.ToList(); 

    // 1. Yatzy → håll allt
    if (calc.FindOfAKind(localDice, 5) > 0 && available.Contains("Yatzy"))
        return Enumerable.Range(1, values.Length).Select(i => i.ToString()).ToArray();

    // 2. Fyrtal → håll fyrtalet
    if (calc.FindOfAKind(localDice, 4) > 0)
    {
        int num = counts.First(kvp => kvp.Value >= 4).Key;
        return values.Select((v, i) => v == num ? (i + 1).ToString() : null)
                     .Where(x => x != null).ToArray();
    }

    // 3. Triss → håll trissen, särskilt om "Treor", "Sexor" etc. finns kvar
    if (calc.FindOfAKind(localDice, 3) > 0)
    {
        int num = counts.First(kvp => kvp.Value >= 3).Key;
        string catName = num + "s"; // ex: "6s"
        if (available.Contains(catName) || available.Contains("Three of a kind") || available.Contains("Full house"))
        {
            return values.Select((v, i) => v == num ? (i + 1).ToString() : null)
                         .Where(x => x != null).ToArray();
        }
    }

    // 4. Par → håll högt par om relevant kategori finns kvar
    if (calc.FindPair(localDice) >= 4)
    {
        int num = counts.Where(kvp => kvp.Value >= 2).OrderByDescending(kvp => kvp.Key).First().Key;
        string catName = num + "s";
        if (available.Contains(catName) || available.Contains("Pair") || available.Contains("Two pairs"))
        {
            return values.Select((v, i) => v == num ? (i + 1).ToString() : null)
                         .Where(x => x != null).ToArray();
        }
    }

    // 5. Straights
    if (available.Contains("Small straight") || available.Contains("Large straight"))
    {
        var sorted = values.Distinct().OrderBy(v => v).ToList();
        var longestRun = new List<int>();
        var currentRun = new List<int>();

        for (int i = 0; i < sorted.Count; i++)
        {
            if (i == 0 || sorted[i] == sorted[i - 1] + 1)
                currentRun.Add(sorted[i]);
            else
            {
                if (currentRun.Count > longestRun.Count)
                    longestRun = new List<int>(currentRun);
                currentRun = new List<int> { sorted[i] };
            }
        }
        if (currentRun.Count > longestRun.Count)
            longestRun = currentRun;

        if (longestRun.Count >= 3) // sikta på straight
        {
            return values.Select((v, i) => longestRun.Contains(v) ? (i + 1).ToString() : null)
                         .Where(x => x != null).ToArray();
        }
    }

    // 6. Om inget annat → håll höga siffror för chans eller "6s", "5s"
    for (int i = 0; i < values.Length; i++)
    {
        if ((values[i] == 6 && available.Contains("6s")) ||
            (values[i] == 5 && available.Contains("5s")) ||
            (values[i] >= 5 && available.Contains("Chance")))
        {
            holdIndexes.Add((i + 1).ToString());
        }
    }

    return holdIndexes.ToArray();
}
public string DecideBestCategory(Dice localDice, bool[] holdDice)
{
    Console.WriteLine("Available categories:");
    foreach (var category in AvailableCategories)
        Console.WriteLine($"- {category}");

    Calculator calc = new Calculator();
    Dictionary<string, int> categoryScores = new Dictionary<string, int>();

    foreach (var category in AvailableCategories)
    {
        int score = category switch
        {
            "1s" => localDice.Count(d => d == 1) * 1,
            "2s" => localDice.Count(d => d == 2) * 2,
            "3s" => localDice.Count(d => d == 3) * 3,
            "4s" => localDice.Count(d => d == 4) * 4,
            "5s" => localDice.Count(d => d == 5) * 5,
            "6s" => localDice.Count(d => d == 6) * 6,
            "Pair" => calc.FindPair(localDice), // OBS: utan *2
            "Two pair" => calc.FindTwoPairs(localDice),
            "Three of a kind" => calc.FindOfAKind(localDice, 3),
            "Four of a kind" => calc.FindOfAKind(localDice, 4),
            "Small straight" => calc.IsSmallStraight(localDice) ? 15 : 0,
            "Large straight" => calc.IsLargeStraight(localDice) ? 20 : 0,
            "Full house" => calc.IsFullHouse(localDice) ? 25 : 0,
            "Chance" => localDice.Sum(),
            "Yatzy" => calc.FindOfAKind(localDice, 5) > 0 ? 50 : 0,
            _ => 0
        };

        categoryScores[category] = score;
    }

    var bestScoringCategory = categoryScores
                                .OrderByDescending(c => c.Value)
                                .FirstOrDefault();

    if (bestScoringCategory.Key != null && bestScoringCategory.Value > 0)
    {
        Console.WriteLine($"{bestScoringCategory.Key} chosen with {bestScoringCategory.Value} points!");
        Thread.Sleep(2000);
        return bestScoringCategory.Key;
    }

    // Fallback: välj en "billig" kategori att korsa ut (1s, 2s först)
    string[] lowPriority = { "1s", "2s", "3s" };
    string categoryToCross = AvailableCategories.FirstOrDefault(c => lowPriority.Contains(c)) 
                              ?? AvailableCategories.First(); // om inget "lågt" finns kvar

    Console.WriteLine($"No good category, crossing out: {categoryToCross}");
    Thread.Sleep(2000);
    return categoryToCross;
}

}