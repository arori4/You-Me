using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Defines behaviour for the teacher
 */
public class TeacherBehavior : MonoBehaviour {

    [Header("Variables")]
    public float speed = 1;
    public float deltaY = 1;

    public static TeacherBehavior instance;
    float startY;
    

    void Awake() {
        //If the instance is null, we set the reference.
        if (instance == null) {
            instance = this;
        }
    }

    private void OnDestroy() {
        instance = null;
    }

    /**
     * Looks at an object. Should maintain x and z rotation
     */
    public void LookAt(GameObject other) {
        Vector3 targetPostition = new Vector3(other.transform.position.x,
                                       this.transform.position.y,
                                       other.transform.position.z);
        StartCoroutine(C_LookAt(targetPostition, 1.0f));
    }

    void Start () {
        startY = transform.position.y;
	}
	
    /**
     * Bobs the teacher up and down
     */
	void Update () {
        transform.position = new Vector3(
            transform.position.x,
            startY + Mathf.Sin(Time.timeSinceLevelLoad * speed) * deltaY,
            transform.position.z);
	}

    /**
     * 
     */
    private IEnumerator C_LookAt(Vector3 target, float time) {
        float startTime = 0;
        Vector3 currentLookAt = new Vector3(
            transform.position.x + transform.forward.x,
            transform.position.y,
            transform.position.z + transform.forward.z
            );

        while (startTime < time) {
            startTime += time * Time.deltaTime;
            Vector3 newTarget = Vector3.Lerp(currentLookAt, target, startTime / time);
            transform.LookAt(newTarget);
            yield return null;
        }
        

    }
}
