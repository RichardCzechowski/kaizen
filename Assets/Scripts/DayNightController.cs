using UnityEngine;
using System.Collections;

public class DayNightController : MonoBehaviour {

	public Light sun;
	public Light moon;
	public float secondsInFullDay = 120f;
	[Range(0,1)]
	public float currentTimeOfDay = 0;
	[HideInInspector]
	public float timeMultiplier = 1f;

	float sunInitialIntensity;

	public Gradient nightDayColor;
	public Gradient nightDayFogColor;

	void Start() {
		sunInitialIntensity = sun.intensity;
	}

	void Update() {
		UpdateSun();

		currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

		if (currentTimeOfDay >= 1) {
			currentTimeOfDay = 0;
		}
	}

	void UpdateSun() {
		sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
		moon.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 270, 170, 0);

		float intensityMultiplier = 1;
		if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f) {
			intensityMultiplier = 0;
		}
		else if (currentTimeOfDay <= 0.25f) {
			intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
		}
		else if (currentTimeOfDay >= 0.73f) {
			intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
		}

		sun.intensity = sunInitialIntensity * intensityMultiplier;

		sun.color = nightDayColor.Evaluate(currentTimeOfDay);
		RenderSettings.ambientLight = sun.color;

		RenderSettings.fogColor = nightDayFogColor.Evaluate(currentTimeOfDay);

	}
}
