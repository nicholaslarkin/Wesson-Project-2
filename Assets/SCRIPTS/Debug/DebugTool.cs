using UnityEngine;

public class DebugTool : MonoBehaviour
{
    public GameObject HUD;
    public bool activeHUD;

    public GameObject debug;
    public bool activeDebug;

    [Header("Transitions")]
    public GameObject title;
    public GameObject day;
    public GameObject sunset;
    public GameObject night;
    public GameObject house;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y) && activeHUD == true)
        {
            TurnHUDOff();
        }
        else if (Input.GetKeyDown(KeyCode.Y) && activeHUD == false)
        {
            TurnHUDOn();
        }

        if (Input.GetKeyDown(KeyCode.T) && activeDebug == true)
        {
            TurnDebugOff();
        }
        else if (Input.GetKeyDown(KeyCode.T) && activeDebug == false)
        {
            TurnDebugOn();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            day.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            sunset.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            night.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            house.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            title.SetActive(true);
        }
    }

    private void TurnHUDOff()
    {
        HUD.SetActive(false);
        activeHUD = false;
    }

    private void TurnHUDOn()
    {
        HUD.SetActive(true);
        activeHUD = true;
    }

    private void TurnDebugOff()
    {
        debug.SetActive(false);
        activeDebug = false;
    }

    private void TurnDebugOn()
    {
        debug.SetActive(true);
        activeDebug = true;
    }
}
