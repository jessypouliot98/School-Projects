using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportCam : MonoBehaviour {

    [SerializeField] GameObject _target;

	// Use this for initialization
	void Start () {
        FollowTarget();
    }

    void FollowTarget ()
    {
        this.transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, -0.5f);
    }
	
	void LateUpdate () 
    {
        FollowTarget();
    }
}
