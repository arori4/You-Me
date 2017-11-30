using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class UserDataGathering : MonoBehaviour {

    private String textFile = "UserData";
    private StreamWriter file;
    public static UserDataGathering instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            file = new StreamWriter(textFile + ".txt", true);
        }
        // If the instance is set and is not this, we destroy the gameObject attached.
        else if (instance != this) {
            Destroy(gameObject);
        }

        // Do not destroy when loading new scenes.
        DontDestroyOnLoad(gameObject);
    }
    
    void Start () {
	}
	
	void Update () {
		
	}

    public void Write(string textToWrite) {
        file.WriteLine(textToWrite);
    }

    private void OnDestroy() {
        file.Close();
    }

}
