using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreatures : MonoBehaviour
{
    public GameObject creature;
    public int spawn;
    public Vector2 range;
    public Transform creatures;

    void Start()
    {
        for (int i = 0; i < spawn; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-range.x, range.x), 0.5f, Random.Range(-range.y, range.y));
            GameObject _spawn = Instantiate(creature, pos, Quaternion.identity, creatures);
            _spawn.GetComponent<Organism>().RandomStart();
        }
    }

    void Update()
    {
        
    }
}
