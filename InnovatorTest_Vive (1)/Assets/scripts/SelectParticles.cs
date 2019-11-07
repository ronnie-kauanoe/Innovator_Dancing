using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectParticles : MonoBehaviour {

    public Wand wand;
    public Vector3 offset;
    private Transform wandTransform;
    private GameObject upParticles;
    private GameObject downParticles;
    private GameObject leftParticles;
    private GameObject rightParticles;
    private GameObject chosenParticles;
    private GameObject tempParticles;

	// Use this for initialization
	void Start () {
        wandTransform = CC_CANOE.WandTransform(wand);
        upParticles = transform.GetChild(0).gameObject;
        downParticles = transform.GetChild(1).gameObject;
        leftParticles = transform.GetChild(2).gameObject;
        rightParticles = transform.GetChild(3).gameObject;
        
	}
	
	// Update is called once per frame
	void Update () {

        float angle = Mathf.Atan2(CC_INPUT.GetAxis(wand, WandAxis.YAxis), CC_INPUT.GetAxis(wand, WandAxis.XAxis));
        if (angle < 0f) {
            angle += 2 * Mathf.PI;
        }

        angle = angle * (180f / Mathf.PI);

        if (CC_INPUT.GetButtonDown(wand, WandButton.TrackpadClick)) {
            if ((angle >= 45f) && (angle <= 135f)) {
                Debug.Log("Up object selected");
                chosenParticles = upParticles;
            } else if ((angle >= 225f) && (angle <= 315f)) {
                Debug.Log("Down object selected");
                chosenParticles = downParticles;
            } else if ((angle > 135f) && (angle < 225f)) {
                Debug.Log("Left object selected");
                chosenParticles = leftParticles;
            } else if (((angle >= 0) && (angle < 45)) || ((angle > 315) && (angle <= 360f))) {
                Debug.Log("Right object selected");
                chosenParticles = rightParticles;
            }
            tempParticles = Instantiate(chosenParticles, wandTransform.position, wandTransform.rotation);
        }
        if (CC_INPUT.GetButtonPress(wand, WandButton.TrackpadClick)) {
            tempParticles.transform.position = wandTransform.position;
        }

        if (CC_INPUT.GetButtonUp(wand, WandButton.TrackpadClick)) {
            tempParticles.AddComponent<DestroyProjectile>();
        }
    }
}
