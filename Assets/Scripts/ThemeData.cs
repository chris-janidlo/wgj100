using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ThemeData
{
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

    public List<Theme> Themes;
    public List<Genre> Genres;
}
