using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentZone : MonoBehaviour
{
    private void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            try
            {
                other.gameObject.GetComponent<PlayerController>().inVentZone = true;
            }
            catch { }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            try
            {
                other.gameObject.GetComponent<PlayerController>().inVentZone = true;
            }
            catch { }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        try
        {
            other.gameObject.GetComponent<PlayerController>().inVentZone = false;
        }
        catch { }
    }
}
