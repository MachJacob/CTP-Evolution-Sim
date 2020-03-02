using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private float mass;
    private float energy;
    private bool over = false;

    void Start()
    {
        if (!over)
        {
            mass = 0;
        }
       
        energy = 5;
    }

    void Update()
    {
        if (mass >= 100)
        {
            GetComponent<Rigidbody>().useGravity = true;
            return;
        }
        mass += Time.deltaTime * 2;
        transform.localScale = new Vector3(mass / 200, mass / 200, mass / 200);
        GetComponent<Rigidbody>().mass = mass / 100;
    }

    public float GetMass()
    {
        return mass;
    }

    public float GetEnergy()
    {
        return energy * mass;
    }

    public void MassOverride(float _mass)
    {
        mass = _mass;
        over = true;
    }
}
