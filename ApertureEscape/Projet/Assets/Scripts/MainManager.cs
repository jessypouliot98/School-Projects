using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    [SerializeField] AudioClip _clip;

    private void Start()
    {
        GameManager.instance.SetMusic(_clip);
    }

    public void ShowMenu()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.menu);
    }

    public void StillAlive()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.success);
    }

    public void GameOver()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.gameover);
    }
}
