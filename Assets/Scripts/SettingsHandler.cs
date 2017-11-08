using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// SettingsHandler pushes any changes in PlayerPreferences to and from here.
// Each setting has a script with SettingsHandler, with a slider and number of elements.
// This is a class that works for each additional setting.
// However, we add the cases in here.
public class SettingsHandler : MonoBehaviour {

	// Public Variables.
	public string m_Setting_Name = "Name";
	public List<string> m_Values;

	// Private GameObjects.
	private GameObject m_Setting;
	private GameObject m_Current_Setting;
	private GameObject m_Selector;

	// Private variables.
	float m_SliderVal = 0.0f;

	// Use this for initialization
	void Start () {

		// Get all the GameObjects.
		m_Setting = this.transform.Find ("Setting").gameObject;
		m_Current_Setting = this.transform.Find ("Current Setting").gameObject;
		m_Selector = this.transform.Find ("Selector").gameObject;

		// Set the name of the current setting.
		m_Setting.GetComponent<Text> ().text = m_Setting_Name;

		// Resize the slider to make it the same as number of settings.
		m_Selector.GetComponent<Slider> ().maxValue = m_Values.Count - 1;
		if (m_Selector.GetComponent<Slider> ().maxValue < 0) {
			m_Selector.GetComponent<Slider> ().maxValue = 0;
		}

		// Load user preference settings.
		LoadSettings ();
		m_SliderVal = m_Selector.GetComponent<Slider> ().value;// Call to load only once.

	}

	// Update is called once per frame
	void Update () {

		// Get the slider value.
		float prevVal = m_SliderVal;
		m_SliderVal = m_Selector.GetComponent<Slider>().value;

		// Change the preview to be the selected setting.
		m_Current_Setting.GetComponent<Text> ().text = SliderText(m_SliderVal);

		// Auto-Save
		if (prevVal != m_SliderVal) {
			SaveSettings ();
		}

	}

	// Return the text based on the slider.
	string SliderText(float slider){
		for (int i = 0; i < m_Values.Count; i++) {
			if (i == slider) {
				return m_Values [i];
			}
		}
		// Returns null if none is available or an error occurred.
		return "null"; 
	}

	// Load the current setting from PlayerPrefs.
	public void LoadSettings(){
		if (PreferencesManager.instance == null)
			return;

		// Depending on the name, set the respective setting.
		switch (m_Setting_Name) {
		case "Level": 
			m_Selector.GetComponent<Slider> ().value = PreferencesManager.instance.level;
			break;
		case "Difficulty":
			m_Selector.GetComponent<Slider> ().value = PreferencesManager.instance.difficulty;
			break;
		case "Tone":
			m_Selector.GetComponent<Slider> ().value = PreferencesManager.instance.tune;
			break;
		case "Volume":
			m_Selector.GetComponent<Slider> ().value = PreferencesManager.instance.volume;
			break;
		}
	}

	// Save the current setting.
	public void SaveSettings(){
		if (PreferencesManager.instance == null)
			return;

		// Depending on the name, set the respective setting.
		switch (m_Setting_Name) {
		case "Level": 
			PreferencesManager.instance.level = m_SliderVal;
			break;
		case "Difficulty":
			PreferencesManager.instance.difficulty = m_SliderVal;
			break;
		case "Tone":
			PreferencesManager.instance.tune = m_SliderVal;
			break;
		case "Volume":
			PreferencesManager.instance.volume = m_SliderVal;
			break;
		}

		// Explicitly Save.
		PreferencesManager.instance.Save ();
	}
}  