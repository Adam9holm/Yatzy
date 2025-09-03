

class PrintDice (){
    

    private readonly Random rng = new Random();

    public void PrintStaticDice(string name, Dice diceRolls, bool[] holdDice)
    {
        Console.Clear();
        Console.WriteLine(name + "s turn");
        Console.WriteLine();
        string redColor = "\u001b[31m";
        string resetColor = "\u001b[0m";
        var diceValues = diceRolls.ToList();

        for (int i = 0; i < diceValues.Count; i++)
        {
            Console.Write(holdDice[i] ? $"{redColor} ------- {resetColor} " : " -------  ");
        }
        Console.WriteLine();

        for (int i = 0; i < diceValues.Count; i++)
        {
            Console.Write(holdDice[i] ? $"{redColor}|       |{resetColor} " : "|       | ");
        }
        Console.WriteLine();

        for (int i = 0; i < diceValues.Count; i++)
        {
            Console.Write(holdDice[i] ? $"{redColor}|   {diceValues[i]}   |{resetColor} " : $"|   {diceValues[i]}   | ");
        }
        Console.WriteLine();

        for (int i = 0; i < diceValues.Count; i++)
        {
            Console.Write(holdDice[i] ? $"{redColor}|       |{resetColor} " : "|       | ");
        }
        Console.WriteLine();

        for (int i = 0; i < diceValues.Count; i++)
        {
            Console.Write(holdDice[i] ? $"{redColor} ------- {resetColor} " : " -------  ");
        }
        Console.WriteLine();
        Console.WriteLine();
    }
private void PrintRow(List<int> diceValues, bool[] holdDice)
{
    string redColor = "\u001b[31m";
    string resetColor = "\u001b[0m";

    // Rad 1
    for (int i = 0; i < diceValues.Count; i++)
    {
        Console.Write(holdDice[i] ? $"{redColor} ------- {resetColor} " : " -------  ");
    }
    Console.WriteLine();

    // Rad 2
    for (int i = 0; i < diceValues.Count; i++)
    {
        Console.Write(holdDice[i] ? $"{redColor}|       |{resetColor} " : "|       | ");
    }
    Console.WriteLine();

    // Rad 3 (siffran)
    for (int i = 0; i < diceValues.Count; i++)
    {
        string display = diceValues[i] == -1 ? " " : diceValues[i].ToString();
        Console.Write(holdDice[i] ? $"{redColor}|   {display}   |{resetColor} " : $"|   {display}   | ");
    }
    Console.WriteLine();

    // Rad 4
    for (int i = 0; i < diceValues.Count; i++)
    {
        Console.Write(holdDice[i] ? $"{redColor}|       |{resetColor} " : "|       | ");
    }
    Console.WriteLine();

    // Rad 5
    for (int i = 0; i < diceValues.Count; i++)
    {
        Console.Write(holdDice[i] ? $"{redColor} ------- {resetColor} " : " -------  ");
    }
    Console.WriteLine();
    Console.WriteLine();
}

public void PrintAnimatedDice(string name, Dice diceRolls, bool[] holdDice)
{
    Console.Clear();
    Console.WriteLine($"{name}'s turn\n");

    var finalValues = diceRolls.ToList();
    var rng = new Random();
    int diceCount = finalValues.Count;
    var currentValues = new List<int>(new int[diceCount]);
    var rollingIndices = Enumerable.Range(0, diceCount).Where(i => !holdDice[i]).ToList();

    for (int i = 0; i < rollingIndices.Count; i++)
    {
        int index = rollingIndices[i];
        int frames = 6;
        for (int f = 0; f < frames; f++)
        {
            for (int j = 0; j < diceCount; j++)
            {
                if (holdDice[j] || j < index)
                    currentValues[j] = finalValues[j]; // hållna tärningar + tidigare satta
                else if (j == index)
                    currentValues[j] = rng.Next(1, 7);  // animera nuvarande tärning
                else
                    currentValues[j] = -1; // tärningar som inte är på tur ännu
            }

            PrintRow(currentValues, holdDice);
            Thread.Sleep(80);
            Console.SetCursorPosition(0, Console.CursorTop - 6);
        }

        currentValues[index] = finalValues[index];
        PrintRow(currentValues, holdDice);
        Thread.Sleep(200); // vänta 0,5 sek innan nästa tärning börjar
        Console.SetCursorPosition(0, Console.CursorTop - 6);
    }

    PrintRow(finalValues, holdDice);
    Console.WriteLine();
}

}
