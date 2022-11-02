using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roches : MonoBehaviour
{
    public GameObject[] rochers;

    public int nbRochers;
    public float rayonSpawn;
    
    private void Awake()
    {
        // On crée un nombre donné de rochers aléatoire dans un rayon donné dans la scène à l'initialisation
        for (int i = 0; i < nbRochers; i++)
        {
            Vector3 position = new Vector3(transform.position.x + Random.Range(-rayonSpawn, rayonSpawn), transform.position.y, transform.position.z + Random.Range(-rayonSpawn, rayonSpawn));
            int rocher = Random.Range(0, rochers.Length);
            int rotation = Random.Range(0, 360);
            Instantiate(rochers[rocher], position, Quaternion.Euler(0, rotation, 0));
        }
    }
}
