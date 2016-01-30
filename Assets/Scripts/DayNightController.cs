using UnityEngine;
using System.Collections;

public class DayNightController : MonoBehaviour {

	public static DayNightController instance = null;
		
	public Light sun;
	public Light moon;
	public float secondsInFullDay = 120f;

	[Range(0,1)]
	float _currentTimeOfDay = 0;

	[HideInInspector]
	public float timeMultiplier = 1f;

	public int daysElapsed = 0;

	float sunInitialIntensity;
	float moonInitialIntensity;

	public Gradient nightDayColor;
	public Gradient nightDayFogColor;

	void Start() {
		instance = this;
		sunInitialIntensity = sun.intensity;
		moonInitialIntensity = moon.intensity;
	}

	public float HoursToGameTime() {
		return secondsInFullDay / 24f;
	}

	public float TimeOfDayActual() {
		return _currentTimeOfDay;
	}

	public float TimeOfDayIncludingPreview() {
		if (paused) {
			return _previewTimeOfDay;
		}
		return _currentTimeOfDay;
	}

	public bool paused = false;

	[Range(0,1)]
	public float _previewTimeOfDay;
	public void BeginPreview(float time) {
		_previewTimeOfDay = time;
		paused = true;
	}

	public void EndPreview() {
		paused = false;
	}

	void Update() {

		UpdateSun();

		if (!paused) { 
			_currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;
			if (_currentTimeOfDay >= 1) {
				_currentTimeOfDay = 0;
				daysElapsed++;
			}
		}
			
	}

	void UpdateSun() {
		sun.transform.localRotation = Quaternion.Euler((TimeOfDayIncludingPreview() * 360f) - 90, 170, 0);
		moon.transform.localRotation = Quaternion.Euler((TimeOfDayIncludingPreview() * 360f) - 270, 170, 0);

		float intensityMultiplier = 1;
		if (TimeOfDayIncludingPreview() <= 0.23f || TimeOfDayIncludingPreview() >= 0.75f) {
			intensityMultiplier = 0;
		}
		else if (TimeOfDayIncludingPreview() <= 0.25f) {
			intensityMultiplier = Mathf.Clamp01((TimeOfDayIncludingPreview() - 0.23f) * (1 / 0.02f));
		}
		else if (TimeOfDayIncludingPreview() >= 0.73f) {
			intensityMultiplier = Mathf.Clamp01(1 - ((TimeOfDayIncludingPreview() - 0.73f) * (1 / 0.02f)));
		}

		sun.intensity = sunInitialIntensity * intensityMultiplier;
		moon.intensity = moonInitialIntensity * (1 - intensityMultiplier);

		sun.color = nightDayColor.Evaluate(TimeOfDayIncludingPreview());
		RenderSettings.ambientLight = sun.color;

		RenderSettings.fogColor = nightDayFogColor.Evaluate(TimeOfDayIncludingPreview());

	}
}
