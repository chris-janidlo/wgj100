using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubGoalBox : MonoBehaviour
{
    public Color FilledColor;
    public TextMeshProUGUI GenreName, AmountDisplay1, AmountDisplay2;
    public RectTransform BlockContainer1, BlockContainer2;

    CreationStats.GenreBarPair bars;

    void Update ()
    {
        if (bars == null) return;

        AmountDisplay1.text = $"{bars.Bar1.CurrentAmount}/{bars.Bar1.Requirement}";

        if (bars.Bar2 != null)
        {
            AmountDisplay2.text = $"{bars.Bar2.CurrentAmount}/{bars.Bar2.Requirement}";
        }
        else
        {
            AmountDisplay2.text = "";
        }
    }

    public void SetIndex (int? index)
    {
        if (index == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            int creationStatsIndex = (int) index;

            bars = CreationStats.Instance.GenreBars[creationStatsIndex];
            var scoredGenre = CreationStats.Instance.SelectedRequirements[creationStatsIndex];

            var genre = ThemeData.Instance.GetGenre(scoredGenre.Genre);

            GenreName.text = scoredGenre.Genre;
            
            bars.Bar1.Filled.AddListener(() => AmountDisplay1.color = FilledColor);

            Instantiate(BlockPrefabRepo.Instance.GetPrefab(genre.IncreaseType1)).rectTransform.SetParent(BlockContainer1, false);

            if (bars.Bar2 != null)
            {
                bars.Bar2.Filled.AddListener(() => AmountDisplay2.color = FilledColor);

                Instantiate(BlockPrefabRepo.Instance.GetPrefab(genre.IncreaseType2)).rectTransform.SetParent(BlockContainer2, false);
            }
        }
    }
}
