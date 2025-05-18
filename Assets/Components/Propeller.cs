using UnityEngine;

public class Propeller : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 360f; // Derece/saniye

    void Update()
    {
        // X ekseninde d�nme
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
