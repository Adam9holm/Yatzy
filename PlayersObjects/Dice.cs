using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//Krav 5 Enumerable 
// Denna IEnumerable-implementation filtrerar tärningsvärden vid iteration.
// Endast värden mellan 1 och 6 returneras, vilket gör att exempelvis ogiltiga värden (t.ex. 0 eller >6)
// automatiskt utesluts i foreach-loopar. Detta visar hur IEnumerable kan användas för att påverka 
// iterationens logik direkt, vilket gör implementationen funktionellt meningsfull – 
// till skillnad från en standard-iteration via ToArray eller ToList.

public class Dice : IEnumerable<int>
{
    public int[] values;

    public Dice(int[] initialValues)
    {
        values = initialValues ?? throw new ArgumentNullException(nameof(initialValues));
    }

    public int this[int index] => values[index];

    public int Count => values.Length;

    public int Sum() => values.Sum();

    public int Max() => values.Max();

    public int Min() => values.Min();

    public int[] ToArray() => values.ToArray();

    public List<int> ToList() => values.ToList();

    public void SetValues(int[] newValues)
    {
        if (newValues.Length != values.Length)
            throw new ArgumentException("Invalid dice length");
        values = newValues;
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", values)}]";
    }

    public IEnumerator<int> GetEnumerator()
    {
        foreach (var val in values)
        {
            if (val >= 1 && val <= 6)
                yield return val;
        }
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return values.GetEnumerator();
    }
}
