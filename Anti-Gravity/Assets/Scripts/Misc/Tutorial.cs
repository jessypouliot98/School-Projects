using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    string _achievedStep;
    [SerializeField] GameObject[] _stepUIPrefs;
    GameObject _currentUIInstance;
    void Start()
    {
        GameManager.instance.tutorialStepEvent += NextStep;
        if(GameManager.instance.Level == 0){
            _achievedStep = "Start";
            StartCoroutine(StepOne());
        }
        else if(GameManager.instance.Level == 1){
            StartCoroutine(StepBurn());
        }
        else if(GameManager.instance.Level == 2){
            StartCoroutine(StepKill());
        }
    }

    void NextStep(string step){
        _achievedStep = step;
    }

    void ResetUI(){
        if(_currentUIInstance != null){
            _currentUIInstance.SetActive(false);
            _currentUIInstance = null;
        }
    }

    //jump
    IEnumerator StepOne(){
        Debug.Log("STEP 1");
        ResetUI();
        _currentUIInstance = _stepUIPrefs[0];
        _currentUIInstance.SetActive(true);
        bool condition = false;
        while(!condition){
            condition = _achievedStep == "Jump";
            yield return null;
        }
        StartCoroutine(StepTwo());
    }

    //double Jump
    IEnumerator StepTwo(){
        Debug.Log("STEP 2");
        ResetUI();
        _currentUIInstance = _stepUIPrefs[1];
        _currentUIInstance.SetActive(true);
        bool condition = false;
        while(!condition){
            condition = _achievedStep == "Double Jump";
            yield return null;
        }
        StartCoroutine(StepThree());
    }

    //jump tips
    IEnumerator StepThree(){
        Debug.Log("STEP 3");
        ResetUI();
        _currentUIInstance = _stepUIPrefs[2];
        _currentUIInstance.SetActive(true);
        yield return new WaitForSeconds(5f);
        StartCoroutine(StepFour());
    }

    //movement tip
    IEnumerator StepFour(){
        Debug.Log("STEP 4");
        ResetUI();
        _currentUIInstance = _stepUIPrefs[3];
        _currentUIInstance.SetActive(true);
        yield return new WaitForSeconds(5f);
        ResetUI();
    }

    //Kill
    IEnumerator StepKill(){
        Debug.Log("STEP Kill");
        ResetUI();
        _currentUIInstance = _stepUIPrefs[4];
        _currentUIInstance.SetActive(true);
        bool condition = false;
        while(!condition){
            condition = _achievedStep == "Attack";
            yield return null;
        }
        StartCoroutine(StepSolo());
    }

    //Burn
    IEnumerator StepBurn(){
        Debug.Log("STEP Burn");
        ResetUI();
        _currentUIInstance = _stepUIPrefs[5];
        _currentUIInstance.SetActive(true);
        bool condition = false;
        while(!condition){
            condition = _achievedStep == "Burned";
            yield return null;
        }
        ResetUI();
    }

    //You're on your own kid
    IEnumerator StepSolo(){
        Debug.Log("STEP SOLO");
        ResetUI();
        _currentUIInstance = _stepUIPrefs[6];
        _currentUIInstance.SetActive(true);
        yield return new WaitForSeconds(5f);
        ResetUI();
    }
}
