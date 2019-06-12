using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Block
{
    public BlockType Type;

    public Block (BlockType type)
    {
        Type = type;
    }
}

public enum BlockType
{
    Gameplay, Art, Music, Story
}
