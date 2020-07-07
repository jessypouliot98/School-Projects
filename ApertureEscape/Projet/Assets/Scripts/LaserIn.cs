using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserIn : MonoBehaviour {

    bool _active = false;
    [SerializeField] Door[] _doors;
    //Activates the door switch if it got hit by a laser
    public void Activate(bool isActive) {
        _active = isActive;
        foreach(Door door in _doors){
            door.Activate(isActive);
        }
    }
}
