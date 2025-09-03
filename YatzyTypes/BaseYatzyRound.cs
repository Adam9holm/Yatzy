using System;
using System.Linq;
using System.Collections.Generic;

public abstract class BaseYatzyRound
{
    protected List<IPlayer> _players = new List<IPlayer>();
    protected Calculator _calc;
    protected BasicRules _basicRules = new BasicRules();
    protected bool[] _holdDice;

    private int _numberOfDice = 5;
    private Dice _diceRolls;
    private Random _random = new Random();
    private int _rollCount = 0;
    private string _currentChoice;
    private PrintDice _dicePrinter = new PrintDice();

    public BaseYatzyRound(List<IPlayer> players)
    {
        _holdDice = new bool[_numberOfDice];
        this._players = players;
        _diceRolls = new Dice(new int[_numberOfDice]);
        _calc = new Calculator();
    }


    // Ny metod som tar hand om hela rundan
    public void PlayRound()
    {
        int round = 1;
        while (_players.Any(p => p.HasAvailableCategories()))
        {
            var roundScores = new List<int>();
            var choices = new List<string>();

            foreach (var player in _players)
            {
                RollDiceAgain(player); // hanterar AI eller människa internt
                int chosenScore = SelectAndRecordScoreCategory(player, _diceRolls);
                player.Score += chosenScore;
                Console.WriteLine($"Total score: {player.Score}");
                Thread.Sleep(2000);

                choices.Add(ReturnChoiceAsString());
                roundScores.Add(chosenScore);

                ResetGame();
            }

            round++;
        }
    }



    private void ResetGame()
    {
        _diceRolls = new Dice(new int[_numberOfDice]);
        Array.Clear(_holdDice, 0, _holdDice.Length);
        _rollCount = 0;
    }


    private string ReturnChoiceAsString()
    {
        return _currentChoice?.ToString() ?? string.Empty;
    }


    private void RollAllDice(string name)
    {
        int[] newValues = new int[_numberOfDice];
        for (int i = 0; i < _numberOfDice; i++)
        {
            newValues[i] = RollDie();
        }
        _diceRolls = new Dice(newValues);
        _dicePrinter.PrintAnimatedDice(name, _diceRolls, _holdDice);
    }

    private void RollDiceWithHold(string name)
    {
        int[] newValues = _diceRolls.ToArray();
        for (int i = 0; i < newValues.Length; i++)
        {
            if (!_holdDice[i])
            {
                newValues[i] = RollDie();
            }
        }
        _diceRolls = new Dice(newValues);
        _dicePrinter.PrintAnimatedDice(name, _diceRolls, _holdDice);
    }

        public void RollDiceAgain(IPlayer currentPlayer)
        {
            while (_rollCount < 3)
            {
                if (_rollCount == 0)
                {
                    RollAllDice(currentPlayer.Name); 
                }
                else
                {
                    string[] selectedDice = currentPlayer.SelectDiceToHold(_diceRolls);

                    if (selectedDice.Length == 1 && selectedDice[0] == "STOP")
                    {
                        Console.WriteLine("You ended your rolling early!");
                        break;
                    }

                    ApplyHoldDice(selectedDice);
                    RollDiceWithHold(currentPlayer.Name);
                }

                _rollCount++;
            }
        }


// Hjälpmetod för att markera tärningar som ska hållas
        private void ApplyHoldDice(string[] selectedDice)
        {
            Array.Clear(_holdDice, 0, _holdDice.Length);

            if (selectedDice.Length > 0)
            {
                for (int i = 0; i < _diceRolls.values.Length; i++)
                {
                    foreach (var die in selectedDice)
                    {
                        if (int.TryParse(die, out int dieValue))
                        {
                            if (_diceRolls.values[i] == dieValue)
                            {
                                _holdDice[i] = true;
                                break;
                            }
                        }
                    }
                }
                for (int i = 0; i < _diceRolls.Count; i++)
                {
                    foreach (var die in selectedDice)
                    {
                        if (int.TryParse(die, out int dieValue))
                        {
                            if (_diceRolls[i] == dieValue)
                            {
                                _holdDice[i] = true;
                                break;
                            }
                        }
                    }
                }

            }
        }
  
    private int SelectAndRecordScoreCategory(IPlayer localPlayer, Dice diceRolls)
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
            selectedCategory = localPlayer.DecideBestCategory(diceRolls, _holdDice);
            _currentChoice = selectedCategory;

            if (localPlayer.AvailableCategories.Contains(selectedCategory))
            {
                score = CalculateScore(selectedCategory, diceRolls);

                if (score == 0)
                {
                    Console.WriteLine($"{selectedCategory} was crossed out");
                            Thread.Sleep(4000);
                }
                else
                {
                    Console.WriteLine($"Points scored: {score}");
                            Thread.Sleep(2000);
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


    private int CalculateScore(string category, Dice diceRolls)
    {
        int baseScore = CalculateBaseScore(category, diceRolls);
        int multiplier = Multiplier(category);
        return baseScore * multiplier;
    }

    private int RollDie()
    {
        return _random.Next(1, 7);
    }
    public abstract void ShowYatzyRules();
    protected abstract int Multiplier(string category);

    private int CalculateBaseScore(string category, Dice diceRolls)
    {
        return category switch
        {
            "1s" => diceRolls.Count(d => d == 1) * 1,
            "2s" => diceRolls.Count(d => d == 2) * 2,
            "3s" => diceRolls.Count(d => d == 3) * 3,
            "4s" => diceRolls.Count(d => d == 4) * 4,
            "5s" => diceRolls.Count(d => d == 5) * 5,
            "6s" => diceRolls.Count(d => d == 6) * 6,
            "Pair" => _calc.FindPair(diceRolls),
            "Two pairs" => _calc.FindTwoPairs(diceRolls),
            "Three of a kind" => _calc.FindOfAKind(diceRolls, 3),
            "Four of a kind" => _calc.FindOfAKind(diceRolls, 4),
            "Small straight" => _calc.IsSmallStraight(diceRolls) ? 15 : 0,
            "Large straight" => _calc.IsLargeStraight(diceRolls) ? 20 : 0,
            "Full house" => _calc.IsFullHouse(diceRolls) ? diceRolls.Sum() : 0,
            "Chance" => diceRolls.Sum(),
            "Yatzy" => _calc.FindOfAKind(diceRolls, 5) > 0 ? 50 : 0,
            _ => 0
        };
    }

}
