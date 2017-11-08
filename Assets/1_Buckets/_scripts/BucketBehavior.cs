using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defined Bucket Behavior for the Scene.
public class BucketBehavior : MonoBehaviour {

	//Initialization.
	void Start () {

		//Get rendered objects.
		Renderer [] renderers = this.GetComponentsInChildren<Renderer> ();

		//Get Level Manager instance.
		if (BucketsLevelManager.instance == null) {
			Debug.Log ("BucketBehavior: BucketsLevelManager instance was not found.");
			return;
		}
			
		//For each render.
		foreach(Renderer r in renderers)
			if(!r.gameObject.tag.Equals("NumberLabel"))
				r.material.color = BucketsLevelManager.instance.colors[BucketsLevelManager.instance.buckets_instantiated];
		BucketsLevelManager.instance.buckets_instantiated++;

	}

}
