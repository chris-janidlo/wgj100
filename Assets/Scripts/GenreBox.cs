using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenreBox : MonoBehaviour
{
    public const int LargeIncreaseThreshold = 10;

    public TextMeshProUGUI Text;
    public RectTransform UpperContainer, LowerContainer;

    public Image ArtBlockPrefab, GameplayBlockPrefab, MusicBlockPrefab, StoryBlockPrefab;
    public Image SmallIncreasePrefab, LargeIncreasePrefab;    

    public void SetGenreAndScore (ScoredGenre scoredGenre)
    {
        Text.text = scoredGenre.Genre;

        Genre genreData = ThemeData.Instance.GetGenre(scoredGenre.Genre);

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
