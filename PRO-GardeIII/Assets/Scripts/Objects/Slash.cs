using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour {

	Rigidbody2D _rBody;
	Animator _anim;
	Vector2 _initPos;
	float _fixAngle = 90;
	float _dmg = 0;
	[SerializeField] float _speed = 650;
	[SerializeField] float _maxDistance = 4f;

	//GET SET
	public float Dmg{
		set{
			_dmg = value;
		}
		get{
			return _dmg;
		}
	}

	//INIT
	private void Start() {
		_rBody = this.GetComponent<Rigidbody2D>();
		_initPos = this.transform.position;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag != "Player"){//pass through players
			Destroy(this.gameObject);//Destroy slash on hit
			if(other.gameObject.tag == "Monster"){
				other.gameObject.GetComponent<Monster>().Hit = _dmg;//hit monster
			}
			else if(other.gameObject.tag == "Box"){
				other.gameObject.GetComponent<Crate>().Smash();//hit box
			}
		}
	}

	void FixedUpdate () {
		float angle = _fixAngle;
		_rBody.velocity = new Vector2(Mathf.Cos( (this.transform.eulerAngles.z - angle) * Mathf.Deg2Rad), Mathf.Sin( (this.transform.eulerAngles.z - angle) * Mathf.Deg2Rad)) * _speed * Time.fixedDeltaTime;
		if(Vector2.Distance(this.transform.position, _initPos) >= _maxDistance){
			Destroy(this.gameObject);//destroy when max distance is reached
		}
	}
}
