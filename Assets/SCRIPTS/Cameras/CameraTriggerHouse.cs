using UnityEngine;

public class CameraTriggerHouse : MonoBehaviour
{
    [SerializeField] private GameObject ActivatingCamera;
    [SerializeField] private GameObject DeactivatingCamera;

    void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the Player
        if (other.CompareTag("Player"))
        {
            DeactivatingCamera.SetActive(false);
            ActivatingCamera.SetActive(true);
        }
    }
}
