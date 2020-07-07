using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    [SerializeField] Door[] _doors;
    [SerializeField] bool _endLevel;

    //Detect player from reaching checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            KeepDoorsOpen();
            DestroyPortals();
            if (_endLevel)
            {
                GameObject.Find("MainManager").GetComponent<MainManager>().StillAlive();
            }
        }
    }

    //Prevent doors from last room from closing
    void KeepDoorsOpen ()
    {
        foreach(Door door in _doors)
        {
            door.Checkpoint();
        }
    }

    //Remove portal from last room on checkpoint enter
    void DestroyPortals()
    {
        foreach (GameObject portal in GameObject.FindGameObjectsWithTag("Portal"))
        {
            portal.GetComponent<Portal>().ResetPortals();
            Destroy(portal);
        }
    }
}
