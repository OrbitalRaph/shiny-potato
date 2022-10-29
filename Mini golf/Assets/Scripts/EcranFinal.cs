using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EcranFinal : MonoBehaviour
{
    public TextMeshProUGUI textNbCoups;

    public void PartieTerminee(int nbCoups)
    {
        gameObject.SetActive(true);
        textNbCoups.text = nbCoups.ToString() + " coups";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Rejouer();
        } 
    }

    public void Rejouer()
    {
        // recharger la scène
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
