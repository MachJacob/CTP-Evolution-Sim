using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionFemale : BaseReproduction
{
    private Organism self;
    private float[] offspringGenes;
    [SerializeField] private float gestationPeriod;
    [SerializeField] private float time;
    private bool noOffspring;
    private float offEnergy;
    private GameObject father;

    void Start()
    {
        offspringGenes = new float[20];
        self = GetComponent<Organism>();
        gestationPeriod = self.GetGene(Enum.GENES.GEST_PER);
        pregnant = false;
        offEnergy = 0;
    }

    void Update()
    {
        if (pregnant)
        {
            time += Time.deltaTime;
            if (time >= gestationPeriod)
            {
                GiveBirth();
                time = 0;
                pregnant = false;
            }
        }
    }

    public void TakeGenes(float[] _genes, GameObject _father)
    {
        if (pregnant)
        {
            return; //cannot get pregnant twice at once
        }
        for (int i = 0; i < _genes.Length; i++)
        {
            offspringGenes[i] = Mathf.Lerp(_genes[i], self.GetGene(i), Random.value);
        }
        pregnant = true;
        father = _father;
    }

    private void GiveBirth()
    {
        GameObject offspring = Instantiate(gameObject);
        offspring.GetComponent<Organism>().SetGenes(offspringGenes);
        offspring.GetComponent<Organism>().SetParents(this.gameObject, father);
    }

    public override void GrowOffspring(float _energy)
    {
        offEnergy += _energy;
    }
}
