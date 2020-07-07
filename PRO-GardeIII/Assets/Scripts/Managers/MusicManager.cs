using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	public static MusicManager instance;
	AudioSource _jukebox;
	[SerializeField] AudioClip _music;

	//INIT
	void Awake () {
		if(instance != null){
			Destroy(this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(this);
		}
		_jukebox = GetComponent<AudioSource>();
	}
	private void Start() {
		GameManager.instance.resetGameEvent += OnReset;
		Initialise();
	}

	//Initialise everything on startup/reset
	void Initialise(){
		_jukebox.clip = WorldManager.instance.Music;
		_jukebox.Play();
	}

	//FUNCTIONS
	public void SwitchTrack(AudioClip music){
		if(music != _jukebox.clip){
			_jukebox.clip = music;
			_jukebox.Play();
		}
	}

	public void PlaySound(AudioClip sound, Vector2 position){
		 AudioSource.PlayClipAtPoint(sound, position);
	}

	//EVENTS
	void OnReset(){
		Initialise();
	}
}
