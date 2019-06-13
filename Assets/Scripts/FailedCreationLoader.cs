using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FailedCreationLoader : MonoBehaviour
{
    public List<float> DelayTimes;
    public List<TextMeshProUGUI> Texts;
    public string Scene;

    IEnumerator Start ()
    {
        foreach (var text in Texts)
        {
            text.enabled = false;
        }

        for (int i = 0; i < Texts.Count; i++)
        {
            yield return new WaitForSeconds(DelayTimes[i]);
            Texts[i].enabled = true;
        }

        while (true)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene(Scene);
            }
            yield return null;
        }
    }
}
