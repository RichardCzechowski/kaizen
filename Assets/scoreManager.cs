using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scoreManager : MonoBehaviour {

	public static scoreManager instance;
	public float currentScore;
	public AudioClip yougottabox;

	void Awake() {
		instance = this;
	}

	public void AddPoints(int pointsToAdd){
		currentScore += pointsToAdd;
		Debug.Log ("You Score is: " + currentScore);
		for(var i = 1 ;i < currentScore; i++)
		{
			AudioSource.PlayClipAtPoint (yougottabox, Camera.main.transform.position);
		}
		tutorial.instance.kickOffIntroduceStar();
	}

	public void DecrementPoints(int pointsToDecrement) {
		if (currentScore - pointsToDecrement < 0) {
			Debug.LogError ("negative points?!!?");
			return;
		}

		currentScore -= pointsToDecrement;
		int remainder = pointsToDecrement;
		foreach (var building in Building.All()) {
			int starsToRemove = remainder;
			if (building.stars - remainder < 0) {
				starsToRemove = building.stars;
			}
			building.stars -= starsToRemove;
			pointsToDecrement -= starsToRemove;
		}
	}

}
