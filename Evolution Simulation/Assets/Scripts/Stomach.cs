using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomach : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float carnivorous;
    private float metabolism;
    [SerializeField] private float vegMass;
    private float vegEnergy;
    [SerializeField] private float meatMass;
    private float meatEnergy;
    void Start()
    {
        vegMass = 0;
        vegEnergy = 0;
        meatMass = 0;
        meatEnergy = 0;
    }

    void Update()
    {
        
    }

    public void SetMetabolism(float _met)
    {
        metabolism = _met;
    }

    public void SetCarnivious(float _carn)
    {
        carnivorous = _carn;
    }

    public void AddMass(float _mass, float _energy, bool _veg)
    {
        if (_veg)
        {
            vegMass += _mass;
            vegEnergy += _energy;
        }
        else
        {
            meatMass += _mass;
            meatEnergy += _energy;
        }
    }

    public float Digest()
    {
        float digEnergy = 0;
        if (vegMass > 0 || meatMass > 0)    //digestion
        {
            float sumMass = vegMass + meatMass;
            float vegRatio = vegMass / sumMass;
            if (vegMass > 0)
            {
                float vegDigest = Time.deltaTime * metabolism * vegRatio;
                float vegGain = (vegDigest / vegMass) * vegEnergy;
                vegMass -= vegDigest;
                vegEnergy -= vegGain;
                digEnergy += vegGain * (1 - carnivorous);
            }
            if (meatMass > 0)
            {
                float meatDigest = Time.deltaTime * metabolism * (1 - vegRatio);
                float meatGain = (meatDigest / meatMass) * meatEnergy;
                meatMass -= meatDigest;
                meatEnergy -= meatGain;
                digEnergy += meatGain * carnivorous;
            }
        }

        return digEnergy;
    }
}
