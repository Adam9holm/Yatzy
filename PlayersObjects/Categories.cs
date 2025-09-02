using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Den implementerar IEnumerable<string> så att vi kan använda foreach och LINQ på ett smidigt sätt.
//Vi har gjort denna klass för att:
//
// 1. Begränsa åtkomst så att kategorier hanteras kontrollerat (t.ex. genom att bara ta bort kategorier via vår Remove-metod).
// 2. Göra det tydligare i koden att vi jobbar med just kategorier, inte en vanlig lista med strängar.
// 3. Underlätta om vi vill lägga till extra funktionalitet i framtiden, som att hålla reda på valda kategorier eller filtrera på speciella sätt.
//
// Sammanfattningsvis fungerar denna implementering bra och är en medveten design för att kapsla in kategorihanteringen i spelet,
// vilket gör koden mer strukturerad och lättare att vidareutveckla.

public class Categories : IEnumerable<string>
{
    private readonly List<string> categories;
    private readonly HashSet<string> usedCategories = new HashSet<string>();

    public Categories(IEnumerable<string> categoryList)
    {
        categories = new List<string>(categoryList);
    }


    public IEnumerator<string> GetEnumerator()
    {
        return categories.Where(c => !usedCategories.Contains(c)).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Contains(string category) => categories.Contains(category);

    public void Remove(string category) => categories.Remove(category);

    public void MarkUsed(string category)
    {
        if (categories.Contains(category))
        {
            usedCategories.Add(category);
        }
    }

    public bool IsUsed(string category) => usedCategories.Contains(category);

    public int Count => categories.Count - usedCategories.Count;

    public bool CanChoose(string category) => categories.Contains(category) && !usedCategories.Contains(category);
}
