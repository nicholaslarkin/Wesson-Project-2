using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBoosting : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float boostSpeed;
    [SerializeField] private float normalSpeed;

    public GameObject sparkL;
    public GameObject sparkR;
    public GameObject flamethrowerL;
    public GameObject flamethrowerR;

    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        sparkL.SetActive(false);
        sparkR.SetActive(false);
        flamethrowerL.SetActive(false);
        flamethrowerR.SetActive(false);
}

    public void OnBoost(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            StartBoosting();
        }
        else if (callbackContext.canceled)
        {
            StopBoosting();
        }
    }

    private void StartBoosting()
    {
        animator.SetBool("isBoosting", true);
        sparkL.SetActive(true);
        sparkR.SetActive(true);
        flamethrowerL.SetActive(true);
        flamethrowerR.SetActive(true);
        playerMovement.maxSpeed = boostSpeed;
    }

    private void StopBoosting()
    {
        animator.SetBool("isBoosting", false);
        sparkL.SetActive(false);
        sparkR.SetActive(false);
        flamethrowerL.SetActive(false);
        flamethrowerR.SetActive(false);
        playerMovement.maxSpeed = normalSpeed;
    }
}
