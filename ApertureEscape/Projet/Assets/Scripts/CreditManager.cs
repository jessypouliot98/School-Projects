using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    public void ShowMenu()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.menu);
    }

}
