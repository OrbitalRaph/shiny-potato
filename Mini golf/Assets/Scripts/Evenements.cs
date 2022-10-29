using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Evenements : MonoBehaviour
{
    public static Evenements instance;

    public event Action<Vector3> OnChangementDeNiveau;
    public event Action OnPartieTerminee;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Evenements dans la scène");
            return;
        }
        instance = this;
    }

    public void ChangementDeNiveau(Vector3 positionProchainNiveau)
    {
        OnChangementDeNiveau?.Invoke(positionProchainNiveau);
    }

    public void PartieTerminee()
    {
        OnPartieTerminee?.Invoke();
    }
}
