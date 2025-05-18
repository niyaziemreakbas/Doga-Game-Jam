using UnityEngine;

public class Airport : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Horde"))
        {
            if(other.GetComponent<Horde>().Count > 30)
            {
                Debug.Log("GameOver");
            }
        }
    }
}
