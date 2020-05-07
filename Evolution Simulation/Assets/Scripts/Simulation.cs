using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    [SerializeField] private float simulationTime;
    private float foodTick;
    [SerializeField] private GameObject fruit;
    [SerializeField][Range(0, 10)] private float timescale;
    [SerializeField] private List<float>[] lists;
    private List<float> values;
    private float snapshot = 0f;
    public Transform creatures;
    public WindowGraph graph;
    [Range(0, 20)] public int graphID;

    void Start()
    {
        simulationTime = 0f;
        timescale = 1.0f;
        lists = new List<float>[21];
        values = new List<float>();
        for (int i = 0; i < 21; i++)
        {
            lists[i] = new List<float>();
        }
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
            lists[20].Add(creatures.childCount);

            //graph.ShowGraph(lists[20]);

            float[] sum = new float[7];        //display genes
            for (int i = 0; i < creatures.childCount; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    sum[j] += creatures.GetChild(i).GetComponent<Organism>().GetGene(j);
                }
            }
            for (int i = 0; i < 7; i++)
            {
                lists[i].Add(sum[i] /= creatures.childCount);
            }
            
            graph.ShowGraph(lists[graphID]);


            snapshot += 60f;
        }
    }

    private void FormAGNES()
    {
        for (int i = 0; i < creatures.childCount; i++)
        {

        }
    }
}
