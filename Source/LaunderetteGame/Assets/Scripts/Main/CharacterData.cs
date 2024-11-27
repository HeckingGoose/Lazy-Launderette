using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public string characterName;
    public Color characterColour;
    public string conversationName;
    public string[] conversations;
    public Dictionary<string, bool> needs = new Dictionary<string, bool>();
    public float pitch;
    public int skip;
    public Texture[] poses;
}
