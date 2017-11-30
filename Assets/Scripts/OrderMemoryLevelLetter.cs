using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OrderMemoryLevelLetter : MonoBehaviour {

    private Color m_naturalColor;
    private Color m_activatedColor;
    private int m_number;
    private OrderMemoryLevelManager m_manager;

    public AudioClip m_audioClip;

    private Material m_material;

	void Start () {
        m_material = GetComponent<Renderer>().material;
    }
	
	void Update () {
		
	}

    /**
     * On select, ding and notify manager
     */
    public void Selected() {
        Ding();
        m_manager.ChosenCallback(m_number);
    }

    /**
     * Plays the associated ding
     */
    public void Ding() {
        StartCoroutine(C_Ding(1));
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator C_Ding(float time) {
        m_material.color = m_activatedColor;

        float interval = 1.0f / time;
        float total = 0;
        yield return null;

        while (total < 1.0f) {
            m_material.color = Color.Lerp(m_activatedColor, m_naturalColor, total);
            total += interval * Time.deltaTime;
            yield return null;
        }

    }

    /**
     * Shrinks out the letter
     */
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

    public void SetNumber(int index) {
        m_number = index;
    }

    public void SetManager(OrderMemoryLevelManager newManager) {
        m_manager = newManager;
    }

    public void SetAudioClip(AudioClip newClip) {
        m_audioClip = newClip;
        GetComponent<AudioSource>().clip = newClip;
    }

    public void SetColors(Color naturalColor, Color activatedColor) {
        m_naturalColor = naturalColor;
        m_activatedColor = activatedColor;
    }
}
