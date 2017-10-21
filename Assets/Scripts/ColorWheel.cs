using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour {

    readonly public int NUM_TRIANGLES = 12;
    readonly public float SEPARATION = 0.2f;
    readonly Vector3 TRIANGLE_SCALE = new Vector3(1, 1, 1) * 0.25f;
    readonly Color[] COLORS = {
        Color.red,
        Color.Lerp(Color.red, Color.yellow, 0.5f),
        Color.yellow,
        Color.Lerp(Color.yellow, Color.green, 0.5f),
        Color.green,
        Color.cyan,
        Color.blue,
        Color.Lerp(Color.red, Color.blue, 0.66f),
        Color.grey,
        Color.white,
        Color.Lerp(Color.red, Color.blue, 0.33f),
        Color.magenta
    };

    public WallMaterialController m_wallController;
    public GameObject m_colorTrianglePrefab;
    
	void Start () {

        // set properties of m_colorTrianglePrefab
        m_colorTrianglePrefab.transform.localScale = TRIANGLE_SCALE;

        // create %NUM_TRIANGLES number of triangles
        for (int index = 0; index < NUM_TRIANGLES; index++) {
            Quaternion rotation = Quaternion.Euler(0, 0, index * 360 / NUM_TRIANGLES);
            GameObject newObject = GameObject.Instantiate(
                m_colorTrianglePrefab, 
                gameObject.transform.position, 
                rotation);
            newObject.GetComponent<Renderer>().material.color = COLORS[index];

            // move the triangle away slightly
            newObject.transform.position += rotation * new Vector3(0, -1, 0) * SEPARATION;

            // register controller with triangle
            ColorWheelTriangle triangleScript = newObject.GetComponent<ColorWheelTriangle>();
            if (triangleScript == null) {
                Debug.Log("error: ColorWheel has created a prefab without a ColorWheelTriangle script");
            }
            else {
                triangleScript.SetController(this);
                triangleScript.SetColors(COLORS[index], COLORS[(index + 1) % 12]);
            }
        }
	}
	
	void Update () {
		
	}

    public void  ChangeColorCallback(Color selectedColor) {
        m_wallController.ChangeColor(selectedColor);
    }
}


