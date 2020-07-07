using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    [SerializeField] World _map;
    [SerializeField] int _scoreMinLength = 6;
    [SerializeField] int _intelMinLength = 2;
    [SerializeField] int _levelMinLength = 2;
    [SerializeField] Text _scoreUI;
    [SerializeField] Text _timerUI;
    [SerializeField] Text _intelUI;
    [SerializeField] Text _levelUI;
    int _score;
    int _time;
    int _intel;
    int _level;
    [SerializeField] int _timeInterval = 15;
    [SerializeField] int _maxTime = 300;//5min
    int _startTime;

    /*
        INIT
    */

    void Awake(){
        if(instance != null){
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    void Start(){
        Initialise();
    }

    void Initialise(){
        _startTime = _timeInterval + Mathf.Clamp(_timeInterval * GameManager.instance.Level, _timeInterval, _maxTime - _timeInterval);

        int seed = GameManager.instance.Seed;
        _map = Instantiate(_map, Vector3.zero, Quaternion.identity);
        StartCoroutine(CountdownTimer());
        _score = GameManager.instance.Score;
        _level = GameManager.instance.Level;
        DisplayScore();
        DisplayLevel();
    }

    /*
        GET/SET
    */

    public int Score{
        get{
            return _score;
        }
        set{
            _score = value;
            DisplayScore();
        }
    }

    public int Intel{
        get{
            return _intel;
        }
        set{
            _intel = value;
            DisplayIntel();
        }
    }

    /*
        FUNCTIONS
    */

    void DisplayScore(){
        string text = _score.ToString();
        while(text.Length < _scoreMinLength){
            text = "0" + text;
        }
        _scoreUI.text = text;
    }

    void DisplayLevel(){
        string text = _level.ToString();
        while(text.Length < _levelMinLength){
            text = "0" + text;
        }
        _levelUI.text = "LVL " + text;
    }

    void DisplayIntel(){
        string text = _intel.ToString();
        while(text.Length < _intelMinLength){
            text = "0" + text;
        }
        _intelUI.text = text;
    }

    void DisplayTimer(){
        int m = _time / 60;
        int s = _time % 60;
        _timerUI.text = m.ToString() + ":" + ((s < 10) ? "0" + s.ToString() : s.ToString());
    }

    IEnumerator CountdownTimer(){
        _time = _startTime;
        DisplayTimer();
        while(_time > 0){
            yield return new WaitForSeconds(1);
            _time--;
            DisplayTimer();
        }
        GameOver();
    }

    public void GameOver(){
        GameManager.instance.TempScore = _score;
        GameManager.instance.Time = _time;
        GameManager.instance.Intel = _intel;
        GameManager.instance.Dead = true;
        GameManager.instance.ChangeScene("GameOverLose");
    }
    public void Win(){
        GameManager.instance.Level++;
        GameManager.instance.Score = _score;
        GameManager.instance.Time = _time;
        GameManager.instance.Intel = _intel;
        GameManager.instance.Dead = false;
        GameManager.instance.ChangeScene("GameOverWin");
    }
    
}
