using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class Flier : MonoBehaviour {

    public Vector3 direction;
    public float speed;

    public bool losslessCollision;

    public bool moving;
	
	void Update () {
	    	
	}

    void FixedUpdate() {
        if (moving) {
            gameObject.transform.root.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision) {

        // Create collison through angle of incidence = angle of refraction
        Vector3 normal = collision.contacts[0].normal;

        float angle = Vector3.Angle(direction, -normal);

        // TODO: implement case for lossy collision
        direction = direction - 2 * Vector3.Dot(direction, normal) * normal;
    }
}
