using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private readonly static float START_SCALE_AMOUNT = 0.75f;
    private readonly static Vector3 START_SCALE = new Vector3(
        START_SCALE_AMOUNT, START_SCALE_AMOUNT, START_SCALE_AMOUNT);
    private readonly static float START_DURATION = 0.5f;
    private readonly static float COLOR_CHANGE_DURATION = 0.25f;

    public string text;
    public float bounciness;

    private Material m_material;

	void Start () {
        m_material = GetComponent<Renderer>().material;
	}
	
	void Update () {
		
	}

    /* A special method to enlarge the ball for Environment_0 */
    public void Environment_Choose_Create(Vector3 position) {
        transform.position = position;
        StartCoroutine(C_Scale(0.0f, START_SCALE_AMOUNT, START_DURATION));
    }

    public void ChangeColor(Color color) {
        StartCoroutine(C_changeColor(color, COLOR_CHANGE_DURATION));
    }


    /** Changes the color, gradually, of the walls
     */
    private IEnumerator C_changeColor(Color newColor, float duration) {
        // get current color
        Color oldColor = m_material.color;
        Color currentColor = oldColor;

        // set duration
        float interval = 1.0f / duration;
        yield return null;

        // linerar interpolation to the new color
        float index = 0;
        while (index < 1) {
            currentColor = Color.Lerp(oldColor, newColor, index);
            index += Time.deltaTime * interval;
            m_material.color = currentColor;
            yield return null;
        }

        // clamp color at end
        currentColor = newColor;
        m_material.color = currentColor;

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
