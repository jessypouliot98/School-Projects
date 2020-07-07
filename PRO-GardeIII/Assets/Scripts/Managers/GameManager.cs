using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	[SerializeField] const string _endScene = "GameOver";
	[SerializeField] const string _menu = "Menu";
	[SerializeField] const string _gameScene = "Headquarter";
	string _currentScene;
	bool _gameActive = false;
	State _gameState = State.Null;
	int _keys;
	[SerializeField] int _nbKeysToWin = 3;
	[SerializeField] int _aliveBonusScore = 5000;

	//GET SET
	public int Keys{
		set{
			_keys = value;
			if(_keys >= _nbKeysToWin){
				Win();
			}
		}
	}
	public string Scene{
		get{
			return _currentScene;
		}
		set{
			_currentScene = value;
		}
	}
	public State GameState{
		get{
			return _gameState;
		}
		set{
			_gameState = value;
		}
	}

	//INIT
	private void Awake()
	{
		if(instance != null){
			Destroy(this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(this);
		}
	}

	void Start(){
		GameManager.instance.resetGameEvent += OnReset;
		SceneManager.sceneLoaded += OnSceneLoaded;
		Initialise();
	}

	//Initialise everything on startup/reset
	void Initialise(){
		_keys = 0;
		Scene = SceneManager.GetActiveScene().name;
		if(CanvasManager.instance){ CanvasManager.instance.Activate(Scene != _endScene); }
		_gameActive = true;
	}

	//
	//	EVENTS
	//

	//reset game event
	public delegate void ResetGameDelegate();
	public event ResetGameDelegate resetGameEvent;
	public void ResetGame(){
		resetGameEvent();
	}

	//select character event
	public delegate void SelectedCharDelegate(string name);
	public event SelectedCharDelegate selectedCharEvent;
	public void SelectedChar(string name){
		selectedCharEvent(name);
	}

	//select scene event
	public delegate void SelectedSceneDelegate(string name);
	public event SelectedSceneDelegate selectedSceneEvent;
	public void SelectScene(string scene){
		if(selectedSceneEvent != null){
			selectedSceneEvent(scene);
		}
		SceneManager.LoadScene(scene);
	}

	//Scene loaded event
	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		Scene = scene.name;
		if(scene.name == _endScene){
			CanvasManager.instance.Activate(false);
		} else {
			CanvasManager.instance.Activate(true);
		}
		if(CanvasManager.instance){ CanvasManager.instance.gameObject.SetActive(true); }
		//Delete and hide everything not usefull for the menu/gameover scene
		if(scene.name == _endScene || scene.name == _menu){
			int score = CanvasManager.instance.Score;
			score = PlayerManager.instance.PlayerAlive * _aliveBonusScore;
			if(PlayerManager.instance){ Destroy(PlayerManager.instance.gameObject); }
			if(ObjectManager.instance){ Destroy(ObjectManager.instance.gameObject); }
			if(InventoryManager.instance){ Destroy(InventoryManager.instance.gameObject); }
			if(scene.name == _menu){
				if(CanvasManager.instance){ CanvasManager.instance.gameObject.SetActive(false); }
			}
		}
	}
	
	//update score score
	public delegate void AddScoreDelegate(int points);
	public event AddScoreDelegate addScoreEvent;
	public void AddScore(int points){
		addScoreEvent(points);
	}

	//Reinitialise Manager
	void OnReset(){
		Initialise();
	}
	//
	//	FUNCTIONS
	//

	public void HQ(){
		SelectScene(_gameScene);
	}
	public void Play(){
		if(_gameActive){ ResetGame(); }
		GameState = State.Null;
		SelectScene(_gameScene);
	}
	public void Win(){
		GameState = State.Win;
		SelectScene(_endScene);
	}
	public void GameOver(){
		GameState = State.Lose;
		SelectScene(_endScene);
	}
}
public enum State
{
	Null,
	Win,
	Lose
}