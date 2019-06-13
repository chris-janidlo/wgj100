using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using crass;

public class CreationStats : Singleton<CreationStats>
{
    public class BlockBar
    {
        public UnityEvent Filled { get; private set; }
        public int Requirement { get; private set; }
        public bool IsFilled { get; private set; }
        public int CurrentAmount { get; private set; }

        public BlockBar (int requirement)
        {
            Requirement = requirement;
            Filled = new UnityEvent();
        }

        // returns any overflow
        public int IncreaseAmount (int increase)
        {
            int left = Requirement - CurrentAmount;
            int overflow = increase - left;

            CurrentAmount = Mathf.Min(CurrentAmount + increase, Requirement);

            if (!IsFilled && CurrentAmount == Requirement)
            {
                IsFilled = true;
                Filled.Invoke();
            }

            return overflow;
        }

        public void IncreaseNoLimit (int increase)
        {
            CurrentAmount += increase;
        }
    }

    public class GenreBarPair
    {
        public BlockBar Bar1, Bar2;
    }

    [Header("Base requirements")]
    public int ArtRequirements;
    public int GameplayRequirements, MusicRequirements, StoryRequirements;

    [Header("Additional requirements")]
    public List<ScoredGenre> SelectedRequirements;

    public BlockBar ArtBar { get; private set; }
    public BlockBar GameplayBar { get; private set; }
    public BlockBar MusicBar { get; private set; }
    public BlockBar StoryBar { get; private set; }

    List<GenreBarPair> genreBars;
    public ReadOnlyCollection<GenreBarPair> GenreBars => genreBars.AsReadOnly();

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

    public void InitializeBars ()
    {
        ArtBar = new BlockBar(ArtRequirements);
        GameplayBar = new BlockBar(GameplayRequirements);
        MusicBar = new BlockBar(MusicRequirements);
        StoryBar = new BlockBar(StoryRequirements);

        SelectedRequirements.Sort((sg1, sg2) => sg2.Score.CompareTo(sg1.Score));

        genreBars = new List<GenreBarPair>();
        foreach (var scoredGenre in SelectedRequirements)
        {
            var genreData = ThemeData.Instance.GetGenre(scoredGenre.Genre);

            var pair = new GenreBarPair
            {
                Bar1 = new BlockBar(genreData.IncreaseAmount1),
                Bar2 = genreData.IncreaseAmount2 > 0 ? new BlockBar(genreData.IncreaseAmount2) : null
            };

            genreBars.Add(pair);
        }
    }

    public void CompleteBlock (BlockType type, int amount)
    {
        BlockBar barToUse = null;

        switch (type)
        {
            case BlockType.Art:
                barToUse = ArtBar;
                break;

            case BlockType.Gameplay:
                barToUse = GameplayBar;
                break;

            case BlockType.Music:
                barToUse = MusicBar;
                break;

            case BlockType.Story:
                barToUse = StoryBar;
                break;

            default:
                throw new Exception($"unexpected type {type}");
        }

        int overflow = barToUse.IncreaseAmount(amount);

        if (barToUse.IsFilled)
        {
            var optionalBarToUse = firstNonFullOptionalBar(type);

            if (optionalBarToUse != null)
            {
                overflow = optionalBarToUse.IncreaseAmount(overflow);

                if (!optionalBarToUse.IsFilled)
                {
                    return;
                }
            }

            barToUse.IncreaseNoLimit(overflow);
        }
    }

    public bool FinishedGame ()
    {
        return ArtBar.IsFilled && GameplayBar.IsFilled && MusicBar.IsFilled && StoryBar.IsFilled;
    }

    public float GetScore ()
    {
        float sum = 0;

        for (int i = 0; i < SelectedRequirements.Count; i++)
        {
            float score = SelectedRequirements[i].Score;
            
            var bars = genreBars[i];
            if (bars.Bar1.IsFilled && (bars.Bar2 == null || bars.Bar2.IsFilled))
            {
                sum += score;
            }
        }

        return sum;
    }

    BlockBar firstNonFullOptionalBar (BlockType type)
    {
        for (int i = 0; i < SelectedRequirements.Count; i++)
        {
            var genre = ThemeData.Instance.GetGenre(SelectedRequirements[i].Genre);

            if (genre.IncreaseType1 == type && !genreBars[i].Bar1.IsFilled)
            {
                return genreBars[i].Bar1;
            }

            if (genre.IncreaseAmount2 > 0 && genre.IncreaseType2 == type && !genreBars[i].Bar2.IsFilled)
            {
                return genreBars[i].Bar2;
            }
        }

        return null;
    }
}
