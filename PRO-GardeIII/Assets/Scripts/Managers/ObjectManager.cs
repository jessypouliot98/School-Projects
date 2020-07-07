using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour {
	public static ObjectManager instance;
	List<string> _missingObjects;

	//GET SET


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
		SceneManager.sceneLoaded += OnSceneLoaded;
		GameManager.instance.resetGameEvent += OnReset;
		Initialise();
	}

	//Initialise everything on startup/reset
	void Initialise(){
		_missingObjects = new List<string>();
	}

	//FUNCTIONS
	public void RemoveObj(GameObject obj){
		_missingObjects.Add(obj.name);
	}

	//EVENTS
	void OnReset(){
		Initialise();
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode){
		List<GameObject> entities = new List<GameObject>();

		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Box")){
			entities.Add(obj);
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item")){
			entities.Add(obj);
		}
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Monster")){
			entities.Add(obj);
		}
		
		foreach (GameObject obj in entities){
			foreach (string name in _missingObjects){
				if(obj.name == name){
					Destroy(obj);
				}
			}
		}
	}
}
