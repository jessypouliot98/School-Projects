using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIKey : MonoBehaviour {

    Image _image;

    private void Start()
    {
        _image = this.GetComponent<Image>();
        DisplayKey(false);
    }

    public void DisplayKey (bool hasKey)
    {
        _image.enabled = hasKey;
    }
}
