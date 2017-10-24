using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controller for the room in the starting phase*/
public class Level0Controller : MonoBehaviour {

    private readonly static Vector3 BALL_START_LOCATION = new Vector3(2, 3, 1.5f);
    private readonly static Vector3 FLOATING_BALL_START_LOCATION = new Vector3(0, 2.5f, 2.5f);
    private readonly static float BALL_START_SCALE_AMOUNT = 0.5f;
    private readonly static float BALL_TEXTURE_WAIT_SPAWN = 0.4f;
    private readonly static float BALL_TEXTURE_WAIT_DELETE = 0.2f;
    private readonly static Vector3[] BALL_START_POSITIONS = {
        new Vector3(-1.5f, 1, 2),
        new Vector3(0, 1, 2),
        new Vector3(1.5f, 1, 2),
        new Vector3(-0.95f, 1, 3.5f),
        new Vector3(0.95f, 1, 3.5f)
    };

    public WallMaterialController m_wallController;
    public GameObject m_helloText;
    public GameObject m_doneButton;
    public ColorWheel wheel;

    public GameObject m_ballPrefab;
    public GameObject m_textureBallPrefab;
    public GameObject m_floatingBallPrefab;

    public Material[] arr_ballMaterials;
    public Text m_directions;

    private GameObject m_currentBall;
    private GameObject[] arr_textureBalls;
    private GameObject m_currentFloatingBall;
    private Material m_chosenTexture;

    enum steps {
        LOAD,
        ROOM_COLOR_PICK,
        BALL_TEXTURE_PICK,
        BALL_COLOR_PICK,
        BALL_SPEED_PICK,
        BALL_BOUNCE_PICK
    };
    int m_currentStep;

	void Start () {
        m_currentStep = (int)steps.LOAD;
        m_helloText.SetActive(true);
        m_doneButton.SetActive(false);
        arr_textureBalls = new GameObject[arr_ballMaterials.Length];
    }
	
	void Update () {
		
	}

    public void ChooseColor(Color color) {
        switch (m_currentStep) {
            case (int)steps.LOAD:
                break;
            case (int)steps.ROOM_COLOR_PICK:
                m_wallController.ChangeColor(color);
                break;
            case (int)steps.BALL_TEXTURE_PICK:
                break;
            case (int)steps.BALL_COLOR_PICK:
                m_currentBall.GetComponent<Ball>().ChangeColor(color);
                break;
            case (int)steps.BALL_SPEED_PICK:
                break;
            case (int)steps.BALL_BOUNCE_PICK:
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
            case (int)steps.BALL_BOUNCE_PICK:
                StepBallBouncePick();
                break;
        }
    }


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
        StartCoroutine(C_StepBallTextureDelete());
        m_currentBall = GameObject.Instantiate(m_ballPrefab);
        m_currentBall.GetComponent<Ball>().Environment_Choose_Create(BALL_START_LOCATION, BALL_START_SCALE_AMOUNT);
        m_currentBall.GetComponent<Renderer>().material = m_chosenTexture;

        m_directions.text = "Choose the color of the ball";

    }

    private void StepBallSpeedPick() {
        wheel.Fold();

        m_directions.text = "Choose the speed of the ball";
    }

    private void StepBallBouncePick() {

        m_directions.text = "Choose the bounciness of the ball";
    }

    /* Spawns all the balls representing texture */
    private IEnumerator C_StepBallTextureSpawn() {
        for (int index = 0; index < arr_ballMaterials.Length; index++) {
            arr_textureBalls[index] = GameObject.Instantiate(
                m_textureBallPrefab, 
                BALL_START_POSITIONS[index],
                Quaternion.identity);
            yield return new WaitForSeconds(BALL_TEXTURE_WAIT_SPAWN);
            arr_textureBalls[index].GetComponent<Renderer>().material = arr_ballMaterials[index];
            arr_textureBalls[index].GetComponent<TextureBall>().SetController(this);
            yield return null;
        }

        yield return null;
    }

    /* Despawns all the balls representing texture */
    private IEnumerator C_StepBallTextureDelete() {
        for (int index = 0; index < arr_ballMaterials.Length; index++) {
            GameObject.Destroy(arr_textureBalls[index]);
            yield return new WaitForSeconds(BALL_TEXTURE_WAIT_DELETE);
        }

        yield return null;
    }

    public void BallTextureCallback(Material material) {
        m_chosenTexture = material;
        m_currentFloatingBall.GetComponent<Renderer>().material = material;
    }
}
