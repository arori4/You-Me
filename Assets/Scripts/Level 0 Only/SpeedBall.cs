using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBall : MonoBehaviour {

    private readonly static float MIN_Y = 0.5f;
    private readonly static float MAX_Y = 3;

    public float m_speed;
    public Level0Controller m_controller;

    /**
     * Provides functionality and movement for Level 0 Choosing speed
     */
    public void Level0UpDown(float speed) {
        m_speed = speed;
        StartCoroutine(C_Level0Move(speed));
    }

    private void OnDisable() {
        StopAllCoroutines();
    }

    /* Animation to move up and down for Level 0 ball speed chooser*/
    private IEnumerator C_Level0Move(float speed) {

        bool down = true;

        while (true) {
            if (down) {
                transform.position += Vector3.down * speed * Time.deltaTime;
                if (transform.position.y < MIN_Y) {
                    down = !down;
                    transform.position = new Vector3(
                        transform.position.x, MIN_Y, transform.position.z);
                }
            }
            else {
                transform.position += Vector3.up * speed * Time.deltaTime;
                if (transform.position.y > MAX_Y) {
                    down = !down;
                    transform.position = new Vector3(
                        transform.position.x, MAX_Y, transform.position.z);
                }
            }

            yield return null;
        }

    }

    public void ControllerCallback() {
        m_controller.BallSpeedCallback(m_speed);
    }

}
