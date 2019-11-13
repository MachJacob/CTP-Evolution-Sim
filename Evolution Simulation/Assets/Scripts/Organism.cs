using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float energy;
    [SerializeField]
    private float detectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(50f, 150f);
        energy = Random.Range(50f, 150f);
        detectionRadius = Random.Range(5f, 15f);
    }

    void Update()
    {
        Vector3 translate = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        translate *= Time.deltaTime;
        transform.Translate(translate);
        energy -= Time.deltaTime;
        if (health < 100)
        {
            health += Time.deltaTime / 2;
            energy -= Time.deltaTime / 2;
        }
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
