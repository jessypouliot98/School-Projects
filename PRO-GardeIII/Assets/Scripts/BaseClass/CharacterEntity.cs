using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterEntity : MonoBehaviour {

    protected Rigidbody2D _rBody;
    protected Collider2D _collider;
    protected AudioSource _audioSrc;
    protected Animator _anim;
    [SerializeField] protected int _score = 100;
    [SerializeField] protected float _dmg = 10f;
    [SerializeField] protected float _maxLife = 10f;
    [SerializeField] protected float _currentLife;
    [SerializeField] AudioClip _hitSound;

    //INIT
    protected void Start() {
        _currentLife = _maxLife;
        _rBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _audioSrc = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    //GET SET
    public Vector2 Velocity{
        get{
            return _rBody.velocity;
        }
    }
    public float Hit{
        set{
            _currentLife -= value;
            MusicManager.instance.PlaySound(_hitSound, this.transform.position);
            SendMessage("Damaged");
        }
    }
    public float X{
        get{
            return this.transform.position.x;
        }
        set{
            this.transform.position = new Vector3(value, Y, Z);
        }
    }
    public float Y{
        get{
            return this.transform.position.y;
        }
        set{
            this.transform.position = new Vector3(X, value, Z);
        }
    }
    public float Z{
        get{
            return this.transform.position.z;
        }
        set{
            this.transform.position = new Vector3(X, Y, value);
        }
    }
    public float MoveX{
        get{
            return _rBody.velocity.x;
        }
        set{
            _rBody.velocity = new Vector2(value, MoveY);
        }
    }
    public float MoveY{
        get{
            return _rBody.velocity.y;
        }
        set{
            _rBody.velocity = new Vector2(MoveX, value);
        }
    }
}
