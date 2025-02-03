using UnityEngine;

public class VentZone : MonoBehaviour
{
    // Cache
    private Player_Move _playerControllerCache = null;

    private void OnTriggerEnter(Collider other)
    {
        // Given the object is a player
        if (other.gameObject.name == "Player")
        {
            try
            {
                // Fetch the player that has just entered the vent
                _playerControllerCache = other.gameObject.GetComponent<Player_Move>();

                // Tell it that it is now in the vent
                _playerControllerCache.InVentZone = true;
            }
            catch { }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Given the object is a player
        if (other.gameObject.name == "Player")
        {
            // If we have something cached
            if (_playerControllerCache != null)
            {
                // Modify this and return
                _playerControllerCache.InVentZone = true;
            }
            // Otherwise barrel ahead
            try
            {
                other.gameObject.GetComponent<Player_Move>().InVentZone = true;
            }
            catch { }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Given the object is a player
        if (other.gameObject.name == "Player")
        {
            // Given that we have something cached
            if (_playerControllerCache != null)
            {
                // Tell it that it has left, then return
                _playerControllerCache.InVentZone = false;
                return;
            }

            // Otherwise we just barrel ahead as usual
            try
            {
                other.gameObject.GetComponent<Player_Move>().InVentZone = false;
            }
            catch { }
        }
    }
}
