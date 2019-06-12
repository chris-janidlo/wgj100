using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using crass;

public class CreationStats : Singleton<CreationStats>
{
    [Header("Base requirements")]
    public int ArtRequirements;
    public int GameplayRequirements, MusicRequirements, StoryRequirements;

    [Header("Additional requirements")]
    public List<ScoredGenre> SelectedRequirements;

    void Awake ()
    {
        if (SingletonGetInstance() != null)
        {
            Destroy(gameObject);
        }
        else
        {
            SingletonSetInstance(this, false);
            DontDestroyOnLoad(gameObject);
        }
    }

    public string Descriptor ()
    {
        string result = "";

        foreach (var genre in SelectedRequirements)
        {
            result += genre.Genre + " ";
        }

        return result + "game";
    }
}
