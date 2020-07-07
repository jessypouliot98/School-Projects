using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] GameObject _pageContainer;
    [SerializeField] Text _creditPage;
    [SerializeField] string[] _texts;
    Vector2 _initPressed;
    Vector2 _initPagePos;
    Vector2 _lastPressedPositionFrameTime;
    bool _isPressed = false;
    Coroutine _transition;
    [SerializeField] float _transitionSpeed = 3;

    void Start(){
        for(int i = 0; i < _texts.Length; i++){
            string pageText = UnescapeString(_texts[i]);
            Text page = Instantiate(_creditPage, (Vector2)_pageContainer.transform.position + Offset(i), Quaternion.identity);
            page.transform.SetParent(_pageContainer.transform);
            page.text = pageText;
        }
    }

    string UnescapeString(string text){
        return text.Replace("\\n","\n");
    }

    Vector2 Offset(int i){
        return new Vector2(Screen.width * i, -20);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(_transition != null){
                StopCoroutine(_transition);
            }
            _initPressed = (Vector2)Input.mousePosition;
            _initPagePos = _pageContainer.transform.position;
            _isPressed = true;
        }
        else if(Input.GetMouseButtonUp(0)){
            _isPressed = false;
            SetPagePosition();
        }
        if(_isPressed){
            _lastPressedPositionFrameTime = Input.mousePosition;
            float deltaX = Input.mousePosition.x - _initPressed.x;
            MovePagePosition(deltaX);
        }
    }

    void SetPagePosition(){
        float deltaFrameMove = _lastPressedPositionFrameTime.x - Input.mousePosition.x;
        int currentIndex = (int)Mathf.Abs(Mathf.Round(_pageContainer.transform.position.x / Screen.width));
        if(deltaFrameMove/Screen.width >= 0.06f){
            currentIndex = Mathf.Min(_texts.Length - 1, currentIndex + 1);
            _transition = StartCoroutine(MoveTransitionTo(new Vector2(-Offset(currentIndex).x + Screen.width/2, _initPagePos.y)));
        }
        else if(deltaFrameMove/Screen.width <= -0.05f) {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            _transition = StartCoroutine(MoveTransitionTo(new Vector2(-Offset(currentIndex).x + Screen.width/2, _initPagePos.y)));
        } 
        else {
            int index = -Mathf.Min(0, Mathf.FloorToInt(_pageContainer.transform.position.x / Screen.width));
            index = Mathf.Min(index, _texts.Length -1);
            _transition = StartCoroutine(MoveTransitionTo(new Vector2(-Offset(index).x + Screen.width/2, _initPagePos.y)));
        }
    }

    void MovePagePosition(float deltaX){
        _pageContainer.transform.position = new Vector2(_initPagePos.x + deltaX, _initPagePos.y);
    }

    IEnumerator MoveTransitionTo(Vector2 targetPos){
        // Debug.Log(Vector2.Distance(_pageContainer.transform.position, targetPos));
        while(Vector2.Distance(_pageContainer.transform.position, targetPos) >= 2){
            float deltaX = targetPos.x - _pageContainer.transform.position.x;
            float percent = Mathf.Abs(deltaX / Screen.width);
            Vector2 moveTo = new Vector2(
                deltaX < 0 ? _pageContainer.transform.position.x - _transitionSpeed * Time.deltaTime * percent : _pageContainer.transform.position.x  + _transitionSpeed * Time.deltaTime * percent,
                targetPos.y
            );
            _pageContainer.transform.position = moveTo;
            yield return null;
        }
    }
}

/*
SOUND EFFECTS:\n
\n
JUMP // https://freesound.org/people/acebrian/sounds/380471/\n
LANDING // https://freesound.org/people/KieranKeegan/sounds/422749/\n
ENERGY JUMP // https://freesound.org/people/ejfortin/sounds/49674/\n
SPINING SAW // https://freesound.org/people/sophiehall3535/sounds/245935/\n
STOMPER1 // https://freesound.org/people/egolessdub/sounds/97563/\n
STOMPER2 // https://freesound.org/people/patchen/sounds/20183/\n
ALIEN // https://freesound.org/people/zippi1/sounds/18868/\n
DEATH // https://freesound.org/people/NeoSpica/sounds/434147/\n



*/