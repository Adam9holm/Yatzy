using System;
using System.Collections.Generic;
using System.Linq;

public class Calculator
{

    private Dictionary<int, int> CountDiceValues(Dice diceRolls)
    {
        return diceRolls
            .GroupBy(val => val)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public int FindPair(Dice diceRolls)
    {
        return CountDiceValues(diceRolls)
            .Where(kvp => kvp.Value >= 2)
            .OrderByDescending(kvp => kvp.Key)
            .Select(kvp => kvp.Key * 2)
            .FirstOrDefault();
    }

    public int FindTwoPairs(Dice diceRolls)
    {
        var pairs = CountDiceValues(diceRolls)
            .Where(kvp => kvp.Value >= 2)
            .OrderByDescending(kvp => kvp.Key)
            .Take(2)
            .ToList();

        return pairs.Count == 2
            ? pairs[0].Key * 2 + pairs[1].Key * 2
            : 0;
    }

    public int FindOfAKind(Dice diceRolls, int targetCount)
    {
        return CountDiceValues(diceRolls)
            .Where(kvp => kvp.Value >= targetCount)
            .OrderByDescending(kvp => kvp.Key)
            .Select(kvp => kvp.Key * targetCount)
            .FirstOrDefault();
    }

    public bool IsSmallStraight(Dice diceRolls)
    {
        var required = new HashSet<int> { 1, 2, 3, 4, 5 };
        return new HashSet<int>(diceRolls).SetEquals(required);
    }

    public bool IsLargeStraight(Dice diceRolls)
    {
        var required = new HashSet<int> { 2, 3, 4, 5, 6 };
        return new HashSet<int>(diceRolls).SetEquals(required);
    }

    public bool IsFullHouse(Dice diceRolls)
    {
        var values = CountDiceValues(diceRolls).Values.OrderByDescending(v => v).ToList();
        return values.Count == 2 && values[0] == 3 && values[1] == 2;
    }

    public int Chance(Dice diceRolls)
    {
        return diceRolls.Sum();
    }
}
