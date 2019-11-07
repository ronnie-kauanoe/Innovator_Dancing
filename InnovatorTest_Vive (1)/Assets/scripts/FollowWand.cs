using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWand : MonoBehaviour {

    public Wand wandToFollow;
    public bool mimicPosition;
    public bool mimicRotation;

    public Vector3 positionOffset;

    private Transform wandTransform;
    private TextMesh ballInfo;

	// Use this for initialization
	void Start () {
        wandTransform = CC_CANOE.WandGameObject(wandToFollow).transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (mimicPosition) {
            transform.position = new Vector3(wandTransform.position.x + positionOffset.x,
                wandTransform.position.y + positionOffset.y,
                wandTransform.position.z + positionOffset.z);
        }

        if (mimicRotation) {
            transform.rotation = wandTransform.rotation;
        }
    }
}
