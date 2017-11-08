using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BucketTrigger : MonoBehaviour {

	private Color color;

	void Start() {
		color = this.GetComponent<Renderer> ().material.color;
	}

	void OnTriggerEnter(Collider other) {
		
		//Get Level Manager instance.
		if (BucketsLevelManager.instance == null) {
			Debug.Log ("BucketTrigger: BucketsLevelManager instance was not found.");
			return;
		}

		if (BucketsLevelManager.instance.repeat_buckets) {
			Renderer r = other.GetComponent<Renderer> ();
			TextMesh ot = other.GetComponentInChildren<TextMesh> ();
			TextMesh t = this.transform.parent.gameObject.GetComponentInChildren<TextMesh> ();
			if (r.material.color.Equals (color) && t.text.Equals (ot.text)) {
				BallScored (other);
			} else {
				IncorrectBall (other);
			}
		} else {
			if (other.GetComponent<Renderer> ().material.color.Equals (color)) {
				BallScored (other);
			} else {
				IncorrectBall (other);
			}
		} 
	}

	void OnTriggerStay(Collider other) {
	
	}

	void OnTriggerExit(Collider other) {
	
	}

	void BallScored(Collider other) {
		BucketsLevelManager.instance.score++;
		Destroy(other.gameObject);
		BucketsLevelManager.instance.num_balls--;
	}

	void IncorrectBall(Collider other) {
		Vector3 respawn = Vector3.Cross (this.transform.position, Vector3.up);
		other.gameObject.transform.position = this.transform.position + respawn.normalized;
	}
}
