using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager instance;
    [SerializeField] Text _scoreUI;
    [SerializeField] Text _timerUI;
    [SerializeField] Text _intelUI;
    [SerializeField] Text _levelUI;
    [SerializeField] int _intelMinLength = 2;
    [SerializeField] int _scoreMinLength = 6;
    [SerializeField] int _levelMinLength = 2;
    int _score;
    int _time;
    int _intel;
    int _level;
    [SerializeField] int _timeToScoreMult = 20;
    [SerializeField] int _intelToScoreMult = 100;
    [SerializeField] float _timeWait = 1;
    [SerializeField] float _scoreVolume = 1;
    [SerializeField] AudioClip _scoreClip;

    Vector3 SoundPosition{
        get{
            return new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z - 2);
        }
    }


    /*
        INIT
    */

    void PlaySound(){
        AudioSource.PlayClipAtPoint(_scoreClip, SoundPosition, _scoreVolume);
    }

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
        _score = !GameManager.instance.Dead ? GameManager.instance.Score : GameManager.instance.TempScore;
        _time = GameManager.instance.Time;
        _intel = GameManager.instance.Intel;
        _level = GameManager.instance.Level;
        if(!GameManager.instance.Dead){
            _level = GameManager.instance.Level - 1;
            GameManager.instance.Score = _score + (_time * _timeToScoreMult) + (_intel * _intelToScoreMult);
            StartCoroutine(TimeToScore());
            StartCoroutine(IntelToScore());
        }
        DisplayTimer();
        DisplayScore();
        DisplayIntel();
        DisplayLevel();

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
    void DisplayTimer(){
        int m = _time / 60;
        int s = _time % 60;
        _timerUI.text = m.ToString() + ":" + ((s < 10) ? "0" + s.ToString() : s.ToString());
    }

    void DisplayLevel(){
        string text = _level.ToString();
        while(text.Length < _levelMinLength){
            text = "0" + text;
        }
        _levelUI.text = "Level: " + text;
    }

    void DisplayIntel(){
        string text = _intel.ToString();
        while(text.Length < _intelMinLength){
            text = "0" + text;
        }
        _intelUI.text = text;
    }

    IEnumerator TimeToScore(){
        yield return new WaitForSeconds(_timeWait);
        while(_time > 0){
            PlaySound();
            if(_time > 100){
                _time -= 5;
                _score += 5 * _timeToScoreMult;
            }
            else if(_time > 10){
                _time -= 3;
                _score += 3 * _timeToScoreMult;
            }
            else {
                _time--;
                _score += _timeToScoreMult;
            }
            DisplayTimer();
            DisplayScore();
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator IntelToScore(){
        yield return new WaitForSeconds(_timeWait);
        while(_intel > 0){
            PlaySound();
            _intel -= 1;
            _score += 1 * _intelToScoreMult;
            DisplayIntel();
            DisplayScore();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
