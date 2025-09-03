using System;
using System.Collections.Generic;
using System.Linq;

public class FunYatzyRound : BaseYatzyRound
{
    private readonly Dictionary<string, int> multiplierCategories;
    public FunYatzyRound(List<IPlayer> players) 
        : base(players)
    {
        multiplierCategories = SelectRandomCategoriesWithMultiplier();
    }

    private Dictionary<string, int> SelectRandomCategoriesWithMultiplier()
    {
        var cats = _players[0].AvailableCategories.ToList(); 
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
        _basicRules.Print();
        Console.WriteLine("- You chose fun Yatzy which means that Three random of your categories will be multiplied in points with a random number between 3-10");
        ShowMultiplierCategories();
        Thread.Sleep(2000);

    }
        public void ShowMultiplierCategories()
    {
        Console.WriteLine("");
        Console.WriteLine("Multiplier Categories:");
        foreach (var entry in multiplierCategories)
        {
            Console.WriteLine($"- {entry.Key}: x{entry.Value}");
        }
        Thread.Sleep(4000);
    }
}


