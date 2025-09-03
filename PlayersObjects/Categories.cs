using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


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
