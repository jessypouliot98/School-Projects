using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : CharacterEntity {

	// VARIABLES
	[SerializeField] string _animMove = "Moving";
	[SerializeField] string _animAxisX = "AxisX";
	[SerializeField] string _animAxisY = "AxisY";
	[SerializeField] string _animAttack = "Attack";
	[SerializeField] string _animDashing = "Dash";
	[SerializeField] string _animAlive = "Alive";
	float _direction = 0;
	[SerializeField] Sprite _uiImage;
	[SerializeField] string _name;
    [SerializeField] bool _isSelected = false;
	[SerializeField] bool _isAlive = true;
	[SerializeField] [Range(0f,1f)] float _healPercent = 0.1f;
	float _maxHunger = 100;//max hunger
	[SerializeField] [Range(0,100)] float _hunger;//current hunger
	[SerializeField] int _starved = 10;//feeds from hunger
	[SerializeField] float _feedTime = 10;//hunger-- time
	[SerializeField] float _feedTimeMult = 0.5f;//feedTime multiplier for when not active (0.5 = t/2)
	[SerializeField] float _speed = 100f;
	[SerializeField] float _dashForce = 500f;
	[SerializeField] float _aiMoveStartTime = 3f;
	Coroutine _dash = null;
	Coroutine _slash = null;
	Coroutine _aiMovement = null;
	[SerializeField] GameObject _slashPrefab;
	[SerializeField] float _slashSpeed = 0.1f;
	float _autoSelectTime = 0.6f;
	string _currentScene;

	// GET SET
	public Sprite UIImage{
		get{
			return _uiImage;
		}
	}
	public string Name{
		get{
			return _name;
		}
	}
	public float CurrentLife{
		get{
			return _currentLife;
		}
		set{
			_currentLife = (value > _maxLife) ? _maxLife : value;
			if(value <= 0){
				SendMessage("Die");//Die when no more hp left
			}
		}
	}
	public float Hungry{
		get{
			return _hunger;
		}
		set{
			_hunger = (value < 0) ? 0 : value;
			SetHungerUI();//refresh display
		}
	}
	public bool Alive{
		get{
			return _isAlive;
		}
	}
	public string Scene{
		set{
			_currentScene = value;
			PlayerManager.instance.UpdatePlayer(this);
		}
		get{
			return _currentScene;
		}
	}

	//INIT
	void Start(){
		base.Start();
        GameManager.instance.selectedCharEvent += OnSelectedChar;
		GameManager.instance.selectedSceneEvent += OnChangeScene;
		if(PlayerManager.instance.GetPlayerSelected(this)){
			OnSelectedChar(Name);
			SelectChar();
		}
		Scene = GameManager.instance.Scene;
		_hunger = _maxHunger;
		StartCoroutine(Hunger());
		Debug.Log(GameManager.instance.Scene);
    }

	// EVENTS
	// select/deselect character on new character select event
    void OnSelectedChar(string name){
		if(_isAlive){
			_isSelected = (name == _name);
			if(_rBody != null){
				_rBody.bodyType = (name == _name) ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
			}
			if(_isSelected){
				SetLifeUI();
				SetHungerUI();
			}
		}
    }

	//update player info on before destroy on change scene
	void OnChangeScene(string scene){
		PlayerManager.instance.UpdatePlayer(this);
	}

	//Select character on click
    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && _isAlive){
            SelectChar();
        }
    }

	//select character from outside (playerUI button)
	public void SelectChar(){
		GameManager.instance.SelectedChar(_name);
	}
	// COROUTINES
	//get hungry over time, heals periodicly when food is not at 0, if at 0 eat something, if you dont have food, lose health (can die)
	IEnumerator Hunger(){
		while(_isAlive){
			yield return new WaitForSeconds((_isSelected) ? _feedTime * _feedTimeMult : _feedTime);
			if(_hunger > 0){
				CurrentLife += _maxLife * _healPercent;
				SetLifeUI();
				_hunger -= _starved;
			} else if(_currentLife > 0) {
				EatSomething();//eat when hunger is at 0
				this.Hit = _maxLife * _healPercent;
				// if(_currentLife >= _maxLife * _healPercent){
					//dont kill
				// }
			}
			if(_isSelected){ SetHungerUI(); }
		}
	}
	//auto move when in targets are available and not selected
	IEnumerator AIMovement(){
		_rBody.velocity = Vector2.zero;

		GameObject[] targets = GameObject.FindGameObjectsWithTag("BaseTargets");

		yield return new WaitForSeconds(_aiMoveStartTime);// starts after x time has pasted by
		
		int i = 0;
		while(!_isSelected && targets.Length != 0){
			Vector2 deltaPos = this.transform.position - targets[i].transform.position;
			float angle = Mathf.Atan2(deltaPos.y, deltaPos.x) * Mathf.Rad2Deg;
			_rBody.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _speed * Time.fixedDeltaTime;
			_anim.SetBool(_animMove, true);
			_rBody.rotation = angle;
			_anim.SetFloat(_animAxisX, Mathf.Cos(angle * Mathf.Deg2Rad + Mathf.PI));
			_anim.SetFloat(_animAxisY, Mathf.Sin(angle * Mathf.Deg2Rad + Mathf.PI));

			if(Vector2.Distance(this.transform.position, targets[i].transform.position) < 1f){//Achieved target pos
				i = Random.Range(0, targets.Length);
				_rBody.velocity = Vector2.zero;
				_anim.SetBool(_animMove, false);
				yield return new WaitForSeconds(_aiMoveStartTime);
			}

			yield return new WaitForFixedUpdate();
		}

		_aiMovement = null;
	}
	//jump
	IEnumerator Dash(){
		_anim.SetBool(_animDashing, true);
		float AxisX = Input.GetAxisRaw("Horizontal");
		float AxisY = Input.GetAxisRaw("Vertical");
		float direction = Mathf.Atan2(AxisY, AxisX);
		if(AxisX == 0 && AxisY == 0){
			yield return null;
		} else {
			_rBody.AddForce(new Vector2(Mathf.Cos(direction), Mathf.Sin(direction)).normalized * _dashForce);
			yield return new WaitForSeconds(0.3f);
		}
		_anim.SetBool(_animDashing, false);
		_dash = null;
	}

	//Attack, create a projectile
	IEnumerator Slash(){
		_anim.SetTrigger(_animAttack);
		GameObject slasher = Instantiate(_slashPrefab, this.transform.position, Quaternion.Euler(0f, 0f, _direction - 90));
		slasher.GetComponent<Slash>().Dmg = _dmg;
		bool active = true;
		while(active){
			yield return new WaitForSeconds(_slashSpeed);
			active = false;
		}
		_slash = null;
	}

	IEnumerator AutoSwitchScene(){
		yield return new WaitForSeconds(_autoSelectTime);
		GameManager.instance.HQ();
	}

	// FUNCTIONS
	//display current life in %
	void SetLifeUI(){
		if(_isSelected){
			CanvasManager.instance.Life = (int)(_currentLife / _maxLife * 100);
		}
	}
	//display hunger in %
	void SetHungerUI(){
		if(_isSelected){
			CanvasManager.instance.Hunger = (int)(_hunger / _maxHunger * 100);
		}
	}
	// Eat food from inv
	void EatSomething(){
		Hungry += InventoryManager.instance.EatFood();
	}
	void Damaged(){
		if(_currentLife <= 0){
			_currentLife = 0;
			Die();
		}
		SetLifeUI();
		PlayerManager.instance.UpdatePlayer(this);
	}
	void Die(){
		_rBody.bodyType = RigidbodyType2D.Static;
		_isAlive = false;
		_anim.SetBool(_animAlive, false);
		PlayerManager.instance.Death(this);
		StartCoroutine(AutoSwitchScene());
	}
	void Move(){
		float AxisX = Input.GetAxisRaw("Horizontal");
		float AxisY = Input.GetAxisRaw("Vertical");
		float direction = Mathf.Atan2(Mathf.Abs(AxisY), Mathf.Abs(AxisX));
		Vector2 speed = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction)).normalized * _speed;
		MoveX = AxisX * speed.x * Time.fixedDeltaTime;
		MoveY = AxisY * speed.y * Time.fixedDeltaTime;
		_anim.SetBool(_animMove, (Mathf.Abs(AxisX) > 0 || Mathf.Abs(AxisY) > 0));
	}
	void Rotate () {
        Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//mouse position
        //calculate angle
        float deltaX = this.transform.position.x - mPos.x;
        float deltaY = this.transform.position.y - mPos.y;
        float angleRad = Mathf.Atan2( deltaY, deltaX );
        float angleDeg = angleRad * Mathf.Rad2Deg;
		_anim.SetFloat(_animAxisX, Mathf.Cos(angleRad + Mathf.PI));
		_anim.SetFloat(_animAxisY, Mathf.Sin(angleRad + Mathf.PI));
		_direction = angleDeg;
    }
	
	// UPDATES
	private void FixedUpdate() {
		if(_isAlive){
			if(_isSelected){
				if(_dash == null){//while not dashing
					Move();
					Rotate();
				}
			} else {
				if(_aiMovement == null){
					_aiMovement = StartCoroutine(AIMovement());
				}
			}
		}
	}

	private void Update() {
		if(_isSelected && _isAlive){
			// if(Input.GetKey(KeyCode.K)){
			// 	this.Hit = _maxLife;
			// } 
			// if(Input.GetKey(KeyCode.L)){
			// 	InventoryManager.instance.AddToInv("Key");
			// }
			if(Input.GetButtonDown("Jump") && _dash == null){//while button is down and not dashing
				_dash = StartCoroutine(Dash());
			} else if(Input.GetButtonDown("Fire1") && _slash == null){
				_slash = StartCoroutine(Slash());
			}
		}
	}
}
