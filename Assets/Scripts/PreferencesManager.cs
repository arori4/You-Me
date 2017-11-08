using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages preferences
 * Every item seen in the inspector can be changed, and will be saved.
 * This script can be seen as the 'glue' that runs the game between the levels.
 * 
 * Script acts as a singleton when in the scene.
 * 
 * Special case: texture is handled by index. arr_textures must have the same
 *               textures in the same order as Level 0 Scene.
 */
public class PreferencesManager : MonoBehaviour {

	// Static reference to the instance of our Preferences Manager.
	public static PreferencesManager instance;

	// Player data
	public string player_Name = "";
	public int player_Age = 0;
	public float player_Score = 0.0f;
    private static string KEY_NAME = "PLAYER_NAME";
    private static string KEY_AGE = "PLAYER_AGE";
    private static string KEY_SCORE = "PLAYER_SCORE";


    // Environment data
    public float level = 0.0f;
    public float difficulty = 0.0f;
    public Color roomColor;
    public Color ballColor;
    public Material ballMaterial; private int ballMaterialIndex;
    public float ballSpeed;
    public float heartbeatSpeed;
    public Material[] arr_materials;
    private static string KEY_LEVEL = "KEY_LEVEL";
    private static string KEY_DIFFICULTY = "KEY_DIFFICULTY";
    private static string KEY_ROOM_COLOR = "ROOM_COLOR";
    private static string KEY_BALL_COLOR = "BALL_COLOR";
    private static string KEY_BALL_TEXTURE = "BALL_TEXTURE";
    private static string KEY_BALL_SPEED = "BALL_SPEED";
    private static string KEY_HEARTBEAT_SPEED = "HEARTBEAT_SPEED";

    // Heuristics
    public float numSuccesses;
    public float numFails;
    public float time;
    public float startTime;
    public float endTime;

    // Settings
    public float tune = 0.0f;
    public float volume = 0.0f;
    //Settings
    private static string KEY_SETTINGS_TUNE = "KEY_SETTINGS_TUNE";
    private static string KEY_SETTINGS_VOLUME = "KEY_SETTINGS_VOLUME";

    // Unused keys
    private static string KEY_AVATAR_COLOR = "AVATAR COLOR";



	// Explicitly Load on start. This allows us to load from playerprefs immediately and fill these values.
	void Start () {
		Load ();
	}

	// Awake is called when the script instance is loaded.
	void Awake () {
		// If the instance is null, we set the reference.
		if (instance == null) {
			instance = this;
		} 
		// If the instance is set and is not this, we destroy the gameObject attached.
		else if (instance != this) {
			Destroy (gameObject);
		}
		// Do not destroy when loading new scenes.
		DontDestroyOnLoad(gameObject);
	}

	// Load from PlayerPrefs.
	public void Load() {
		Debug.Log ("PreferencesManger: Loading data...\n");

		if (PlayerPrefs.HasKey(KEY_NAME)) player_Name = PlayerPrefs.GetString(KEY_NAME);
		if (PlayerPrefs.HasKey(KEY_AGE)) player_Age = PlayerPrefs.GetInt(KEY_AGE);
		if (PlayerPrefs.HasKey(KEY_SCORE)) player_Score = PlayerPrefs.GetFloat(KEY_SCORE);

		if (PlayerPrefs.HasKey(KEY_LEVEL)) level = PlayerPrefs.GetFloat(KEY_LEVEL);
		if (PlayerPrefs.HasKey(KEY_DIFFICULTY)) difficulty = PlayerPrefs.GetFloat(KEY_DIFFICULTY);
        if (PlayerPrefs.HasKey(KEY_ROOM_COLOR)) roomColor = StringToColor(PlayerPrefs.GetString(KEY_ROOM_COLOR));
        if (PlayerPrefs.HasKey(KEY_BALL_COLOR)) roomColor = StringToColor(PlayerPrefs.GetString(KEY_BALL_COLOR));
        if (PlayerPrefs.HasKey(KEY_BALL_TEXTURE)) ballMaterial = arr_materials[PlayerPrefs.GetInt(KEY_BALL_TEXTURE)];
        if (PlayerPrefs.HasKey(KEY_BALL_SPEED)) ballSpeed = PlayerPrefs.GetFloat(KEY_BALL_SPEED);
        if (PlayerPrefs.HasKey(KEY_HEARTBEAT_SPEED)) ballSpeed = PlayerPrefs.GetFloat(KEY_HEARTBEAT_SPEED);

        //Settings
        if (PlayerPrefs.HasKey(KEY_SETTINGS_TUNE)) tune = PlayerPrefs.GetFloat(KEY_SETTINGS_TUNE);
		if (PlayerPrefs.HasKey(KEY_SETTINGS_VOLUME)) volume = PlayerPrefs.GetFloat(KEY_SETTINGS_VOLUME);
	}


	// Save the data found in each variable to the specific key in PlayerPrefs.
	public void Save() {
		Debug.Log ("PreferencesManger: Saving data...\n");

		// Set data.
		PlayerPrefs.SetString (KEY_NAME, player_Name);
		PlayerPrefs.SetInt (KEY_AGE, player_Age);
		PlayerPrefs.SetFloat (KEY_SCORE, player_Score);

        // Level
		PlayerPrefs.SetFloat(KEY_LEVEL, level);
		PlayerPrefs.SetFloat(KEY_DIFFICULTY, difficulty);
        PlayerPrefs.SetString(KEY_ROOM_COLOR, roomColor.ToString());
        PlayerPrefs.SetString(KEY_BALL_COLOR, ballColor.ToString());
        PlayerPrefs.SetFloat(KEY_BALL_TEXTURE, ballMaterialIndex);
        PlayerPrefs.SetFloat(KEY_BALL_SPEED, ballSpeed);
        PlayerPrefs.SetFloat(KEY_HEARTBEAT_SPEED, heartbeatSpeed);

        // Settings
        PlayerPrefs.SetFloat (KEY_SETTINGS_TUNE, tune);
		PlayerPrefs.SetFloat (KEY_SETTINGS_VOLUME, volume);

		// Explicit Save.
		PlayerPrefs.Save();
	}

    /*
     * Changes a string back into a color. 
     * Essentially a deserializer, because toString serializes the color
     */
    private Color StringToColor(string colorString) {

        Debug.Log(colorString);

        // Remove the header and brackets
        colorString = colorString.Replace("RGBA(", "");
        colorString = colorString.Replace(")", "");

        // Get the individual values (red green blue and alpha)
        var strings = colorString.Split(","[0]);

        Color returnColor;
        returnColor = Color.black;
        for (var i = 0; i < 4; i++) {
            returnColor[i] = System.Single.Parse(strings[i]);
        }

        return returnColor;
    }

}
