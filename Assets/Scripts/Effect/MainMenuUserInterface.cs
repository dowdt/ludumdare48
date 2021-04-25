using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUserInterface : UserInterfaceManager
{
    public void Close()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneFader.instance.fadeToNewScene("Level1");
    }
}
