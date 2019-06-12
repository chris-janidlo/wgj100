using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using crass;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextRandomizer : MonoBehaviour
{
    [Serializable]
    public class SizedFont
    {
        public TMP_FontAsset Font;
        public float Size;
    }

    [Serializable]
    public class FontBag : BagRandomizer<SizedFont> {}

    public FontBag Fonts;
    public Vector2 FontSwapTimeRange;

    IEnumerator Start ()
    {
        var text = GetComponent<TextMeshProUGUI>();
        
        while (true)
        {
            var next = Fonts.GetNext();
            
            text.font = next.Font;
            text.fontSize = next.Size;

            yield return new WaitForSeconds(RandomExtra.Range(FontSwapTimeRange));
        }
    }
}
