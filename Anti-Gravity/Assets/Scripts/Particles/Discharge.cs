using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discharge : MonoBehaviour
{   
    public static Discharge instance;
    void Awake(){
        if(instance != null){
            Destroy(instance.gameObject);
        }
        instance = this;
    }
    void Start(){
        Destroy(this.gameObject, 0.6f);
    }
}
