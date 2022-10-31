using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Balle : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] private float forceMax;
    [SerializeField] private float forceMin;
    [SerializeField] private float vitesseChangementAngle;
    [SerializeField] private float vitesseChangementForce;

    [SerializeField] private float tempsReapparition;
    
    [Header("UI")]
    public Slider sliderForce;
    public TextMeshProUGUI textNbCoups;
    public EcranFinal ecranFinal;

    [Header("Autres")]
    private Rigidbody balle;
    private LineRenderer ligne;
    private Vector3 dernierePosition;
    private float angle;
    private float force;
    private int nbCoups;


    private void Awake()
    {
        balle = GetComponent<Rigidbody>();
        ligne = GetComponent<LineRenderer>();
        angle = transform.eulerAngles.y;
        force = forceMin;
        nbCoups = 0;
        dernierePosition = transform.position;
        balle.maxAngularVelocity = 1500;
        sliderForce.value = force / forceMax;
    }

    private void Start()
    {
        Evenements.instance.OnChangementDeNiveau += Reapparition;
        Evenements.instance.OnPartieTerminee += PartieTerminee;
    }

    private void Update()
    {
        if (balle.velocity.magnitude < 0.05f)
        {

            if (Input.GetKey(KeyCode.A))
            {
                ChangerAngle(-1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                ChangerAngle(1);
            }
            if (Input.GetKey(KeyCode.W))
            {
                ChangerForce(1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                ChangerForce(-1);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Lancer();
            }
            UpdatePositionLigne();
        }
        else
        {
            ligne.enabled = false;
        }

        if (Input.GetMouseButton(1))
        {
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * 5);
        }
    }

    private void ChangerAngle(int direction)
    {
        angle += direction * vitesseChangementAngle * Time.deltaTime;
    }

    private void ChangerForce(int direction)
    {
        if (!(force >= forceMax && direction > 0) && !(force <= forceMin && direction < 0))
        {
            force += direction * vitesseChangementForce * Time.deltaTime;
            sliderForce.value = force / forceMax;
        }
    }

    private void Lancer()
    {
        dernierePosition = transform.position;

        GetComponent<AudioSource>().Play();

        // Lancer la balle dans la direction de la ligne avec la force choisie horizontallement
        balle.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * force, ForceMode.Impulse);

        // reset la force
        force = forceMin;
        sliderForce.value = force / forceMax;

        // set le nombre de coups
        nbCoups++;
        textNbCoups.text = nbCoups.ToString();
    }

    private void UpdatePositionLigne()
    {
        ligne.enabled = true;
        ligne.SetPosition(0, transform.position);
        ligne.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * (force / forceMax));
    }

    private void OnTriggerEnter(Collider other)
    {
        // detecte si la balle tombe dans l'eau et la remet au point précédent
        if (other.CompareTag("Eau"))
        {
            other.GetComponent<AudioSource>().Play();
            balle.GetComponent<TrailRenderer>().enabled = false;
            balle.GetComponent<ParticleSystem>().Play();
            Stop();
            
            StartCoroutine(DelaisReapparition());
        }
    }

    public IEnumerator DelaisReapparition()
    {
        yield return new WaitForSeconds(tempsReapparition);

        Reapparition();
    }

    public void Reapparition(Vector3 position = default)
    {
        Stop();
        transform.position = position == default ? dernierePosition : position;
        balle.GetComponent<TrailRenderer>().enabled = true;
    }

    public void PartieTerminee()
    {
        ecranFinal.PartieTerminee(nbCoups);
    }

    private void Stop()
    {
        balle.velocity = Vector3.zero;
        balle.angularVelocity = Vector3.zero;
    }

    private void OnDestroy()
    {
        Evenements.instance.OnChangementDeNiveau -= Reapparition;
        Evenements.instance.OnPartieTerminee -= PartieTerminee;
    }

}
