using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    float _speed = 20f;//rotate speed
    float _rotation;//initial rotation
    bool _rotateDirection = false;//left = true, right = false
    Vector2 _direction;
    [SerializeField] float _rotateSpeed = 1f;
    [SerializeField] float _rotateDiff = 22.5f;
    [SerializeField] GameObject _key;
    [SerializeField] GameObject _player;
    BoxCollider2D _box2D;
    Rigidbody2D _rBody;
    Laser _laser;
    bool _playedSound = false;

    void Start()
    {
        _rotation = this.transform.eulerAngles.z;
        _box2D = GetComponent<BoxCollider2D>();
        _rBody = GetComponent<Rigidbody2D>();
        _laser = GetComponent<Laser>();
    }

    //Rotate {this} to face at target {player}
    void Rotate()
    {
        if(_rotateDirection){
            _rBody.rotation = this.transform.eulerAngles.z + _rotateSpeed;
            _rotateDirection = (this.transform.eulerAngles.z >= _rotation + _rotateDiff) ? false : true;
        }
        else {
            _rBody.rotation = this.transform.eulerAngles.z - _rotateSpeed;
            _rotateDirection = (this.transform.eulerAngles.z <= _rotation - _rotateDiff) ? true : false;
        }
        _direction = new Vector2( Mathf.Cos( (this.transform.eulerAngles.z + 180) * Mathf.Deg2Rad ), Mathf.Sin((this.transform.eulerAngles.z + 180) * Mathf.Deg2Rad) ).normalized;
    }

    //Destroy and instanciate a key if it contains one
    public void Kill() {
        _laser.Activate(false);
        if (_key) { Instantiate(_key, this.transform.position, Quaternion.Euler(0,0,0)); }
        Destroy(this.gameObject);
    }

    //Plays sound when player gets close to this
    private void PlaySound ()
    {
        float distance = Vector2.Distance(this.transform.position, _player.transform.position);
        if (!_playedSound && distance < 6)
        {
            _playedSound = true;
            GetComponent<AudioSource>().Play();
        }
    }

    void FixedUpdate() {
        Vector2 thisPos = this.transform.position;
        Rotate();
        //Set laser vision
        _laser.Activate(true);
        _laser.SetLaser(thisPos, _direction);
        _laser.SetLaserColor(Color.red);
        //
        PlaySound();
    }
}
