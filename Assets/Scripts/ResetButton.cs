using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Button Button;

    void Start ()
    {
        Button.onClick.AddListener(() => CreationStats.Instance.SelectedRequirements.Clear());
    }

    void Update ()
    {
        Button.interactable = CreationStats.Instance.SelectedRequirements.Count > 0;
    }
}
