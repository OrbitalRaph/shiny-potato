using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trou : MonoBehaviour
{
    public Vector3 positionProchainNiveau;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Balle"))
        {
            other.transform.position = positionProchainNiveau;
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
