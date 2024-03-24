using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIValidator
{
    public static bool IsAnyUIOpen => SettingsController.Instance.IsEnabled;
}
