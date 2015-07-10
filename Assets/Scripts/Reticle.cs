using UnityEngine;
using System.Collections;

public class Reticle : MonoBehaviour {
	
	public Camera cameraFacing = null;
	private Vector3 originalScale;

	// Use this for initialization
	void Start () {
		originalScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraFacing != null) {
			RaycastHit hit;
			float distance;
			if (Physics.Raycast(new Ray(cameraFacing.transform.position, cameraFacing.transform.forward), 
			                    out hit)) {
				distance = hit.distance;
			} else {
				distance = cameraFacing.farClipPlane * 0.95f;
			}
		

			transform.position = cameraFacing.transform.position + 
			(cameraFacing.transform.forward * distance);
			
			this.transform.LookAt (cameraFacing.transform.position);
			// rotate around y axis because the above has back of quad facing camera
			transform.Rotate (0.0f, 180.0f, 0.0f);

			// fix screen size of reticle (regardless of distance)
			// if very close to reticle, make reticle appear slightly larger
			if (distance < 10.0f) {
				distance *= 1 + 5*Mathf.Exp(-distance);
			}
			transform.localScale = originalScale * distance;
		}
	}
}
