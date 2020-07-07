using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Laser))]
public class Mirror : MonoBehaviour {

    bool _active = false;
    Laser _laserReflect;
    Vector2 _direction;
    float _rotation;
    Vector2 _reflectPoint;
    Vector2 _reflectDir;
    GameObject _lastTarget;

    // Use this for initialization
    void Start () {
        _laserReflect = this.gameObject.GetComponent<Laser>();
        _laserReflect.Activate(false);
    }

    //Finds direction in which the mirror faces
    void SetDirection () {
        _rotation = this.transform.eulerAngles.z;
        float aRad = (((_rotation + 90) % 180) - 45) * Mathf.Deg2Rad;
        _direction = new Vector2(Mathf.Cos(aRad % Mathf.PI), Mathf.Sin(aRad % Mathf.PI));
    }

    //Activates the mirror and reflection
    public void Activate (bool isActive) {
        _active = isActive;
        _laserReflect.Activate(isActive);
    }

    //Finds angle of reflection and returns the reflected Vector
    private Vector2 MirrorAngle (Vector2 fromDir, Vector2 hitPoint) {
        Vector2 vIncidence = (hitPoint - fromDir).normalized;
        Vector2 vNormal = _direction.normalized;
        Vector2 vReflected = Vector2.Reflect(vIncidence, vNormal);
        return vReflected;
    }

    //Sets the direction and hit point of where the laser hit {this}
    public void LaserHit (Vector2 fromDir, Vector2 hitPoint) {
        SetDirection();
        _reflectPoint = hitPoint;
        _reflectDir = MirrorAngle(fromDir, hitPoint);
    }

    void Update () {
        //if mirror is active cast laser in relfected direction
        if (_active) {
            _laserReflect.SetLaser(_reflectPoint, _reflectDir);
        }
    }
}
