using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Horde : MonoBehaviour
{
    [SerializeField] private int count;

    private Location currentLocation;

    public int Count => count;

    public void Initialize(int initialCount, Location startLocation)
    {
        count = initialCount;
        SetLocation(startLocation);
    }

    public void SetLocation(Location newLocation)
    {
        currentLocation = newLocation;
        transform.position = newLocation.transform.position;
    }

    public Location GetLocation()
    {
        return currentLocation;
    }

    public void MergeWith(Horde other)
    {
        count += other.count;
        Destroy(other.gameObject);
    }
}
