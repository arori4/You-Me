using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBall : MonoBehaviour {

    private Level0Controller m_controller;

    public void SetController(Level0Controller controller) {
        m_controller = controller;
    }

    // Sends back the material to the controller
    public void ChooseCallback() {
        m_controller.BallTextureCallback(GetComponent<Renderer>().material);
    }

}
