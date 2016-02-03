using UnityEngine;
using System.Collections;
/// <summary>
/// Cam follower. aquele scriptzao de seguir alguem
/// </summary>
public class CamFollower : MonoBehaviour {

	public Transform followTarget;

	public float followSpeed;

	public Vector2 limitesDaArena;

	private Vector2 limitesDaCamera;

	void Start(){
		limitesDaCamera = Vector2.one * GetComponent<Camera>().orthographicSize;
	}

	// Update is called once per frame
	void Update () {
		if(followTarget){
			Vector3 lerpingPos = Vector3.Lerp(transform.position, followTarget.position, Time.deltaTime * followSpeed);
			lerpingPos.z = -10;
			transform.position =  lerpingPos;

			Vector3 restoredPos = transform.position;

			if(transform.position.x > limitesDaArena.x + limitesDaCamera.x){
				restoredPos.x = limitesDaArena.x + limitesDaCamera.x;
			}else if(transform.position.x <  -limitesDaArena.x - limitesDaCamera.x){
				restoredPos.x = -limitesDaArena.x - limitesDaCamera.x;
			}

			if(transform.position.y > limitesDaArena.y + limitesDaCamera.y){
				restoredPos.y = limitesDaArena.y + limitesDaCamera.y;
			}else if(transform.position.y <  -limitesDaArena.y - limitesDaCamera.y){
				restoredPos.y = -limitesDaArena.y - limitesDaCamera.y;
			}

			transform.position = restoredPos;
		}
	}
}
