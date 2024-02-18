using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Ticket
{
    public List<int> Cuts;
    private float _time;
    private float _maxTime;


    public Ticket(float time, int amountOfCuts)
    {
        Cuts = new List<int>();
        //generate cut
        _maxTime = time;
        _time = time;

        
        for(int i = 0; i < amountOfCuts; i++)
        {
            
            Cuts.Add(UnityEngine.Random.Range(0, 7));
        }
        Cuts.Distinct();
        Cuts.Sort();
    }

    public bool ValidatePizza(CutInfo[] Pizza)
    {
        int[] PizzaValues = CutInfoArrayToFloat(Pizza);
        int[] TicketValues = Cuts.ToArray();
        Debug.Log("Pizza Values = " + String.Join("",
            new List<int>(PizzaValues)
            .ConvertAll(i => i.ToString())
            .ToArray()));

      //  if (PizzaValues.Length != TicketValues.Length)
      //      return false;

        for (int i = 0; i < PizzaValues.Length; i++)
        {
            bool matching = false;

            for(int j = 0; j < TicketValues.Length; j++)
            {
                if (PizzaValues[i] == TicketValues[j])
                    matching = true;

            }
            if(!matching)
                return false;
        }

        return true;
    }

    private int[] CutInfoArrayToFloat(CutInfo[] array)
    {
        List<int> ints = new List<int>();
        for(int i = 0; i < array.Length; i++)
        {
            ints.Add((int)array[i].Start);
            ints.Add((int)array[i].End);
        }
        ints.Distinct();
        ints.Sort();
        return ints.ToArray();
    }

    public float GetTime()
    {
        return Mathf.Max(0f, _time);
    }
    public float GetTimePercent()
    {
        return _time / _maxTime;
    }
}
