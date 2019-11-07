using UnityEngine;

public class ControllerInfo : MonoBehaviour {

    private TextMesh leftInfo;
    private TextMesh rightInfo;
    private CC_TRACKER tracker;

	// Use this for initialization
	void Start () {
        leftInfo = GameObject.Find("left_info").gameObject.GetComponent<TextMesh>();
        rightInfo = GameObject.Find("right_info").gameObject.GetComponent<TextMesh>();
        tracker = GameObject.Find("CC_CANOE").gameObject.GetComponent<CC_TRACKER>();
    }

    // Update is called once per frame
    void Update () {
        updateText(Wand.Left, leftInfo);
        updateText(Wand.Right, rightInfo);
	}

    private void updateText(Wand wand, TextMesh mesh) {
        mesh.text = wand.ToString() + " Info\npos: " + tracker.GetWandPosition((int)wand)
            + "\nrot: " + tracker.GetWandRotation((int)wand);

        mesh.text += "\nmenu clicked ? " + CC_INPUT.GetButtonPress(wand, WandButton.Menu);
        mesh.text += "\ngrip clicked ? " + CC_INPUT.GetButtonPress(wand, WandButton.Grip);

        mesh.text += "\nx = " + CC_INPUT.GetAxis(wand, WandAxis.XAxis).ToString("0.00")
            + "\ny = " + CC_INPUT.GetAxis(wand, WandAxis.YAxis).ToString("0.00")
            + "\nclicked ? " + CC_INPUT.GetButtonPress(wand, WandButton.TrackpadClick);

        mesh.text += "\ntrigger axis = " + CC_INPUT.GetAxis(wand, WandAxis.Trigger).ToString("0.00")
            + "\ntrigger pulled ? " + CC_INPUT.GetButtonPress(wand, WandButton.Trigger);
    }
}
