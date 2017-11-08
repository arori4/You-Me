using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* For Level 0 use
 * Code for the square that visualizes the heartbeat.
 */
[RequireComponent(typeof(AudioSource))]
public class HeartbeatSquare : MonoBehaviour {

    private AudioSource m_heartbeat;
    private float m_heartbeat_totalTime;
    private Color m_endColor = new Color(1, 1, 1);
    private Color m_startColor = new Color(1, 0, 0);
    private Material m_material;
    private Level0Controller m_controller;

    public Color m_selectedColor = new Color(0, 1, 0);
    public Color m_unselectedColor = new Color(1, 0, 0);
    private float m_speed = 1.0f;
    
	void Start () {
        m_heartbeat = GetComponent<AudioSource>();
        m_heartbeat_totalTime = m_heartbeat.clip.length;
        m_material = GetComponent<Renderer>().material;
	}

    public void SetController (Level0Controller controller) {
        m_controller = controller;
    }

    public void SetSpeed(float speed) {
        m_speed = speed;
        m_heartbeat.pitch = m_speed; // pitch is changed to change speed of sound.
    }
	
	void Update () {
        // Lerps between start of audio clip to end of audio clip
        m_material.color = Color.Lerp(m_startColor, m_endColor, m_heartbeat.time / m_heartbeat_totalTime);
	}

    /* Plays the heartbeat, and changes color */
    public void PlayCallback() {
        m_heartbeat.Play();
        m_startColor = m_selectedColor;
        m_controller.HeartbeatSquareCallback(this, m_speed);
    }

    /* Stops the heartbeat, and changes color */
    public void StopCallback() {
        m_heartbeat.Stop();
        m_startColor = m_unselectedColor;
    }
}
