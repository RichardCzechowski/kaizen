using UnityEngine;
using System.Collections;

public class personState : MonoBehaviour {
	public enum State {settingPath, waiting, walking};
	public State state = State.settingPath;

	// Use this for initialization
	void Start () {
		setState (State.waiting);
	}
	
	// Update is called once per frame
	public void setState (State newState) {
		state = newState;
		switch(state){
		case State.settingPath:
			// Don't move until path is set
			Debug.Log("settingPath");
			setState(State.waiting);
			break;
		case State.waiting:
			//Hangout for a length of time
			Debug.Log("waiting");
			StartCoroutine(wait(10));
			break;
		case State.walking:
			// Animate walking
			break;
		}
	}

	IEnumerator wait(int time) {
		Debug.Log("Before Waiting 2 seconds");
		yield return new WaitForSeconds(time);
		Debug.Log("After Waiting 2 Seconds");
	}

}
