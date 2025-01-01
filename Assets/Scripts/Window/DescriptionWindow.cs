using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] TextMeshProUGUI description;

    public void SetData(string title)
    {
        header.text = title;
        description.text = SaveManager.SearchBy(title);
    }
}
