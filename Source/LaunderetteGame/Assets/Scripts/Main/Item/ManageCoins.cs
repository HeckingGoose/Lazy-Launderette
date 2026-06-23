using TMPro;
using UnityEngine;

public class ManageCoins : MonoBehaviour
{
    // Const
    private const string MONEY_PREFIX = "Money: £";
    private const string MONEY_SUFFIX = " / £";

    // Editor variables
    [SerializeField]
    private TextMeshProUGUI _display;

    // Private variables
    private int _coinCount;

    // Unity Methods
    private void Start()
    {
        // Set starting text
        UpdateDisplayText();
    }

    // Private Methods
    /// <summary>
    /// Updates the display text to represent the current coin count and coin goal.
    /// </summary>
    private void UpdateDisplayText()
    {
        // Update display
        _display.text = $"{MONEY_PREFIX}{_coinCount}{MONEY_SUFFIX}{GameRules.COINS_TOWIN}";
    }

    // Accessors
    public int CoinCount
    {
        get { return _coinCount; }
        set
        {
            // Update coin count
            _coinCount = value;

            // Update display
            UpdateDisplayText();
        }
    }
}
