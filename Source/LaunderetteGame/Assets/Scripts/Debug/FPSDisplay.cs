using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    // Const
    private const string PREFACE = "FPS: ";
    private const int SIZEOF_CACHE = 400;

    // Editor
    [Header("Display")]
    [SerializeField]
    private TextMeshProUGUI _display;

    // Private
    private float[] _fpsCache = new float[400];
    private int _pointer;

    // On start
    private void Start()
    {
        // Set all vaues to first frame delta
        Array.Fill(_fpsCache, Time.deltaTime);
    }

    // Every frame
    private void Update()
    {
        // Read time
        _fpsCache[_pointer] = Time.deltaTime;

        // Update pointer
        _pointer++;

        // Wrap pointer
        if (_pointer >= SIZEOF_CACHE)
        {
            _pointer = 0;
        }

        // Update display
        _display.text = $"{PREFACE}{1 / _fpsCache.Average()}";
    }
}
