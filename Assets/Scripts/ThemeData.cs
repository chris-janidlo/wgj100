using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThemeData
{
    public static readonly ThemeData Instance;

    public List<Theme> Themes;
    public List<Genre> Genres;

    static readonly string ResourcePath = "ThemeData";

    static ThemeData ()
    {
        TextAsset file = Resources.Load<TextAsset>(ResourcePath);
        Instance = JsonUtility.FromJson<ThemeData>(file.text);
    }

    public Genre GetGenre (string name)
    {
        return Genres.Single(g => g.Name.Equals(name));
    }
}

[Serializable]
public class Theme
{
    public string Name;
    public List<ScoredGenre> GenresAndScores;
}

[Serializable]
public class Genre
{
    public string Name;
    public BlockType IncreaseType1, IncreaseType2;
    public int IncreaseAmount1, IncreaseAmount2;
}

[Serializable]
public class ScoredGenre
{
    public string Genre;
    public float Score;
}
