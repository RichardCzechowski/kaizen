using UnityEngine;
using System.Collections;

public class statusIcon : MonoBehaviour {

	public static statusIcon instance;
	Animator anim;
	// Use this for initialization
	void Awake() {
		instance = this;
	}
	void Start(){
		anim = gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void Popup(){
		anim.Play ("popup");
	}

	public void Popdown(){
		anim.Play ("popdown");
	}
}