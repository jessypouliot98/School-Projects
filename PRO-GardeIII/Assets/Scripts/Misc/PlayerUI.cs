using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {
	Player _player;
	bool _enabled = true;
	[SerializeField] Text _name;
	[SerializeField] Text _scene;
	Image _image;
	Color _dColor = new Color(50f, 50f, 50f, .3f); // death color

	//GET SET
	public Player Player{
		set{
			_player = value;
			GetComponent<Image>().sprite = _player.GetComponent<SpriteRenderer>().sprite;
		}
	}

	public string Name{
		set{
			_name.text = value;
		}
	}
	public string Scene{
		set{
			_scene.text = value;
		}
	}

	//INIT
	void Start() {
		_image = this.GetComponent<Image>();
		PlayerManager.instance.playerDeathEvent += OnPlayerDeath;
		_image.sprite = _player.GetComponent<SpriteRenderer>().sprite;
	}

	//EVENTS
	void OnPlayerDeath(string name){
		if(name == _player.Name){
			Disable();
		}
	}

	//FUNCTIONS
	public void SelectPlayer(){
		if(_enabled){
			_player.SelectChar();
			if(PlayerManager.instance.GetPlayerScene(_player) != GameManager.instance.Scene){
				GameManager.instance.SelectScene(PlayerManager.instance.GetPlayerScene(_player));
			}
			GameObject.Find("EventSystem").GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);//remove focus from button
		} else {
			Debug.Log(_player.Name + " is Dead :(");
		}
	}

	void Disable(){
		_enabled = false;
		_image.color = _dColor;
	}
}
