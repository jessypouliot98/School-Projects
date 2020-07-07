using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour {

    [SerializeField] AudioClip _clip;

    private void Start()
    {
        GameManager.instance.SetMusic(_clip);
    }

    public void Play()
    {
        GameManager.instance.KeepLastSong(false);
        GameManager.instance.ChangeScene(GameManager.ScenesName.main);
    }

    public void ShowMenu()
    {
        GameManager.instance.KeepLastSong(true);
        GameManager.instance.ChangeScene(GameManager.ScenesName.menu);
    }
}
