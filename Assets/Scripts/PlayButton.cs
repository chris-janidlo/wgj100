using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayButton : MonoBehaviour
{
    public int GenreRequirement;
    public TextMeshProUGUI Text;
    public Button Button;

    void Start ()
    {
        Button.onClick.AddListener(() => SceneManager.LoadScene("CreationPhase"));
    }

    void Update ()
    {
        int genresLeft = GenreRequirement - CreationStats.Instance.SelectedRequirements.Count;

        bool bad = genresLeft > 0;

        Button.interactable = !bad;

        if (bad)
        {
            Text.text = $"select {genresLeft} more ideas to make a {CreationStats.Instance.Descriptor()}...";
        }
        else
        {
            Text.text = $"make a {CreationStats.Instance.Descriptor()}";
        }
    }
}
