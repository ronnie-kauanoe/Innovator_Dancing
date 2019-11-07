using UnityEngine;

public class PlaySound : MonoBehaviour {

    private AudioSource source;
    private ColorChanger cc;

    // Use this for initialization
    void Start() {
        source = GetComponent<AudioSource>();
        cc = GameObject.Find("ballpit").GetComponent<ColorChanger>();
        source.pitch = (cc.GetForceIndex() + 2f) / 2;
    }

    void OnCollisionEnter(Collision collision) {
        source.Play();
    }
}
