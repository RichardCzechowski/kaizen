using UnityEngine;
using System.Collections;

public class statusIcon : MonoBehaviour {

	Animator anim;

	void Awake() {
		anim = gameObject.GetComponent<Animator> ();
	}
		
	public void Popup(){
		anim.Play ("popup");
	}

	public void Popdown(){
		anim.Play ("popdown");
	}
}