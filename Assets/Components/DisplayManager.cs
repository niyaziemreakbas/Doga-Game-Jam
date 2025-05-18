using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private GameObject locationParent; // T�m Location'lar�n parent GameObject'i
    [SerializeField] private GameObject hordeParent; // T�m Horde'lar�n parent GameObject'i (opsiyonel)
    [SerializeField] private Canvas worldSpaceCanvas; // D�nya uzay� Canvas referans�
    [SerializeField] private Vector3 offset = new Vector3(0, 15f, 0); // UI'nin obje �zerindeki konumu (daha yukar�)
    
    private Dictionary<Location, TextMeshPro> locationDisplays = new Dictionary<Location, TextMeshPro>();
    private Dictionary<Horde, TextMeshPro> hordeDisplays = new Dictionary<Horde, TextMeshPro>();
    
    private Camera mainCamera; // Ana kamera referans�

    private void Awake()
    {
        // Canvas kontrol�
        if (worldSpaceCanvas == null)
        {
            Debug.LogError("WorldSpaceCanvas atanmam��! L�tfen bir d�nya uzay� Canvas'i atay�n.");
            return;
        }

        // Ana kameray� bul
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Ana kamera bulunamad�!");
            return;
        }

        // T�m Location ve Horde'lar�n UI'lar�n� ba�lat
        InitializeLocationDisplays();
        InitializeHordeDisplays();
    }

    private void OnDestroy()
    {
        // Event aboneliklerini kald�r
        foreach (var location in locationDisplays.Keys)
        {
            location.OnHordeChanged -= () => UpdateLocationUI(location);
        }
        foreach (var horde in hordeDisplays.Keys)
        {
            horde.OnCountChanged -= () => UpdateHordeUI(horde);
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
        foreach (var textMeshPro in hordeDisplays.Values)
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
        Debug.Log($"Bulunan Location say�s�: {locations.Length}");
        foreach (var location in locations)
        {
            Debug.Log($"Location bulundu: {location.name}");
            TextMeshPro textMeshPro = SetupTextMeshPro(location.transform);
            locationDisplays.Add(location, textMeshPro);
            location.OnHordeChanged += () => UpdateLocationUI(location);
            UpdateLocationUI(location);
        }
    }

    private void InitializeHordeDisplays()
    {
        if (hordeParent == null)
        {
            Debug.LogWarning("HordeParent atanmam��, sahnedeki t�m Horde'lar aranacak.");
            hordeParent = gameObject;
        }

        Horde[] hordes = hordeParent.GetComponentsInChildren<Horde>();
        Debug.Log($"Bulunan Horde say�s�: {hordes.Length}");
        foreach (var horde in hordes)
        {
            Debug.Log($"Horde bulundu: {horde.name}");
            AddHordeDisplay(horde);
        }
    }

    public void AddHordeDisplay(Horde horde)
    {
        TextMeshPro textMeshPro = SetupTextMeshPro(horde.transform);
        hordeDisplays.Add(horde, textMeshPro);
        horde.OnCountChanged += () => UpdateHordeUI(horde);
        UpdateHordeUI(horde);
    }

    public void RemoveHordeDisplay(Horde horde)
    {
        if (hordeDisplays.TryGetValue(horde, out TextMeshPro textMeshPro))
        {
            horde.OnCountChanged -= () => UpdateHordeUI(horde);
            Destroy(textMeshPro.gameObject);
            hordeDisplays.Remove(horde);
        }
    }

    private TextMeshPro SetupTextMeshPro(Transform targetTransform)
    {
        Debug.Log($"TextMeshPro olu�turuluyor, target: {targetTransform.name}");
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
            textMeshPro.transform.position = location.transform.position + offset;
        }
    }

    private void UpdateHordeUI(Horde horde)
    {
        if (hordeDisplays.TryGetValue(horde, out TextMeshPro textMeshPro))
        {
            textMeshPro.text = horde.Count.ToString();
            // Pozisyonu g�ncelle
            textMeshPro.transform.position = horde.transform.position + offset;
        }
    }
}