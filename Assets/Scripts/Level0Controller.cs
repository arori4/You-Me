using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Controller for the room in the starting phase*/
public class Level0Controller : MonoBehaviour {

    readonly static Vector3 BALL_START_LOCATION = new Vector3(2, 3, 1.5f);

    public WallMaterialController m_wallController;
    public GameObject m_helloText;
    public GameObject m_doneButton;
    public ColorWheel wheel;
    public GameObject m_ballPrefab;

    private GameObject m_currentBall;

    enum steps {
        LOAD,
        ROOM_COLOR_PICK,
        BALL_COLOR_PICK,
        BALL_TEXTURE_PICK,
        BALL_SPEED_PICK,
        BALL_BOUNCE_PICK
    };
    int m_currentStep;

	void Start () {
        m_currentStep = (int)steps.LOAD;
        m_helloText.SetActive(true);
        m_doneButton.SetActive(false);
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
            case (int)steps.BALL_COLOR_PICK:
                m_currentBall.GetComponent<Ball>().ChangeColor(color);
                break;
            case (int)steps.BALL_TEXTURE_PICK:
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
            case (int)steps.BALL_COLOR_PICK:
                StepBallColorPick();
                break;
            case (int)steps.BALL_TEXTURE_PICK:
                break;
            case (int)steps.BALL_SPEED_PICK:
                break;
            case (int)steps.BALL_BOUNCE_PICK:
                break;
        }
    }


    private void StepLoad() {

    }

    private void StepRoomColorPick() {
        wheel.StartLevel();
        m_doneButton.SetActive(true);
        m_helloText.SetActive(false);
    }

    private void StepBallColorPick() {
        m_currentBall = GameObject.Instantiate(m_ballPrefab);
        m_currentBall.GetComponent<Ball>().Environment_Choose_Create(BALL_START_LOCATION);
    }
    
}
