using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    void Awake() {
        GameObject music = GameObject.FindWithTag("MusicManager");

        if (music != null && music != this.gameObject) {
            Destroy(this.gameObject);
        }
    }
}
