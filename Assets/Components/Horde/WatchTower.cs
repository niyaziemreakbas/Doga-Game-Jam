using UnityEngine;

public class WatchTower : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackCooldown = 3f; // saniye

    [Header("Canvas ve UI")]
    private Canvas worldSpaceCanvas;
    [SerializeField] private GameObject targetPrefab;
    private RectTransform targetUI;
    [SerializeField] private Vector3 uiOffset = new Vector3(0, -10, 0);

    private Horde currentTarget;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    void Start()
    {
        worldSpaceCanvas = GameManager.Instance.ReturnCanvas();

        if (targetPrefab != null && worldSpaceCanvas != null)
        {
            GameObject instance = Instantiate(targetPrefab, worldSpaceCanvas.transform);
            targetUI = instance.GetComponent<RectTransform>();
            print(targetUI);
        }
        else
        {
            Debug.LogWarning("Target prefab ya da canvas atanmadý!");
        }
    }

    void Update()
    {
        // Cooldown süresi
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                if (targetUI != null) targetUI.gameObject.SetActive(true);
            }
            return;
        }

        if (currentTarget == null)
        {
            DetectNearbyHordes();
        }

        if (currentTarget != null)
        {
            AttackTarget();
        }

        UpdateTargetUIPosition();
    }

    private void DetectNearbyHordes()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Horde") && hit.gameObject != gameObject)
            {
                Horde horde = hit.GetComponent<Horde>();
                if (horde != null) // isAlive fonksiyonu varsa
                {
                    currentTarget = horde;
                    break;
                }
            }
        }
    }

    private void AttackTarget()
    {
        if (currentTarget != null)
        {
            Destroy(currentTarget.gameObject);
            currentTarget = null;

            // Cooldown'a gir
            isOnCooldown = true;
            cooldownTimer = attackCooldown;

            // UI'yi kapat
            if (targetUI != null)
                targetUI.gameObject.SetActive(false);

        }
    }

    private void UpdateTargetUIPosition()
    {
        if (targetUI != null)
        {
            Vector3 worldPosition = transform.position + uiOffset;
            targetUI.position = worldPosition;

            Vector3 currentEuler = targetUI.eulerAngles;
            targetUI.eulerAngles = new Vector3(90f, currentEuler.y, currentEuler.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
