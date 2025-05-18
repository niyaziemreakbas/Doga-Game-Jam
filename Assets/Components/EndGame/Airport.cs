using UnityEngine;

public class Airport : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Horde"))
        {

        }
    }
}
