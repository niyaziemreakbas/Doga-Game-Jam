using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    [SerializeField] private GameObject locationParent; // Tüm Location'larýn parent GameObject'i
    [SerializeField] private GameObject hordeParent; // Tüm Horde'larýn parent GameObject'i (opsiyonel)
    [SerializeField] private Canvas worldSpaceCanvas; // Dünya uzayý Canvas referansý
    [SerializeField] private Vector3 offset = new Vector3(0, 15f, 0); // UI'nin obje üzerindeki konumu (daha yukarý)
    
    private Dictionary<Location, TextMeshPro> locationDisplays = new Dictionary<Location, TextMeshPro>();
    private Dictionary<Horde, TextMeshPro> hordeDisplays = new Dictionary<Horde, TextMeshPro>();
    
    private Camera mainCamera; // Ana kamera referansý

    private void Awake()
    {
        // Canvas kontrolü
        if (worldSpaceCanvas == null)
        {
            Debug.LogError("WorldSpaceCanvas atanmamýþ! Lütfen bir dünya uzayý Canvas'i atayýn.");
            return;
        }

        // Ana kamerayý bul
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Ana kamera bulunamadý!");
            return;
        }

        // Tüm Location ve Horde'larýn UI'larýný baþlat
        InitializeLocationDisplays();
        InitializeHordeDisplays();
    }

    private void OnDestroy()
    {
        // Event aboneliklerini kaldýr
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

        // Tüm TextMeshPro objelerinin kameraya bakmasýný saðla (sadece y ekseninde)
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
            Debug.LogError("LocationParent atanmamýþ!");
            return;
        }

        Location[] locations = locationParent.GetComponentsInChildren<Location>();
        Debug.Log($"Bulunan Location sayýsý: {locations.Length}");
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
            Debug.LogWarning("HordeParent atanmamýþ, sahnedeki tüm Horde'lar aranacak.");
            hordeParent = gameObject;
        }

        Horde[] hordes = hordeParent.GetComponentsInChildren<Horde>();
        Debug.Log($"Bulunan Horde sayýsý: {hordes.Length}");
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
        Debug.Log($"TextMeshPro oluþturuluyor, target: {targetTransform.name}");
        GameObject textObj = new GameObject("CountText");

        // Canvas'in çocuðu yap
        textObj.transform.SetParent(worldSpaceCanvas.transform, false);

        // TextMeshPro bileþeni ekle
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
            // Pozisyonu güncelle
            textMeshPro.transform.position = location.transform.position + offset;
        }
    }

    private void UpdateHordeUI(Horde horde)
    {
        if (hordeDisplays.TryGetValue(horde, out TextMeshPro textMeshPro))
        {
            textMeshPro.text = horde.Count.ToString();
            // Pozisyonu güncelle
            textMeshPro.transform.position = horde.transform.position + offset;
        }
    }
}