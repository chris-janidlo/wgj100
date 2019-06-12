using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenreBox : MonoBehaviour
{
    public const int LargeIncreaseThreshold = 10;

    [Header("UI References")]
    public TextMeshProUGUI Text;
    public RectTransform UpperContainer, LowerContainer;
    public Button Button;

    [Header("Prefabs")]
    public Image ArtBlockPrefab;
    public Image GameplayBlockPrefab, MusicBlockPrefab, StoryBlockPrefab;
    public Image SmallIncreasePrefab, LargeIncreasePrefab;

    ScoredGenre genre;

    void Update ()
    {
        Button.interactable = genre != null && !CreationStats.Instance.SelectedRequirements.Contains(genre);
    }

    public void SetGenreAndScore (ScoredGenre scoredGenre)
    {
        genre = scoredGenre;
        Text.text = genre.Genre;

        Button.onClick.AddListener(() => CreationStats.Instance.SelectedRequirements.Add(genre));

        Genre genreData = ThemeData.Instance.GetGenre(genre.Genre);

        Instantiate(getPrefab(genreData.IncreaseType1)).rectTransform.SetParent(UpperContainer, false);
        Instantiate(genreData.IncreaseAmount1 >= LargeIncreaseThreshold ? LargeIncreasePrefab : SmallIncreasePrefab).rectTransform.SetParent(UpperContainer, false);

        if (genreData.IncreaseAmount2 > 0)
        {
            Instantiate(getPrefab(genreData.IncreaseType2)).rectTransform.SetParent(LowerContainer, false);
            Instantiate(genreData.IncreaseAmount2 >= LargeIncreaseThreshold ? LargeIncreasePrefab : SmallIncreasePrefab).rectTransform.SetParent(LowerContainer, false);
        }
    }

    Image getPrefab (BlockType type)
    {
        switch (type)
        {
            case BlockType.Art:
                return ArtBlockPrefab;
            case BlockType.Gameplay:
                return GameplayBlockPrefab;
            case BlockType.Music:
                return MusicBlockPrefab;
            case BlockType.Story:
                return StoryBlockPrefab;
            default:
                throw new System.Exception($"unexpected BlockType {type}");
        }
    }
}
