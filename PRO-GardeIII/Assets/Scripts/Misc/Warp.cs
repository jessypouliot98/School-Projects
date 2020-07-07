using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

	[SerializeField] string _warpZone = "Headquarter";

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Player"){
			Player player = other.gameObject.GetComponent<Player>();
			player.Scene = _warpZone;
			player.transform.position = Vector2.zero; // prog3
			//player.transform.position = Vector2.zero;
			GameManager.instance.SelectScene(_warpZone);
		}
	}
}
