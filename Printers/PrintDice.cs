

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
            Console.Write(holdDice[i] ? $"{redColor} ------- {resetColor} " : " -------  ");
        Console.WriteLine();

        // Rad 2
        for (int i = 0; i < diceValues.Count; i++)
            Console.Write(holdDice[i] ? $"{redColor}|       |{resetColor} " : "|       | ");
        Console.WriteLine();

        // Rad 3 (siffran)
        for (int i = 0; i < diceValues.Count; i++)
            Console.Write(holdDice[i] ? $"{redColor}|   {diceValues[i]}   |{resetColor} " : $"|   {diceValues[i]}   | ");
        Console.WriteLine();

        // Rad 4
        for (int i = 0; i < diceValues.Count; i++)
            Console.Write(holdDice[i] ? $"{redColor}|       |{resetColor} " : "|       | ");
        Console.WriteLine();

        // Rad 5
        for (int i = 0; i < diceValues.Count; i++)
            Console.Write(holdDice[i] ? $"{redColor} ------- {resetColor} " : " -------  ");
        Console.WriteLine();
        Console.WriteLine();
    }

    public void PrintAnimatedDice(string name, Dice diceRolls, bool[] holdDice)
{
    Console.Clear();
    Console.WriteLine(name + "s turn");
    Console.WriteLine();

    var finalValues = diceRolls.ToList();
    var rng = new Random();
    int diceCount = finalValues.Count;

    // Tillfällig lista som håller de tärningar som redan "satt sig"
    var currentValues = new List<int>(new int[diceCount]);

    for (int i = 0; i < diceCount; i++)
    {
        // För varje frame snurrar de tärningar som inte är färdiga
        int frames = 4;
        for (int f = 0; f < frames; f++)
        {
            for (int j = 0; j < diceCount; j++)
            {
                if (holdDice[j] || j < i)
                    currentValues[j] = finalValues[j]; // redan satta eller hållna
                else
                    currentValues[j] = rng.Next(1, 7); // snurrande tärningar
            }

            PrintRow(currentValues, holdDice);
            //Thread.Sleep(100);
            Console.SetCursorPosition(0, Console.CursorTop - 6);
        }

        // Sätt den aktuella tärningen till sitt slutvärde
        currentValues[i] = finalValues[i];
        PrintRow(currentValues, holdDice);
        //Thread.Sleep(300); // halv sekund mellan varje tärning
        Console.SetCursorPosition(0, Console.CursorTop - 6);
    }

    // Rita slutresultatet
    PrintRow(finalValues, holdDice);
    Console.WriteLine();
}

}
