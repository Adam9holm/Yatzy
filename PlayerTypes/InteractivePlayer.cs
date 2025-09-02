using System;
using System.Linq;

public class InteractivePlayer : IPlayer
{
    public string Name { get; set; }
    public int Score { get; set; }
    private readonly Menu menu = new Menu();
    public Categories AvailableCategories { get; set; }
    public bool HasAvailableCategories()
        {
            return AvailableCategories.Any();
        }
     public string[] SelectDiceToHold(Dice localDice)
    {
        // var diceIterableObj = new DiceIterable(localDice);
        // var diceIterable = diceIterableObj.CreateIterator();

        //string diceString = string.Join(", ", localDice.dice);
        //Console.WriteLine($"Your dice: {diceString}");
        Console.WriteLine("Enter which dice values to hold (comma-separated, e.g. 6,6,2).");
        Console.WriteLine("Press Backspace if you are done rolling.");

        // Lyssna på första knapptryck
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        // Om användaren trycker Backspace direkt → avsluta
        if (keyInfo.Key == ConsoleKey.Backspace)
        {
            return new string[] { "STOP" }; // markerar att spelaren vill avsluta rullningarna
        }

        // Annars – läs in raden (inklusive den första tangenten vi redan tryckt)
        Console.Write(keyInfo.KeyChar);
        string input = keyInfo.KeyChar + (Console.ReadLine() ?? "");

        string[] diceToHold = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

        // Skriv ut vilka tärningar användaren försökte hålla
        Console.WriteLine("\nYou chose to hold: " + string.Join(", ", diceToHold));

        return diceToHold;
    }

    public string DecideBestCategory(Dice localDice, bool[] holdDice)
    {

        int selectedCategoryIndex = menu.MenuWithDice(AvailableCategories.ToList(), "Choose a category to score points: ", Name, localDice, holdDice);
        string selectedCategory = AvailableCategories.ToList()[selectedCategoryIndex];
        
        Console.WriteLine($"You chose {selectedCategory}!");
        return selectedCategory;
    }
}




