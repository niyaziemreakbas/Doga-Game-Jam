using System;
using UnityEngine;

public class Horde : MonoBehaviour
{
    private int count;

    private Location currentLocation;

    public int Count => count;

    public event Action OnCountChanged;

    public void Initialize(int initialCount)
    {
        count = initialCount;

        currentLocation = null;
        OnCountChanged?.Invoke();
    }

    public void OnSelected()
    {
        Debug.Log($"{name} seçildi. {count} askerimiz mevcut.");
    }

    public void SetLocation(Location newLocation)
    {
        currentLocation = newLocation;
       // transform.position = newLocation.transform.position;
    }

    public Location GetLocation()
    {
        return currentLocation;
    }

    //public void MergeWith(Horde other)
    //{
    //    count += other.count;
    //    //Destroy(other.gameObject);
    //}
}
