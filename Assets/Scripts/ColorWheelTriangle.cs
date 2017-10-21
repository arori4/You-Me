using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// another class to interact with the color wheel triangle
public class ColorWheelTriangle : MonoBehaviour {

    private const float REGULAR_SCALE = 0.15f;
    private const float SELECTED_SCALE = 0.2f;
    private const float ANIMATION_DURATION = 0.5f;
    private const float ANIMATION_START_DURATION = 0.5f;

    private ColorWheel m_wheel;
    private bool m_selected;
    private Color m_wallColor;
    private Color m_displayColor;
    private Color m_hoverHighlightColor;

    private void Start() {
    }

    /* Sets the colors used for the triangle */
    public void SetColors(Color wallColor, Color displayColor, Color hoverHighlightColor) {
        m_wallColor = wallColor;
        m_displayColor = displayColor;
        m_hoverHighlightColor = hoverHighlightColor;
        GetComponent<Renderer>().material.color = m_displayColor;
    }

    /* Sets the color wheel controller */
    public void SetController(ColorWheel colorWheel) {
        m_wheel = colorWheel;
    }

    /* Changes the color on gazing */
    public void SetGazedAt(bool gazedAt) {
        if (gazedAt || m_selected) {
            GetComponent<Renderer>().material.color = m_displayColor;
        }
        else {
            GetComponent<Renderer>().material.color = m_hoverHighlightColor;
        }
    }

    private void OnEnable() {
        gameObject.transform.localScale = Vector3.zero;
        StartCoroutine(C_Scale(0, REGULAR_SCALE, ANIMATION_START_DURATION));
    }

    /* Toggles whether the object has been selected or not */
    public void Select(bool selected) {
        // something is only done when there is a change in status
        if (selected != m_selected) {
            m_selected = selected;
            
            if (selected) {
                m_wheel.ChangeColorCallback(m_wallColor, this); // if actually selected, call back to switch color
                StartCoroutine(C_Scale(REGULAR_SCALE, SELECTED_SCALE, ANIMATION_DURATION));
            }
            else {
                StartCoroutine(C_Scale(SELECTED_SCALE, REGULAR_SCALE, ANIMATION_DURATION));
            }
        }
    }

    /* Animation to enlarge/reduce */
    private IEnumerator C_Scale(float startScale, float endScale, float duration) {

        // Create start and end vectors
        float index = 0;
        Vector3 startScaleVector = new Vector3(startScale, startScale, startScale);
        Vector3 endScaleVector = new Vector3(endScale, endScale, endScale);

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
