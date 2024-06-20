using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIComponent_RandomText : MonoBehaviour
{
    private TextMeshProUGUI txt = null;

    [SerializeField] List<string> randomText = new List<string>();

    private void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        if (randomText != null && randomText.Count > 0)
        {
            int index = Random.Range(0, randomText.Count);
            txt.SafeSetText(randomText[index]);
        }
    }
}
