using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {

	//Private Variables.
	private Rigidbody rb;
	private bool pickedUp;

	// Use this for initialization
	void Start () {

		//Get RigidBody.
		rb = GetComponent<Rigidbody> ();

		//Get Level Manager instance.
		if (BucketsLevelManager.instance == null) {
			Debug.Log ("BallBehavior: BucketsLevelManager instance was not found.");
			return;
		}

		//Get all the materials.
		GetComponent<Renderer>().material.color = BucketsLevelManager.instance.colors[Random.Range(0, BucketsLevelManager.instance.colors.Length)];

		//Set ball variables according to difficulty.
		SetBouncinessAndMovement (BucketsLevelManager.instance.difficulty);

		//Check for repeat buckets.
		if (BucketsLevelManager.instance.repeat_buckets) {
			Color c = GetComponent<Renderer>().material.color;
			ArrayList ar = (ArrayList)BucketsLevelManager.instance.labels_map [c];
			object [] labels = ar.ToArray();

			int desired_label = 0;
			if (labels.Length == 1) {
				desired_label = (int)labels [0];
			} else {
				desired_label = Random.Range (0.0f, 1.0f) > 0.5f ? (int)labels [0] : (int)labels [1];
			}

			GetComponentInChildren<TextMesh> ().text = desired_label.ToString();
		}
	
	}
		
	void SetBouncinessAndMovement(int difficulty) {	
		Component[] mesh_colliders = GetComponents<SphereCollider> ();

		switch (difficulty) {
		case 0:
			foreach (SphereCollider mc in mesh_colliders) {
				mc.material.bounciness = 1.0f;
				mc.material.dynamicFriction = 0.0f;
				mc.material.staticFriction = 0.0f;
				mc.material.bounceCombine = PhysicMaterialCombine.Average;
				mc.material.frictionCombine = PhysicMaterialCombine.Average;
			}
			break;
		case 1:
			foreach (SphereCollider mc in mesh_colliders) {
				mc.material.bounciness = 1.0f;
				mc.material.dynamicFriction = 0.25f;
				mc.material.staticFriction = 0.1f;
				mc.material.bounceCombine = PhysicMaterialCombine.Maximum;
				mc.material.frictionCombine = PhysicMaterialCombine.Minimum;
			}
			break;
		case 2:
			foreach (SphereCollider mc in mesh_colliders) {
				mc.material.bounciness = 1.0f;
				mc.material.dynamicFriction = 0.25f;
				mc.material.staticFriction = 0.1f;
				mc.material.bounceCombine = PhysicMaterialCombine.Maximum;
				mc.material.frictionCombine = PhysicMaterialCombine.Minimum;
			}
			rb.velocity = (new Vector3 (Random.Range (-5, 5), Random.Range (-5, 5), Random.Range (-5, 5)));
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (pickedUp && BucketsLevelManager.instance != null) {
			Vector3 direction = this.transform.position - Camera.main.transform.position;
			direction.Normalize ();
			this.transform.position = (direction * BucketsLevelManager.instance.radius) + Camera.main.transform.position;
		}
	}

	void FixedUpdate () {
		if (pickedUp) {
			Quaternion q = Quaternion.identity * Quaternion.Inverse (Camera.main.transform.rotation);
			rb.MoveRotation (q);
		}
	}

	public void PickedUp() {
		this.transform.parent = Camera.main.transform;
		pickedUp = true;
		rb.useGravity = false;
		rb.freezeRotation = true;
		rb.velocity = new Vector3 (0, 0, 0);
	}

	public void LetGo() {
		this.transform.parent = null;
		pickedUp = false;
		rb.useGravity = true;
		rb.freezeRotation = false;
	}
}
