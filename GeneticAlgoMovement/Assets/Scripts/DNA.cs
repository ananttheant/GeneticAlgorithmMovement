using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    List<int> genes = new List<int>();

    int DNALength = 0;
    int maxValues = 0;

    /// <summary>
    /// Constructor for the class, sets basic values
    /// </summary>
    /// <param name="len"></param>
    /// <param name="val"></param>
    public DNA(int len, int val)
    {
        DNALength = len;
        maxValues = val;
        SetRandom();
    }

    /// <summary>
    /// Sets the gene sequence randomly based on <see cref="maxValues"/> and <see cref="DNALength"/>
    /// </summary>
    public void SetRandom()
    {
        genes.Clear();
        for (int i = 0; i < DNALength; i++)
        {
            genes.Add(Random.Range(0, maxValues));
        }
    }

    /// <summary>
    /// To hard code certain gene sequence if needed
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="value"></param>
    public void SetInt(int pos, int value)
    {
        genes[pos] = value;
    }


    /// <summary>
    /// Splits the gene of the parent and combines them back together into a single DNA
    /// </summary>
    /// <param name="parent1"></param>
    /// <param name="parent2"></param>
    public void Combine(DNA parent1, DNA parent2)
    {
        for (int i = 0; i < DNALength; i++)
        {
            //Splits into half
            if (i < DNALength / 2)
            {
                int c = parent1.genes[i];
                genes[i] = c;
            }
            else
            {
                int c = parent2.genes[i];
                genes[i] = c;
            }
        }
    }

    /// <summary>
    /// Mutates/ changes the particular value of the gene sequence based on Random range
    /// from <see cref="maxValues"/> and <see cref="DNALength"/>
    /// </summary>
    public void Mutate()
    {
        genes[Random.Range(0, DNALength)] = Random.Range(0, maxValues);
    }


    /// <summary>
    /// Gets the Gene at the position
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetGene(int pos)
    {
        return genes[pos];
    }

}
