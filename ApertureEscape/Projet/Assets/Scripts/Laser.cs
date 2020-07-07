using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour {

    bool _active = false;//laser on/off
    [SerializeField] LayerMask _laserMask;//LayerMask visible to raycast
    LineRenderer _laserRender;//line renderer instance
    Collider2D _lastTarget = null;//last target touched by laser
    Vector2 _initPos;//starting position of laser
    Vector2 _direction;//direction of laser
    Color _laserColor = Color.green;
    float _laserWidth = 0.1f;

    // Use this for initialization
    void Start () {
        //Laser Render
        _laserRender = this.gameObject.GetComponent<LineRenderer>();
        _laserRender.enabled = _active;
        
        _laserRender.SetWidth(_laserWidth, _laserWidth);
        _laserRender.numCapVertices = 1;
        _laserRender.material = new Material(Shader.Find("Particles/Additive"));
        _laserRender.SetColors(_laserColor, _laserColor);
    }

    //Activates laser et renderer if active
    public void Activate (bool isActive) {
        _active = isActive;
        this.gameObject.GetComponent<LineRenderer>().enabled = isActive;
        if (_laserRender) { _laserRender.enabled = isActive; }
    }

    public void SetLaserColor (Color color)
    {
        _laserRender.SetColors(color, color);
    }

    //Sets laser in the desired direction and position
    public void SetLaser (Vector2 castPos, Vector2 targetDirection) {
        _initPos = castPos;
        _direction = targetDirection.normalized;
    }

    //Deactivate's target
    void DeactivateTarget ()
    {
        if (_lastTarget.tag == "Mirror")
        {
            Mirror mirror = _lastTarget.gameObject.GetComponent<Mirror>();
            mirror.Activate(false);
        }
        else if (_lastTarget.tag == "LaserIn")
        {
            LaserIn input = _lastTarget.gameObject.GetComponent<LaserIn>();
            input.Activate(false);
        }
        else if (_lastTarget.tag == "Portal")
        {
            Portal portal = _lastTarget.gameObject.GetComponent<Portal>();
            portal.Activate(false);
        }
    }

    //Activate target
    void ActivateTarget(RaycastHit2D laserHit)
    {
        if (laserHit.collider != null)//check if raycast hits a target
        {
            //if laser isnt from a monster
            if(this.gameObject.tag != "Monster"){
                if (laserHit.collider.tag == "Mirror")//if target is a mirror, activate it
                {
                    Mirror mirror = _lastTarget.gameObject.GetComponent<Mirror>();
                    mirror.Activate(true);
                    mirror.LaserHit(_initPos, laserHit.point);
                }
                else if (laserHit.collider.tag == "LaserIn")//if target is a LaserIn, activate it
                {
                    LaserIn input = _lastTarget.gameObject.GetComponent<LaserIn>();
                    input.Activate(true);
                }
                else if (_lastTarget.tag == "Portal")
                {
                    Portal portal = _lastTarget.gameObject.GetComponent<Portal>();
                    portal.Activate(true);
                    portal.LaserHit(_initPos, laserHit.point);
                }
                else if (laserHit.collider.tag == "Monster")//if target is a Monster, kill it
                {
                    Monster monster = _lastTarget.gameObject.GetComponent<Monster>();
                    monster.Kill();
                }
            }
            //If laser is from monster
            else {
                if (laserHit.collider.tag == "Player")//if target is a Player, kill it
                {
                    Player player = _lastTarget.gameObject.GetComponent<Player>();
                    player.Kill();
                }
            }
        }
    }

    //Disable last target when
    void ChangeTarget (RaycastHit2D laserHit) {
        if(_lastTarget != null && _lastTarget != laserHit.collider) {
            DeactivateTarget();
        }
        _lastTarget = laserHit.collider;
    }

    //Deactivate last target when this's stops and remove this's lastTarget memory
    void Stop()
    {
        if (_lastTarget != null)
        {
            DeactivateTarget();
        }
        _lastTarget = null;
    }

    void CastLaser ()
    {
        //Creates Laser
        RaycastHit2D laserHit = Physics2D.Raycast((_initPos + _direction / 2), _direction, Mathf.Infinity, _laserMask);
        _laserRender.SetPosition(0, _initPos);
        _laserRender.SetPosition(1, laserHit.point);
        //Check target difference
        ChangeTarget(laserHit);
        ActivateTarget(laserHit);
    }

    void Update () {
        //cast a laser while active
        if (_active) {
            CastLaser();
        }
        //Stops everything triggered by this
        else
        {
            Stop();
        }
    }
}