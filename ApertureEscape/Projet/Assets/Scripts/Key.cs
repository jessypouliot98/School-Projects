using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    AudioSource _sound;

    private void Start()
    {
        _sound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player"){
			other.gameObject.GetComponent<Player>().AddKey();
			Destroy(this.gameObject);
		}
	}
}
