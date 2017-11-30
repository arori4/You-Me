using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Controller for the room in the starting phase*/
public class Level0Controller : MonoBehaviour {

    // Constants
    private readonly static Vector3 BALL_START_LOCATION = new Vector3(2, 3, 1.5f);
    private readonly static Vector3 FLOATING_BALL_START_LOCATION = new Vector3(0, 2.5f, 2.5f);
    private readonly static float BALL_START_SCALE_AMOUNT = 0.5f;
    private readonly static float SPAWN_WAIT = 0.4f;
    private readonly static float BALL_WAIT_DELETE = 0.2f;
    private readonly static Vector3[] BALL_TEXTURE_START_POSITIONS = {
        new Vector3(-1.5f, 1, 2),
        new Vector3(0, 1, 2),
        new Vector3(1.5f, 1, 2),
        new Vector3(-0.95f, 1, 3.5f),
        new Vector3(0.95f, 1, 3.5f)
    };
    private readonly static Vector3[] BALL_SPEED_START_POSITIONS = {
        new Vector3(-2, 3, 3),
        new Vector3(0, 3, 3),
        new Vector3(2, 3, 3)
    };
    private readonly static Vector3[] HEARTBEAT_SQUARES_START_POSITIONS = {
        new Vector3(-2, 3, 3),
        new Vector3(0, 3, 3),
        new Vector3(2, 3, 3)
    };
    private readonly static float[] BALL_SPEED_SPEEDS = { 0.2f, 0.5f, 0.7f };
    private readonly static float[] HEARTBEAT_SQUARE_SPEEDS = { 0.8f, 1.0f, 1.15f };

    public WallMaterialController m_wallController;
    public GameObject m_doneButton;
    public ColorWheel wheel;

    [Header("Prefabs")]
    public GameObject m_ballPrefab;
    public GameObject m_textureBallPrefab;
    public GameObject m_floatingBallPrefab;
    public GameObject m_speedBallPrefab;
    public GameObject m_metronomeSquarePrefab;
    public Material[] arr_ballMaterials; // keep the same in preferenceManager

    [Header ("Text")]
    public Text m_directions;
    public GameObject m_helloText;

    // Internal objects
    private GameObject m_currentBall;
    private GameObject[] arr_textureBalls;
    private GameObject[] arr_speedBalls;
    private GameObject[] arr_heartbeatSquares;
    private GameObject m_currentFloatingBall;
    private HeartbeatSquare m_currentHeartbeatSquare;

    // Chosen variables
    private Color m_chosenWallColor;
    private Color m_chosenBallColor;
    private Material m_chosenMaterial;
    private float m_chosenBallSpeed;
    private float m_chosenMetronomeSpeed;

    enum steps {
        LOAD,
        ROOM_COLOR_PICK,
        BALL_TEXTURE_PICK,
        BALL_COLOR_PICK,
        BALL_SPEED_PICK,
        METRONOME_SPEED_PICK,
        NEXT_LEVEL
    };
    int m_currentStep;

	void Start () {
        m_currentStep = (int)steps.LOAD;
        m_helloText.SetActive(true);
        m_doneButton.SetActive(false);
        arr_textureBalls = new GameObject[arr_ballMaterials.Length];
        arr_speedBalls = new GameObject[BALL_SPEED_SPEEDS.Length];
        arr_heartbeatSquares = new GameObject[HEARTBEAT_SQUARE_SPEEDS.Length];
    }

    /*
     * Callback for choosing a different color.
     * Behavior changes based on step.
     */
    public void ChooseColor(Color color) {
        switch (m_currentStep) {
            case (int)steps.LOAD:
                break;
            case (int)steps.ROOM_COLOR_PICK:
                m_chosenWallColor = color;
                m_wallController.ChangeColor(color);
                break;
            case (int)steps.BALL_TEXTURE_PICK:
                break;
            case (int)steps.BALL_COLOR_PICK:
                m_chosenBallColor = color;
                m_currentBall.GetComponent<Ball>().ChangeColor(color);
                break;
            case (int)steps.BALL_SPEED_PICK:
                break;
            case (int)steps.METRONOME_SPEED_PICK:
                break;
        }
    }

    /* Manages the stages of the level. */
    public void NextStage() {
        m_currentStep++;

        switch (m_currentStep) {
            case (int)steps.LOAD:
                StepLoad();
                break;
            case (int)steps.ROOM_COLOR_PICK:
                StepRoomColorPick();
                break;
            case (int)steps.BALL_TEXTURE_PICK:
                StepBallTexturePick();
                break;
            case (int)steps.BALL_COLOR_PICK:
                StepBallColorPick();
                break;
            case (int)steps.BALL_SPEED_PICK:
                StepBallSpeedPick();
                break;
            case (int)steps.METRONOME_SPEED_PICK:
                StepMetronomeSpeedPick();
                break;
            case (int)steps.NEXT_LEVEL:
                StepNextLevel();
                break;
        }
    }

    /*
     * Loads the next level, buckets.
     * Saves all to the preferenceManager, then goes to the next scene.
     */
    private void StepNextLevel() {
        PreferencesManager instance = PreferencesManager.instance;

        instance.roomColor = m_chosenWallColor;
        instance.ballColor = m_chosenBallColor;
        instance.ballMaterial = m_chosenMaterial;
        instance.ballSpeed = m_chosenBallSpeed;
        instance.metronomeSpeed = m_chosenMetronomeSpeed;

        // Set initial game
        instance.level = 0;

        // Save preferences
        instance.Save();

        SceneManager.LoadScene("1_Buckets");
    }

    /* Loads assets necessary for the level.
     * For performance, we may use a coroutine to load, as well as fade in screen.
     */
    private void StepLoad() {

    }

    /* Unfolds the color wheel and allows user to change room color */
    private void StepRoomColorPick() {
        wheel.Unfold();
        m_doneButton.SetActive(true);
        m_helloText.SetActive(false);

        m_directions.text = "Choose the color of the room";
    }

    /* Folds color wheel and spawns different textured balls */
    private void StepBallTexturePick() {
        wheel.Fold();
        StartCoroutine(C_StepBallTextureSpawn());
        m_currentFloatingBall = Instantiate(m_floatingBallPrefab,
            FLOATING_BALL_START_LOCATION, Quaternion.identity);
        m_directions.text = "Choose the texture of the ball";
    }

    /* Unfolds color wheel and allows user to change color of ball */
    private void StepBallColorPick() {
        wheel.Unfold();
        m_currentFloatingBall.GetComponent<Ball>().Shrink(1.0f, 0.5f);
        StartCoroutine(C_StepBallDelete(arr_textureBalls));
        m_currentBall = GameObject.Instantiate(m_ballPrefab);
        m_currentBall.GetComponent<Ball>().Environment_Choose_Create(BALL_START_LOCATION, BALL_START_SCALE_AMOUNT);
        m_currentBall.GetComponent<Renderer>().material = m_chosenMaterial;

        m_directions.text = "Choose the color of the ball";

    }

    /* Spawns the ball for speed picking */
    private void StepBallSpeedPick() {
        wheel.Fold();
        StartCoroutine(C_StepBallSpeedSpawn());

        m_directions.text = "Choose the speed of the ball";
    }

    /* Spawns the blocks to choose the metronome speed */
    private void StepMetronomeSpeedPick() {
        StartCoroutine(C_StepBallDelete(arr_speedBalls));
        StartCoroutine(C_StepMetronomeSquareSpawn());

        m_directions.text = "Choose the speed of the heartbeat";
    }

    /* Spawns all the balls representing texture */
    private IEnumerator C_StepBallTextureSpawn() {
        for (int index = 0; index < arr_ballMaterials.Length; index++) {
            arr_textureBalls[index] = GameObject.Instantiate(
                m_textureBallPrefab, 
                BALL_TEXTURE_START_POSITIONS[index],
                Quaternion.identity);
            yield return new WaitForSeconds(SPAWN_WAIT);
            arr_textureBalls[index].GetComponent<Renderer>().material = arr_ballMaterials[index];
            arr_textureBalls[index].GetComponent<TextureBall>().SetController(this);
            yield return null;
        }

        yield return null;
    }

    /* Despawns all the balls representing texture */
    private IEnumerator C_StepBallDelete(GameObject[] array) {
        for (int index = 0; index < array.Length; index++) {
            GameObject.Destroy(array[index]);
            yield return new WaitForSeconds(BALL_WAIT_DELETE);
        }

        yield return null;
    }

    /* Spawns all the balls representing speed, and activates them */
    private IEnumerator C_StepBallSpeedSpawn() {
        for (int index = 0; index < BALL_SPEED_START_POSITIONS.Length; index++) {
            arr_speedBalls[index] = GameObject.Instantiate(
                m_speedBallPrefab,
                BALL_SPEED_START_POSITIONS[index],
                Quaternion.identity);
            yield return new WaitForSeconds(SPAWN_WAIT);
            arr_speedBalls[index].GetComponent<SpeedBall>().Level0UpDown(BALL_SPEED_SPEEDS[index]);
            arr_speedBalls[index].GetComponent<SpeedBall>().m_controller = this;
            yield return null;
        }

        yield return null;
    }

    /* Spawns all the squares representing heartbeats, and activates them */
    private IEnumerator C_StepMetronomeSquareSpawn() {
        for (int index = 0; index < HEARTBEAT_SQUARES_START_POSITIONS.Length; index++) {
            arr_heartbeatSquares[index] = GameObject.Instantiate(
                m_metronomeSquarePrefab,
                HEARTBEAT_SQUARES_START_POSITIONS[index],
                Quaternion.identity);
            yield return new WaitForSeconds(SPAWN_WAIT);
            arr_heartbeatSquares[index].GetComponent<HeartbeatSquare>().SetController(this);
            arr_heartbeatSquares[index].GetComponent<HeartbeatSquare>().SetSpeed(HEARTBEAT_SQUARE_SPEEDS[index]);
            arr_heartbeatSquares[index].GetComponent<HeartbeatSquare>().StopCallback();
        }

        yield return null;
    }

    public void BallTextureCallback(Material material) {
        m_chosenMaterial = material;
        m_currentFloatingBall.GetComponent<Renderer>().material = material;
    }

    /* Callback for choosing ball speed event. Also goes to the next stage */
    public void BallSpeedCallback(float speed) {
        m_chosenBallSpeed = speed;
        NextStage();
    }

    /* Callback for choosing a new heartbeat. Stops the current heartbeat and selects a new one */
    public void HeartbeatSquareCallback(HeartbeatSquare current, float speed) {
        if (m_currentHeartbeatSquare != null) {
            m_currentHeartbeatSquare.StopCallback();
        }
        m_currentHeartbeatSquare = current; // no need to call start, handled by heartbeatSquare
        m_chosenMetronomeSpeed = speed;
    }
}
