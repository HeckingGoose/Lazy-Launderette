using UnityEngine;

public class VentZone : MonoBehaviour
{
    // Cache
    private PlayerController playerControllerCache = null;

    private void OnTriggerEnter(Collider other)
    {
        // Given the object is a player
        if (other.gameObject.name == "Player")
        {
            try
            {
                // Fetch the player that has just entered the vent
                playerControllerCache = other.gameObject.GetComponent<PlayerController>();

                // Tell it that it is now in the vent
                playerControllerCache.inVentZone = true;
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
            if (playerControllerCache != null)
            {
                // Modify this and return
                playerControllerCache.inVentZone = true;
            }
            // Otherwise barrel ahead
            try
            {
                other.gameObject.GetComponent<PlayerController>().inVentZone = true;
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
            if (playerControllerCache != null)
            {
                // Tell it that it has left, then return
                playerControllerCache.inVentZone = false;
                return;
            }

            // Otherwise we just barrel ahead as usual
            try
            {
                other.gameObject.GetComponent<PlayerController>().inVentZone = false;
            }
            catch { }
        }
    }
}
