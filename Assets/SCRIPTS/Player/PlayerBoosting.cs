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

    public GameObject boostStart;
    public GameObject boostLoop;

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
        SoundManager.Instance.PlaySFXWithSource(Random.Range(0, 7), 0.5f);
        SoundManager.Instance.PlaySFXWithSource(16, 0.2f);
        boostStart.SetActive(true);
        boostLoop.SetActive(true);
        animator.SetBool("isBoosting", true);
        sparkL.SetActive(true);
        sparkR.SetActive(true);
        flamethrowerL.SetActive(true);
        flamethrowerR.SetActive(true);
        playerMovement.maxSpeed = boostSpeed;
    }

    private void StopBoosting()
    {
        boostStart.SetActive(false);
        boostLoop.SetActive(false);
        animator.SetBool("isBoosting", false);
        sparkL.SetActive(false);
        sparkR.SetActive(false);
        flamethrowerL.SetActive(false);
        flamethrowerR.SetActive(false);
        playerMovement.maxSpeed = normalSpeed;
    }
}
