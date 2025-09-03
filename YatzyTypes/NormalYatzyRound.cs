
public class NormalYatzyRound : BaseYatzyRound
{
    public NormalYatzyRound(List<IPlayer> players) 
        : base(players) 
    {
    }

    public override void ShowYatzyRules()
    {
        _basicRules.Print();
        Thread.Sleep(4000);
    }

    protected override int Multiplier(string category) => 1;


}