using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOMScript : MonoBehaviour
{

    public static TOMScript Instance { get; private set; }


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }

    public byte SetTile(GameObject chunk, float temp, float hum) {
        return 0;
    }
}
