using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaterialController : MonoBehaviour {

    private const float COLOR_CHANGE_DURATION = 1.0f;

    public GameObject floor;
    public GameObject ceiling;
    public GameObject northWall, southWall, westWall, eastWall;

    Material m_sharedMaterial;

    void Start() {
        m_sharedMaterial = floor.GetComponent<Renderer>().material;

        //ceiling.GetComponent<Renderer>().material = m_sharedMaterial; //not needed now
        northWall.GetComponent<Renderer>().material = m_sharedMaterial;
        southWall.GetComponent<Renderer>().material = m_sharedMaterial;
        westWall.GetComponent<Renderer>().material = m_sharedMaterial;
        eastWall.GetComponent<Renderer>().material = m_sharedMaterial;
    }

    void Update() {

    }

    /** Sets the color of the walls
     */
    private void SetColor(Color color) {
        m_sharedMaterial.color = color;
    }

    public void ChangeColor(Color color) {
        StartCoroutine(Co_changeColor(color, COLOR_CHANGE_DURATION));
    }


    /** Changes the color, gradually, of the walls
     */
    private IEnumerator Co_changeColor(Color newColor, float duration) {
        // get current color
        Color oldColor = m_sharedMaterial.color;
        Color currentColor = oldColor;

        // set duration
        float interval = 1.0f / duration;
        yield return null;

        // linerar interpolation to the new color
        float index = 0;
        while (index < 1) {
            currentColor = Color.Lerp(oldColor, newColor, index);
            index += Time.deltaTime * interval;
            SetColor(currentColor);
            yield return null;
        }

        // clamp color at end
        currentColor = newColor;
        SetColor(currentColor);

    }

}
