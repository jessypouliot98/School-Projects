using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    bool _active;//is this button active ? yes/no
    [SerializeField] Door[] _doors;

    public void Activate(bool isActive)
    {
        _active = isActive;
        if (isActive)
        {
            foreach (Door door in _doors)
            {
                door.Activate(isActive);//Activates all doors linked to this button
            }
        }
    }
}
