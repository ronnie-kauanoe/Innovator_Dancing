using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorTracking : MonoBehaviour {

    public GameObject objToMirror;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = new Vector3(objToMirror.transform.position.x,
            objToMirror.transform.position.y,
            -objToMirror.transform.position.z);

        transform.localRotation = Quaternion.Euler(180f - objToMirror.transform.localEulerAngles.x,
            -objToMirror.transform.localEulerAngles.y,
            objToMirror.transform.localEulerAngles.z);

        transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y,
            transform.localRotation.z, -transform.localRotation.w);
	}
}
