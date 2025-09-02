//Vad som måste göras här näst: Points scored fungerar inte utöver att bara välja 1-6 tex four of a kind etc. 
// Och har både points scored och your total score som visas efter varje runda

public class YatzyGameSession
{
    private List<string> DefaultNames = new List<string> { "Bot 1", "Bot 2", "Bot 3", "Bot 4", "Bot 5" };

    List<string> gameModes = new List<string> { "Normal Yatzy", "Large Yatzy", "Fun Yatzy" };
    private int numberOfPlayers;
    private int numberOfAIPlayers;
    private readonly List<IPlayer> players = new List<IPlayer>();
    private BasicYatzyRound TypeOfYatzy;
    private readonly Menu menu = new Menu();

    //private RecordedGame currentGame;
    //private readonly FileManager<RecordedGame> recordedGameManager = new FileManager<RecordedGame>("recordedgames.json");

    public YatzyGameSession()
    {
        //currentGame = new RecordedGame("", "", "", "", 0, 0, "");
    }

    public void Run()
    {
        do
        {
            SelectNumberOfPlayers();
            SelectNumberOfAIPlayers();            
            InitializePlayers();
            SetPlayerNames();
            InitializeGameMode();
            StartGameLoop();
            //GameOver();
        } while (PlayAgain());
    }

        private void SelectNumberOfPlayers()
        {
            List<int> numberOfPlayersList = new List<int> { 2, 3, 4, 5 };
            int selectedIndex = menu.ShowMenuOptions(numberOfPlayersList, "Select your desired number of players between 2 and 5");
            numberOfPlayers = numberOfPlayersList[selectedIndex]; // <-- hämta värdet från listan, inte indexet
            Console.WriteLine($"Total players: {numberOfPlayers}");
            //Thread.Sleep(2000);
        }

        private void SelectNumberOfAIPlayers()
        {
            // Skapa lista från 0 till numberOfPlayers
            List<int> aiOptions = new List<int>();
            for (int i = 0; i <= numberOfPlayers; i++) aiOptions.Add(i);

            int selectedIndex = menu.ShowMenuOptions(aiOptions, "Select number of AI players");
            numberOfAIPlayers = aiOptions[selectedIndex]; // <-- hämta värdet från listan
            Console.WriteLine($"AI players: {numberOfAIPlayers}");
            //Thread.Sleep(2000);
        }


private void InitializePlayers()
{
    players.Clear();

    // Skapa AI-spelare först
    for (int i = 0; i < numberOfAIPlayers; i++)
    {
        players.Add(new ComputerPlayer { Name = $"Bot {i + 1}" });
    }

    // Resten blir interaktiva spelare
    int numberOfInteractivePlayers = numberOfPlayers - numberOfAIPlayers;
    for (int i = 0; i < numberOfInteractivePlayers; i++)
    {
        players.Add(new InteractivePlayer { Name = $"Player {i + 1}" });
    }

    // Sätt AvailableCategories för alla spelare
    foreach (var player in players)
    {
        player.AvailableCategories = new Categories(new List<string>
        {
            "1s", "2s", "3s", "4s", "5s", "6s",
            "Pair", "Two pairs", "Three of a kind", "Four of a kind",
            "Small straight", "Large straight", "Full house", "Chance", "Yatzy"
        });
    }

    // Debug: skriv ut alla spelare
    Console.WriteLine("Players in game:");
    foreach (var p in players)
    {
        Console.WriteLine($"- {p.Name} ({(p is ComputerPlayer ? "AI" : "Interactive")})");
    }
    //Thread.Sleep(2000);
}

private void SetPlayerNames()
{
    int aiIndex = 0; // håller reda på vilken AI-namn som ska användas

    for (int i = 0; i < players.Count; i++)
    {
        if (players[i] is InteractivePlayer interactivePlayer)
        {
            string inputName;
            do
            {
                Console.WriteLine($"Choose a name for player {i + 1} (max 10 characters, cannot be empty or duplicate):");
                inputName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(inputName))
                {
                    Console.WriteLine("Name cannot be empty or only spaces. Please try again.");
                }
                else if (inputName.Length > 10)
                {
                    Console.WriteLine("Name cannot be longer than 10 characters. Please try again.");
                }
                else if (players.Any(p => p.Name == inputName))
                {
                    Console.WriteLine("This name is already taken by another player. Please choose a different name.");
                }
                else if (DefaultNames.Contains(inputName))
                {
                    Console.WriteLine("This name is reserved for AI players. Please choose a different name.");
                }

            } while (string.IsNullOrWhiteSpace(inputName) 
                     || inputName.Length > 10 
                     || players.Any(p => p.Name == inputName) 
                     || DefaultNames.Contains(inputName));

            interactivePlayer.Name = inputName;
        }
        else // AI-spelare
        {
            if (aiIndex < DefaultNames.Count)
            {
                players[i].Name = DefaultNames[aiIndex];
                aiIndex++;
            }
            else
            {
                players[i].Name = $"Bot {aiIndex + 1}";
                aiIndex++;
            }
        }
    }
}


            private void InitializeGameMode()
            {
               

                 var gameFactories = new List<Func<List<IPlayer>, BasicYatzyRound>>
                    {
                        players => new NormalYatzyRound(players),
                        players => new LargeYatzyRound(players),
                        players => new FunYatzyRound(players)
                    };

                    // Visa menyn och få valet
                    int selectedIndex = menu.ShowMenuOptions(gameModes, "Select Game Mode:");

                    // Skapa rätt typ av rundlogik från fabriken
                    TypeOfYatzy = gameFactories[selectedIndex](players);

                    // Visa regler för spelet
                    TypeOfYatzy.ShowYatzyRules();
                            }

        private void StartGameLoop()
        {
            TypeOfYatzy.PlayRound();
        }


            private bool PlayAgain()
    {
                //Thread.Sleep(10000);

        List<string> options = new List<string> { "Yes", "No" };
        int selection = menu.ShowMenuOptions(options, "Do you want to play again?");
        return selection == 0;
    }
}




//     private void LogGame()
//     {
//         currentGame = currentGame with 
//         {
//             Player1TotalScore = players[0].Score,
//             Player2TotalScore = players[1].Score,
//             Winner = players[0].Score > players[1].Score ? players[0].Name :
//                      players[0].Score < players[1].Score ? players[1].Name : "Tie"
//         };

//         DisplayFinalScores();
//         SaveGameResults();
//         DisplayTopScores();
//     }

//     private void DisplayFinalScores()
//     {
//         Console.WriteLine("\n--- Game Over ---");
//         Console.WriteLine($"{players[0].Name}'s final score: {currentGame.Player1TotalScore}");
//         Console.WriteLine($"{players[1].Name}'s final score: {currentGame.Player2TotalScore}");
//         Console.WriteLine($"Winner: {currentGame.Winner}");
//     }

//     private void SaveGameResults()
//     {
//         var existingGames = recordedGameManager.Load() ?? new List<RecordedGame>();
//         existingGames.Add(currentGame);
//         recordedGameManager.Save(existingGames);
//     }

//   private void DisplayTopScores()
// {

//     Console.WriteLine("\nHow many top scores would you like to see?");
//     int topScoresCount = int.TryParse(Console.ReadLine(), out var result) ? result : 5; 

  
//     var topGames = recordedGameManager.Load()?.OrderByDescending(g => g.Player1TotalScore + g.Player2TotalScore).Take(topScoresCount).ToList();
//     if (topGames != null)
//     {
//         Console.WriteLine($"\nTop {topScoresCount} Games:");
//         foreach (var game in topGames)
//         {
//             Console.WriteLine(game.GetGameSummary());
//         }
//     }

    
//     var topRounds = roundScoreManager.Load()?.OrderByDescending(r => r.Player1Points + r.Player2Points).Take(topScoresCount).ToList();
//     if (topRounds != null)
//     {
//         Console.WriteLine($"\nTop {topScoresCount} Rounds:");
//         foreach (var round in topRounds)
//         {
//             Console.WriteLine(round.GetFormattedScore());
//         }
//     }

//     Console.WriteLine("\nPress any key to continue...");
//     Console.ReadKey(); 
// }



// public record RecordedGame(
//     string PlayerMode,
//     string GameMode,
//     string Player1Name,
//     string Player2Name,
//     int Player1TotalScore,
//     int Player2TotalScore,
//     string Winner
// )
// {
//     public string GetGameSummary()
//     {
//         return $"{Player1Name} ({Player1TotalScore}) vs {Player2Name} ({Player2TotalScore}) - Winner: {Winner}";
//     }
// }

// public record RoundScore(
//     string Category1,
//     string Category2,
//     int RoundNumber,
//     int Player1Points,
//     int Player2Points
// )
// {
//     public string GetFormattedScore()
//     {
//         return $"Round {RoundNumber}: {Category1} ({Player1Points}) vs {Category2} ({Player2Points})";
//     }
// }
