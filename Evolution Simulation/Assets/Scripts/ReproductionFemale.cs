using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionFemale : BaseReproduction
{
    private Organism self;
    private Genes offspringGenes;
    [SerializeField] private float gestationPeriod;
    [SerializeField] private float time;
    private bool noOffspring;
    private float offEnergy;
    private GameObject father;

    void Start()
    {
        offspringGenes = new Genes();
        self = GetComponent<Organism>();
        gestationPeriod = self.GetGene(GENES.GEST_PER);
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

    public void TakeGenes(Genes _genes, GameObject _father)
    {
        if (pregnant) return; //cannot get pregnant twice at once
        Genes ownGenes = self.GetAllGenes();
        offspringGenes.MergeChromosomes(_genes, ownGenes);
        pregnant = true;
        father = _father;
    }

    private void GiveBirth()
    {
        GameObject offspring = Instantiate(gameObject, transform.parent);
        offspring.GetComponent<Organism>().SetGenes(offspringGenes);
        offspring.GetComponent<Organism>().SetParents(this.gameObject, father);
        offspring.GetComponent<Organism>().Mutate();
    }

    public override void GrowOffspring(float _energy)
    {
        offEnergy += _energy;
    }
}
