using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Balle : MonoBehaviour
{
    public float forceMax;
    public float forceMin;
    public float vitesseChangementAngle;
    public float vitesseChangementForce;

    public Slider sliderForce;
    public TextMeshProUGUI textNbCoups;

    private Rigidbody balle;
    private LineRenderer ligne;
    private float angle;
    private float force;
    private int nbCoups;

    private void Awake()
    {
        balle = GetComponent<Rigidbody>();
        balle.maxAngularVelocity = 1500;

        ligne = GetComponent<LineRenderer>();

        force = forceMin;
        sliderForce.value = force/forceMax;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A)) {
            ChangerAngle(-1);
        }
        if (Input.GetKey(KeyCode.D)) {
            ChangerAngle(1);
        }
        if (Input.GetKey(KeyCode.W)) {
            ChangerForce(1);
        }
        if (Input.GetKey(KeyCode.S)) {
            ChangerForce(-1);
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            Lancer();
        }

        UpdatePositionLigne();
    }

    private void ChangerAngle(int direction) {
        angle += direction * vitesseChangementAngle * Time.deltaTime;
    }

    private void ChangerForce(int direction) {
        if (!(force >= forceMax && direction > 0) && !(force <= forceMin && direction < 0)) {
            force += direction * vitesseChangementForce * Time.deltaTime;
            sliderForce.value = force/forceMax;
        }
    }

    private void Lancer() {
        // Lancer la balle dans la direction de la ligne avec la force choisie horizontallement
        balle.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * force, ForceMode.Impulse);

        // reset la force
        force = forceMin;
        sliderForce.value = force/forceMax;

        // set le nombre de coups
        nbCoups++;
        textNbCoups.text = nbCoups.ToString();
    }

    private void UpdatePositionLigne() {
        ligne.SetPosition(0, transform.position);
        ligne.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * (force/forceMax));
    }
}
