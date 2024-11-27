using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageBlinker : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private float timeBetweenToggles;

    // Private variables
    private float timer;
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > timeBetweenToggles)
        {
            image.enabled = !image.enabled;
            timer = 0;
        }

        timer += Time.deltaTime;
    }
}
