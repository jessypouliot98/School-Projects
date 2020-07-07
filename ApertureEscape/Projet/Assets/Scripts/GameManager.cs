using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    bool _keepLastSong = false;
    AudioSource _music;

    public enum ScenesName
    {
        main,
        menu,
        instructions,
        credits,
        gameover,
        success,
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        _music = GetComponent<AudioSource>();
    }

    public void KeepLastSong(bool kept)
    {
        _keepLastSong = kept;
    }

    public void SetMusic (AudioClip clip)
    {
        if (!_keepLastSong)
        {
            if (_music.clip != clip || _music.clip == null)
            {
                _music.clip = clip;
                _music.Play();
            }
        }
    }

    public void ChangeScene(ScenesName scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

}