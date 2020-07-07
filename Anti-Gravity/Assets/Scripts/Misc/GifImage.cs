using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifImage : MonoBehaviour
{
    Image _source;
    [SerializeField] Sprite[] _pngSequence;
    [SerializeField] int _frameRate = 10;
    void Start()
    {
        _source = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        int index = (int)((Time.time * _frameRate) % _pngSequence.Length);
        _source.sprite = _pngSequence[index];
    }
}
