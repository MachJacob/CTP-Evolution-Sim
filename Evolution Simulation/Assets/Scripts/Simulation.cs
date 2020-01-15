using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    [SerializeField] private float simulationTime;
    private float foodTick;
    [SerializeField] private GameObject fruit;
    [SerializeField][Range(0, 10)] private float timescale;

    void Start()
    {
        simulationTime = 0f;
        timescale = 1.0f;
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
    }
}
