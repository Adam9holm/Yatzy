using System;
using System.Linq;
using System.Collections.Generic;

public abstract class BasicYatzyRound
{
    protected List<IPlayer> players = new List<IPlayer>();
    //protected int currentPlayerIndex = 0;
    protected Calculator calc;
    private int numberOfDice = 5;
    public Dice diceRolls;
    //protected Categories categories;
    protected bool[] holdDice;
    private Random random = new Random();
    public int rollCount = 0;
    public string currentChoice;
    private PrintDice DicePrinter = new PrintDice();

    //private readonly FileManager<RoundScore> roundScoreManager = new FileManager<RoundScore>("roundscores.json");


    public BasicYatzyRound(List<IPlayer> players)
    {
        holdDice = new bool[numberOfDice];
        this.players = players;
        diceRolls = new Dice(new int[numberOfDice]);
        calc = new Calculator();
    }


    // Ny metod som tar hand om hela rundan
    public void PlayRound()
    {
        int round = 1;
        while (players.Any(p => p.HasAvailableCategories()))
        {
            var roundScores = new List<int>();
            var choices = new List<string>();

            foreach (var player in players)
            {
                RollDiceAgain(player); // hanterar AI eller människa internt
                int chosenScore = SelectAndRecordScoreCategory(player, diceRolls);
                player.Score += chosenScore;
                Console.WriteLine($"Total score: {player.Score}");
//Thread.Sleep(2000);

                choices.Add(ReturnChoiceAsString());
                roundScores.Add(chosenScore);

                ResetGame();
            }

            // Här kan du spara runda-poäng osv.

            // var currentRoundScore = new RoundScore(
            //     choices[0],
            //     choices[1],
            //     round,
            //     roundScores[0],
            //     roundScores[1]
            //);
            //SaveRoundScore(currentRoundScore);
            round++;
        }
    }

    //     private void SaveRoundScore(RoundScore roundScore)
    // {
    //     var existingScores = roundScoreManager.Load() ?? new List<RoundScore>();
    //     existingScores.Add(roundScore);
    //     roundScoreManager.Save(existingScores);
    // }




    public void ResetGame()
    {
        diceRolls = new Dice(new int[numberOfDice]);
        Array.Clear(holdDice, 0, holdDice.Length);
        rollCount = 0;
    }


    public virtual string ReturnChoiceAsString()
    {
        return currentChoice?.ToString() ?? string.Empty;
    }


    // public bool HasAvailableCategories()
    // {
    //     return players.Availablecategories.Any();
    // }


    private void RollAllDice(string name)
    {
        int[] newValues = new int[numberOfDice];
        for (int i = 0; i < numberOfDice; i++)
        {
            newValues[i] = RollDie();
        }
        diceRolls = new Dice(newValues);
        DicePrinter.PrintAnimatedDice(name, diceRolls, holdDice);
    }

    private void RollDiceWithHold(string name)
    {
        int[] newValues = diceRolls.ToArray();
        for (int i = 0; i < newValues.Length; i++)
        {
            if (!holdDice[i])
            {
                newValues[i] = RollDie();
            }
        }
        diceRolls = new Dice(newValues);
        DicePrinter.PrintAnimatedDice(name, diceRolls, holdDice);
    }

public void RollDiceAgain(IPlayer currentPlayer)
{
            while (rollCount < 3)
            {
                if (rollCount == 0)
                {
                    RollAllDice(currentPlayer.Name); 
                }
                else
                {
                    string[] selectedDice = currentPlayer.SelectDiceToHold(diceRolls);

                    if (selectedDice.Length == 1 && selectedDice[0] == "STOP")
                    {
                        Console.WriteLine("You ended your rolling early!");
                        break;
                    }

                    ApplyHoldDice(selectedDice);
                    RollDiceWithHold(currentPlayer.Name);
                }

                rollCount++;
            }
        }


// Hjälpmetod för att markera tärningar som ska hållas
private void ApplyHoldDice(string[] selectedDice)
{
    Array.Clear(holdDice, 0, holdDice.Length);

    if (selectedDice.Length > 0)
    {
        for (int i = 0; i < diceRolls.values.Length; i++)
        {
            foreach (var die in selectedDice)
            {
                if (int.TryParse(die, out int dieValue))
                {
                    if (diceRolls.values[i] == dieValue)
                    {
                        holdDice[i] = true;
                        break;
                    }
                }
            }
        }
        for (int i = 0; i < diceRolls.Count; i++)
{
    foreach (var die in selectedDice)
    {
        if (int.TryParse(die, out int dieValue))
        {
            if (diceRolls[i] == dieValue)
            {
                holdDice[i] = true;
                break;
            }
        }
    }
}

    }
}
  
    public int SelectAndRecordScoreCategory(IPlayer localPlayer, Dice diceRolls)
    {
        string selectedCategory;
        int score = 0;
        bool validSelection = false;

        if (!localPlayer.HasAvailableCategories())
        {
        Console.WriteLine("No categories left to select.");
        return 0;  
        }
        while (!validSelection)
        {
            selectedCategory = localPlayer.DecideBestCategory(diceRolls, holdDice);
            currentChoice = selectedCategory;

            if (localPlayer.AvailableCategories.Contains(selectedCategory))
            {
                score = CalculateScore(selectedCategory, diceRolls);

                if (score == 0)
                {
                    Console.WriteLine($"{selectedCategory} was crossed out");
                            //Thread.Sleep(4000);
                }
                else
                {
                    Console.WriteLine($"Points scored: {score}");
                            //Thread.Sleep(2000);
                }

                localPlayer.AvailableCategories.Remove(selectedCategory);
                validSelection = true;
            }
            else
            {
                Console.WriteLine($"Category {selectedCategory} has already been chosen or is invalid. Please try again.");
            }
        }
        return score;
    }



    public abstract void ShowYatzyRules();
    protected abstract int Multiplier(string category);

    protected int CalculateBaseScore(string category, Dice diceRolls)
    {
        return category switch
        {
            "1s" => diceRolls.Count(d => d == 1) * 1,
            "2s" => diceRolls.Count(d => d == 2) * 2,
            "3s" => diceRolls.Count(d => d == 3) * 3,
            "4s" => diceRolls.Count(d => d == 4) * 4,
            "5s" => diceRolls.Count(d => d == 5) * 5,
            "6s" => diceRolls.Count(d => d == 6) * 6,
            "Pair" => calc.FindPair(diceRolls),
            "Two pairs" => calc.FindTwoPairs(diceRolls),
            "Three of a kind" => calc.FindOfAKind(diceRolls, 3),
            "Four of a kind" => calc.FindOfAKind(diceRolls, 4),
            "Small straight" => calc.IsSmallStraight(diceRolls) ? 15 : 0,
            "Large straight" => calc.IsLargeStraight(diceRolls) ? 20 : 0,
            "Full house" => calc.IsFullHouse(diceRolls) ? diceRolls.Sum() : 0,
            "Chance" => diceRolls.Sum(),
            "Yatzy" => calc.FindOfAKind(diceRolls, 5) > 0 ? 50 : 0,
            _ => 0
        };
    }

    private int CalculateScore(string category, Dice diceRolls)
    {
        int baseScore = CalculateBaseScore(category, diceRolls);
        int multiplier = Multiplier(category);
        return baseScore * multiplier;
    }

    private int RollDie()
    {
        return random.Next(1, 7);
    }

}
