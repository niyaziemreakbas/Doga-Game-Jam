using System;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private NavMeshAgent agent;

    private Location selectedLocation;

    [SerializeField] private GameObject hordePrefab;
    [SerializeField] private GameObject hordeParent;
    
    [SerializeField] private DisplayManager displayManager;

    [SerializeField] private Canvas worldSpaceCanvas;


    private void Awake()
    {
        print(worldSpaceCanvas);

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Canvas ReturnCanvas()
    {
        return worldSpaceCanvas;
    }

    public void HandleClick(RaycastHit hit)
    {
        if (hit.collider.GetComponent<Airport>())
        {
            
        }

        if (hit.collider.TryGetComponent<Location>(out Location location))
        {
            if(location == selectedLocation)
            {
                location.OnDeselected();
                selectedLocation = null;
                return;
            }

            if (selectedLocation != null)
            {
                //Move Soldiers to new Location
                if (selectedLocation.SoldierCount > 0)
                {
                    selectedLocation.OnDeselected();
                    SpawnHorde(location);
                }
                else { Debug.Log("Seçili konumda asker yok!"); }
            }
            else
            {
                selectedLocation = location;
                location.OnSelected();
            }
        }
    }


    public GameObject SpawnHorde(Location targetLoc)
    {

        GameObject createdHorde = Instantiate(hordePrefab, selectedLocation.transform.position + new Vector3(0, 0, 5), Quaternion.identity);

        createdHorde.GetComponent<Horde>().MovePosition(targetLoc, selectedLocation.SoldierCount);

        selectedLocation.HordeCreated();

        createdHorde.transform.SetParent(hordeParent.transform);

        return createdHorde;
    }


}
