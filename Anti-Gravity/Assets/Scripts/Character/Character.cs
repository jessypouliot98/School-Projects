using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Rigidbody _rbody;
    protected Collider _col;
    protected bool _isAlive = true;


    /*
        INIT
    */

    protected void Awake()
    {
        _rbody = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();
    }

    /*
        GET/SET
    */

    public bool Alive{
        get{
            return _isAlive;
        }
    }

    public Vector2 velocity{
        get{
            return _rbody.velocity;
        }
        set{
            _rbody.velocity = value;
        }
    }

    public Vector2 push{
        set{
            _rbody.AddForce(value);
        }
    }

    /*
        Functions
    */

    public void Hit(){
        this.SendMessage("Kill");
    }

}
