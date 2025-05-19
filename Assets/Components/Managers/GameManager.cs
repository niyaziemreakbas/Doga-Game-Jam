using System;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private NavMeshAgent agent;

    bool gameOver = false;

    public Camera endGameCamera;

    private Location selectedLocation;

    [SerializeField] private GameObject hordePrefab;
    [SerializeField] private GameObject hordeParent;
    
    [SerializeField] private DisplayManager displayManager;

    [SerializeField] private Canvas worldSpaceCanvas;

    [SerializeField] GameObject Plane;

    private Animator animator;

    void Start()
    {
        animator = Plane.GetComponent<Animator>();
    }

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

    public void GameOver()
    {
        Debug.Log("Game Over");
        gameOver = true;
        animator.SetTrigger("GameOver");

        Camera mainCam = Camera.main;
        if (mainCam != null)
            mainCam.gameObject.SetActive(false);

        // EndGame kamerasını aktif et
        if (endGameCamera != null)
            endGameCamera.gameObject.SetActive(true);

        // Burada oyun bitirme mantığını ekleyebilirsiniz
    }

    public void HandleClick(RaycastHit hit)
    {


        if (hit.collider.TryGetComponent<Location>(out Location location))
        {

            if (location == selectedLocation)
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
                    selectedLocation = null;
                }
                else 
                { 
                    Debug.Log("Seçili konumda asker yok!");
                    location.OnDeselected();
                    selectedLocation = null;
                }
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

        GameObject createdHorde = Instantiate(hordePrefab, selectedLocation.transform.position + new Vector3(5, 0, 5), Quaternion.identity);

        createdHorde.GetComponent<Horde>().MovePosition(targetLoc, selectedLocation.SoldierCount);

        selectedLocation.HordeCreated();

        createdHorde.transform.SetParent(hordeParent.transform);

        return createdHorde;
    }


}
