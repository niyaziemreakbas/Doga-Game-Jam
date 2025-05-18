using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyHorde : MonoBehaviour
{
    private NavMeshAgent agent;

    Horde currentTarget;

    [Header("Canvas ve UI")]
    [SerializeField] private Canvas worldSpaceCanvas;
    [SerializeField] private GameObject targetPrefab;
    private RectTransform targetUI;
    [SerializeField] private Vector3 uiOffset = new Vector3(0, -10, 0);

    [Header("Patrol Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;

    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 10f;
    
    [Header("Chase Settings")]
    [SerializeField] private float maxChaseDistance = 15f;
    [SerializeField] private float killDistance = 3f;





    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
    }

    void Start()
    {
        if (patrolPoints != null && patrolPoints.Count > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }

        if (targetPrefab != null && worldSpaceCanvas != null)
        {
            GameObject instance = Instantiate(targetPrefab, worldSpaceCanvas.transform);
            targetUI = instance.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogWarning("Target prefab ya da canvas atanmadý!");
        }
    }

    void Update()
    {
        if (currentTarget == null)
        {
            PatrolLogic();
            DetectNearbyHordes();
        }
        else
        {
            ChaseTarget();

            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
            if (distanceToTarget <= killDistance)
            {
                currentTarget.gameObject.SetActive(false);
                currentTarget = null; // Þimdilik hedefi sýfýrla
            }
            else if (distanceToTarget > maxChaseDistance)
            {
                Debug.Log($"[{name}] Target çok uzaða kaçtý: {currentTarget.name}, takibi býrakýldý.");
                currentTarget = null;
                GoToNextPatrolPoint(); // Optional: hemen bir patrol noktasýna dönsün
            }
        }

        UpdateTargetUIPosition();
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

    void ChaseTarget()
    {
        agent.SetDestination(currentTarget.transform.position);
    }


    private void PatrolLogic()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0)
            return;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private void DetectNearbyHordes()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Horde") && hit.gameObject != gameObject)
            {
                currentTarget = hit.gameObject.GetComponent<Horde>();
                break;
            }
        }
    }

    // Editor'da Detection Radius'u görselleþtirmek için
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxChaseDistance);
    }
}
