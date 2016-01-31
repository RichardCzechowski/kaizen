using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class DynamicDOF : MonoBehaviour {

	public Transform Origin;
	public Transform target;
	public DepthOfField DOF34;

	void Update () {

		Ray ray = new Ray(Origin.position, Origin.forward);
		RaycastHit hit = new RaycastHit ();

		if (Physics.Raycast (ray, out hit, Mathf.Infinity))
		{
			DOF34.focalTransform = target;
			target.transform.position = hit.point;
		}
		else
		{
			DOF34.focalTransform = null;
		}

	}
}
