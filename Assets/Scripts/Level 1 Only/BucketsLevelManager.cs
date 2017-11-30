using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Manages the buckets level
 */
public class BucketsLevelManager : MonoBehaviour {

	//Public Level Manager Instance.
	public static BucketsLevelManager instance;

    // Environment Variables
    public WallMaterialController wallMaterialController;
    
    [Header("Game Variables")]
	public int difficulty; 
	public int current_level;
	public int score;
	public int num_balls;
	public float radius = 3.0f;

    [Header("Game Objects")]
	public Color [] colors;
	public int [] labels;
	public Hashtable labels_map;
	public int num_buckets;
	public int buckets_instantiated;

	public bool repeat_buckets;
    
    [Header("UI")]
	public Text uiText;
	public Button restartButton;

    [Header("Prefabs")]
	public GameObject colorSphere;
	public GameObject bucket;
	public GameObject environment;

	//Private game variable.
	private bool beat_game = false;
    private List<GameObject> arr_buckets = new List<GameObject>();
    private List<GameObject> arr_balls = new List<GameObject>();

	//Use this for initialization
	void Start () {

		//Default level.
		difficulty = 0;
		current_level = 0;

        StartCoroutine(C_Initialize());
        UserDataGathering.instance.Write("Now Playing BucketsLevel at " + System.DateTime.Now);
    }

	void Awake (){
		//If the instance is null, we set the reference.
		if (instance == null) {
			instance = this;
		}
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
        ConfigureDifficulty(difficulty);
        StartLevel(current_level);
        yield return null;
    }

    // Update is called once per frame
    void Update () {
		if (num_balls <= 0 && !beat_game) {
			uiText.text = "You win!";
			restartButton.gameObject.SetActive (true);
		}
	}


	void ConfigureDifficulty(int difficulty) {
		Component[] mesh_colliders = environment.GetComponentsInChildren<MeshCollider> ();

		switch (difficulty) {
		case 0:
			foreach (MeshCollider mc in mesh_colliders) {
				mc.material.bounciness = 0.0f;
				mc.material.dynamicFriction = 0.0f;
				mc.material.staticFriction = 0.0f;
				mc.material.bounceCombine = PhysicMaterialCombine.Average;
				mc.material.frictionCombine = PhysicMaterialCombine.Average;
			}
			break;
		case 1:
			foreach (MeshCollider mc in mesh_colliders) {
				mc.material.bounciness = 1.0f;
				mc.material.dynamicFriction = 0.25f;
				mc.material.staticFriction = 0.1f;
				mc.material.bounceCombine = PhysicMaterialCombine.Maximum;
				mc.material.frictionCombine = PhysicMaterialCombine.Minimum;
			}
			break;
		case 2:
			foreach (MeshCollider mc in mesh_colliders) {
				mc.material.bounciness = 1.0f;
				mc.material.dynamicFriction = 0.25f;
				mc.material.staticFriction = 0.1f;
				mc.material.bounceCombine = PhysicMaterialCombine.Maximum;
				mc.material.frictionCombine = PhysicMaterialCombine.Minimum;
			}
			break;
		}
	}

	private void StartLevel(int level) {
		
		// Todo: Should really be taking something like a JSON as an argument,
		// then extracting its parameters to initialize levels. Levels
		// are currently hardcoded below

        // Set variables
		restartButton.gameObject.SetActive (false);

        // Remove unneded game objects
        foreach(GameObject bucket1 in arr_buckets) {
            if (bucket1 != null) {
                bucket1.GetComponent<BucketBehavior>().ShrinkOut();
            }
        }
        arr_buckets.Clear();
        foreach (GameObject ball in arr_balls) {
            if (ball != null) {
                ball.GetComponent<Ball>().ShrinkOut();
            }
        }
        arr_balls.Clear();

        switch (level) {
		case 0:
			InitializeGame (0, 3, 
                new Color[] { Color.red });
			break;
		case 1:
			InitializeGame (0, 5, 
                new Color[] { Color.red, Color.green});
			break;
		case 2: 
			InitializeGame (0, 8, 
                new Color[] { Color.red, Color.green, Color.blue });
			break;
		case 3:
			InitializeGame (0, 10, 
                new Color[] { Color.red, Color.green, Color.blue, Color.blue }, true);
			break;
		case 4:
			InitializeGame (0, 10, 
                new Color[] { Color.red, Color.red, Color.green, Color.blue, Color.blue }, true);
			break;
		case 5:
			InitializeGame (0, 15, 
                new Color[] { Color.red, Color.red, Color.green, Color.green, Color.blue, Color.blue }, true);
			break;
		default:
			uiText.text = "You beat the game! Congratulations.";
            UserDataGathering.instance.Write("Beat BucketsLevel at " + System.DateTime.Now);
			restartButton.gameObject.SetActive (false);
			beat_game = true;
			return;
		}

        SpawnBalls();
        SpawnBuckets();
	}

	void InitializeGame(int oldScore, int numberBalls, Color [] col_arr, bool rb = false) {
		score = oldScore;
		num_balls = numberBalls;
		colors = col_arr;
		num_buckets = colors.Length;
		buckets_instantiated = 0;
		uiText.text = "Put the balls into the buckets!";
		repeat_buckets = rb;

		if (repeat_buckets) {
			labels_map = new Hashtable ();
			labels = new int[num_buckets];
			int i = 0;

			foreach (Color c in colors) {
				if (!labels_map.ContainsKey (c)) {
					int rand = Random.Range (1, 100);
					while (labels_map.ContainsKey (rand))
						rand = Random.Range (1, 100);

					ArrayList ar = new ArrayList ();
					ar.Add (rand);
					labels_map.Add (c, ar);
					labels[i] = rand;
				} else {
					int rand = Random.Range (1, 100);
					while (labels_map.ContainsKey (rand))
						rand = Random.Range (1, 100);

					ArrayList ar = (ArrayList)labels_map[c];
					ar.Add(rand);
					labels[i] = rand;
				}
				i++;
			}
		}
	}

    private void SpawnBalls() {
        for (int i = 0; i < this.num_balls; i++) {
            Vector3 pos = new Vector3(
                Random.Range(-10, 10),
                Random.Range(2, 5),
                Random.Range(-10, 10));
            GameObject newBall = GameObject.Instantiate(colorSphere, pos, Quaternion.identity);

            // Add to List
            arr_balls.Add(newBall);

            // Set color and material
            newBall.GetComponent<Renderer>().material.color = 
                colors[Random.Range(0, BucketsLevelManager.instance.colors.Length)];

            // Set direction
            newBall.GetComponent<Flier>().direction = new Vector3(
                Random.Range(-1.0f, 1.0f),
                0,
                Random.Range(-1.0f, 1.0f));
            newBall.GetComponent<Flier>().speed = Random.Range(2.0f, 5.0f);
        }
    }

    private void SpawnBuckets() {
        for (int index = 0; index < this.num_buckets; index++) {
            if (repeat_buckets) {
                bucket.GetComponentInChildren<TextMesh>().text = labels[index].ToString();
            }
            else {
                bucket.GetComponentInChildren<TextMesh>().text = "";
            }

            float placement = (float)index / this.num_buckets;
            Vector3 position2 = RandomPosOnCircle(this.transform.position, radius, 0.75f, placement);

            Quaternion q = Quaternion.LookRotation (new Vector3(0.0f, 0.0f, 0.0f) - position2);
            q *= Quaternion.Euler (-90.0f, 180.0f, 0.0f);
            GameObject newBucket = GameObject.Instantiate(bucket, position2, q);
            newBucket.transform.position = position2;

            // Add to list
            arr_buckets.Add(newBucket);

            // Set color
            newBucket.GetComponent<Renderer>().material.color = colors[index];
            newBucket.GetComponentInChildren<BucketTrigger>().SetColor(colors[index]);
        }
    }

    /**
     * Finds a vector on a random part of a circle
     */
	private Vector3 RandomPosOnCircle(Vector3 center , float radius, float height, float placement){
		float ang = placement * 360.0f;
		Vector3 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y + height;
		pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		return pos;
	}

	public void NextLevel() {

		//Update the current level.
		current_level++;

		//Updated Preferences.
		if (PreferencesManager.instance != null) {
			PreferencesManager.instance.level = current_level;
			PreferencesManager.instance.Save ();
		}

		//Start.
		StartLevel (current_level);
	}

}
