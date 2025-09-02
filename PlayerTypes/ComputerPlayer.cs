public class ComputerPlayer : IPlayer
{
    public string Name { get; set; }
    public int Score { get; set; }
    public Categories AvailableCategories { get; set; }
        public bool HasAvailableCategories()
            {
                return AvailableCategories.Any();
            }



    //NY AV CHAT
    public bool RollDiceAgain(Dice localDice)
{
    Console.WriteLine("AI analyserar om den ska rulla igen...");
    //Thread.Sleep(1000);

    var calc = new Calculator();

    // 1. Starka kombinationer – håll alltid
    if (calc.FindOfAKind(localDice, 5) > 0 ||   // yatzy
        calc.FindOfAKind(localDice, 4) > 0 ||   // fyrtal
        calc.IsFullHouse(localDice) || 
        calc.IsSmallStraight(localDice) || 
        calc.IsLargeStraight(localDice) || 
        calc.FindTwoPairs(localDice) > 0)
    {
        return false; // stanna
    }

    // 2. Triss
    int triss = calc.FindOfAKind(localDice, 3);
    if (triss > 0)
    {
        // Om trissen är hög (t.ex. 15+ poäng) → stanna
        if (triss >= 12)
            return false;
    }

    // 3. Par / två höga tärningar
    int pair = calc.FindPair(localDice);
    if (pair > 0)
    {
        // Håll om paret är högt (t.ex. 10 eller mer)
        if (pair >= 10)
            return false;
    }

    // 4. Chans – summa av alla tärningar
    int chance = calc.Chance(localDice);
    if (chance >= 22) // t.ex. 22+ poäng → stanna
        return false;

    // 5. Annars rulla om
    return true;
}


// NY AV CHAT:
    public string[] SelectDiceToHold(Dice localDice)
{
    Console.WriteLine("AI analyserar bästa draget...");
    //Thread.Sleep(1000);

    var holdIndexes = new List<string>();
    var values = localDice.ToArray();
    var calc = new Calculator();

    // 1. Om vi redan har yatzy – håll allt
    if (calc.FindOfAKind(localDice, 5) > 0)
        return Enumerable.Range(1, localDice.Count).Select(i => i.ToString()).ToArray();

    // 2. Prioritera nästan-yatzy eller fyrtal (t.ex. tre lika)
    var counts = values.GroupBy(v => v).ToDictionary(g => g.Key, g => g.Count());
    var bestGroup = counts.OrderByDescending(kvp => kvp.Value).ThenByDescending(kvp => kvp.Key).First();

    if (bestGroup.Value >= 2)
    {
        // Håll alla tärningar av den "bästa" sorten (t.ex. två 6:or)
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] == bestGroup.Key)
                holdIndexes.Add((i + 1).ToString());
        }
        return holdIndexes.ToArray();
    }

    // 3. Nästan-straight: håll ihop sekvenser på minst 4
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

    if (longestRun.Count >= 4)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (longestRun.Contains(values[i]))
                holdIndexes.Add((i + 1).ToString());
        }
        return holdIndexes.ToArray();
    }

    // 4. Annars: håll höga tärningar (6:or och 5:or) för chans
    for (int i = 0; i < values.Length; i++)
    {
        if (values[i] >= 5)
            holdIndexes.Add((i + 1).ToString());
    }

    return holdIndexes.ToArray();
}


    public string DecideBestCategory(Dice localDice, bool[] holdDice)
    {
        Console.WriteLine("Available categories:");
        foreach (var category in AvailableCategories)
        {
            Console.WriteLine($"- {category}");

        }

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
                "Pair" => calc.FindPair(localDice) * 2,
                "Two pair" => calc.FindTwoPairs(localDice),
                "Three of a kind" => calc.FindOfAKind(localDice, 3) * 3,
                "Four of a kind" => calc.FindOfAKind(localDice, 4) * 4,
                "Small straight" => calc.IsSmallStraight(localDice) ? 15 : 0,
                "Large straight" => calc.IsLargeStraight(localDice) ? 20 : 0,
                "Full house" => calc.IsFullHouse(localDice) ? 25 : 0,
                "Chance" => localDice.Sum(),
                "Yatzy" => calc.FindOfAKind(localDice, 5) > 0 ? 50 : 0,
                _ => 0
            };

            categoryScores[category] = score;
        }

        var bestScoringCategory = categoryScores.OrderByDescending(c => c.Value)
                                              .FirstOrDefault();

        if (bestScoringCategory.Key != null && bestScoringCategory.Value > 0)
        {
            Console.WriteLine(bestScoringCategory.Key + " Chosen!");
                    //Thread.Sleep(2000);

            return bestScoringCategory.Key;
        }
        
        // Fallback om ingen bra kategori hittades
        var availableCategories = AvailableCategories.ToList();
        string randomCategory = availableCategories[new Random().Next(availableCategories.Count)];
        Console.WriteLine($"No good category, crossing out: {randomCategory}");
                //Thread.Sleep(2000);

        return randomCategory;
    }
}