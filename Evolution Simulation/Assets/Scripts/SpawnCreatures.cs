using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCreatures : MonoBehaviour
{
    public int numSpecies;
    public int numCreatures;
    public Vector2 spawnRange;
    public int numChromosomes;
    public GameObject creature;
    public Transform creatures;
    private List<Genes> species;

    void Start()
    {
        species = new List<Genes>();
        for (int i = 0; i < numSpecies; i++)
        {
            Genes idk = new Genes();
            idk.RandomGenes();
            species.Add(idk);
        }
        for (int i = 0; i < numCreatures; i++)
        {
            int spec = Random.Range(0, species.Count);
            Vector3 pos = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), 1f, Random.Range(-spawnRange.y, spawnRange.y));
            GameObject _spawn = Instantiate(creature, pos, Quaternion.identity, creatures);
            //_spawn.GetComponent<Organism>().RandomStart();
            _spawn.GetComponent<Organism>().SetStart(species[spec]);
            
        }
    }
}
