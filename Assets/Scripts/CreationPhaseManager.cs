using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreationPhaseManager : MonoBehaviour
{
    public Board Board { get; private set; }

    public Color FilledColor;
    public TextMeshProUGUI ArtAmountText, GameplayAmountText, MusicAmountText, StoryAmountText;
    public List<SubGoalBox> SubGoals;

    void Awake ()
    {
        Board = new Board();
        CreationStats.Instance.InitializeBars();

        for (int i = 0; i < SubGoals.Count; i++)
        {
            SubGoals[i].SetIndex(i < CreationStats.Instance.SelectedRequirements.Count ? (int?) i : null);
        }

        CreationStats.Instance.ArtBar.Filled.AddListener
        (
            () => ArtAmountText.color = FilledColor
        );
        CreationStats.Instance.GameplayBar.Filled.AddListener
        (
            () => GameplayAmountText.color = FilledColor
        );
        CreationStats.Instance.MusicBar.Filled.AddListener
        (
            () => MusicAmountText.color = FilledColor
        );
        CreationStats.Instance.StoryBar.Filled.AddListener
        (
            () => StoryAmountText.color = FilledColor
        );
    }

    void Update ()
    {
        foreach (BoardInput inputDir in Enum.GetValues(typeof(BoardInput)))
        {
            if (Input.GetButtonDown(inputDir.ToString()))
            {
                Board.Slide(inputDir);
            }
        }

        ArtAmountText.text = $"{CreationStats.Instance.ArtBar.CurrentAmount}/{CreationStats.Instance.ArtBar.Requirement}";
        GameplayAmountText.text = $"{CreationStats.Instance.GameplayBar.CurrentAmount}/{CreationStats.Instance.GameplayBar.Requirement}";
        MusicAmountText.text = $"{CreationStats.Instance.MusicBar.CurrentAmount}/{CreationStats.Instance.MusicBar.Requirement}";
        StoryAmountText.text = $"{CreationStats.Instance.StoryBar.CurrentAmount}/{CreationStats.Instance.StoryBar.Requirement}";
    }
}
