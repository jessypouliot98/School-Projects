using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour {

	[SerializeField] GameObject[] _items;
	[Range(0,100)][SerializeField] int _chanceToDropTwo = 30;
	[SerializeField] int _score = 25;
	[SerializeField] AudioClip _breakSound;

	public void Smash(){
		int chance = Random.Range(0,100);
		if(chance <= 100-_chanceToDropTwo){
			Instantiate(_items[Random.Range(0,_items.Length-1)], this.transform.position, Quaternion.identity);
		} else {
			Instantiate(_items[Random.Range(0,_items.Length-1)], new Vector2(this.transform.position.x - 0.5f, this.transform.position.y), Quaternion.identity);
			Instantiate(_items[Random.Range(0,_items.Length-1)], new Vector2(this.transform.position.x + 0.5f, this.transform.position.y), Quaternion.identity);
		}
		GameManager.instance.AddScore(_score);
		ObjectManager.instance.RemoveObj(this.gameObject);
		MusicManager.instance.PlaySound(_breakSound, this.transform.position);
		Destroy(this.gameObject);
	}
}
