using System;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private NavMeshAgent agent;

    private Horde selectedHorde;
    private Location selectedLocation;

    [SerializeField] private GameObject hordePrefab;
    [SerializeField] private GameObject hordeParent;


    [SerializeField] private DisplayManager displayManager;



    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void HandleLocationClicked(Location clickedLocation)
    {
        clickedLocation.OnSelected();
        if (selectedLocation == clickedLocation)
        {
            ClearLocation(); // Seçili konumu sıfırla
            return;
        }
        ClearLocation(); // Seçili horde'u sıfırla
        selectedLocation = clickedLocation;
        SetRed(selectedLocation.gameObject);


    }

    public void HandleHordeClicked(Horde clickedHorde)
    {
        clickedHorde.OnSelected();
        if (selectedHorde == clickedHorde)
        {
            ClearHorde(); // Seçili horde'u sıfırla
            return;
        }
        selectedHorde = clickedHorde;
        SetRed(selectedHorde.gameObject);
    }

    public void ClearHorde()
    {
        if(selectedHorde == null)
            return;
        ResetColor(selectedHorde.gameObject); // Seçili horde'u sıfırla
        selectedHorde = null;
    }
    public void ClearLocation()
    {
        if (selectedLocation == null)
            return;
        ResetColor(selectedLocation.gameObject); // Seçili horde'u sıfırla
        selectedLocation = null;
    }

    public void HandleClick(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<Location>(out Location location))
        {
            ClearHorde(); // Seçili horde'u sıfırla
            HandleLocationClicked(location);
            //Debug.Log("Aloo kardeşim location bu handle şunu");
            return;
        }
        if (hit.collider.TryGetComponent<Horde>(out Horde horde))
        {
            ClearLocation();
            HandleHordeClicked(horde);
            //Debug.Log("Aloo kardeşim horde   bu handle şunu");
            return;
        }

        if(selectedLocation != null && selectedLocation.SoldierCount > 0)
        {
            GameObject createdHorde = SpawnHorde();
            createdHorde.GetComponent<NavMeshAgent>().SetDestination(hit.point);
            ClearLocation();
        }

        //Moves Horde
        if (selectedHorde!= null)
        {

            agent = selectedHorde.GetComponent<NavMeshAgent>();
            agent.SetDestination(hit.point);
        }

    }

    public void AddHorde()
    {
        selectedLocation.AddSoldiers();
    }

    public GameObject SpawnHorde()
    {
        GameObject createdHorde = Instantiate(hordePrefab, selectedLocation.transform.position + new Vector3(0, 0, 2), Quaternion.identity);

        createdHorde.GetComponent<Horde>().Initialize(selectedLocation.SoldierCount);

        selectedLocation.HordeCreated();

        createdHorde.transform.SetParent(hordeParent.transform);

        displayManager.AddHordeDisplay(createdHorde.GetComponent<Horde>());

        return createdHorde;
    }

    private void SetRed(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }

    private void ResetColor(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white;
        }
    }
}
