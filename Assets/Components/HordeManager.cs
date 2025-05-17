using UnityEngine;

public class HordeManager : MonoBehaviour
{
    public static HordeManager Instance { get; private set; }

    private Location selectedLocation;
    [SerializeField] private GameObject hordePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void HandleLocationClick(Location clickedLocation)
    {
        if (selectedLocation == null)
        {
            if (clickedLocation.HasFriendlyHorde())
            {
                selectedLocation = clickedLocation;
                clickedLocation.OnSelected();
            }
        }
        else
        {
            MoveHorde(selectedLocation, clickedLocation);
            selectedLocation = null;
        }
    }

    private void MoveHorde(Location from, Location to)
    {
        Horde hordeToMove = from.GetFirstHorde();

        if (hordeToMove == null)
            return;

        from.RemoveHorde(hordeToMove);

        if (to.HasFriendlyHorde())
        {
            Horde target = to.GetFirstHorde();
            target.MergeWith(hordeToMove);
        }
        else
        {
            to.AddHorde(hordeToMove);
        }

        Debug.Log($"Horde {from.name} → {to.name} taşındı.");
    }

    public void SpawnHorde(Location location, int count)
    {
        GameObject hordeGO = Instantiate(hordePrefab, location.transform.position, Quaternion.identity);
        Horde horde = hordeGO.GetComponent<Horde>();
        horde.Initialize(count, location);
        location.AddHorde(horde);
    }
}
