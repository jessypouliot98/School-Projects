using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    GameObject _other;//other collider instance
    FixedJoint2D _joint;//fixedjoint instance
    bool _isGrabbing = false;//grab on/off
    Player _player;//reference to player script

	// Use this for initialization
	void Start () {
        _joint = this.gameObject.GetComponent<FixedJoint2D>();
        _player = GetComponent<Player>();
	}

    //Attach collider to {this}
    void Interact(bool interacting) {
        if (_other != null)
        {
            if (_other.gameObject.tag == "Button")
            {
                if (_player.HasKey)
                {
                    _other.GetComponent<Button>().Activate(interacting);
                    _player.RemoveKey();
                }
            }
            else if (_other.gameObject.tag == "Props")
            {
                _isGrabbing = interacting;
                _joint.enabled = interacting;
                _joint.connectedBody = interacting ? _other.GetComponent<Rigidbody2D>() : null;
                _other.GetComponent<Prop>().Grab(interacting);
            }
        }
    }

    //Adds collider to global variable
    private void OnCollisionEnter2D (Collision2D collision) {
        if(collision.gameObject.tag == "Props" || collision.gameObject.tag == "Button") {
            _other = collision.gameObject;
        }
    }

    //Removes last collider from global variable
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject == _other && !_isGrabbing) {
            _other = null;
        }
    }

    void Update() {
        if (Input.GetButtonDown("Interact")) { Interact(true); }//Interact on
        else if (Input.GetButtonUp("Interact")) { Interact(false); }//Interact off
    }
}
