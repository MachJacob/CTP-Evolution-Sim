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
    private GameObject goal;
    [SerializeField] private Vector3 direction;
    private float timestamp;
    private bool male;
    private BaseReproduction reproSystem;
    private Stomach stomach;
    [SerializeField] private float matingUrge;
    [SerializeField] private GameObject mother;
    [SerializeField] private GameObject father;
    [SerializeField] string currentState;

    void Start()
    {
        age = 0;

        Destroy(this.GetComponent<BaseReproduction>()); //destroy residual organs
        Destroy(this.GetComponent<Stomach>());

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
        stomach = gameObject.AddComponent<Stomach>();
        stomach.SetMetabolism(genes[(int)Enum.GENES.METABOLISM]);
        stomach.SetCarnivious(genes[(int)Enum.GENES.CARNIVOROUS]);
    }

    void Update()
    {
        float gainEnergy = 0;
        float lossEnergy = 0;
        List<GameObject> fruit = new List<GameObject>();
        List<GameObject> females = new List<GameObject>();

        age += Time.deltaTime;
        //if (health < 100)   //health
        //{
        //    health += Time.deltaTime / 2;
        //    energy -= Time.deltaTime / 2;
        //}
        if (energy <= 0)
        {
            Destroy(gameObject);
        }

        lossEnergy += Time.deltaTime;

        gainEnergy += stomach.Digest();

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
            Fruit fruit = other.gameObject.GetComponent<Fruit>();
            stomach.AddMass(fruit.GetMass(), fruit.GetEnergy(), true);
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
        stomach.SetMetabolism(genes[(int)Enum.GENES.METABOLISM]);
        stomach.SetCarnivious(genes[(int)Enum.GENES.CARNIVOROUS]);
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
        genes[(int)Enum.GENES.BITE] = Random.Range(0f, 10f);
        genes[(int)Enum.GENES.CARNIVOROUS] = Random.value;
        health = Random.Range(50f, 150f);
        energy = Random.Range(50f, 150f);
        direction = new Vector3(0.0f, 0.0f, 0.0f);
        detectionRadius = genes[(int)Enum.GENES.DET_RAD];
        speed = genes[(int)Enum.GENES.SPEED];
        timestamp = Random.Range(-0.5f, 0.5f);
    }

    public void SetParents(GameObject _mother, GameObject _father)
    {
        mother = _mother;
        father = _father;
    }

    public void Mutate()
    {
        if (Random.Range(0, 100) > 95)
        {
            genes[Random.Range(0, 20)] += Random.Range(-0.5f, 0.5f);
        }
    }
}
