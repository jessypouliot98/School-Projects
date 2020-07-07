using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Prop : MonoBehaviour {

    Rigidbody2D _rBody;

	// Use this for initialization
	void Start () {
        _rBody = this.gameObject.GetComponent<Rigidbody2D>();
        _rBody.gravityScale = 0;
        _rBody.mass = 0.1f;
    }

    //Sets state of box when grabbed or not
    public void Grab (bool grabbing)
    {
        _rBody.freezeRotation = grabbing;
        _rBody.bodyType = grabbing ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        _rBody.velocity = Vector2.zero;
    }
}
