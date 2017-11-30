using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defined Bucket Behavior for the Scene.
public class BucketBehavior : MonoBehaviour {

    public void ShrinkOut() {
        StartCoroutine(C_ShrinkOut(1));
    }

    private IEnumerator C_ShrinkOut(float time) {

        // Set variables
        float increment = 1.0f / time;
        float total = 0;
        Vector3 originalScale = transform.localScale;
        Vector3 target = new Vector3(0, 0, 0);
        yield return null;

        // Shrink
        while (total < 1) {
            total += increment * Time.deltaTime;
            transform.root.localScale = Vector3.Lerp(originalScale, target, total);
            yield return null;
        }

        // Finally, destroy object
        Destroy(gameObject.transform.root.gameObject);
    }
}
