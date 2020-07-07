using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

	//
	//	VARIABLES
	//
	public static PlayerManager instance;
	[SerializeField] Player[] _players;
	List<string> _playerScene;
	List<Vector2> _playerPosition;
	List<float> _playercurrentLife;
	List<float> _playerHunger;
	[SerializeField] int _playerSelected = 0;
	GameObject[] _targets;
	//
	//	GET SET
	//
	public Player[] Players{
		get{
			return _players;
		}
	}
	public int PlayerSelected{
		get{
			return _playerSelected;
		}
		set{
			_playerSelected = value;
		}
	}
	public string GetPlayerScene(Player player){
		for(int i = 0; i < _players.Length; i++){
			if(_players[i].Name == player.Name){
				// Debug.Log(_playerScene[i]);
				return _playerScene[i];
			}
		}
		return null;
	}

	public bool GetPlayerSelected(Player player){
		if(_players[_playerSelected].Name == player.Name){
			return true;
		}
		return false;
	}

	public int PlayerAlive{
		get{
			int nbAlive = _players.Length-1;
			for(int i = 0; i < _players.Length; i++){
				if(_playercurrentLife[i] <= 0){
					nbAlive--;
				}
			}
			return nbAlive;
		}
	}
	//
	//	INIT
	//
	private void Awake() {
		if(instance != null){
			Destroy(this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(this);
		}
	}
	private void Start() {
		SceneManager.sceneLoaded += OnSceneLoaded;
		GameManager.instance.selectedCharEvent += OnSelectedChar;
		GameManager.instance.resetGameEvent += OnReset;
		Initialise();
	}

	//Initialise everything on startup/reset
	void Initialise(){
		_targets = GameObject.FindGameObjectsWithTag("BaseTargets");
		_playerScene = new List<string>();
		_playerPosition = new List<Vector2>();
		_playercurrentLife = new List<float>();
		_playerHunger = new List<float>();
		for(int i = 0; i < _players.Length; i++){
			Vector2 spawn;
			try{
				spawn = _targets[i].transform.position;
			}
			catch{
				spawn = GameObject.Find("Spawn").transform.position;
			}
			GameObject character = Instantiate(_players[i].gameObject, spawn, Quaternion.identity);
			_playerPosition.Add(_players[i].transform.position);
			_playerScene.Add(GameManager.instance.Scene);
			_playercurrentLife.Add(_players[i].CurrentLife);
			_playerHunger.Add(_players[i].Hungry);
		}
		GameObject[] playerUI = GameObject.FindGameObjectsWithTag("PlayerUI");
		foreach(GameObject ui in playerUI){
			Destroy(ui);
		}
		CanvasManager.instance.SetPlayerUI();
	}
	//
	//	EVENTS
	//

	//select scene event
	public delegate void PlayerDeathDelegate(string name);
	public event PlayerDeathDelegate playerDeathEvent;
	public void Death(Player player){
		playerDeathEvent(player.Name);
		if(PlayerAlive == 0){
			GameManager.instance.GameOver();
			return;
		}
	}

	//Reinitialise Manager
	void OnReset(){
		Initialise();
	}
	//when a player is selected
	void OnSelectedChar(string name){
		for(int i = 0; i < _players.Length; i++){
			if(_players[i].Name == name){
				_playerSelected = i;
			}
		}
    }
	//when scene is loaded
	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		//loop all players
		for(int i = 0; i < _players.Length; i++){
			//if they are in this current scene, spawn them.
			if(_playerScene[i] == scene.name){
				Vector2 spawn;//spawn location
				spawn = Vector2.zero;//base spawn location
				
				//if player selected (playing character)
				if(i == _playerSelected){
					if(_playerPosition[i] == Vector2.zero){
						if(GameObject.Find("Spawn")){
							// Debug.Log("Spawn Position");
							spawn = GameObject.Find("Spawn").transform.position;//spawn at spawn gizmo
						}
					} else {
						// Debug.Log("Old Position");
						spawn = _playerPosition[i];//spawn at its old position
					}
				//other character
				} else {
					// Debug.Log("Old Position");
					spawn = _playerPosition[i];//spawn at it old position
				}

				//Spawns player if alive
				if(_playercurrentLife[i] > 0){
					GameObject character = Instantiate(_players[i].gameObject, spawn, Quaternion.identity);
					character.GetComponent<Player>().CurrentLife = _playercurrentLife[i];
					character.GetComponent<Player>().Hungry = _playerHunger[i];
				}
			}
		}
	}
	//
	//	FUNCTIONS
	//
	//Update player information
	public void UpdatePlayer(Player player){
		int nbDead = 0;
		for(int i = 0; i < _players.Length; i++){
			if(player != null){
				if(_players[i].Name == player.Name){
					_playerPosition[i] = player.transform.position;
					_playerScene[i] = player.Scene;
					_playercurrentLife[i] = player.CurrentLife;
					_playerHunger[i] = player.Hungry;
					if(player.CurrentLife <= 0){ nbDead++; }
					break;
				}
			}
		}
		//if every player is dead, game over
		if(nbDead == _players.Length){
			Debug.Log("Game Over");
		}
	}

}
