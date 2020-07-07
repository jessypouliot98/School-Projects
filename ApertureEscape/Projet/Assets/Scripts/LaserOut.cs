using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Laser))]
public class LaserOut : MonoBehaviour {

    Laser _laserCast;
    Vector2 _direction;
    float _rotation;

    // Use this for initialization
    void Start () {
        _laserCast = this.gameObject.GetComponent<Laser>();
        _laserCast.Activate(true);
        SetDirection();
    }
    //Set laser direction in laser output direction
    void SetDirection () {
        _rotation = this.transform.eulerAngles.z;
        float aRad = _rotation * Mathf.Deg2Rad;
        _direction = new Vector2(Mathf.Cos(aRad), Mathf.Sin(aRad));
    }

    // Update is called once per frame
    void Update () {
        SetDirection(); 
        _laserCast.SetLaser(this.transform.position, _direction.normalized);//Cast laser
    }
}
