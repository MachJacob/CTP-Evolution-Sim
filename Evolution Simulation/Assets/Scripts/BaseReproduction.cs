using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseReproduction : MonoBehaviour
{
    [SerializeField] protected bool pregnant;

    public bool IsPregnant()
    {
        return pregnant;
    }

    public virtual void GrowOffspring(float _energy) { }
    public virtual void Impregnate(ReproductionFemale _female) { }
}
