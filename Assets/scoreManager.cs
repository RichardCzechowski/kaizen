using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scoreManager : MonoBehaviour {

	public static scoreManager instance;
	public float currentScore;

	void Awake() {
		instance = this;
	}

	public void AddPoints(int pointsToAdd){
		currentScore += pointsToAdd;
		Debug.Log ("You Score is: " + currentScore);
	}

	public void DecrementPoints(int pointsToDecrement) {
		if (currentScore - pointsToDecrement < 0) {
			Debug.LogError ("negative points?!!?");
			return;
		}

		currentScore -= pointsToDecrement;
		int remainder = pointsToDecrement;
		foreach (var building in Building.All()) {
			building.stars -= remainder;
			remainder = Mathf.Abs(building.stars);
			if (remainder == 0) {
				return;
			} else {
				building.stars = 0;
			}
		}
	}

}
