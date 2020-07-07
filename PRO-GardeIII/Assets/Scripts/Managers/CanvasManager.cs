using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	public static CanvasManager instance;
	[SerializeField] Text _scoreUI;//score UI
	[SerializeField] Text _scoreUIGO;//score UI
	[SerializeField] int _scoreDigit;//number of digits in score display
	[SerializeField] int _initScore = 0;//initial score
	int _score;//current score
	[SerializeField] GameObject _inGame;//game screen UI
	[SerializeField] GameObject _endScreen;//end screen UI
	[SerializeField] List<GameObject> _playerUI;//List of button
	[SerializeField] PlayerUI _playerUIPrefab;//UI button prefab
	[SerializeField] float _playerUIOffset = 30f;//distance between UI elements
	[SerializeField] Text _screenText;//end screen text
	[SerializeField] Text _food;//text UI for food
	[SerializeField] Text _key;//Test UI for key
	[SerializeField] Text _life;//Text UI for life
	[SerializeField] Text _hunger;//Text UI for life
	[SerializeField] string _loseText = "Vous avez perdu..";//gameover text
	[SerializeField] string _winText = "Vous avez gagné !";//game win text

	//
	//	GET SET
	//

	public int Score{
		get{
			return _score;
		}
		set{
			_score = value;
		}
	}

	//Display life UI
	public int Life{
		set{
			_life.text = "PDV " + value + "%";
		}
	}

	//display hunger ui
	public int Hunger{
		set{
			_hunger.text = "Faim " + value + "%";
		}
	}

	//display nbfood UI inventory
	public int Food{
		set{
			_food.text = "Nourriture " + value;
		}
	}

	//display nb of keys
	public int Key{
		set{
			_key.text = "Clef " + value;
		}
	}

	//INIT
	void Awake () {
		if(instance != null){
			Destroy(this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(this);
		}
	}
	private void Start() {
		GameManager.instance.addScoreEvent += OnAddScore;
		GameManager.instance.resetGameEvent += OnReset;
		Initialise();
	}

	//Initialise everything on startup/reset
	void Initialise(){
		_score = _initScore;
		DisplayScore(_score);
	}

	//
	//	EVENTS
	//

	//Reinitialise Manager
	void OnReset(){
		Initialise();
	}

	//add score event
	void OnAddScore(int points){
		_score += points;
		DisplayScore(_score);
	}

	//
	//	FUNCTIONS
	//

	//change displays for different scenes
	public void Activate(bool isActive){
		_inGame.SetActive(isActive);
		_endScreen.SetActive(!isActive);
		_screenText.text = (GameManager.instance.GameState == State.Lose) ? _loseText : (GameManager.instance.GameState == State.Win) ? _winText : "";//Lose / Win / Null
	}

	//Displays the score in ScoreUI ex:"0001933"
	public void DisplayScore(int score){
		string result = "";
		int zeros = _scoreDigit - score.ToString().Length;
		for(int i = 0; i < zeros; i++){
			result += "0";
		}
		_scoreUI.text = result + score;
		_scoreUIGO.text = result + score;
	}

	//instanciate PlayerUI buttons
	public void SetPlayerUI(){
		_playerUI = new List<GameObject>();
		Player[] players = PlayerManager.instance.Players;
		int i = 0;
		foreach(Player player in players){
			Vector2 prefabSize = _playerUIPrefab.GetComponent<RectTransform>().sizeDelta;
			Vector2 uiPos = new Vector2(prefabSize.x, (Camera.main.scaledPixelHeight) - (prefabSize.y + (i * prefabSize.y) + (i * _playerUIOffset)));//Hard Code
			PlayerUI playerUI = (PlayerUI)Instantiate(_playerUIPrefab, uiPos, Quaternion.identity);
			playerUI.transform.SetParent(_inGame.transform);
			playerUI.Player = player;
			playerUI.Name = player.Name;
			playerUI.Scene = player.Scene;
			playerUI.GetComponent<Image>().sprite = player.UIImage;
			_playerUI.Add(playerUI.gameObject);
			i++;
		}
	}
}
