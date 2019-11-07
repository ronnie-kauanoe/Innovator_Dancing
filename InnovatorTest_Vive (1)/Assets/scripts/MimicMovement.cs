using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicMovement : MonoBehaviour {

    public bool mirror;

    public Transform headReference;
    public Transform leftWandReference;
    public Transform rightWandReference;
    public Transform waistReference;
    public Transform leftFootReference;
    public Transform rightFootReference;

	// Use this for initialization
	void Start () {
        //Initial calls to VRPN for puck 0, 1, 2 entries
        for (int i = 0; i < 3; i++) {
            VRPN.vrpnTrackerPos("CC_FLAT_PUCK" + i + "@" + CC_CONFIG.innovatorIP, 0);
            VRPN.vrpnTrackerQuat("CC_FLAT_PUCK" + i + "@" + CC_CONFIG.innovatorIP, 0);
        }

        headReference = transform.GetChild(0).gameObject.transform;
        leftWandReference = transform.GetChild(1).gameObject.transform;
        rightWandReference = transform.GetChild(2).gameObject.transform;
        waistReference = transform.GetChild(3).gameObject.transform;
        leftFootReference = transform.GetChild(4).gameObject.transform;
        rightFootReference = transform.GetChild(5).gameObject.transform;
        
    }

    // Update is called once per frame
    void Update () {
        //Head tracking
        Transform source = CC_CANOE.HeadGameObject().transform;
        headReference.transform.position = convertPosition(source.position);
        headReference.transform.localRotation = convertRotation(source.rotation);

        //Left wand tracking
        source = CC_CANOE.WandGameObject(Wand.Left).transform;
        leftWandReference.transform.position = convertPosition(source.position);
        leftWandReference.transform.localRotation = convertRotation(source.rotation);

        //Right wand tracking
        source = CC_CANOE.WandGameObject(Wand.Right).transform;
        rightWandReference.transform.position = convertPosition(source.position);
        rightWandReference.transform.localRotation = convertRotation(source.rotation);

        //Waist tracking
        waistReference.transform.localPosition = convertPuckPosition(waistReference.transform.localPosition, 0);
        waistReference.transform.localRotation = convertPuckRotation(0);

        //Left foot tracking
        leftFootReference.transform.localPosition = convertPuckPosition(leftFootReference.transform.localPosition, 1);
        leftFootReference.transform.localRotation = convertPuckRotation(1);

        //Right foot tracking
        rightFootReference.transform.localPosition = convertPuckPosition(rightFootReference.transform.localPosition, 2);
        rightFootReference.transform.localRotation = convertPuckRotation(2);
    }

    private Vector3 convertPosition(Vector3 source) {
        if (mirror) {
            return new Vector3(source.x + transform.position.x,
                source.y + transform.position.y,
                -source.z + transform.position.z);
        }
        return source;
    }

    private Quaternion convertRotation(Quaternion source) {
        if (mirror) {
            return new Quaternion(source.x, source.y, -source.z, -source.w);
        }
        return source;
    }

    private Vector3 convertToLeftHandPosition(Vector3 position) {
        return new Vector3(position.x, position.y, -position.z);
    }

    private Quaternion convertToLeftHandRotation(Quaternion quaternion) {
        return new Quaternion(-quaternion.x, -quaternion.y, quaternion.z, quaternion.w);
    }
    
    private Vector3 convertPuckPosition(Vector3 currentPos, int puckNum) {
        Vector3 rawVRPNpos = VRPN.vrpnTrackerPos("CC_FLAT_PUCK" + puckNum + "@" + CC_CONFIG.innovatorIP, 0);
        if ((rawVRPNpos.x != -505) || (rawVRPNpos.y != -505) || (rawVRPNpos.z != -505)) {
            return convertPosition(convertToLeftHandPosition(rawVRPNpos));
        }
        return currentPos;
    }

    private Quaternion convertPuckRotation(int puckNum) {
        return convertRotation(
            convertToLeftHandRotation(VRPN.vrpnTrackerQuat("CC_FLAT_PUCK" + puckNum + "@" + CC_CONFIG.innovatorIP, 0) * Quaternion.AngleAxis(90, Vector3.left) * Quaternion.AngleAxis(180, Vector3.up))
            );
    }
}
