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
    [SerializeField]
    private float speed;
    private GameObject goal;
    [SerializeField]
    private Vector3 direction;
    private float timestamp;

    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(50f, 150f);
        energy = Random.Range(50f, 150f);
        detectionRadius = Random.Range(2.5f, 7.5f);
        direction = new Vector3(0.0f, 0.0f, 0.0f);
        speed = Random.Range(0.5f, 1.5f);
        timestamp = Random.Range(-0.5f, 0.5f);
    }

    void Update()
    {
        if (health < 100)
        {
            health += Time.deltaTime / 2;
            energy -= Time.deltaTime / 2;
        }
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
        energy -= Time.deltaTime;

        
        
        Collider[] nearby = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider col in nearby)
        {
            if(col.gameObject.tag == "Fruit")
            {
                goal = col.gameObject;
                break;
            }
            else
            {
                goal = null;
            }
        }

        if (goal)
        {
            //Vector3 moveTo = Vector3.MoveTowards(transform.position, goal.transform.position, 1);
            Vector3 moveTo = goal.transform.position - transform.position;
            moveTo.Normalize();
            moveTo *= Time.deltaTime;
            transform.Translate(moveTo.x * speed, 0.0f, moveTo.z * speed, Space.World);
        }
        else
        {
            if (timestamp > 1)
            {
                direction.x += Random.Range(-5.0f, 5.0f);
                direction.z += Random.Range(-5.0f, 5.0f);
                timestamp = 0;
                if (direction.magnitude > 1)
                {
                    direction.Normalize();
                }
                direction *= Time.deltaTime;
            }
            transform.Translate(direction * speed);
        }
        timestamp += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fruit")
        {
            energy += 50;
            Destroy(other.gameObject);
        }
    }
}
