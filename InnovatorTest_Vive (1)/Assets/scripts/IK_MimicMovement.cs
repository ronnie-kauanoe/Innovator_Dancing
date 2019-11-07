using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_MimicMovement : MonoBehaviour {

    public GameObject leftHandGoal;
    public GameObject rightHandGoal;

    public bool reverseX;
    public bool reverseZ;
    public bool mirror; 

    private Animator animator;

    private CharacterController controller;

    private Transform parentTransform;
    private Transform headTransform;
    private Transform leftHandTransform;
    private Transform rightHandTransform;

    private Vector3 offset;

    private Transform leftMirrorGoal;
    private Transform rightMirrorGoal;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        controller = CC_CANOE.CanoeCharacterController();

        parentTransform = transform.GetComponentInParent<Transform>();
        headTransform = CC_CANOE.HeadGameObject().gameObject.transform;
        leftHandTransform = CC_CANOE.WandGameObject(Wand.Left).gameObject.transform;
        rightHandTransform = CC_CANOE.WandGameObject(Wand.Right).gameObject.transform;

        //Initial call to chest puck
        VRPN.vrpnTrackerQuat("CC_FLAT_PUCK0@" + CC_CONFIG.innovatorIP, 0);

        //Brings model out of ground
        offset = new Vector3(0, 2f * transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().bounds.extents.x, 0);
        transform.localPosition = offset;

        leftMirrorGoal = transform.parent.GetChild(1).transform;
        rightMirrorGoal = transform.parent.GetChild(2).transform;
    }

    private void OnAnimatorIK(int layerIndex) {

        transform.localPosition = new Vector3(controller.center.x, offset.y - 0.1f, controller.center.z);

        if (animator) {
            //Head movement
            animator.SetBoneLocalRotation(HumanBodyBones.Head, convertHeadRotation(headTransform.rotation));

            //Spine movement
            Quaternion spineRotation = VRPN.vrpnTrackerQuat("CC_FLAT_PUCK0@" + CC_CONFIG.innovatorIP, 0)
                * Quaternion.AngleAxis(-90, Vector3.up) * Quaternion.AngleAxis(90, Vector3.forward);
            spineRotation = Quaternion.Euler(90f + spineRotation.eulerAngles.y, spineRotation.eulerAngles.x,
                90f - spineRotation.eulerAngles.z);
            if (mirror) {
                spineRotation = Quaternion.Euler(spineRotation.eulerAngles.x,
                    -spineRotation.eulerAngles.y,
                    spineRotation.eulerAngles.z);
            }
            animator.SetBoneLocalRotation(HumanBodyBones.Spine, spineRotation);

            //Hip movement

            //Hand movement
            if (mirror) {
                handUpdate(AvatarIKGoal.LeftHand, rightMirrorGoal.localPosition, rightMirrorGoal.localRotation);
                handUpdate(AvatarIKGoal.RightHand, leftMirrorGoal.localPosition, leftMirrorGoal.localRotation);
            } else {
                //Left hand goal local tranformations
                leftHandGoal.transform.localPosition = leftHandTransform.localPosition;
                leftHandGoal.transform.localRotation = leftHandTransform.rotation * Quaternion.LookRotation(transform.forward);

                /*
                Quaternion temp = leftHandGoal.transform.rotation;
                if (reverseX) {
                    leftHandGoal.transform.rotation = new Quaternion(temp.x, temp.y, temp.z, -temp.w);
                }
                if (reverseZ) {
                    leftHandGoal.transform.rotation = new Quaternion(temp.x, temp.y, -temp.z, temp.w);
                }
                */

                //Right hand goal local transformations
                rightHandGoal.transform.localPosition = rightHandTransform.localPosition;
                rightHandGoal.transform.localRotation = rightHandTransform.localRotation * Quaternion.LookRotation(transform.forward);
            }
            //Update hand transforms
            handUpdate(AvatarIKGoal.LeftHand,
                leftHandGoal.transform.position,
                leftHandGoal.transform.rotation);
            handUpdate(AvatarIKGoal.RightHand,
                rightHandGoal.transform.position,
                rightHandGoal.transform.rotation);
        }
    }

    private Vector3 updateHandDifference(Vector3 pos) {
        return new Vector3(pos.x - controller.center.x, pos.y, pos.z - controller.center.y);
    }

    private Quaternion convertHeadRotation(Quaternion rot) {
        return new Quaternion(-rot.y, rot.z, -rot.x, rot.w);
    }

    private void handUpdate(AvatarIKGoal goal, Vector3 pos, Quaternion rot) {
        //Set IK position/rotation weights
        animator.SetIKPositionWeight(goal, 1);
        animator.SetIKRotationWeight(goal, 1);

        animator.SetIKPosition(goal, pos);
        animator.SetIKRotation(goal, rot);
    }
}
