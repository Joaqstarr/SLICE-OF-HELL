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




    public Ticket( float time, int amountOfCuts, bool ready = false)
    {
        if (ready)
        {
            Cuts = new List<int>();
            Cuts.Add(0);
            Cuts.Add(4);
            Cuts.Add(2);
            Cuts.Add(6);

            return;

        }

        Cuts = new List<int>();
        //generate cut
        _maxTime = time;
        _time = time;

        
        for(int i = 0; i < amountOfCuts; i++)
        {

            Cuts.Add(UnityEngine.Random.Range(0, 7));
        }

        Cuts.Sort();

        MakeUnique(Cuts);

        if(Cuts.Count == 1 ) { 
            int ranInt = UnityEngine.Random.Range(1, 6);
      
            Cuts.Add((Cuts[0] + ranInt)%7);
            Debug.Log("Post Unique = " + String.Join("",
              new List<int>(Cuts)
              .ConvertAll(i => i.ToString())
              .ToArray()));
        }
        Cuts.Sort();
        

    }
    public void UpdateTime()
    {
        _time -= Time.deltaTime;
    }
    public bool ValidatePizza(CutInfo[] Pizza)
    {
        int[] PizzaValues = CutInfoArrayToFloat(Pizza);
        int[] TicketValues = Cuts.ToArray();
        Debug.Log("Pizza Values = " + String.Join("",
            new List<int>(PizzaValues)
            .ConvertAll(i => i.ToString())
            .ToArray()));
        Debug.Log("Ticket Values = " + String.Join("",
            new List<int>(TicketValues)
            .ConvertAll(i => i.ToString())
            .ToArray()));
          if (PizzaValues.Length != TicketValues.Length)
              return false;
        if (PizzaValues.Length == 0)
        {
            if(TicketValues.Length == 0)
            {
                return true;
            }
            return false;
        }

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
        MakeUnique(ints);
        ints.Sort();
        return ints.ToArray();
    }

    public float GetTime()
    {
        return Mathf.Max(0f, _time);
    }
    public float GetTimePercent()
    {
        return GetTime() / _maxTime;
    }
    public void FulfillTicket()
    {

    }
    private void MakeUnique(List<int> numList)
    {

        for (int i = 0;i < numList.Count;i++)
        {
            for(int j = 0; j < numList.Count;j++) { 
                if(i != j)
                {
                    if (numList[i] == numList[j])
                    {
                        numList.RemoveAt(j);
                        j--;
                        if (j < i)
                            i--;
                    }
                        
                }
            }
        }

        if(numList.Count == 2)
        {
            if (numList[0] == numList[1])
            {
                numList.RemoveAt(1);
            }
        }

    }
}
