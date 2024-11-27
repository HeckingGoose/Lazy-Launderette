using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManageCoins : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;
    private int lastNumCoins;
    public int numCoins;

    private void Start()
    {
        moneyText.text = $"Money: £{numCoins} / £5";
    }
    private void Update()
    {
        if (lastNumCoins != numCoins)
        {
            moneyText.text = $"Money: £{numCoins} / £5";
        }

        lastNumCoins = numCoins;
    }
}
