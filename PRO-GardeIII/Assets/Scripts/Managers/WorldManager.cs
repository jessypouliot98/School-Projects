using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour {
	public static WorldManager instance;

	[SerializeField] AudioClip _music;

	//
	//	INIT
	//
	private void Awake() {
		instance = this;
	}
	
	void Start(){
		MusicManager.instance.SwitchTrack(_music);
	}
	//
	// GET SET
	//
	
	public AudioClip Music{
		get{
			return _music;
		}
	}
}
