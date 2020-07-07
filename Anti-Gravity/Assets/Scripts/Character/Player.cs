using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] GameObject _deathParticles;
    [SerializeField] AudioClip _deathClip;
    [SerializeField] float _deathVolume;

    Vector3 SoundPosition{
        get{
            return new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z - 2);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch(other.gameObject.tag){
            case "Enemy":
            case "Saw":
            case "Stomper":
                this.gameObject.SendMessage("Hit");
                break;
        }
    }
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Intel"){
            other.gameObject.GetComponent<Intelligence>().Take();
        }
        else if(other.gameObject.tag == "Finish"){
            WorldManager.instance.Win();
        }
    }
    void Kill(){
        Debug.Log("KILL");
        _isAlive = false;
        GameManager.instance.Dead = true;
        StartCoroutine(Dying());
    }

    public void VelocityStop(){
        foreach(Transform child in this.gameObject.GetComponentsInChildren<Transform>()){
            if(child.gameObject.GetComponent<Rigidbody>()){
                child.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    IEnumerator Dying(){
        //DEATH SOUND
        AudioSource.PlayClipAtPoint(_deathClip, SoundPosition, _deathVolume);
        //PARTICLES
        GameObject particles = Instantiate(_deathParticles, this.transform.position, Quaternion.identity);
        particles.transform.SetParent(this.transform);
        //CHANGE SCENE
        yield return new WaitForSeconds(1f);
        WorldManager.instance.GameOver();
    }
}
