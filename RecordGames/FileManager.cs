using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
// I den nuvarande implementationen fungerar helheten betydligt bättre än tidigare.
//FileManager har nu generaliserats och är inte längre hårdkopplad till specifika klasser, 
//vilket gör den återanvändbar i flera sammanhang. Samtidigt har strukturen blivit mer meningsfull 
//eftersom RecordedGame och RoundScore inte bara är simpla data-wrappers, 
//utan fyller en tydlig funktion: de lagrar spelhistorik och rundresultat 
//på ett strukturerat och lättläst sätt.

//Det gör att vi faktiskt drar nytta av den generiska designen – FileManager kan användas för att spara både 
//ronder och spel, och vi kan skriva ut datan i valfritt format via en formatter-funktion. 
//Det är dessutom enkelt att justera mängden resultat vi vill se, vilket gör logiken dynamisk snarare än hårdkodad.

//Sammanfattningsvis har vi gått från att bara demonstrera ett generiskt koncept till att faktiskt 
//använda det på ett sätt som gör applikationen både robustare och mer flexibel.
public class FileManager<T> where T : class
{
    private readonly string _filePath;

    public FileManager(string filePath)
    {
        _filePath = filePath;
    }

    public void Save(List<T> items)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(items, options);
        File.WriteAllText(_filePath, json);
    }

    public List<T> Load()
    {
        if (!File.Exists(_filePath)) return new List<T>();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
    }

    public void PrintAll(Func<T, string> formatter)
    {
        var items = Load();
        items.ForEach(item => Console.WriteLine(formatter(item)));
        Thread.Sleep(4000);

    }
}



