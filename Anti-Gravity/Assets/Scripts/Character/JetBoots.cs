using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetBoots : MonoBehaviour
{
    Vector2 _initPos;
    bool _wallJump = false;
    bool _doubleJump = true;

    Player _char;
    // [SerializeField] Rigidbody[] _legs;
    [SerializeField] float _maxForce = 15f;
    [SerializeField] float _minForce = 5f;
    [SerializeField] float _jumpMult = 3f;
    [SerializeField] float _boostMult = 5f;
    [SerializeField] Discharge _dischargePrefab;
    [SerializeField] AudioClip _jumpClip;
    [SerializeField] AudioClip _powerJumpClip;
    [SerializeField] AudioClip _landingClip;
    [SerializeField] float _bootsVolume = 1;

    void Start(){
        _char = GetComponent<Player>();
    }

    Vector3 SoundPosition{
        get{
            return new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z - 2);
        }
    }

    private void Update() {
        if(_char.Alive){
            if(Input.GetMouseButtonDown(0)){
                _initPos = Input.mousePosition;
            } 
            else if(Input.GetMouseButtonUp(0)){
                if(_wallJump || _doubleJump){
                    Vector2 endPos = Input.mousePosition;
                    Vector2 jumpVel = endPos - _initPos;
                    float force = Mathf.Abs(jumpVel.x) + Mathf.Abs(jumpVel.y);
                    float deltaX = endPos.x - _initPos.x;
                    float deltaY = endPos.y - _initPos.y;
                    float angle = Mathf.Atan2(deltaY, deltaX);

                    if(force >= _minForce){
                        this.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90);
                        //WALL JUMP SOUND
                        AudioSource.PlayClipAtPoint(_jumpClip, SoundPosition, _bootsVolume);
                        if(_wallJump){
                            _wallJump = false;
                            GameManager.instance.AchievedStep("Jump");
                        } else {
                            _doubleJump = false;
                            //DOUBLE JUMP SOUND
                            AudioSource.PlayClipAtPoint(_powerJumpClip, SoundPosition, _bootsVolume);
                            //DISCHARGE ENERGY BLAST
                            Discharge blast = Instantiate(_dischargePrefab, this.transform.position, Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg));
                            blast.transform.SetParent(this.transform);
                            GameManager.instance.AchievedStep("Double Jump");
                        }

                        _char.VelocityStop();

                        force = Mathf.Clamp(force, _minForce, _maxForce);
                        Vector2 push = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * (force * (!_doubleJump ? _boostMult : _jumpMult));
                        _char.push = -push;
                    } else {
                        Debug.Log("Player Canceled Jump");
                    }

                } else {
                    Debug.Log("Unable to Jump");
                }
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch(other.gameObject.tag){
            case "Wall":
                 //LANDING SOUND
                _wallJump = true;
                _doubleJump = true;
                AudioSource.PlayClipAtPoint(_landingClip, SoundPosition, _bootsVolume);
                _char.VelocityStop();
                break;
        }
    }

    void OnCollisionStay(Collision other) {
        switch(other.gameObject.tag){
            case "Wall":
                _wallJump = true;
                _doubleJump = true;
                break;
        }
    }

    private void OnCollisionExit(Collision other) {
        switch(other.gameObject.tag){
            case "Wall":
                _wallJump = false;
                break;
        }
    }
}

