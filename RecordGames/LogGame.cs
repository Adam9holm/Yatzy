
public class LogGame
{
    public string TypeOfYatzy { get; set; }
    public int NumberOfPlayers { get; set; }
    public int NumberOfAIPlayers { get; set; }
    public List<string> PlayerNames { get; set; } = new List<string>();
    public string Winner { get; set; }
    public List<string> PlayerScores { get; set; } = new List<string>();

    public LogGame(YatzyGameSession session)
    {
        TypeOfYatzy = session.GameModeName;
        NumberOfPlayers = session.NumberOfPlayers;
        NumberOfAIPlayers = session.NumberOfAIPlayers;
        PlayerNames = session.PlayerNames;
        Winner = session.WinnerName;
        PlayerScores = session.PlayerScores;
    }

    public string GetGameLog()
        {
        Console.Clear();
        Console.WriteLine("Game Over!\n");
        Console.WriteLine("And the winner is....");
        Thread.Sleep(1000); // Vänta 1 sekund innan vinnaren skrivs ut
        Console.WriteLine();
        Console.WriteLine(Winner);
        Console.WriteLine();

        string recap = $"Game Recap:\n" +
                    $"Type of Yatzy: {TypeOfYatzy}\n" +
                    $"Number of Players: {NumberOfPlayers}\n" +
                    $"Number of AI Players: {NumberOfAIPlayers}\n" +
                    $"Final scores:\n{string.Join("\n", PlayerScores)}";

        return recap;
    }

}
