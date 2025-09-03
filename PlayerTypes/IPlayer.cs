public interface IPlayer 
{
    string Name {get; set;}
    int Score {get; set;}
    Categories AvailableCategories { get; set; } 
    bool HasAvailableCategories();
    string[] SelectDiceToHold(Dice dice);
    public string DecideBestCategory(Dice dice, bool[] holdDice);

}



