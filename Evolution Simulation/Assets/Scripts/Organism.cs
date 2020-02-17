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
    [SerializeField] private GameObject goal;
    private Vector3 direction;
    private float timestamp;
    private bool male;
    private BaseReproduction reproSystem;
    private Stomach stomach;
    [SerializeField] private float matingUrge;
    [SerializeField] private GameObject mother;
    [SerializeField] private GameObject father;
    [SerializeField] string currentState;
    private bool hunting;
    private bool alive;
    [SerializeField] private float pause;
    private float[] input = new float[5];
    private NeuralNet net;

    void Start()
    {
        age = 0;
        alive = true;

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

        net.FeedForward(input);
    }

    void Update()
    {
        float gainEnergy = 0;
        float lossEnergy = 0;
        List<GameObject> fruit = new List<GameObject>();
        List<GameObject> creatures = new List<GameObject>();
        List<GameObject> corpses = new List<GameObject>();

        if (health <= 0)
        {
            alive = false;
            return;
        }

        if (pause > 0)
        {
            pause -= Time.deltaTime;
        }

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
            if (col.gameObject.CompareTag("Fruit"))
            {
                fruit.Add(col.gameObject);
            }
            else if (col.gameObject.CompareTag("Organism") && col.gameObject != this.gameObject)
            {
                if (col.gameObject.GetComponent<Organism>().alive)
                {
                    creatures.Add(col.gameObject);
                }
                else
                {
                    corpses.Add(col.gameObject);
                }
            }
        }

        if (energy < 100)   //TODO: refactor        find food
        {
            hunting = false;
            float lowestDist = 50000;
            GameObject closest = null;
            float carn = stomach.CarnVal();
            foreach (GameObject food in fruit)
            {
                if (Vector3.Distance(transform.position, food.transform.position) * (1 - carn) < lowestDist)
                {
                    lowestDist = Vector3.Distance(transform.position, food.transform.position) * (1 - carn);
                    closest = food;
                    currentState = "Find Fruit";
                }
            }
            foreach(GameObject prey in creatures)
            {
                if (Vector3.Distance(transform.position, prey.transform.position) * carn * 0.5f < lowestDist)
                {
                    lowestDist = Vector3.Distance(transform.position, prey.transform.position) * carn * 0.5f;
                    closest = prey;
                    hunting = true;
                    currentState = "Hunt";
                }
            }
            foreach (GameObject corpse in corpses)
            {
                if (Vector3.Distance(transform.position, corpse.transform.position) * carn < lowestDist)
                {
                    lowestDist = Vector3.Distance(transform.position, corpse.transform.position) * carn;
                    closest = corpse;
                    hunting = true;
                    currentState = "Find Carcass";
                }
            }
            goal = closest;
        }
        else if (matingUrge > 100)      //find mate
        {
            float lowestDist = 50000;
            GameObject closest = null;
            foreach (GameObject potMate in creatures)
            {
                if (!potMate.GetComponent<Organism>().male && Vector3.Distance(transform.position, potMate.transform.position) < lowestDist)
                {
                    lowestDist = Vector3.Distance(transform.position, potMate.transform.position);
                    closest = potMate;
                }
            }
            goal = closest;
            currentState = "Find Mate";
        }
        else
        {
            goal = null;
        }

        if (goal && pause <= 0)   
        {
            //Vector3 moveTo = Vector3.MoveTowards(transform.position, goal.transform.position, 1);
            Vector3 moveTo = goal.transform.position - transform.position;
            moveTo.Normalize();
            moveTo *= Time.deltaTime;
            transform.Translate(moveTo.x * speed, 0.0f, moveTo.z * speed, Space.World);
        }
        else if (pause <= 0)       //wander
        {
            if (timestamp > 5)
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
        if (other.gameObject.CompareTag("Fruit"))
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
                pause = 2;
            }
            else if (hunting)
            {
                if (other.gameObject.GetComponent<Organism>().alive)
                {
                    other.gameObject.GetComponent<Organism>().DealDamage(genes[(int)Enum.GENES.BITE]);
                    other.gameObject.transform.Translate((other.gameObject.transform.position - transform.position) * 0.1f);
                    pause = 2;
                }
                else
                {
                    Destroy(other.gameObject);
                    stomach.AddMass(500, 1000, false);
                }
                
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
        //stomach.SetMetabolism(genes[(int)Enum.GENES.METABOLISM]);
        //stomach.SetCarnivious(genes[(int)Enum.GENES.CARNIVOROUS]);
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
        genes[(int)Enum.GENES.BITE] = Random.Range(0f, 25f);
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

    public void DealDamage(float _dam)
    {
        health -= _dam;
    }
}
