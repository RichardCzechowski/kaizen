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

	public int numberOfShifts = 6;

	void Awake() {
		instance = this;
	}

	void Start() {
		sunInitialIntensity = sun.intensity;
		moonInitialIntensity = moon.intensity;
	}

	public float RealSecondsPerGameHour() {
		return secondsInFullDay / 24f;
	}

	public float TimeOfDayActual() {
		return _currentTimeOfDay;
	}

	public int NumberOfShifts() {
		return numberOfShifts;
	}

	public float ShiftLengthHours() {
		return 24f / NumberOfShifts ();
	}

	public float ShiftStartHour(int shiftIndex) { 
		return ShiftLengthHours () * shiftIndex;
	}

	public float ShiftEndHour(int shiftIndex) { 
		if (shiftIndex > NumberOfShifts () - 1) {
			return 0;
		}
		return ShiftLengthHours () * shiftIndex + 1;
	}

	public int CurrentShift() {
		return Mathf.FloorToInt (TimeOfDayIncludingPreview () * 24 / ShiftLengthHours ());
	}

	public int NextShift() {
		var thisShift = CurrentShift ();
		if (thisShift + 1 == NumberOfShifts ()) {
			return 0;
		} else {
			return thisShift + 1;
		}
	}

	public int PreviousShift() {
		var thisShift = CurrentShift ();
		if (thisShift - 1 < 0) {
			return NumberOfShifts () - 1;
		} else {
			return thisShift - 1;
		}
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
		Debug.Log ("preview for " + time);
		_previewTimeOfDay = time;
		paused = true;
	}

	public void BeginPreviewHours(float hours) {
		BeginPreview (hours / 24f);
	}

	public void EndPreview() {
		paused = false;
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			paused = !paused;
		}
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
