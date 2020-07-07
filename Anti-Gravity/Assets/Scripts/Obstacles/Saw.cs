using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    AudioSource _audio;
    Transform _player;
    float _rotation = 0;
    [SerializeField] float _speed = 5;
    [SerializeField] float _volumeMult = 0.7f;
    
    void Start()
    {
        _rotation = this.transform.rotation.z;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _audio = this.GetComponent<AudioSource>();
    }

    void Rotate(){
        _rotation += _speed * Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(0f, 0f, _rotation);
    }

    void PanSound(){
        float maxDistance = 22;
        float distance = Vector2.Distance(this.transform.position, _player.position);
        float deltaX = _player.position.x - this.transform.position.x;
        _audio.panStereo = Mathf.Clamp(-1, -(deltaX / maxDistance), 1);
        _audio.volume = Mathf.Clamp(0, 1 - (distance / maxDistance), 1) * _volumeMult;
    }

    void Update()
    {
        Rotate();
        PanSound();
    }
}
