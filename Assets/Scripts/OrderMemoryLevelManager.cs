using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Manages the OrderMemory Level
 * 
 * The OrderMemory Level consists of 5 stages
 * Each stage, the player will be presented with a group of letters, from 3 to 8.
 * The letters will blink in a pattern.
 * The player must click on the letters to make that pattern.
 * If successful, the level increases.
 * Otherwise, the pattern plays again.
 */
public class OrderMemoryLevelManager : MonoBehaviour {

    // Environment Variables
    public WallMaterialController wallMaterialController;

    [Header("Game Variables")]
    public int difficulty;
    public int current_level;
    public int score;

    [Header("UI")]
    public Text uiText;
    public Button restartButton;

    [Header("Prefabs")]
    public GameObject[] arr_letters;
    public Color naturalColor;
    public Color activatedColor; // lerp between the two
    public AudioClip[] arr_clips;

    // Game Objects
    private List<GameObject> arr_lettersToAdd = new List<GameObject>();
    private List<GameObject> arr_currentLetters = new List<GameObject>();
    private List<int> arr_order = new List<int>();
    private List<int> arr_chosenOrder = new List<int>();
    private bool beat_game = false;

    void Start () {
        //Default level.
        difficulty = 0;
        current_level = 0;

        StartCoroutine(C_Initialize());

        UserDataGathering.instance.Write("Now Playing OrderMemoryLevel at " + System.DateTime.Now);
    }


    /*
     * On a delay, we load and change elements of the scene to match the preferences
     * chosen in the beginning. This is necessary for both performance reasons and for
     * waiting for all gameObjects to be loaded into the scene.
     * 
     * This is a crude impleentation, but by my marks, provides the least performance
     * penalty and least stuttering by allowing user updates while the scene loads.
     */
    IEnumerator C_Initialize() {

        yield return new WaitForSeconds(0.1f); // random, small number chosen

        //Load settings if level and difficulty are different.
        if (PreferencesManager.instance != null) {
            difficulty = (int)PreferencesManager.instance.difficulty;
            current_level = (int)PreferencesManager.instance.level;

            // Change the color of the room to the player preference
            wallMaterialController.ChangeColorInstant(PreferencesManager.instance.roomColor);
            yield return null;

        }
        yield return null;

        //Configure difficulty and start level.
        StartLevel(current_level);
        yield return null;
    }

    private void StartLevel(int level) {
        StartCoroutine(C_StartLevel(level));
    }

    private IEnumerator C_StartLevel(int level) {

        // Set variables
        restartButton.gameObject.SetActive(false);

        // Remove unneded game objects
        foreach (GameObject letter1 in arr_currentLetters) {
            if (letter1 != null) {
                letter1.GetComponent<OrderMemoryLevelLetter>().ShrinkOut();
            }
        }
        // Clear arrays
        arr_currentLetters.Clear();
        arr_lettersToAdd.Clear();
        arr_order.Clear();
        arr_chosenOrder.Clear();

        yield return null;

        // change level properties based on level
        switch (level) {
            case 0:
                InitializeGame(3);
                SpawnLetters();
                break;
            case 1:
                InitializeGame(4);
                SpawnLetters();
                break;
            case 2:
                InitializeGame(5);
                SpawnLetters();
                break;
            case 3:
                InitializeGame(6);
                SpawnLetters();
                break;
            case 4:
                InitializeGame(7);
                SpawnLetters();
                break;
            case 5:
                InitializeGame(8);
                SpawnLetters();
                break;
            default:
                uiText.text = "You beat the game! Congratulations.";
                UserDataGathering.instance.Write("Beat OrderMemoryLevel at " + System.DateTime.Now);
                restartButton.gameObject.SetActive(false);
                beat_game = true;
                break;
        }

        yield return new WaitForSeconds(1.2f);

        // First initial play
        PlayOrder();

    }

    public void InitializeGame(int numLetters) {
        List<int> numbers = new List<int>();
        
        for (int index = 0; index < numLetters; index++) {
            numbers.Add(index);
        }

        for (int index = 0; index < numLetters; index++) {
            // Create letters, randomly
            arr_lettersToAdd.Add(arr_letters[Random.Range(0, arr_letters.Length - 1)]);

            // Create order, randomly
            int indexToAdd = Random.Range(0, numbers.Count - 1);
            arr_order.Add(numbers[indexToAdd]);
            numbers.RemoveAt(indexToAdd);
        }

        // Check work
        for (int index = 0; index < numLetters; index++) {
            print(arr_order[index]);
        }

    }

    private void SpawnLetters() {
        StartCoroutine(C_SpawnLetters(1));
    }

    private IEnumerator C_SpawnLetters(float time) {

        for (int index = 0; index < arr_lettersToAdd.Count; index++) {
            // Calculate position
            Vector3 newPosition = new Vector3(index - arr_lettersToAdd.Count / 2, 2, 3);

            GameObject newLetter = GameObject.Instantiate(arr_lettersToAdd[index]);
            newLetter.transform.position = newPosition;
            arr_currentLetters.Add(newLetter);

            // Set variables
            newLetter.GetComponent<OrderMemoryLevelLetter>().SetNumber(index);
            newLetter.GetComponent<OrderMemoryLevelLetter>().SetManager(this);
            newLetter.GetComponent<OrderMemoryLevelLetter>().SetAudioClip(arr_clips[index]);
            newLetter.GetComponent<OrderMemoryLevelLetter>().SetColors(naturalColor, activatedColor);

            // TODO: expand newLetter

            yield return new WaitForSeconds(time / arr_lettersToAdd.Count);
        }

    }

    void Update () {
		
	}

    /**
     * Plays the letters in order
     */
    private void PlayOrder() {
        StartCoroutine(C_PlayOrder(0.5f));
    }

    private IEnumerator C_PlayOrder(float interval) {

        for (int index = 0; index < arr_currentLetters.Count; index++) {
            arr_currentLetters[arr_order[index]].GetComponent<OrderMemoryLevelLetter>().Ding();

            yield return new WaitForSeconds(interval);
        }

    }

    /**
     * Callback from OrderMemoryLevelLetter.
     * Input is the index of the letter
     */
    public void ChosenCallback(int chosenIndex) {

        // Ding is handled by OrderMemoryLevelLetter

        // Add chosen index to order
        arr_chosenOrder.Add(chosenIndex);

        // Check if order is full
        if (arr_chosenOrder.Count == arr_order.Count) {
            // Check if order is correct
            bool isCorrect = true;
            for (int index = 0; index < arr_order.Count; index++) {
                isCorrect = isCorrect && (arr_chosenOrder[index] == arr_order[index]);
            }

            if (isCorrect) {
                uiText.text = "Correct!";
                restartButton.gameObject.SetActive(true);
                UserDataGathering.instance.Write("Correct Order " + System.DateTime.Now);
            }
            else {
                PlayOrder();
                uiText.text = "Not quite right! Try again.";
                UserDataGathering.instance.Write("Incorrect Order " + System.DateTime.Now);
            }

            // Clear order
            arr_chosenOrder.Clear();

        }

    }

    public void NextLevel() {

        //Update the current level.
        current_level++;

        //Updated Preferences.
        if (PreferencesManager.instance != null) {
            PreferencesManager.instance.level = current_level;
            PreferencesManager.instance.Save();
        }

        //Start.
        StartLevel(current_level);
    }
}
