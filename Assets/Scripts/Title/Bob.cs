using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    // Editor variables
    [SerializeField]
    private float amplitude;
    [SerializeField]
    private float speed;

    // Internal variables
    private Vector3 basePos;
    private RectTransform cache;
    private float timer = 0;
    void Start()
    {
        // Fetch starting position
        cache = GetComponent<RectTransform>();
        basePos = cache.position;
    }

    // Update is called once per frame
    void Update()
    {
        cache.position = basePos + new Vector3(0, amplitude * Mathf.Sin(timer), 0);

        timer += Time.deltaTime * speed;
    }
}
