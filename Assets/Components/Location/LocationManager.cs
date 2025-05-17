using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    [SerializeField] private GameObject locationParent; // Tüm Location'larýn parent GameObject'i
    [SerializeField] private Vector3 offset = new Vector3(0, 2, 0); // UI'nin kale üstündeki konumu
    private Dictionary<Location, TextMeshPro> locationDisplays = new Dictionary<Location, TextMeshPro>();

    private void Awake()
    {
        // Tüm Location'larý bul ve UI'larý oluþtur
        InitializeLocationDisplays();
    }

    private void OnDestroy()
    {
        // Event aboneliklerini kaldýr
        foreach (var location in locationDisplays.Keys)
        {
            location.OnHordeChanged -= () => UpdateUI(location);
        }
    }

    private void InitializeLocationDisplays()
    {
        if (locationParent == null)
        {
            Debug.LogError("LocationParent atanmamýþ!");
            return;
        }

        // Tüm Location'larý bul
        Location[] locations = locationParent.GetComponentsInChildren<Location>();

        foreach (var location in locations)
        {
            // TextMeshPro oluþtur
            TextMeshPro textMeshPro = SetupTextMeshPro(location.transform);
            locationDisplays.Add(location, textMeshPro);

            // OnHordeChanged event'ine abone ol
            location.OnHordeChanged += () => UpdateUI(location);

            // Ýlk UI güncellemesini yap
            UpdateUI(location);
        }
    }

    private TextMeshPro SetupTextMeshPro(Transform parent)
    {
        GameObject textObj = new GameObject("HordeCountText");
        textObj.transform.SetParent(parent);
        TextMeshPro textMeshPro = textObj.AddComponent<TextMeshPro>();
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.fontSize = 2;
        textMeshPro.transform.localPosition = offset;
        return textMeshPro;
    }

    private void UpdateUI(Location location)
    {
        if (locationDisplays.TryGetValue(location, out TextMeshPro textMeshPro))
        {
            textMeshPro.text = location.SoldierCount.ToString();
        }
    }

}