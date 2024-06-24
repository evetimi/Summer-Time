using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysKeepResolution : MonoBehaviourService<AlwaysKeepResolution>
{
    public int width = 1920;
    public int height = 1080;

    private void Update() {
        if (Screen.currentResolution.width != width || Screen.currentResolution.height != height) {
            Screen.SetResolution(width, height, false);
        }
    }
}
