using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherCameraTestScript : MonoBehaviour {
	float raycastDistance = 15f;

	float spherecastDistance = 5f;

	float sphereRadius = 2f;

	float speedToFixClipping = 0.2f;

	void FixClippingThroughWalls () {
		RaycastHit hit;
		Vector3 direction = transform.parent.position - transform.position;
		Vector3 localPos = transform.localPosition;

		for (float i = 0; i <= 0f; i += speedToFixClipping) {
			Vector3 pos = transform.TransformPoint (new Vector3 (localPos.x, localPos.y, i));
			if (Physics.Raycast (pos, direction, out hit, raycastDistance)) {
				if (!hit.collider.CompareTag ("Player"))
					continue;

				if (!Physics.SphereCast (pos, sphereRadius, transform.forward * -1, out hit, spherecastDistance)) {
					localPos.z = i;
					break;
				}
			}
		}
		transform.localPosition = Vector3.Lerp (transform.localPosition, localPos, 0.5f * Time.deltaTime);
	}
}
