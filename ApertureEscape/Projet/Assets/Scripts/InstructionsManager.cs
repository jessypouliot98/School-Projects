using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    public void Play()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.main);
    }
    public void ShowMenu()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.menu);
    }

}
