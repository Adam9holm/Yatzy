
public class NormalYatzyRound : BasicYatzyRound
{
    public NormalYatzyRound(List<IPlayer> players) 
        : base(players) // Pass the whole list of players to the base class
    {
    }

    public override void ShowYatzyRules()
    {
        Console.WriteLine("Rules:");
        Console.WriteLine("- Yatzy is played with five dice");
        Console.WriteLine("- Players take turns rolling the dice up to Three times, setting aside dice between rolls to form desired combinations.");
        Console.WriteLine("- Each combination can only be scored once");
        Console.WriteLine("- The player with the highest total score when no combination is left wins");
        //Thread.Sleep(2000);

    }

    protected override int Multiplier(string category) => 1;


}