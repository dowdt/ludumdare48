using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameUserInterface : UserInterfaceManager
{
    public static InGameUserInterface instance;
    private void Start()
    {
        instance = this;
    }

    public void Die() {
        openWindow(1);
    }

    [SerializeField]
    Image WebOverlay;

    public void Menu()
    {
        SceneFader.instance.fadeToNewScene("MainMenu");
    }


    public bool canMove() {
        return (currentWindow < 0);
    }

    private void Update()
    {
        float web = Mathf.Clamp(GameManager.playerInstance.move.getInWeb(), 0, 1)*0.1f;
        if (WebOverlay.color.a != web)
        WebOverlay.color = new Color(WebOverlay.color.r, WebOverlay.color.g, WebOverlay.color.b, Mathf.Lerp(WebOverlay.color.a,web,Time.deltaTime*10f));

        if (currentWindow < 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                openWindow(0);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (currentWindow == 0)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    openWindow(-1);
   
            }
        }
        
   


    }
}
