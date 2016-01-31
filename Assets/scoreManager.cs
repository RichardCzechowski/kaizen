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
			int starsToRemove = remainder;
			if (building.stars - remainder < 0) {
				starsToRemove = building.stars;
			}
			Debug.Log ("removing " + starsToRemove);
			building.stars -= starsToRemove;
			pointsToDecrement -= starsToRemove;
		}
	}

}
