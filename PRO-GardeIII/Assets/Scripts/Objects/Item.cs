using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    [SerializeField] string _itemName = "Item Name";
    [SerializeField] Item _itemObj;
    [SerializeField] int _score = 10;
    [SerializeField] AudioClip _lootSound;

    public string itemName{
        get{
            return _itemName;
        }
    }

    //Collect and Destroy
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            GameManager.instance.AddScore(_score);
            InventoryManager.instance.AddToInv(_itemName);
            ObjectManager.instance.RemoveObj(this.gameObject);
            MusicManager.instance.PlaySound(_lootSound, this.transform.position);
            Destroy(this.gameObject);
        }
    }
}
