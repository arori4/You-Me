using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Generic behaviour for a ball.
 * 
 * Copied from previous collaborator, with a lot of refactoring.
 */
public class Ball : MonoBehaviour {
    
    private readonly static float POPUP_DURATION = 0.5f;
    private readonly static float COLOR_CHANGE_DURATION = 0.25f;

    public string m_text;
    public float m_bounciness;
    public float m_speed;
    public bool m_pickedUp;

    private Material m_material;
    private Rigidbody rb;

    void Start () {
        // Get Components
        rb = GetComponent<Rigidbody>();
        m_material = GetComponent<Renderer>().material;

        //Set ball variables according to difficulty.
        SetBouncinessAndMovement(BucketsLevelManager.instance.difficulty);
    }
	
	void Update () {
		
	}

    private void FixedUpdate() {
        // Keeps the ball a set radius away from the user.
        if (m_pickedUp && BucketsLevelManager.instance != null) {
            Vector3 direction = this.transform.position - Camera.main.transform.position;
            direction.Normalize();
            this.transform.position = (direction * BucketsLevelManager.instance.radius) + Camera.main.transform.position;
        }

        // Keeps the ball always facing the user
        // TODO: rotate the correct way, it's not doing it correctly.
        if (m_pickedUp) {
            Quaternion q = Quaternion.identity * Quaternion.Inverse(Camera.main.transform.rotation);
            rb.MoveRotation(q);
        }
    }

    void SetBouncinessAndMovement(int difficulty) {
        Component[] mesh_colliders = GetComponents<SphereCollider>();

        switch (difficulty) {
            case 0:
                foreach (SphereCollider mc in mesh_colliders) {
                    mc.material.bounciness = 1.0f;
                    mc.material.dynamicFriction = 0.0f;
                    mc.material.staticFriction = 0.0f;
                    mc.material.bounceCombine = PhysicMaterialCombine.Average;
                    mc.material.frictionCombine = PhysicMaterialCombine.Average;
                }
                break;
            case 1:
                foreach (SphereCollider mc in mesh_colliders) {
                    mc.material.bounciness = 1.0f;
                    mc.material.dynamicFriction = 0.25f;
                    mc.material.staticFriction = 0.1f;
                    mc.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    mc.material.frictionCombine = PhysicMaterialCombine.Minimum;
                }
                break;
            case 2:
                foreach (SphereCollider mc in mesh_colliders) {
                    mc.material.bounciness = 1.0f;
                    mc.material.dynamicFriction = 0.25f;
                    mc.material.staticFriction = 0.1f;
                    mc.material.bounceCombine = PhysicMaterialCombine.Maximum;
                    mc.material.frictionCombine = PhysicMaterialCombine.Minimum;
                }
                rb.velocity = (new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), Random.Range(-5, 5)));
                break;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        GetComponent<Rigidbody>().velocity *= -m_bounciness; // causes bouncing

    }

    public void PickedUp() {
        this.transform.parent = Camera.main.transform;
        m_pickedUp = true;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.velocity = new Vector3(0, 0, 0);

        // make sure it stops moving
        GetComponent<Flier>().moving = false;
    }

    public void LetGo() {
        this.transform.parent = null;
        m_pickedUp = false;
        rb.useGravity = true;
        rb.freezeRotation = false;

        // make sure it starts moving
        GetComponent<Flier>().moving = true;
    }

    /* A special method to enlarge the ball for Environment_0 */
    public void Environment_Choose_Create(Vector3 position, float scale) {
        transform.position = position;
        StartCoroutine(C_Scale(0.0f, scale, POPUP_DURATION));
    }

    public void Shrink(float duration, float startScale) {
        StartCoroutine(C_Scale(startScale, 0.0f, duration));
    }

    public void ChangeColor(Color color) {
        StartCoroutine(C_changeColor(color, COLOR_CHANGE_DURATION));
    }

    public void ChangeTexture(Texture texture) {
        m_material.mainTexture = texture;
    }

    /** Changes the color, gradually, of the walls
     */
    private IEnumerator C_changeColor(Color newColor, float duration) {
        // get current color
        Color oldColor = m_material.color;
        Color currentColor = oldColor;

        // set duration
        float interval = 1.0f / duration;
        yield return null;

        // linerar interpolation to the new color
        float index = 0;
        while (index < 1) {
            currentColor = Color.Lerp(oldColor, newColor, index);
            index += Time.deltaTime * interval;
            m_material.color = currentColor;
            yield return null;
        }

        // clamp color at end
        currentColor = newColor;
        m_material.color = currentColor;

    }

    /* Animation to enlarge/reduce */
    private IEnumerator C_Scale(float startScale, float endScale, float duration) {

        // Create start and end vectors
        float index = 0;
        Vector3 startScaleVector = new Vector3(startScale, startScale, startScale);
        Vector3 endScaleVector = new Vector3(endScale, endScale, endScale);

        // Create intervals
        float interval = 1.0f / duration;
        yield return null;

        // Animation loop
        while (index < 1.0f) {
            index += interval * Time.deltaTime;
            gameObject.transform.localScale = Vector3.Lerp(startScaleVector, endScaleVector, index);
            yield return null;
        }

        // End clamp for consistency
        gameObject.transform.localScale = endScaleVector;

    }

    public void ShrinkOut() {
        StartCoroutine(C_ShrinkOut(1));
    }

    private IEnumerator C_ShrinkOut(float time) {

        // Set variables
        float increment = 1.0f / time;
        float total = 0;
        Vector3 originalScale = transform.localScale;
        Vector3 target = new Vector3(0, 0, 0);
        yield return null;

        // Shrink
        while (total < 1) {
            total += increment * Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, target, total);
            yield return null;
        }

        // Finally, destroy object
        Destroy(this);
    }

}
