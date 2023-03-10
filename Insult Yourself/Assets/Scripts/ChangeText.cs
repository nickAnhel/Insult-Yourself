using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public TextMeshProUGUI oldCount;

    public void ChangeStr(int count)
    {
        oldCount.text = count.ToString();
    }
}
