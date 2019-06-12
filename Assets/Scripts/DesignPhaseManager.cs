using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using crass;

public class DesignPhaseManager : MonoBehaviour
{
    public TextMeshProUGUI ThemeLabel, ThemeValue, NotationLabel, PlayButtonLabel;
    public List<GenreBox> GenreBoxes;

    public float ThemeLabelShowDelay, ThemeValueShowDelay, NotationLabelShowDelay, FirstGenreBoxShowDelay, OtherGenreBoxesShowDelay;

    Theme theme;

    IEnumerator Start ()
    {
        theme = ThemeData.Instance.Themes.PickRandom();
        ThemeValue.text = theme.Name;
    
        ThemeLabel.enabled = false;
        ThemeValue.enabled = false;
        NotationLabel.enabled = false;
        PlayButtonLabel.enabled = false;

        foreach (var genre in GenreBoxes)
        {
            genre.Text.enabled = false;
        }

        yield return new WaitForSeconds(ThemeLabelShowDelay);
        ThemeLabel.enabled = true;

        yield return new WaitForSeconds(ThemeValueShowDelay);
        ThemeValue.enabled = true;

        yield return new WaitForSeconds(NotationLabelShowDelay);
        NotationLabel.enabled = true;

        yield return new WaitForSeconds(FirstGenreBoxShowDelay);

		for (int i = 0; i < GenreBoxes.Count; i++)
        {
			var genre = GenreBoxes[i];

            genre.SetGenreAndScore(theme.GenresAndScores[i]);
			genre.Text.enabled = true;

            yield return new WaitForSeconds(OtherGenreBoxesShowDelay);
        }

        PlayButtonLabel.enabled = true;
    }
}
