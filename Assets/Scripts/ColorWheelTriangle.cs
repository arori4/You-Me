﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// another class to interact with the color wheel triangle
public class ColorWheelTriangle : MonoBehaviour {

    private const float REGULAR_SCALE = 0.25f;
    private const float SELECTED_SCALE = 0.4f;
    private const float ANIMATION_DURATION = 0.5f;

    private ColorWheel m_controller;
    private bool m_gazedAt;
    private bool m_selected;
    private Color m_baseColor;
    private Color m_selectedColor;

    /* Sets the colors used for the triangle */
    public void SetColors(Color baseColor, Color selectedColor) {
        m_baseColor = baseColor;
        m_selectedColor = selectedColor;
    }

    /* Sets the color wheel controller */
    public void SetController(ColorWheel colorWheel) {
        m_controller = colorWheel;
    }

    /* Changes the color on gazing */
    public void SetGazedAt(bool gazedAt) {
        if (m_baseColor != null && m_selectedColor != null) {
            if (gazedAt || m_selected) {
                GetComponent<Renderer>().material.color = m_selectedColor;
            }
            else {
                GetComponent<Renderer>().material.color = m_baseColor;
            }
        }
        else {
            Debug.Log("Error: ColorWheelTriangle has missing colors");
        }
    }

    /* Toggles whether the object has been selected or not */
    public void Select(bool selected) {
        // something is only done when there is a change in status
        if (selected != m_selected) {
            m_selected = selected;
            StartCoroutine(C_Scale(selected, ANIMATION_DURATION));
            m_controller.ChangeColorCallback(m_baseColor);
        }
    }

    /* Animation to enlarge/reduce */
    private IEnumerator C_Scale(bool selected, float duration) {

        // Create start and end vectors
        float index = 0;
        float startScale = REGULAR_SCALE;
        float endScale = SELECTED_SCALE;
        if (!selected) {
            startScale = SELECTED_SCALE;
            endScale = REGULAR_SCALE;
        }
        Vector3 startScaleVector = new Vector3(startScale, startScale, REGULAR_SCALE);
        Vector3 endScaleVector = new Vector3(endScale, endScale, REGULAR_SCALE);

        // Create intervals
        float interval = 1.0f / duration;
        yield return null;

        // Animation loop
        while (index < 1.0f) {
            index += interval * Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(startScaleVector, endScaleVector, index);
            yield return null;
        }

        // End clamp for consistency
        gameObject.transform.localScale = endScaleVector;

    }


}
