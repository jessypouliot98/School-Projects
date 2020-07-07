using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intelligence : MonoBehaviour
{
    [SerializeField] ParticleSystem _particles;
    [SerializeField] int _IntelPoints = 1;
    [SerializeField] float _scoreVolume = 1;
    [SerializeField] AudioClip _scoreClip;
    float rotationX = 0;
    float rotationY = 0;
    float rotationZ = 0;

    public int Intel {
        get{
            return _IntelPoints;
        }
    }

    Vector3 SoundPosition{
        get{
            return new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z - 2);
        }
    }

    public void Take(){
        StartCoroutine(Vanish());
    }

    IEnumerator Vanish(){
        AudioSource.PlayClipAtPoint(_scoreClip, SoundPosition, _scoreVolume);
        WorldManager.instance.Intel += Intel;
        ParticleSystem particle = Instantiate(_particles, this.transform.position, this.transform.rotation);
        particle.transform.SetParent(null);
        this.GetComponent<Collider>().enabled = false;
        bool isVanishing = true;
        float vanishTimeTotal = _particles.main.duration;;
        float vanishTime = vanishTimeTotal;
        while(isVanishing){
            vanishTime -= Time.deltaTime;
            if(vanishTime <= 0){
                isVanishing = false;
            }
            yield return null;
        }
        Destroy(this.gameObject);
    }

    void Update(){
        rotationX += 70 * Time.deltaTime;
        rotationY += 40 * Time.deltaTime;
        rotationZ -= 90 * Time.deltaTime;
        this.transform.rotation = Quaternion.Euler(
            rotationX,
            rotationY,
            rotationZ
        );
    }
}
