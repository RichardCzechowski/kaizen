using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	void Update() {
		Vector3 targetPostition = new Vector3( Camera.main.transform.position.x, 
			this.transform.position.y, 
			Camera.main.transform.position.z ) ;
		this.transform.LookAt( targetPostition ) ;
	}

}
