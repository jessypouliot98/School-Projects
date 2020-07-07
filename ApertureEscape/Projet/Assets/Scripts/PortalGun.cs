using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour {

	public static float _portalAngle;
	[SerializeField]
	LayerMask _laserMask;//LayerMask visible to raycast
	[SerializeField]
	GameObject _portalA;//prefab portal A
	[SerializeField]
	GameObject _portalB;//prefab portal B
	Player _player;
	[SerializeField] AudioClip _clip;

	private void Start()
	{
		_player = GetComponent<Player>();
	}

	//Returns rotation for portal to face opposite of the wall it hit
	Quaternion GetPortalOrientation (RaycastHit2D target)
	{
		Vector2 targetPos = target.collider.gameObject.transform.position;
		Vector2 hitPos = target.point;
		Vector2 hitDiff =  targetPos - hitPos;
		if( Mathf.Abs(hitDiff.x) >= Mathf.Abs(hitDiff.y) ) { hitDiff = new Vector2(hitDiff.x, 0f); }
		else { hitDiff = new Vector2(0f, hitDiff.y); }
		float angleDeg = Mathf.Atan2(hitDiff.y, hitDiff.x) * Mathf.Rad2Deg;
		angleDeg = (angleDeg < 0) ? angleDeg + 360 : angleDeg;//angle from 0 to 360
		Quaternion rotation =  Quaternion.Euler(new Vector3(0, 0, angleDeg - 180) );
		return rotation;
	}

	//Instantiate Portal A/B
	void CreatePortal(bool portalSelect)
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 tPos = this.transform.position;
		Vector2 direction = mousePos - tPos;
		RaycastHit2D rayShoot = Physics2D.Raycast(tPos, direction.normalized, Mathf.Infinity, _laserMask);
		if(rayShoot.collider.gameObject.tag == "Wall") {
			GameObject other = rayShoot.collider.gameObject;
			Instantiate(portalSelect ? _portalA : _portalB, other.transform.position, GetPortalOrientation(rayShoot));
			_player.PlaySound(_clip);
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) { CreatePortal(true); }//Cast Portal A
		else if (Input.GetMouseButtonDown(1)) { CreatePortal(false); }//Cast Portal B
	}
}
