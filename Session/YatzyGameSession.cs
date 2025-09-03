public class YatzyGameSession
{
    private List<string> _defaultNames = new List<string> { "Bot 1", "Bot 2", "Bot 3", "Bot 4", "Bot 5" };
    private List<string> _gameModes = new List<string> { "Normal Yatzy", "Large Yatzy", "Fun Yatzy" };

    private int _numberOfPlayers;
    private int _numberOfAIPlayers;
    private List<IPlayer> _players = new List<IPlayer>();
    private BaseYatzyRound _typeOfYatzy;
    private Menu _menu = new Menu();

    public int NumberOfPlayers => _numberOfPlayers;
    public int NumberOfAIPlayers => _numberOfAIPlayers;
    public List<string> PlayerNames => _players.Select(p => p.Name).ToList();
    public string GameModeName => _typeOfYatzy?.GetType().Name ?? "Unknown";

        public string WinnerName => _players
        .OrderByDescending(p => p.Score)
        .FirstOrDefault()?.Name ?? "No winner";

    public List<string> PlayerScores => _players
        .Select(p => $"{p.Name}: {p.Score} points")
        .ToList();


    public YatzyGameSession()
    {
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
            EndGame();
        } while (PlayAgain());
    }

        private void SelectNumberOfPlayers()
        {
            List<int> numberOfPlayersList = new List<int> { 2, 3, 4, 5 };
            int selectedIndex = _menu.ShowMenuOptions(numberOfPlayersList, "Select your desired number of players between 2 and 5");
            _numberOfPlayers = numberOfPlayersList[selectedIndex]; // <-- hämta värdet från listan, inte indexet
            Console.WriteLine($"Total players: {_numberOfPlayers}");
            Thread.Sleep(2000);
        }

        private void SelectNumberOfAIPlayers()
        {
            // Skapa lista från 0 till numberOfPlayers
            List<int> aiOptions = new List<int>();
            for (int i = 0; i <= _numberOfPlayers; i++) aiOptions.Add(i);

            int selectedIndex = _menu.ShowMenuOptions(aiOptions, "Select number of AI players");
            _numberOfAIPlayers = aiOptions[selectedIndex]; // <-- hämta värdet från listan
            Console.WriteLine($"AI players: {_numberOfAIPlayers}");
            Thread.Sleep(2000);
        }


        private void InitializePlayers()
        {
            _players.Clear();

             var defaultCategories = new List<string>
            {
                "1s", "2s", "3s", "4s", "5s", "6s",
                "Pair", "Two pairs", "Three of a kind", "Four of a kind",
                "Small straight", "Large straight", "Full house", "Chance", "Yatzy"
            };

            // Skapa interaktiva spelare först
            int numberOfInteractivePlayers = _numberOfPlayers - _numberOfAIPlayers;
            for (int i = 0; i < numberOfInteractivePlayers; i++)
            {
                _players.Add(new InteractivePlayer
                {
                    Name = $"Player {i + 1}",
                    AvailableCategories = new Categories(defaultCategories)
                });
            }

            // Lägg till AI-spelare
            for (int i = 0; i < _numberOfAIPlayers; i++)
            {
                _players.Add(new ComputerPlayer
                {
                    Name = $"Bot {i + 1}",
                    AvailableCategories = new Categories(defaultCategories)
                });
            }

            // Debug: skriv ut alla spelare
            Console.WriteLine("Players in game:");
            foreach (var p in _players)
            {
                Console.WriteLine($"- {p.Name} ({(p is ComputerPlayer ? "AI" : "Human")})");
            }
            Thread.Sleep(2000);
        }

        private void SetPlayerNames()
        {
            int aiIndex = 0; // håller reda på vilket AI-namn som ska användas

            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i] is InteractivePlayer interactivePlayer)
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
                        else if (_players.Any(p => p.Name == inputName))
                        {
                            Console.WriteLine("This name is already taken by another player. Please choose a different name.");
                        }
                        else if (_defaultNames.Contains(inputName))
                        {
                            Console.WriteLine("This name is reserved for AI players. Please choose a different name.");
                        }

                    } while (string.IsNullOrWhiteSpace(inputName) 
                            || inputName.Length > 10 
                            || _players.Any(p => p.Name == inputName) 
                            || _defaultNames.Contains(inputName));

                    interactivePlayer.Name = inputName;
                }
                else // AI-spelare
                {
                    if (aiIndex < _defaultNames.Count)
                    {
                        _players[i].Name = _defaultNames[aiIndex];
                        aiIndex++;
                    }
                    else
                    {
                        _players[i].Name = $"Bot {aiIndex + 1}";
                        aiIndex++;
                    }
                }
            }
        }


            private void InitializeGameMode()
            {
                 var gameFactories = new List<Func<List<IPlayer>, BaseYatzyRound>>
                    {
                        players => new NormalYatzyRound(players),
                        players => new LargeYatzyRound(players),
                        players => new FunYatzyRound(players)
                    };

                    // Visa menyn och få valet
                    int selectedIndex = _menu.ShowMenuOptions(_gameModes, "Select Game Mode:");

                    // Skapa rätt typ av rundlogik från fabriken
                    _typeOfYatzy = gameFactories[selectedIndex](_players);

                    // Visa regler för spelet
                    _typeOfYatzy.ShowYatzyRules();
                            }

        private void StartGameLoop()
        {
            _typeOfYatzy.PlayRound();
        }

         private void EndGame()
        {
                // Skapa loggen
                var log = new LogGame(this);
                Console.WriteLine(log.GetGameLog());
                Console.WriteLine();
                Console.Write("Press enter to continue");
                Console.ReadLine();
            
        }
        private bool PlayAgain()
        {
            List<string> options = new List<string> { "Yes", "No" };
            int selection = _menu.ShowMenuOptions(options, "Do you want to play again?");
            return selection == 0;
        }

}

