using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trou : MonoBehaviour
{
    public Vector3 positionProchainNiveau;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Balle")) {
            if (positionProchainNiveau == Vector3.zero) {
                Evenements.instance.PartieTerminee();
            } else {
                Evenements.instance.ChangementDeNiveau(positionProchainNiveau);
            }
        }
    }
}
