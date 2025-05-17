using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private NavMeshAgent agent;
    private Horde selectedHorde;
    private Location selectedLocation;

    [SerializeField] private GameObject hordePrefab;


    //public GameObject testLocation;

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
        if(selectedHorde == clickedHorde)
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
            Debug.Log("Aloo kardeşim location bu handle şunu");
            return;
        }
        if (hit.collider.TryGetComponent<Horde>(out Horde horde))
        {
            ClearLocation();
            HandleHordeClicked(horde);
            Debug.Log("Aloo kardeşim horde   bu handle şunu");
            return;
        }

        if(selectedHorde!= null)
        {
            agent = selectedHorde.GetComponent<NavMeshAgent>();
            agent.SetDestination(hit.point);
        }

    }

    private void MoveHorde(Location from, Location to)
    {
       // Horde hordeToMove = from.GetFirstHorde();

        Instantiate(hordePrefab, from.transform.position, Quaternion.identity); // Yeni horde prefabını oluştur

        Debug.Log(" aktif mi? : " + gameObject.activeInHierarchy);
        
        hordePrefab.transform.position = to.transform.position;
        //hordePrefab.GetComponent<MoveableHorde>().MoveToLocation(to.gameObject.transform); // Yeni horde prefabını hedef konuma taşı

        Debug.Log($"Horde {from.name} → {to.name} taşındı.");
    }

    public void SpawnHorde()
    {
        Location location = selectedLocation;


        int count = 20;
        Horde newHorde = new Horde();
        newHorde.Initialize(count, location);
        location.AddHorde(newHorde);
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
