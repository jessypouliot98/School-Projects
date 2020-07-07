using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour {

    [SerializeField] public float _timerDuration = 1f;
    Text _uiField;

    float timer;

    private void Start()
    {
        timer = _timerDuration;//set timer to timer duration
        _uiField = this.GetComponent<Text>();
        _uiField.color = Color.white;
    }

    string DisplayTime (float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        if (minutes == 0) { _uiField.color = (seconds <= 10) ? Color.red : Color.yellow; }
        return minutes + ":" + ((seconds < 10) ? "0" + seconds.ToString() : seconds.ToString());//diplay as mm:ss
    }

    void Update()
    {
        timer = (timer <= 0.5f) ? 0f : timer - Time.deltaTime;//timer doesnt go below 0
        _uiField.text = DisplayTime(timer);//Diplay timer on UI
        if(timer <= 0) { GameObject.Find("Player").GetComponent<Player>().Kill(); }//Kill player on timer end
    }
}
