using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static int Level {
        get => PlayerPrefs.GetInt("level");
        set => PlayerPrefs.SetInt("level", value);
    }
}
