using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreationTimer : MonoBehaviour
{
    public float LevelTime;
    public TextMeshProUGUI Text;

    void Update ()
    {
        Text.text = Mathf.CeilToInt(LevelTime).ToString();

        LevelTime -= Time.deltaTime;
        if (LevelTime <= 0)
        {
            SceneManager.LoadScene("PostCreation");
        }
    }
}
