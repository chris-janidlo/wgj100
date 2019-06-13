using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescriptorText : MonoBehaviour
{
    public TextMeshProUGUI Text;

    void Start ()
    {
        Text.text = "Makin a " + CreationStats.Instance.Descriptor();
    }
}
