using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    bool _active = false;
    bool _checkpoint = false;
    string _animDoor = "OpenDoor";
    Animator _anim;
    AudioSource _sound;

	// Use this for initialization
	void Start () {
        _anim = GetComponent<Animator>();
        _sound = GetComponent<AudioSource>();
	}

    //activates door and animation
    public void Activate(bool isActive)
    {
        if (!_checkpoint)
        {
            _active = isActive;
            Open(isActive);
        }
    }

    public void Checkpoint()
    {
        _active = true;
        _checkpoint = true;
    }

    //Open door
    private void Open (bool isOpen)
    {
        _anim.SetBool(_animDoor, isOpen);
        _sound.Play();
    }
}
