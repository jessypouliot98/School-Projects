using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CamBehavior : MonoBehaviour {

	Transform _target = null;
	Vector2 _refVelocity = Vector2.zero;
	[SerializeField] float _followSpeed = 2f;
	[SerializeField] Tilemap _map;

	void Start () {
		GameManager.instance.selectedCharEvent += OnSelectedChar;
	}

	// EVENTS
	// switch target to follow
    void OnSelectedChar(string name){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject character in players){
			Player player = character.GetComponent<Player>();
			if(player.Name == name){
				if(_target == null && this){
					this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
				}
				_target = player.transform;
				break;
			}
		}
    }
	// UPDATES
	void LateUpdate() {
		if(_target){
			//map borders
			float borderBottom = 0;
			float borderTop = 0;
			float borderLeft = 0;
			float borderRight = 0;
			foreach(GameObject gizmo in GameObject.FindGameObjectsWithTag("MapBound")){
				if(borderBottom == 0 && borderTop == 0 && borderLeft == 0 && borderRight == 0){//1st cycle;
					borderBottom = gizmo.transform.position.y;
					borderTop = gizmo.transform.position.y;
					borderLeft = gizmo.transform.position.x;
					borderRight = gizmo.transform.position.x;
				}
				//replace for correct value of map border
				if(gizmo.transform.position.y > borderTop){ borderTop = gizmo.transform.position.y; }
				if(gizmo.transform.position.y < borderBottom){ borderBottom = gizmo.transform.position.y; }
				if(gizmo.transform.position.x < borderLeft){ borderLeft = gizmo.transform.position.x; }
				if(gizmo.transform.position.y > borderRight){ borderRight = gizmo.transform.position.x; }
			}

			borderBottom += Camera.main.orthographicSize;
			borderTop -= Camera.main.orthographicSize;
			borderLeft += Camera.main.orthographicSize * Camera.main.aspect;
			borderRight -= Camera.main.orthographicSize * Camera.main.aspect;
			//Debug.Log("top: " + borderTop + " right: " + borderRight + " bottom: " + borderBottom + " left: " + borderLeft);

			Vector2 movePos = Vector2.SmoothDamp(this.transform.position , _target.position, ref _refVelocity, _followSpeed / 10);
			this.transform.position = new Vector3(Mathf.Clamp(movePos.x, borderLeft, borderRight), Mathf.Clamp(movePos.y, borderBottom, borderTop), this.transform.position.z);
		}
	}
}
