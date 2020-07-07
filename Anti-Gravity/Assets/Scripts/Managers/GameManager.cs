using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] AudioSource _music;
    [SerializeField] int _score;
    [SerializeField] int _tmpScore;
    [SerializeField] int _time;
    [SerializeField] int _intel;
    [SerializeField] int _seed;
    [SerializeField] int _level;
    [SerializeField] bool _isDead;
    [SerializeField] AudioClip[] _musicLibrary;

    /*
        GET/SET
    */

    public int Seed{
        get{
            return _seed;
        }
        set{
            _seed = value;
        }
    }

    public int Score{
        get{
            return _score;
        }
        set{
            _score = value;
        }
    }
    public int TempScore{
        get{
            return _tmpScore;
        }
        set{
            _tmpScore = value;
        }
    }

    public int Time{
        get{
            return _time;
        }
        set{
            _time = value;
        }
    }
    public int Intel{
        get{
            return _intel;
        }
        set{
            _intel = value;
        }
    }
    public int Level{
        get{
            return _level;
        }
        set{
            _level = value;
        }
    }
    public bool Dead{
        get{
            return _isDead;
        }
        set{
            _isDead = value;
        }
    }

    /*
        INIT
    */

    void Awake(){
        if(instance != null){
            Destroy(this.gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start(){
        Initialise();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Initialise(){
        PlayMusic();
        Seed = Random.Range(0, 9);
        Score = 0;
        Time = 0;
        Level = 0;
        Dead = false;
    }


    /*
        FUNCTIONS
    */

    public void Reset(){
        Initialise();
    }

    /*
        EVENTS
    */

    public delegate void TutorielStepDelegate(string step);
	public event TutorielStepDelegate tutorialStepEvent;
	public void AchievedStep(string step){
		tutorialStepEvent(step);
	}

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log(scene.name);
        PlayMusic(scene.name);
    }

    /*
        SCENES
    */
    void PlayMusic(string scene = null){
        AudioClip nextClip;
        switch(scene){
            case "Game":
                nextClip = _musicLibrary[1];
                break;
            case "GameOverLose":
                nextClip = _musicLibrary[2];
                break;
            case "GameOverWin":
                nextClip = _musicLibrary[3];
                break;
            case "Menu":
            case "Credits":
            default:
                nextClip = _musicLibrary[0];
                break;
        }
        if(nextClip != _music.clip){
            _music.clip = nextClip;
            _music.Play();
        }
    }

    public void Play(bool random = true){
        Dead = false;

        if(random){
            Seed = Random.Range(0, 999999);
        }
        Random.InitState(Seed);

        ChangeScene("Game");
    }
    public void ChangeScene(string scene){
        SceneManager.LoadScene(scene);
    }
}
