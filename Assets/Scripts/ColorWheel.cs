using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour {

    private readonly float ANIMATION_START_TIME = 1.0f;
    private readonly float ANIMATION_FOLD_TIME = 0.5f;
    private readonly int NUM_TRIANGLES = 12;
    private readonly float SEPARATION = 0.0f;
    private readonly Vector3 TRIANGLE_SCALE = new Vector3(1, 1, 1) * 0.15f;
    private readonly Color[] WALL_COLORS = {
        new Color(255/255.0f, 105/255.0f, 97/255.0f), //Red
        new Color(255/255.0f, 153/255.0f, 51/255.0f), //Orange
        new Color(248/255.0f, 222/255.0f, 126/255.0f), //Yellow
        new Color(227/255.0f, 249/255.0f, 136/255.0f), //Yellow-Green
        new Color(113/255.0f, 188/255.0f,120/255.0f), //Green
        new Color(150/255.0f, 222/255.0f, 209/255.0f), //Cyan
        new Color(133/255.0f, 196/255.0f, 234/255.0f), //Blue
        new Color(255/255.0f, 153/255.0f, 204/255.0f), //Magenta
        new Color(216/255.0f, 145/255.0f, 239/255.0f), //Lilac
        new Color(172/255.0f, 172/255.0f, 230/255.0f), //Purple
        new Color(221/255.0f, 221/255.0f, 221/255.0f), //Grey
        new Color(239/255.0f, 207/255.0f, 151/255.0f) //Beige
    };
    readonly Color[] DISPLAY_COLORS = {
        new Color(230/255.0f, 32/255.0f, 32/255.0f), //Red
        new Color(255/255.0f, 140/255.0f, 0/255.0f), //Orange
        new Color(255/255.0f, 204/255.0f, 0/255.0f), //Yellow
        new Color(186/255.0f, 227/255.0f, 3/255.0f), //Yellow-Green
        new Color(0/255.0f, 165/255.0f, 80/255.0f), //Green
        new Color(10/255.0f, 186/255.0f, 181/255.0f), //Cyan
        new Color(49/255.0f, 140/255.0f, 231/255.0f), //Blue
        new Color(249/255.0f, 132/255.0f, 229/255.0f), //Magenta
        new Color(182/255.0f, 102/255.0f, 210/255.0f), //Lilac
        new Color(89/255.0f, 70/255.0f, 178/255.0f), //Purple
        new Color(186/255.0f, 186/255.0f, 186/255.0f), //Grey
        new Color(200/255.0f, 173/255.0f, 127/255.0f) //Beige
    };
    readonly Color[] HOVER_HIGHLIGHT_COLORS = {
        new Color(255/255.0f,  36/255.0f,  0/255.0f), //Red
        new Color(255/255.0f,  120/255.0f,  0/255.0f), //Orange
        new Color(255/255.0f,  219/255.0f,  0/255.0f), //Yellow
        new Color(223/255.0f,  255/255.0f,  0/255.0f), //Yellow-Green
        new Color(3/255.0f,  192/255.0f,  60/255.0f), //Green
        new Color(64/255.0f,  224/255.0f,  208/255.0f), //Cyan
        new Color(58/255.0f,  173/255.0f,  255/255.0f), //Blue
        new Color(255/255.0f,  29/255.0f,  206/255.0f), //Magenta
        new Color(208/255.0f,  114/255.0f,  255/255.0f), //Lilac
        new Color(103/255.0f,  54/255.0f,  249/255.0f), //Purple
        new Color(209/255.0f,  209/255.0f,  209/255.0f), //Grey
        new Color(222/255.0f,  184/255.0f,  135/255.0f) //Beige
    };

    public Level0Controller controller;
    public GameObject m_colorTrianglePrefab;

    private ColorWheelTriangle m_currentlySelectedTriangle;
    private GameObject m_ball;
    private GameObject[] arr_triangles;

    void Start() {
        arr_triangles = new GameObject[NUM_TRIANGLES];
        StartCoroutine(C_Load());
    }

    void Update() {

    }

    /* Start level sequence */
    public void Unfold() {
        StartCoroutine(C_Start(ANIMATION_START_TIME));
    }

    public void Fold() {
        StartCoroutine(C_Fold(ANIMATION_FOLD_TIME));
    }

    /* Clicking on a color wheel triangle calls back to set the wall material color */
    public void ChangeColorCallback(Color wallColor, ColorWheelTriangle selector) {
        if (m_currentlySelectedTriangle != null) {
            m_currentlySelectedTriangle.Select(false);
        }
        m_currentlySelectedTriangle = selector;
        controller.ChooseColor(wallColor);
    }

    /* Sets the ball to be colored */
    public void SetBall(GameObject ball) {
        m_ball = ball;
    }

    /* Loading Coroutine, for performance reasons. Reduces load by initializing one object per update*/
    private IEnumerator C_Load() {

        // set properties of m_colorTrianglePrefab
        m_colorTrianglePrefab.transform.localScale = TRIANGLE_SCALE;
        yield return null;

        // create %NUM_TRIANGLES number of triangles
        for (int index = 0; index < NUM_TRIANGLES; index++) {
            Quaternion rotation = Quaternion.Euler(0, 0, index * 360 / NUM_TRIANGLES);
            GameObject newObject = GameObject.Instantiate(
                m_colorTrianglePrefab,
                gameObject.transform.position,
                rotation);
            newObject.SetActive(false);

            yield return null;

            // move the triangle away slightly
            newObject.transform.position += rotation * new Vector3(0, -1, 0) * SEPARATION;
            arr_triangles[index] = newObject;

            // register controller with triangle
            ColorWheelTriangle triangleScript = newObject.GetComponent<ColorWheelTriangle>();
            if (triangleScript == null) {
                Debug.Log("error: ColorWheel has created a prefab without a ColorWheelTriangle script");
            }
            else {
                triangleScript.SetController(this);
                triangleScript.SetColors(WALL_COLORS[index], DISPLAY_COLORS[index], HOVER_HIGHLIGHT_COLORS[index]);
            }

            yield return null;
        }
    }

    /* Launches each of the triangle animations */
    private IEnumerator C_Start(float loadTime) {
        float waitTime = loadTime / NUM_TRIANGLES;

        for (int index = 0; index < NUM_TRIANGLES; index++) {
            arr_triangles[index].SetActive(true);

            yield return new WaitForSeconds(waitTime);
        }
    }

    /* Folds the wheel, and deactivates all of the triangles */
    private IEnumerator C_Fold(float foldTime) {
        float waitTime = foldTime / NUM_TRIANGLES;

        for (int index = 0; index < NUM_TRIANGLES; index++) {
            arr_triangles[index].GetComponent<ColorWheelTriangle>().Fold();

            yield return new WaitForSeconds(waitTime);
        }
    }
}


