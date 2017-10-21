using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaterialController : MonoBehaviour {

    private const float COLOR_CHANGE_DURATION = 1.0f;

    Material currentMaterial;

    public GameObject floor;
    public GameObject ceiling;
    public GameObject northWall, southWall, westWall, eastWall;

    void Start() {
        
    }

    void Update() {

    }

    /** Sets the material of the walls
     */
    private void SetMaterial(Material newMaterial) {
        floor.GetComponent<Renderer>().material = newMaterial;
        ceiling.GetComponent<Renderer>().material = newMaterial;
        northWall.GetComponent<Renderer>().material = newMaterial;
        southWall.GetComponent<Renderer>().material = newMaterial;
        westWall.GetComponent<Renderer>().material = newMaterial;
        eastWall.GetComponent<Renderer>().material = newMaterial;
    }

    /** Sets the color of the walls
     */
    private void SetColor(Color color) {
        currentMaterial = floor.GetComponent<Renderer>().material;
        currentMaterial.color = color;
        SetMaterial(currentMaterial);
    }

    public void ChangeColor(Color color) {
        StartCoroutine(Co_changeColor(color, COLOR_CHANGE_DURATION));
    }


    /** Changes the color, gradually, of the walls
     */
    private IEnumerator Co_changeColor(Color newColor, float duration) {
        // get current color
        Color oldColor = floor.GetComponent<Renderer>().material.color;
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
