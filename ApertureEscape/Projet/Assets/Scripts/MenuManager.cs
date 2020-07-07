using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

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
    public void ShowInstructions()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.instructions);
    }
    public void ShowCredits()
    {
        GameManager.instance.ChangeScene(GameManager.ScenesName.credits);
    }
}
