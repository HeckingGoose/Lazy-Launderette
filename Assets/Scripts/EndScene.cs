using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Image cover;
    [SerializeField]
    private TextMeshProUGUI text;

    private uint state = 0;
    private float timer = 0f;
    private float finTime = 1f;
    private string charactersToAdd;
    private float textSpeed = 0.1f;
    public void Begin()
    {
        playerController.enabled = false;
        text.enabled = true;
        state = 1;
    }
    public void Update()
    {
        switch (state)
        {
            // Fade in background
            case 1:
                timer += Time.deltaTime;

                cover.color = new Color(
                    cover.color.r,
                    cover.color.g,
                    cover.color.b,
                    Mathf.Lerp(0, 1, timer / finTime)
                    );

                if (timer > finTime)
                {
                    state++;
                    timer = 0f;
                    finTime = textSpeed;
                    charactersToAdd = "And so, you sit down for 35 minutes while you wait for your clothes to wash.";
                }
                break;

            // Walk in text
            case 2:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    if (charactersToAdd.Length > 0)
                    {
                        text.text += charactersToAdd[0];
                        charactersToAdd = charactersToAdd.Remove(0, 1);
                    }
                    else
                    {
                        state++;
                        finTime = 2f;
                    }
                    timer = 0f;
                }
                break;

            // Wait for reading
            case 3:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    state++;
                    timer = 0f;
                    finTime = textSpeed;
                    text.text = "";
                    charactersToAdd = "* Credits *";
                }
                break;

            // Walk in text
            case 4:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    if (charactersToAdd.Length > 0)
                    {
                        text.text += charactersToAdd[0];
                        charactersToAdd = charactersToAdd.Remove(0, 1);
                    }
                    else
                    {
                        state++;
                        finTime = 1f;
                    }
                    timer = 0f;
                }
                break;

            // Wait for reading
            case 5:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    state++;
                    timer = 0f;
                    finTime = textSpeed;
                    text.text = "";
                    charactersToAdd = "HeckingGoose, who did literally everything.";
                }
                break;

            // Walk in text
            case 6:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    if (charactersToAdd.Length > 0)
                    {
                        text.text += charactersToAdd[0];
                        charactersToAdd = charactersToAdd.Remove(0, 1);
                    }
                    else
                    {
                        state++;
                        finTime = 2f;
                    }
                    timer = 0f;
                }
                break;

            // Wait for reading
            case 7:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    state++;
                    timer = 0f;
                    finTime = textSpeed;
                    text.text = "";
                    charactersToAdd = "You, for playing the game.";
                }
                break;

            // Walk in text
            case 8:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    if (charactersToAdd.Length > 0)
                    {
                        text.text += charactersToAdd[0];
                        charactersToAdd = charactersToAdd.Remove(0, 1);
                    }
                    else
                    {
                        state++;
                        finTime = 2f;
                    }
                    timer = 0f;
                }
                break;

            // Wait for reading
            case 9:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    state++;
                    timer = 0f;
                    finTime = textSpeed;
                    text.text = "";
                    charactersToAdd = "Fin";
                }
                break;

            // Walk in text
            case 10:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    if (charactersToAdd.Length > 0)
                    {
                        text.text += charactersToAdd[0];
                        charactersToAdd = charactersToAdd.Remove(0, 1);
                    }
                    else
                    {
                        state++;
                        finTime = 1f;
                    }
                    timer = 0f;
                }
                break;

            // Wait for reading
            case 11:
                timer += Time.deltaTime;

                if (timer > finTime)
                {
                    state++;
                    timer = 0f;
                    finTime = 0.2f;
                    GLOBAL.LoadTarget = "Menu";
                    SceneManager.LoadScene("Loading");
                }
                break;
        }
    }
}
