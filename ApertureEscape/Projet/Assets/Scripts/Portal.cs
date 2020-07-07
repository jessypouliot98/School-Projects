using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Laser))]
public class Portal : MonoBehaviour {

    public static Portal _instanceA;
    public static Portal _instanceB;

    bool _activeIn = false;
    bool _activeOut = false;
    BoxCollider2D _collider;
    Laser _laser;
    Portal _otherPortal;
    Vector2 _direction;
    [SerializeField] Vector2 _laserInitPos;
    [SerializeField] Vector2 _laserDir;
    float _rotation;

    // Use this for initialization
    void Start () {
        _laser = this.gameObject.GetComponent<Laser>();
        _collider = this.gameObject.GetComponent<BoxCollider2D>();
        PortalInit();
    }

    //Initialise Portal
    void PortalInit ()
    {
        //PortalA init
        if (this.gameObject.name == "PortalA(Clone)" || this.gameObject.name == "PortalA")
        {
            //Instanciate this
            if (_instanceA == null)
            {
                _instanceA = this;
            }
            //Instanciate this and destroy previous instance
            else if (_instanceA.gameObject.name == this.gameObject.name)
            {
                Destroy(_instanceA.gameObject);
                _instanceA = this;
            }
        }
        //PortalB init
        else if (this.gameObject.name == "PortalB(Clone)" || this.gameObject.name == "PortalB")
        {
            //Instanciate this
            if (_instanceB == null)
            {
                _instanceB = this;
            }
            //Instanciate this and destroy previous instance
            else if (_instanceB.gameObject.name == this.gameObject.name)
            {
                Destroy(_instanceB.gameObject);
                _instanceB = this;
            }
        }
        ResetPortals();
        LinkPortals();
    }

    public void ResetPortals ()
    {
        GameObject[] mirrors = GameObject.FindGameObjectsWithTag("Mirror");
        foreach (GameObject mirror in mirrors)
        {
            mirror.GetComponent<Mirror>().Activate(false);
        }
    }

    //Links this to other portal
    void LinkPortals ()
    {
        if (this != _instanceA) { _otherPortal = _instanceA; }
        else if (this != _instanceB) { _otherPortal = _instanceB; }

        if (_otherPortal) { _otherPortal.SetPortalLink(this); }
    }

    //Set link between other portal and this
    public void SetPortalLink (Portal instance)
    {
        _otherPortal = instance;
    }

    //Activate this
    public void Activate(bool isActive)
    {
        _activeIn = isActive;
    }

    //Get _activeIn
    public bool GetActiveIn
    {
        get
        {
            return _activeIn;
        }
    }

    //Activate other
    void ActivateOut(bool isActive)
    {
        _activeOut = isActive;
        _laser.Activate(isActive);
    }

    //Finds direction of portal
    void SetDirection()
    {
        _rotation = this.transform.eulerAngles.z * Mathf.Deg2Rad;
        _direction = new Vector2( Mathf.Cos(_rotation), Mathf.Sin(_rotation)).normalized;
    }

    //Hitpoint of laser
    public void LaserHit(Vector2 fromDir, Vector2 hitPoint)
    {
        //while linked, send laser data to other portal
        if (_otherPortal)
        {
            SetDirection();
            //calculate direction and hitpoint difference
            Vector2 thisPosition = this.gameObject.transform.position;
            _otherPortal.OtherLaserHit(fromDir, hitPoint, thisPosition, _rotation);
        }
    }

    //Hitpoint of laser on other
    public void OtherLaserHit(Vector2 fromDir, Vector2 mainHitPoint, Vector2 mainPos, float mainAngle)
    {
        SetDirection();
        Vector2 thisPos = this.transform.position;
        Vector2 hitDiff = mainPos - mainHitPoint;//Distance difference between center and hitpoint

        //Pythagora
        float a = hitDiff.x;//side1
        float b = hitDiff.y;//side2
        float h = Mathf.Sqrt( Mathf.Pow(a, 2) + Mathf.Pow(b, 2) );//hypothenus

        //Laser Direction
        //Vector2 laserInitPosDir = thisPos + new Vector2( Mathf.Cos(-_rotation) * hitDiff.x, Mathf.Sin(-_rotation) * hitDiff.y);
        //Laser angle portal in
        float laserAngleIn = Mathf.Atan2(mainHitPoint.y - fromDir.y, mainHitPoint.x - fromDir.x) * Mathf.Rad2Deg;
        //Laser angle portal out
        float laserAngle = ( laserAngleIn % 90 );
        float laserAngleOut = _rotation - (laserAngle * Mathf.Deg2Rad);
        //Laser Starting position
        _laserInitPos = thisPos + _direction * h;
        //Laser direction
        _laserDir = new Vector2(Mathf.Cos(laserAngleOut), Mathf.Sin(laserAngleOut));
    }

    void Update () {
        //do while linked
        if (_otherPortal)
        {
            //Activate if link is active
            ActivateOut(_otherPortal.GetActiveIn);
            //Cast laser
            if (_activeOut) { _laser.SetLaser(_laserInitPos, _laserDir); }
        }
    }
}
