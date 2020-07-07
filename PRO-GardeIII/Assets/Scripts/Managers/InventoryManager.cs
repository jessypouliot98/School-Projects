using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

	public static InventoryManager instance;// instance
	[SerializeField] List<string> _inventory; 
	[SerializeField] int _foodValue = 50;
	string _key = "Key";

	//INIT
	private void Awake() {
		if(instance != null){
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this);
	}

	private void Start() {
		GameManager.instance.resetGameEvent += OnReset;
		Initialise();
	}

	//Initialise everything on startup/reset
	void Initialise(){
		_inventory = new List<string>();
	}

	//EVENTS
	void OnReset(){
		Initialise();
	}

	//FUNCTIONS

	//adds something in inventory
	public void AddToInv(string obj){
		_inventory.Add(obj);
		DisplayItem();
	}

	//Checks for food, eats it and destroy eaten object. Returns food value
	public int EatFood(){
		int feed = 0;
		foreach(string item in _inventory){
			if(item != _key){
				feed = _foodValue;
				RemoveFromInv(item);
				break;
			}
		}
		DisplayItem();
		return feed;
	}

	//remove something form inventory
	public void RemoveFromInv(string obj){
		for(int i=0; i<_inventory.Count; i++){
			if(obj == _inventory[i]){
				_inventory.Remove(_inventory[i]);
				break;
			}
		}
		DisplayItem();
	}

	//calculate nb of food item left
	void DisplayItem(){
		int nbFood = 0;
		int nbKey = 0;
		foreach(string item in _inventory){
			if(item != _key){
				nbFood++;
			} else {
				nbKey++;
			}
		}
		CanvasManager.instance.Key = nbKey;
		CanvasManager.instance.Food = nbFood;
		GameManager.instance.Keys = nbKey;
	}

}
