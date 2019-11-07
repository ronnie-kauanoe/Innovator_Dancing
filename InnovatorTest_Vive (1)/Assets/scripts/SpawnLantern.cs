using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLantern : MonoBehaviour {

    public Wand wand;
    public GameObject upObject;
    public GameObject downObject;
    public GameObject leftObject;
    public GameObject rightObject;
    private GameObject lanternObj;
    public float verticalOffset = 0.1f;
    public float releaseForce = 0.001f;
    private GameObject lantern;
    private WandButton chosenButton;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Transform wandTransform = CC_CANOE.WandTransform(wand);
        Vector3 offset = new Vector3(wandTransform.position.x,
            wandTransform.position.y + verticalOffset,
            wandTransform.position.z);

        float angle = Mathf.Atan2(CC_INPUT.GetAxis(wand, WandAxis.YAxis), CC_INPUT.GetAxis(wand, WandAxis.XAxis));
        if (angle < 0f) {
            angle += 2 * Mathf.PI;
        }

        angle = angle * (180f / Mathf.PI);

        if (CC_INPUT.GetButtonDown(wand, WandButton.TrackpadClick)) {
            if ((angle >= 45f) && (angle <= 135f)) {
                Debug.Log("Up object selected");
                lanternObj = upObject;
                chosenButton = WandButton.Up;
            } else if ((angle >= 225f) && (angle <= 315f)) {
                Debug.Log("Down object selected");
                lanternObj = downObject;
                chosenButton = WandButton.Down;
            } else if ((angle > 135f) && (angle < 225f)) {
                Debug.Log("Left object selected");
                lanternObj = leftObject;
                chosenButton = WandButton.Left;
            } else if (((angle >= 0) && (angle < 45)) || ((angle > 315) && (angle <= 360f))) {
                Debug.Log("Right object selected");
                lanternObj = rightObject;
                chosenButton = WandButton.Right;
            }
        }

        if (CC_INPUT.GetButtonDown(wand, WandButton.TrackpadClick)) {
            lanternObj.transform.localScale = Vector3.zero;
            Vector3 rotation = lanternObj.transform.eulerAngles;
            Quaternion quat = Quaternion.identity;
            lantern = Instantiate(lanternObj, offset, quat);
            lantern.gameObject.SetActive(true);
            lantern.transform.position = offset;
        } else if (CC_INPUT.GetButtonPress(wand, WandButton.TrackpadClick)) {
            lantern.transform.position = offset;
        } else if (CC_INPUT.GetButtonUp(wand, WandButton.TrackpadClick)) {
            lantern.GetComponent<Rigidbody>().isKinematic = false;
            lantern.GetComponent<Rigidbody>().AddForce(new Vector3(0, releaseForce, 0));
            lantern.GetComponent<Lantern>().startExpiring = true;
        }
    }
}
