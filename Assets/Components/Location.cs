using UnityEngine;
using System.Collections.Generic;

public class Location : MonoBehaviour
{
    private List<Horde> hordes = new List<Horde>();

    public void AddHorde(Horde horde)
    {
        hordes.Add(horde);
        horde.SetLocation(this);
    }

    public void RemoveHorde(Horde horde)
    {
        hordes.Remove(horde);
    }

    public bool HasFriendlyHorde()
    {
        return hordes.Count > 0;
    }

    public Horde GetFirstHorde()
    {
        return hordes.Count > 0 ? hordes[0] : null;
    }

    public void OnSelected()
    {
        Debug.Log($"{name} seçildi. {hordes.Count} horde var.");
    }
}
