using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyAlien : Character
{
    Player _target;
    AudioSource _audio;
    [SerializeField] float _maxFollowDistance = 5f;
    [SerializeField] float _speed = 100f;
    [SerializeField] float _speedPerLevel = 5f;
    [SerializeField] float _maxSpeed = 200f;
    [SerializeField] int _score = 100;
    int _movePath = 0;
    float _radian = 0;
    [SerializeField] float _cycle = 0.8f;
    [SerializeField] float _cycleOffset = 0.75f;
    [SerializeField] float _timeBetweenVoice = 5f;
    Coroutine _voice = null;
    [SerializeField] ParticleSystem _deathParticles;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _audio = GetComponent<AudioSource>();
    }

    void Follow(){
        float deltaX = _target.transform.position.x - this.transform.position.x;
        float deltaY = _target.transform.position.y - this.transform.position.y;
        float direction = Mathf.Atan2(deltaY, deltaX);
        _movePath += 1;
        _radian += Mathf.PI * _cycle * Time.deltaTime;
        this.velocity = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction)) * _speed * Time.deltaTime * (Mathf.Sin(_radian) + _cycleOffset);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Particles"){
            StartCoroutine(Kill());
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Saw"){
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill(){
        Instantiate(_deathParticles, this.transform.position, this.transform.rotation);
        this.GetComponent<Collider>().enabled = false;
        bool isDying = true;
        float dieTimeTotal = _deathParticles.main.duration;;
        float dieTime = dieTimeTotal;
        while(isDying){
            dieTime -= Time.deltaTime;
            if(dieTime <= 0){
                isDying = false;
            }
            yield return null;
        }
        WorldManager.instance.Score += _score;
        GameManager.instance.AchievedStep("Attack");
        Destroy(this.gameObject);
    }

    IEnumerator Voice(){
        _audio.Play();
        yield return new WaitForSeconds(_timeBetweenVoice);
        _voice = null;
    }

    void Update()
    {   

        if(Vector2.Distance(_target.transform.position, this.transform.position) <= _maxFollowDistance){
            Follow();
            if(_voice == null){
                _voice = StartCoroutine(Voice());
            }
        }
    }
}

