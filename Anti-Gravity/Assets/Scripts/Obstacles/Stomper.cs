using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomper : MonoBehaviour
{
    [SerializeField] float _speedClose = 400f;
    [SerializeField] float _speedOpen = 100f;
    bool _state = true; //true = closing / false = opening
    
    Rigidbody _rbody;
    Vector2 _initPosition = Vector2.zero;
    float _angle = 0;
    Transform _player;
    AudioSource _audio;
    [SerializeField] AudioClip _stompUpSound;
    [SerializeField] AudioClip _stompDownSound;
    [SerializeField] float _volumeMult = 0.7f;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _rbody = GetComponent<Rigidbody>();
        _initPosition = this.transform.position;
        _angle = this.transform.eulerAngles.z - 90;
        _angle = Mathf.Floor(_angle / 90) * 90;
        this.transform.rotation = Quaternion.Euler(0f, 0f, _angle + 90);
        _rbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ | (((_angle / 90) % 2 == 0 ) ? RigidbodyConstraints.FreezePositionY : RigidbodyConstraints.FreezePositionX);
    }

    public float SetAngle{
        set{
            _angle = value + 90;
            this.transform.rotation = Quaternion.Euler(0f, 0f, value + 90);
        }
    }

    void PanSound(){
        float maxDistance = 15;
        float distance = Vector2.Distance(this.transform.position, _player.position);
        float deltaX = _player.position.x - this.transform.position.x;
        _audio.panStereo = Mathf.Clamp(-1, -(deltaX / maxDistance), 1);
        _audio.volume = Mathf.Clamp(0, 1 - (distance / maxDistance), 1) * _volumeMult;
    }

    void FixedUpdate(){
        _rbody.velocity = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad)) * (_state ? -_speedClose : _speedOpen) * Time.fixedDeltaTime;
    }

    void Update(){
        PanSound();
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Wall"){
            _state = !_state;
            _audio.clip = !_state ? _stompDownSound : _stompUpSound;
            _audio.Play();
        }
    }
}
