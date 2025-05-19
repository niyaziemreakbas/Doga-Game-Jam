using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Horde : MonoBehaviour
{
    private int count;

    public int Count => count;

    [Header("Canvas ve UI")]
    private Canvas worldSpaceCanvas;
    private RectTransform hordeText;
    TextMeshPro textMeshPro;
    private Vector3 uiOffset = new Vector3(0, 5, 0);

    NavMeshAgent agent;
    Collider myCollider;    


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = 10f;
        worldSpaceCanvas = GameManager.Instance.ReturnCanvas();
        SetupTextMeshPro();

        myCollider = GetComponent<Collider>(); 

        if (myCollider != null)
            myCollider.enabled = false;
    }

    public void OnSelected()
    {
        Debug.Log($"{name} seçildi. {count} askerimiz mevcut.");
    }

    private void OnDestroy()
    {
        textMeshPro.gameObject.SetActive( false );
    }

    private void Start()
    {
        StartCoroutine(EnableColliderAfterDelay(3f));
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (myCollider != null)
            myCollider.enabled = true;
    }

    private void Update()
    {
        UpdateTargetUIPosition();
    }

    private void UpdateTargetUIPosition()
    {

        if (hordeText != null)
        {
            Vector3 worldPosition = transform.position + uiOffset;
            hordeText.position = worldPosition;

            Vector3 currentEuler = hordeText.eulerAngles;
            hordeText.eulerAngles = new Vector3(90f, currentEuler.y, currentEuler.z);
        }

        textMeshPro.text = $"{count}";

    }

    private TextMeshPro SetupTextMeshPro()
    {
        Transform targetTransform = transform;
        //Debug.Log($"TextMeshPro oluþturuluyor, target: {targetTransform.name}");
        GameObject textObj = new GameObject("CountText");

        // Canvas'in çocuðu yap
        textObj.transform.SetParent(worldSpaceCanvas.transform, false);

        // TextMeshPro bileþeni ekle
        textMeshPro = textObj.AddComponent<TextMeshPro>();
        textMeshPro.alignment = TextAlignmentOptions.Center;
        textMeshPro.fontSize = 20;
        textMeshPro.color = Color.black; // Font rengini siyah yap


        hordeText = textObj.GetComponent<RectTransform>();
        hordeText.position = targetTransform.position + uiOffset;
        hordeText.sizeDelta = new Vector2(100, 50);
        hordeText.localEulerAngles = new Vector3(0f, 180f, 0f); // X rotasyonunu 90 derece yap


        return textMeshPro;
    }

    //public void MovePosition(Location Location, int SoldierCount)
    //{
    //    count += SoldierCount;

    //    agent.SetDestination(Location.transform.position);

    //}

    public void MovePosition(Location location, int soldierCount)
    {
        count += soldierCount;

        Vector3 targetPos = location.transform.position;
        agent.SetDestination(targetPos);

        // Karakteri hedefe döndür
        Vector3 direction = (targetPos - transform.position).normalized;

        // Sadece yatay düzlemde döndür (y ekseninde)
        //direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
