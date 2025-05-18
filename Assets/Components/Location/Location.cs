using UnityEngine;
using System.Collections.Generic;
using System;

public class Location : MonoBehaviour
{
    int soldierCount;

    public int SoldierCount => soldierCount;

    public event Action OnHordeChanged;

    public void AddHorde(Horde horde)
    {
        soldierCount += horde.Count;

        OnHordeChanged?.Invoke();
    }

    public void HordeCreated()
    {
        soldierCount = 0;
        OnHordeChanged?.Invoke();

    }

    public void AddSoldiers()
    {
        soldierCount += 5;
        OnHordeChanged?.Invoke();

    }

    public void OnSelected()
    {
        Debug.Log($"{name} seçildi. {soldierCount} askerimiz mevcut.");
    }
}
