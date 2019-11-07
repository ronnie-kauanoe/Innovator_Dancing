using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParent : MonoBehaviour {

    private CharacterController controller;
    private Vector3 ccPosition;
    private Vector3 originalPos;
    private Vector3 headPos;

	// Use this for initialization
	void Start () {
        controller = CC_CANOE.CanoeCharacterController();
        originalPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        headPos = CC_CANOE.HeadGameObject().transform.position;

        ccPosition = controller.center;
        
        transform.position = new Vector3(headPos.x + originalPos.x, 0, headPos.z + originalPos.z);
	}
}
