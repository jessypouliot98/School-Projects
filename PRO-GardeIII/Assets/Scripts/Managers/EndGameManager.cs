using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour {
	public static EndGameManager instance;
	int _score = 0;
	[SerializeField] Text _state;
	[SerializeField] Text _scoreUI;

	public int Score{
		set{
			_score = value;
			_scoreUI.text = _score.ToString();
		}
		get{
			return _score;
		}
	}

	private void Awake() {
		instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
