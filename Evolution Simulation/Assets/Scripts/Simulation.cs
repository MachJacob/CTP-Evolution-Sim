using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    [SerializeField] private float simulationTime;
    private float foodTick;
    [SerializeField] private GameObject fruit;
    [SerializeField][Range(0, 10)] private float timescale;
    private List<float> values;
    private float snapshot = 0f;
    public Transform creatures;
    public WindowGraph graph;

    void Start()
    {
        simulationTime = 0f;
        timescale = 1.0f;
        values = new List<float>();
    }

    void Update()
    {
        Time.timeScale = timescale;
        simulationTime += Time.deltaTime;
        foodTick += Time.deltaTime;

        if (foodTick > 20)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-50, 50), 2f, Random.Range(-50, 50));
                GameObject _spawn = Instantiate(fruit, pos, Quaternion.identity);
                _spawn.GetComponent<Fruit>().MassOverride(100);
            }
            foodTick = 0;
        }

        if (simulationTime >= snapshot)
        {
            values.Add(creatures.childCount);
            graph.ShowGraph(values);

            //float sum = 0;        //display genes
            //for (int i = 0; i < creatures.childCount; i++)
            //{
            //    sum += creatures.GetChild(i).GetComponent<Organism>().GetGene(0);
            //}
            //values.Add(sum /= creatures.childCount);
            //graph.ShowGraph(values);


            snapshot += 60f;
        }
    }
}
