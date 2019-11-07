using UnityEngine;

/*
This is an example script used in CC_HelloWorld, an example scene from the CyberCANOE package. 

This script handles spawning projectiles using the left Dpad, changing projectile models,


CyberCANOE Virtual Reality API for Unity3D
(C) 2016 Ryan Theriot, Jason Leigh, Laboratory for Advanced Visualization & Applications, University of Hawaii at Manoa.
Version: 1.14, August 6th, 2019.
*/

public class ColorChanger : MonoBehaviour {

    public GameObject orbObject;
    public GameObject cubeObject;

    public int projectileLimit = 10;

    private enum Force {
        Low = 0,
        Medium = 1,
        High = 2,
    }

    private enum Model {
        Orb = 0,
        Cube = 1,
    }

    private Force forceEnum = Force.Medium;
    private Model modelEnum = Model.Orb;
    private float forceFactor = 35f;
    private GameObject projectile;
    private GameObject controlledObject;
    private int projectileIncrement = 0;
    private float x = 0f;
    private float y = 0f;
    private float angle = 0f;

    private TextMesh toggleInfo;
    private AudioSource audioSource;

    private float zForce;

    void Start() {
        toggleInfo = gameObject.transform.GetChild(1).GetComponent<TextMesh>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {

        //Default color
        Color color = Color.white;
        updateModel();
        updateTrackpad();

        //On the first frame of the click, instantiate a projectile at the position of the trackpad click
        if (CC_INPUT.GetButtonDown(Wand.Left, WandButton.TrackpadClick)) {
            Debug.Log("trackpad just clicked");
            //Color is calculated using the angle of the click
            color = new Color(Mathf.Cos(angle), Mathf.Cos(angle + (3 * Mathf.PI / 4)), Mathf.Cos(angle - (3 * Mathf.PI / 4)));
            controlledObject = Instantiate(projectile, new Vector3(x, y + transform.position.y, transform.position.z), Quaternion.identity);
            controlledObject.GetComponent<Renderer>().material.color = color;
            controlledObject.name = "obj_" + projectileIncrement;
            projectileIncrement = (projectileIncrement + 1) % projectileLimit;
        //When the trackpad is held, the projectile follows the position of the trackpad click and its color is updated
        //to match the new angle
        } else if (CC_INPUT.GetButtonPress(Wand.Left, WandButton.TrackpadClick) && (controlledObject != null)) {
            Debug.Log("trackpad being held");
            controlledObject.transform.position = new Vector3(x, y + transform.position.y, transform.position.z);
            color = new Color(Mathf.Cos(angle), Mathf.Cos(angle + (3 * Mathf.PI / 4)), Mathf.Cos(angle - (3 * Mathf.PI / 4)));
            controlledObject.GetComponent<Renderer>().material.color = color;
        //When the trackpad is released, the force is added to the projectile at the angle it was released at
        } else if (CC_INPUT.GetButtonUp(Wand.Left, WandButton.TrackpadClick)) {
            Debug.Log("trackpad just released");
            //A random z-axis force is applied to the projectile
            float force = (float)forceEnum * forceFactor + forceFactor;
            zForce = CC_CANOE.RandomRange(0f, 1f) * force;
            if (CC_CANOE.RandomRange(0, 1) == 0) { zForce = -zForce; }
            //Force is added to the projectile
            if (controlledObject != null) {
                controlledObject.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Cos(angle) * force, Mathf.Sin(angle) * force, zForce));
            }
            //If an object of the same name exists, destroy it. This enforces the number of projectiles allowed at once.
            if (GameObject.Find("obj_" + projectileIncrement) != null) {
                Destroy(GameObject.Find("obj_" + projectileIncrement));
            }
        }

        if (CC_INPUT.GetButtonDown(Wand.Left, WandButton.Trigger)) {
            forceEnum = (Force)(((int)(forceEnum) + 1) % System.Enum.GetValues(typeof(Force)).Length);
        }

        if (CC_INPUT.GetButtonDown(Wand.Left, WandButton.Grip)) {
            modelEnum = (Model)(((int)(modelEnum) + 1) % System.Enum.GetValues(typeof(Model)).Length);
            switch (modelEnum) {
                case Model.Orb:
                    projectile = orbObject;
                    break;
                case Model.Cube:
                    projectile = cubeObject;
                    break;
                default:
                    break;
            }
        }

        //Press the left Menu button to delete all existing projectiles
        if (CC_INPUT.GetButtonDown(Wand.Left, WandButton.Menu)) {
            for (int i = 0; i < projectileLimit; i++) {
                if (GameObject.Find("obj_" + i) != null) {
                    Destroy(GameObject.Find("obj_" + i));
                    audioSource.Play();
                }
            }
        }

        toggleInfo.text = "Newly spawned projectiles"
            + "\nwill be " + modelEnum + "s with " + forceEnum + " force";
    }


    private void updateModel() {
        switch (modelEnum) {
            case Model.Orb:
                projectile = orbObject;
                break;
            case Model.Cube:
                projectile = cubeObject;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Updates the values for the x, y, and angle variables using the GetAxis methods.
    /// </summary>
    private void updateTrackpad() {
        x = CC_INPUT.GetAxis(Wand.Left, WandAxis.XAxis) / 2;
        y = CC_INPUT.GetAxis(Wand.Left, WandAxis.YAxis) / 2;
        angle = Mathf.Atan2(y, x);

        if (angle < 0) {
            angle += (2 * Mathf.PI);
        }
    }

    /// <summary>
    /// Returns force enum.
    /// </summary>
    public int GetForceIndex() {
        return (int)forceEnum;
    }
}
