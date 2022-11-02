using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestionUI : MonoBehaviour
{
    [Header("Paramètres, éléments du UI")]
    [SerializeField] private GameObject ecranFinPartie;
    [SerializeField] private TextMeshProUGUI textNbCoups;
    [SerializeField] private TextMeshProUGUI textNbCoupsEcranFinal;
    [SerializeField] private Slider sliderForce;
    [SerializeField] private Slider sliderVolume;
    private int nbCoups = 0;


    private void Start()
    {
        Evenements.instance.OnPartieTerminee += AfficherEcranFinal;
        Evenements.instance.OnChangementForce += ChangerForce;
        Evenements.instance.OnLancerBalle += IncrementerNbCoups;
    }

    private void IncrementerNbCoups()
    {
        nbCoups++;
        textNbCoups.text = nbCoups.ToString();
    }

    public void AfficherEcranFinal()
    {
        ecranFinPartie.SetActive(true);
        textNbCoupsEcranFinal.text = nbCoups.ToString() + " coups";
        Time.timeScale = 0f;
    }

    public void Rejouer()
    {
        // recharger la scène
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void ChangerForce(float pourcentageForce)
    {
        sliderForce.value = pourcentageForce;
    }
    

}
