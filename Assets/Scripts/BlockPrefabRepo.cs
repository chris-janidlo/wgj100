using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using crass;

public class BlockPrefabRepo : Singleton<BlockPrefabRepo>
{
    public Image ArtBlockPrefab, GameplayBlockPrefab, MusicBlockPrefab, StoryBlockPrefab;

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

    public Image GetPrefab (BlockType type)
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
