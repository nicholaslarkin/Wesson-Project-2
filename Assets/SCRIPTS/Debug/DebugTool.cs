using UnityEngine;

public class DebugTool : MonoBehaviour
{
    public GameObject HUD;
    public bool activeHUD;
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
}
