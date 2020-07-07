using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    string _moveAnim = "moving";
    private bool _isAlive;//Alive/Dead
	private float _speed = 4f;//Movement Speed
	private BoxCollider2D _box2D;//collider instance
	private Rigidbody2D _rBody;//rigidbody instance
    Animator _anim;
    bool _hasKey;
    float _startTime;
    AudioSource _sound;
    [SerializeField] UIKey _uiKey;

	void Start () {
        _hasKey = false;
        _isAlive = true;
        _box2D = GetComponent<BoxCollider2D>();//collider instance
        _rBody = GetComponent<Rigidbody2D>();//rigidbody instance
        _sound = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }
	
    //Movements
	void Move () {
        //movement directions (controls)
		float axisX = Input.GetAxis("Horizontal");
		float axisY = Input.GetAxis("Vertical");

        if(Mathf.Abs(axisX) > 0.1){
            _anim.SetBool(_moveAnim, true);
        }
        else if(Mathf.Abs(axisY) > 0.1){
            _anim.SetBool(_moveAnim, true);
        }
        else{
            _anim.SetBool(_moveAnim, false);
        }

        //calculate velocity
        Vector2 moveDir = new Vector2(axisX, axisY);
        _rBody.velocity = moveDir * _speed;//set velocity

    }

    //Rotations
    void Rotate () {
        Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//mouse position
        //calculate angle
        float deltaX = this.transform.position.x - mPos.x;
        float deltaY = this.transform.position.y - mPos.y;
        float angleRad = Mathf.Atan2( deltaY, deltaX );
        float angleDeg = angleRad * Mathf.Rad2Deg;

        _rBody.rotation = angleDeg - 90;
    }

    public void PlaySound(AudioClip clip)
    {
        _sound.clip = clip;
        _sound.Play();
    }

    //Add key to inventory
    public void AddKey () {
        _hasKey = true;
        _uiKey.DisplayKey(true);
    }
    //Remove key from inventory
    public void RemoveKey () {
        _hasKey = false;
        _uiKey.DisplayKey(false);
    }
    public bool HasKey
    {
        get
        {
            return _hasKey;
        }
    }

    public void Kill()
    {
        _isAlive = false;
        _rBody.velocity = Vector2.zero;
        GameObject.Find("MainManager").GetComponent<MainManager>().GameOver();
    }

    void FixedUpdate () {
        if (_isAlive)
        {
            Move();
            Rotate();
        }
	}
}
