using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollower : MonoBehaviour
{
    float _camDistance;
    [SerializeField] float _xOffset = 1;
    [SerializeField] float _yOffset = 1;
    [SerializeField] float _speed = 0.2f;
    Transform _target;
    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        _camDistance = this.transform.position.z;
        StartCoroutine(FindTarget());
    }

    IEnumerator FindTarget(){
        bool foundtarget = false;
        while(!foundtarget){
            if(GameObject.FindWithTag("Player")){
                foundtarget = true;
                _target = GameObject.FindWithTag("Player").transform;
                Vector3 goTo = new Vector3(_target.position.x + _xOffset, _target.position.y + _yOffset, _camDistance);
                this.transform.position = goTo;
            }
            yield return null;
        }
    }

    void LateUpdate(){
        if(_target){
            Vector3 goTo = new Vector3(_target.position.x + _xOffset, _target.position.y + _yOffset, _camDistance);
            this.transform.position = Vector3.SmoothDamp(transform.position, goTo, ref velocity, _speed);
        }
    }
}
