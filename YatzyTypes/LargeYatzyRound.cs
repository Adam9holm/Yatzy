

public class LargeYatzyRound : BaseYatzyRound
{
    public LargeYatzyRound(List<IPlayer> players) 
        : base(players) 
    {
    }
    public override void ShowYatzyRules()
    {
        _basicRules.Print();
        Console.WriteLine("You chose fun Yatzy which means that all scores are x10");
        Thread.Sleep(4000);

    }
    protected override int Multiplier(string category) => 10;

}
