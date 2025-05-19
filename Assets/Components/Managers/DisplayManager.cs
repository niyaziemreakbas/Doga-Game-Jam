using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private GameObject locationParent; // T�m Location'lar�n parent GameObject'i
    [SerializeField] private GameObject hordeParent; // T�m Horde'lar�n parent GameObject'i (opsiyonel)
    [SerializeField] private GameObject enemyHordeParent;
    [SerializeField] private Vector3 offset; // UI'nin obje �zerindeki konumu (daha yukar�)
    
    private Dictionary<Location, TextMeshPro> locationDisplays = new Dictionary<Location, TextMeshPro>();

    Canvas worldSpaceCanvas;
    
    private Camera mainCamera; // Ana kamera referans�

    private void Awake()
    {
        // Ana kameray� bul
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Ana kamera bulunamad�!");
            return;
        }
    }

    private void Start()
    {
        worldSpaceCanvas = GameManager.Instance.ReturnCanvas();

        InitializeLocationDisplays();
    }

    private void OnDestroy()
    {
        // Event aboneliklerini kald�r
        foreach (var location in locationDisplays.Keys)
        {
            location.OnSoldierChanged -= () => UpdateLocationUI(location);
        }
    }

    private void Update()
    {
        if (mainCamera == null) return;

        // T�m TextMeshPro objelerinin kameraya bakmas�n� sa�la (sadece y ekseninde)
        foreach (var textMeshPro in locationDisplays.Values)
        {
            if (textMeshPro != null)
            {
                Vector3 direction = mainCamera.transform.position - textMeshPro.transform.position;
                direction.y = 0; // Sadece y ekseninde hizala
                if (direction != Vector3.zero)
                {
                    textMeshPro.transform.rotation = Quaternion.LookRotation(-direction);
                }
            }
        }
    }

    private void InitializeLocationDisplays()
    {
        if (locationParent == null)
        {
            Debug.LogError("LocationParent atanmam��!");
            return;
        }

        Location[] locations = locationParent.GetComponentsInChildren<Location>();
        foreach (var location in locations)
        {
            TextMeshPro textMeshPro = SetupTextMeshPro(location.transform);
            locationDisplays.Add(location, textMeshPro);
            location.OnSoldierChanged += () => UpdateLocationUI(location);
            UpdateLocationUI(location);
        }
    }

    private TextMeshPro SetupTextMeshPro(Transform targetTransform)
    {
        //Debug.Log($"TextMeshPro olu�turuluyor, target: {targetTransform.name}");
        GameObject textObj = new GameObject("CountText");

        // Canvas'in �ocu�u yap
        textObj.transform.SetParent(worldSpaceCanvas.transform, false);

        // TextMeshPro bile�eni ekle
        TextMeshPro textMeshPro = textObj.AddComponent<TextMeshPro>();
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.fontSize = 20;
        textMeshPro.color = Color.black; // Font rengini siyah yap
        textMeshPro.text = "0";


        //textObj.transform.position = targetTransform.position + offset;

        RectTransform rectTransform = textObj.GetComponent<RectTransform>();
        rectTransform.position = targetTransform.position + offset;
        rectTransform.sizeDelta = new Vector2(100, 50);


        return textMeshPro;
    }

    private void UpdateLocationUI(Location location)
    {
        if (locationDisplays.TryGetValue(location, out TextMeshPro textMeshPro))
        {
            textMeshPro.text = location.SoldierCount.ToString();
            // Pozisyonu g�ncelle
            textMeshPro.rectTransform.position = location.transform.position + offset;
        }
    }


}