using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

public class Location : MonoBehaviour
{
    [SerializeField] int soldierCount;

    [SerializeField] Material selectedMaterial;
    [SerializeField] Material deselectedMaterial;



    public int SoldierCount => soldierCount;

    public event Action OnSoldierChanged;

    private void Awake()
    {
        soldierCount = 20;
    }

    public void AddHorde(Horde horde)
    {
        soldierCount += horde.Count;

        OnSoldierChanged?.Invoke();
    }

    public void HordeCreated()
    {
        soldierCount = 0;
        OnSoldierChanged?.Invoke();

    }

    public void AddSoldiers()
    {
        soldierCount += 5;
        OnSoldierChanged?.Invoke();

    }

    public void OnSelected()
    {
        Debug.Log($"{name} seçildi. {soldierCount} askerimiz mevcut.");
        SetRed(gameObject);
    }
    public void OnDeselected()
    {
        ResetColor(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Horde"))
        {
            soldierCount += other.GetComponent<Horde>().Count;
            Destroy(other.gameObject);
            OnSoldierChanged?.Invoke();
        }
    }

    private void SetRed(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = selectedMaterial;
        }
    }

    private void ResetColor(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = deselectedMaterial;
        }
    }

}
