using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    
	void Start () {
		
	}
	
	void Update () {
		
	}

    public void StartLevel0() {
        SceneManager.LoadSceneAsync("0_Beginning");
        Debug.Log("Gk");
    }

    public void StartLevel1() {
        SceneManager.LoadSceneAsync("1_Buckets");
    }

    public void StartLevel2() {
        SceneManager.LoadSceneAsync("2_OrderMemory");
    }

    public void StartLevelMainMenu() {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
