using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : MonoBehaviour
{
    [SerializeField] private float[] genes;
    [SerializeField] private float age;
    [SerializeField] private float health;
    [SerializeField] private float energy;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float speed;
    private float metabolism;
    private GameObject goal;
    [SerializeField] private Vector3 direction;
    private float timestamp;
    private bool male;
    [SerializeField][Range(0,1)] private float carnivorous;
    private float vegMass;
    private float vegEnergy;
    private float meatMass;
    private float meatEnergy;
    private BaseReproduction reproSystem;
    [SerializeField] private float matingUrge;
    [SerializeField] private GameObject mother;
    [SerializeField] private GameObject father;
    [SerializeField] string currentState;

    void Start()
    {
        age = 0;
        Destroy(this.GetComponent<BaseReproduction>());
        int sex = Random.Range(0, 2);
        if (sex == 0)
        {
            male = true;
            reproSystem = gameObject.AddComponent<ReproductionMale>();
        }
        else
        {
            male = false;
            reproSystem = gameObject.AddComponent<ReproductionFemale>();
        }

        vegMass = 0;
        vegEnergy = 0;
        meatMass = 0;
        meatEnergy = 0;
    }

    void Update()
    {
        float gainEnergy = 0;
        float lossEnergy = 0;
        List<GameObject> fruit = new List<GameObject>();
        List<GameObject> females = new List<GameObject>();

        age += Time.deltaTime;
        if (health < 100)   //health
        {
            health += Time.deltaTime / 2;
            energy -= Time.deltaTime / 2;
        }
        if (energy <= 0)
        {
            Destroy(gameObject);
        }

        lossEnergy += Time.deltaTime;

        if (vegMass > 0 || meatMass > 0)    //digestion
        {
            float sumMass = vegMass + meatMass;
            float vegRatio = vegMass / sumMass;
            if (vegMass > 0)
            {
                float vegDigest = Time.deltaTime * metabolism * vegRatio;
                float vegGain = (vegDigest / vegMass) * vegEnergy;
                vegMass -= vegDigest;
                vegEnergy -= vegGain;
                gainEnergy += vegGain * (1 - carnivorous);
            }
            if (meatMass > 0)
            {
                float meatDigest = Time.deltaTime * metabolism * (1 - vegRatio);
                float meatGain = (meatDigest / meatMass) * meatEnergy;
                meatMass -= meatDigest;
                meatEnergy -= meatGain;
                gainEnergy += meatGain * carnivorous;
            }
        }

        Collider[] nearby = Physics.OverlapSphere(transform.position, detectionRadius); //detect nearby objects
        foreach (Collider col in nearby)
        {
            if(col.gameObject.CompareTag("Fruit"))
            {
                fruit.Add(col.gameObject);
                break;
            }
            else if (col.gameObject.CompareTag("Organism") && !col.gameObject.GetComponent<Organism>().male)
            {
                females.Add(col.gameObject);
                break;
            }
        }

        if (energy < 100)   //TODO: refactor
        {
            float lowestDist = 50000;
            GameObject closest = null;
            foreach (GameObject food in fruit)
            {
                if (Vector3.Distance(transform.position, food.transform.position) < lowestDist)
                {
                    lowestDist = Vector3.Distance(transform.position, food.transform.position);
                    closest = food;
                }
            }
            goal = closest;
            currentState = "Find Food";
        }
        else if (matingUrge > 100)
        {
            float lowestDist = 50000;
            GameObject closest = null;
            foreach (GameObject mate in females)
            {
                if (Vector3.Distance(transform.position, mate.transform.position) < lowestDist)
                {
                    lowestDist = Vector3.Distance(transform.position, mate.transform.position);
                    closest = mate;
                }
            }
            goal = closest;
            currentState = "Find Mate";
        }
        else
        {
            goal = null;
        }

        if (goal)   
        {
            //Vector3 moveTo = Vector3.MoveTowards(transform.position, goal.transform.position, 1);
            Vector3 moveTo = goal.transform.position - transform.position;
            moveTo.Normalize();
            moveTo *= Time.deltaTime;
            transform.Translate(moveTo.x * speed, 0.0f, moveTo.z * speed, Space.World);
        }
        else    //wander
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
            currentState = "Wander";
        }

        if (reproSystem.IsPregnant())   //spend energy on offspring
        {
            float offEnergy = gainEnergy * 0.15f;
            reproSystem.GrowOffspring(offEnergy);
            gainEnergy -= offEnergy;
        }

        if (male)
        {
            matingUrge += Time.deltaTime;
        }

        timestamp += Time.deltaTime;
        energy += gainEnergy;
        energy -= lossEnergy;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Fruit")
        {
            vegMass += other.gameObject.GetComponent<Fruit>().GetMass();
            vegEnergy += other.gameObject.GetComponent<Fruit>().GetEnergy();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Organism"))
        {
            if (!other.gameObject.GetComponent<Organism>().male && matingUrge > 100)
            {
                reproSystem.Impregnate(other.gameObject.GetComponent<ReproductionFemale>());
                matingUrge = 0;
            }
        }
    }

    public float[] GetAllGenes()
    {
        return genes;
    }

    public void SetGenes(float[] _genes)
    {
        genes = _genes;

        detectionRadius = genes[(int)Enum.GENES.DET_RAD];
        speed = genes[(int)Enum.GENES.SPEED];
        metabolism = genes[(int)Enum.GENES.METABOLISM];
    }

    public float GetGene(Enum.GENES _gene)
    {
        return genes[(int)_gene];
    }

    public float GetGene(int _gene)
    {
        return genes[_gene];
    }

    public void RandomStart()
    {
        genes = new float[20];
        genes[(int)Enum.GENES.SPEED] = Random.Range(0.5f, 1.5f);
        genes[(int)Enum.GENES.DET_RAD] = Random.Range(2.5f, 15.5f);
        genes[(int)Enum.GENES.METABOLISM] = Random.Range(0f, 10f);
        genes[(int)Enum.GENES.GEST_PER] = Random.Range(50f, 100f);
        health = Random.Range(50f, 150f);
        energy = Random.Range(50f, 150f);
        direction = new Vector3(0.0f, 0.0f, 0.0f);
        detectionRadius = genes[(int)Enum.GENES.DET_RAD];
        speed = genes[(int)Enum.GENES.SPEED];
        metabolism = genes[(int)Enum.GENES.METABOLISM];
        timestamp = Random.Range(-0.5f, 0.5f);
        carnivorous = Random.value;
    }

    public void SetParents(GameObject _mother, GameObject _father)
    {
        mother = _mother;
        father = _father;
    }
}
