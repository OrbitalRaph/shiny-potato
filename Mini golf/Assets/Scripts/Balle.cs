using System.Collections;
using UnityEngine;

public class Balle : MonoBehaviour
{
    [Header("Paramètres")]
    [SerializeField] private float forceMax;
    [SerializeField] private float forceMin;
    [SerializeField] private float vitesseChangementAngle;
    [SerializeField] private float vitesseChangementForce;
    [SerializeField] private float tempsReapparition;

    [Header("Autres")]
    private Rigidbody balle;
    private LineRenderer ligne;
    private Vector3 dernierePosition;
    private float vitesseMaxDeVisee = 0.05f;
    private float angle;
    private float force;


    private void Awake()
    {
        balle = GetComponent<Rigidbody>();
        ligne = GetComponent<LineRenderer>();
        force = forceMin;
        dernierePosition = transform.position;
        balle.maxAngularVelocity = 1500;
    }

    private void Start()
    {
        Evenements.instance.OnChangementDeNiveau += Reapparition;
    }

    private void Update()
    {
        if (balle.velocity.magnitude < vitesseMaxDeVisee)
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

    /// <summary>
    /// Change l'angle de la balle
    /// </summary>
    /// <param name="direction">-1 pour gauche, 1 pour droite</param>
    private void ChangerAngle(int direction)
    {
        angle += direction * vitesseChangementAngle * Time.deltaTime;
    }

    /// <summary>
    /// Change la force de la balle
    /// </summary>
    /// <param name="direction">-1 pour diminuer, 1 pour augmenter</param>
    private void ChangerForce(int direction)
    {
        if (!(force >= forceMax && direction > 0) && !(force <= forceMin && direction < 0))
        {
            force += direction * vitesseChangementForce * Time.deltaTime;
            Evenements.instance.ChangementForce(force / forceMax);
        }
    }

    /// <summary>
    /// Lance la balle
    /// </summary>
    private void Lancer()
    {
        dernierePosition = transform.position;

        GetComponent<AudioSource>().Play();

        Evenements.instance.LancerBalle();

        // Lancer la balle dans la direction de la ligne avec la force choisie horizontallement
        balle.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * force, ForceMode.Impulse);

        // reset la force
        force = forceMin;
        Evenements.instance.ChangementForce(force / forceMax);
    }

    /// <summary>
    /// Met à jour la position de la ligne de visée
    /// </summary>
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
        // detecte si la balle est proche du moulin et le fait tourner
        if (other.CompareTag("Moulin")) {
            other.GetComponent<Animator>().Play("Tourne");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // detecte si la balle est loin du moulin et l'arrête
        if (other.CompareTag("Moulin")) {
            other.GetComponent<Animator>().Play("Immobile");
        }
    }

    public IEnumerator DelaisReapparition()
    {
        yield return new WaitForSeconds(tempsReapparition);

        Reapparition();
    }

    /// <summary>
    /// Fait réapparaitre la balle à la position donnée ou au point précédent si aucune position n'est donnée
    /// </summary>
    /// <param name="position">Position de réapparition</param>
    public void Reapparition(Vector3 position = default)
    {
        Stop();
        transform.position = position == default ? dernierePosition : position;
        balle.GetComponent<TrailRenderer>().enabled = true;
    }

    private void Stop()
    {
        balle.velocity = Vector3.zero;
        balle.angularVelocity = Vector3.zero;
    }

    private void OnDestroy()
    {
        Evenements.instance.OnChangementDeNiveau -= Reapparition;
    }

}
