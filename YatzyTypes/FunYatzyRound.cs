using System;
using System.Collections.Generic;
using System.Linq;

public class FunYatzyRound : BasicYatzyRound
{
    private readonly Dictionary<string, int> multiplierCategories;

    public FunYatzyRound(List<IPlayer> players) 
        : base(players)
    {
        multiplierCategories = SelectRandomCategoriesWithMultiplier();
    }

    private Dictionary<string, int> SelectRandomCategoriesWithMultiplier()
    {
        // Använd ToList() för att få en lista av kategorier
        var cats = players[0].AvailableCategories.ToList();  // Ersätt categories.availableCategories med categories.ToList()

        Random rand = new Random();
        var selectedCategories = cats.OrderBy(x => rand.Next()).Take(3).ToList();

        return selectedCategories.ToDictionary(
            category => category,
            category => rand.Next(3, 11) 
        );
    }

    protected override int Multiplier(string category)
    {
        return multiplierCategories.TryGetValue(category, out int multiplier) ? multiplier : 1;
    }

    public override void ShowYatzyRules()
    {
        Console.WriteLine("Rules:");
        Console.WriteLine("- Yatzy is played with five dice");
        Console.WriteLine("- Players take turns rolling the dice up to Three times, setting aside dice between rolls to form desired combinations.");
        Console.WriteLine("- Each combination can only be scored once");
        Console.WriteLine("- The player with the highest total score when no combination is left wins");
        Console.WriteLine("- You chose fun Yatzy which means that Three random of your categories will be multiplied in points with a random number between 3-10");
        ShowMultiplierCategories();

                //Thread.Sleep(2000);

    }
        public void ShowMultiplierCategories()
    {
        Console.WriteLine("");
        Console.WriteLine("Multiplier Categories:");
        foreach (var entry in multiplierCategories)
        {
            Console.WriteLine($"- {entry.Key}: x{entry.Value}");
        }

                //Thread.Sleep(2000);

    }
}


