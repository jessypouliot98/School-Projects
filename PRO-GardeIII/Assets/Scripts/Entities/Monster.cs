using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : CharacterEntity {
	GameObject[] _targets;//list of players
	[SerializeField] float _followDistance = 5f;//distance maximum to continue following atarget
	[SerializeField] float _speed = 150f;//movement speed
	[SerializeField] float _attackTime = 0.5f;//attck cooldown in seconds
	[SerializeField] LayerMask _layerMask;//vision for raycast
	Coroutine _attack = null;//attack
	Coroutine _dash = null;//dash
	[SerializeField] float _dragCoefficient = 1f;
	[SerializeField] float _dragStop = 1f;
	[SerializeField] float _dashPause = 1f;//wait time before dashing
	[SerializeField] float _dashForce = 150f;//force of dash
	[SerializeField] float _dashDistance = 2;//distance to be able to dash
	[SerializeField] float _dashCooldown = 2f;//time in seconds for next dash to be possible
	[SerializeField] GameObject _drop = null;
	string _directionAnim = "axisX";
	string _hitAnim = "hit";

	//INIT
	private void Start() {
		base.Start();
	}

	//EVENTS
	private void OnCollisionEnter2D(Collision2D other) {
		if(other.gameObject.tag == "Player" && _attack == null){
			_attack = StartCoroutine(Attack(other.gameObject.GetComponent<Player>()));
		}
	}
	private void OnCollisionExit2D(Collision2D other) {
		if(other.gameObject.tag == "Player" && _attack != null){
			StopCoroutine(_attack);
			_attack = null;
		}
	}

	//COROUTINES
	IEnumerator Attack(Player player){
		while(player.Alive){
			player.Hit = _dmg;
			yield return new WaitForSeconds(_attackTime);
		}
	}
	IEnumerator Dash(Vector2 direction){
		_rBody.bodyType = RigidbodyType2D.Dynamic;
		_rBody.drag = _dragCoefficient;
		yield return new WaitForSeconds(_dashPause);
		_rBody.AddForce(direction * _dashForce);
		yield return new WaitForSeconds(_dashPause);
		//yield return new WaitForFixedUpdate();
		while(_rBody.velocity != Vector2.zero){
			if(Mathf.Abs(_rBody.velocity.x + _rBody.velocity.y) <= _dragStop){
				_rBody.velocity = Vector2.zero;
			}
			yield return new WaitForFixedUpdate();
		}
		_rBody.bodyType = RigidbodyType2D.Kinematic;
		yield return new WaitForSeconds(_dashCooldown);
		_dash = null;
	}

	//FUNCTIONS
	void Damaged(){
		if(_currentLife <= 0){
			_anim.SetTrigger(_hitAnim);
			_currentLife = 0;
			Die();
		}
	}
	void Die(){
		ObjectManager.instance.RemoveObj(this.gameObject);
		Destroy(this.gameObject);
		GameManager.instance.AddScore(_score);
		//make the ennemie drop an item if it is set
		if(_drop){
			Instantiate(_drop, this.transform.position, Quaternion.identity);
		}
	}
	void FollowTarget(){
		if(_targets == null){
			_targets = GameObject.FindGameObjectsWithTag("Player");
		}
		foreach (GameObject target in _targets){
			if(target.GetComponent<Player>().Alive){
				Vector2 targetPos = (target.transform.position - this.transform.position).normalized;
				RaycastHit2D sight = Physics2D.Raycast(this.transform.position, targetPos, _followDistance, _layerMask);
				if(sight.collider != null){
					float deltaY = sight.collider.transform.position.y - this.transform.position.y;
					float deltaX = sight.collider.transform.position.x - this.transform.position.x;
					float angle = Mathf.Atan2(deltaY, deltaX);
					Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
					_anim.SetFloat(_directionAnim, (direction.x < 0)? -1 : 1);
					if(_dash == null) {
						if(Vector2.Distance((Vector2)this.transform.position, (Vector2)sight.collider.gameObject.transform.position) < _dashDistance){
							_dash = StartCoroutine(Dash(direction));
						} else {
							transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, _speed * Time.fixedDeltaTime);
						}
					}
					break;
				}
			}
		}
	}

	//UPDATES
	private void FixedUpdate() {
		FollowTarget();
	}
}
