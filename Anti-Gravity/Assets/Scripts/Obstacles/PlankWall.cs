using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankWall : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;
    [SerializeField] int _score = 50;
    [SerializeField] float _burnVolume = 1;
    [SerializeField] AudioClip _burnClip;
    float _angle = 0;
    public float SetAngle{
        set{
            _angle = value + 90;
            this.transform.rotation = Quaternion.Euler(0f, 0f, value + 90);
        }
    }

    Vector3 SoundPosition{
        get{
            return new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z - 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Particles"){
            StartCoroutine(BurnWall());
        }
    }

    IEnumerator BurnWall(){
        Debug.Log("Burning it down");
        AudioSource.PlayClipAtPoint(_burnClip, SoundPosition, _burnVolume);
        Instantiate(_particles, this.transform.position, this.transform.rotation);
        bool isBurning = true;
        float totalBurnTime = _particles.main.duration;
        float burnTime = totalBurnTime;
        while(isBurning){
            burnTime -= Time.deltaTime;
            if(burnTime <= 0){
                isBurning = false;
            }
            yield return null;
        }
        WorldManager.instance.Score += _score;
        GameManager.instance.AchievedStep("Burned");
        Destroy(this.gameObject);
    }
}
