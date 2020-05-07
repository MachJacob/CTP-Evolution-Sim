using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproductionMale : BaseReproduction
{
    private Organism self;
    private ReproductionFemale female;

    void Start()
    {
        self = GetComponent<Organism>();
    }

    void Update()
    {
        
    }

    public override void Impregnate(ReproductionFemale _female)
    {
        _female.TakeGenes(self.GetAllGenes().GetFloat(), this.gameObject);
    }
}
