using UnityEngine;
using System.Collections.Generic;
using System;

public class Location : MonoBehaviour
{
    int soldierCount;
    //private List<Horde> hordes = new List<Horde>();

    public int SoldierCount => soldierCount;

    public event Action OnHordeChanged;

    public void AddHorde(Horde horde)
    {
        soldierCount += horde.Count;

        OnHordeChanged?.Invoke();
    }

    public void OnSelected()
    {
        Debug.Log($"{name} seçildi. {soldierCount} askerimiz mevcut.");
    }
}
