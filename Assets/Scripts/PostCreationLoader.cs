using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PostCreationLoader : MonoBehaviour
{
    public List<float> DelayTimes;
    public List<TextMeshProUGUI> Texts;

    public TextMeshProUGUI DescriptorText, ReactionText, RatingText;

    IEnumerator Start ()
    {
        var score = CreationStats.Instance.GetScore();

        string reaction;

        if (score <= 2)
        {
            reaction = "People hated it.";
        }
        else if (score <= 5)
        {
            reaction = "The general reaction wasn't very good.";
        }
        else if (score <= 7)
        {
            reaction = "People seemed to like it.";
        }
        else if (score <= 10)
        {
            reaction = "People loved it!";
        }
        else
        {
            reaction = "It broke the rating scale!";
        }

        DescriptorText.text = $"a {CreationStats.Instance.Descriptor()}.";
        ReactionText.text = reaction;
        RatingText.text = $"It got an average score of {score}/10.";

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
                SceneManager.LoadScene("MainMenu");
            }
            yield return null;
        }
    }
}
