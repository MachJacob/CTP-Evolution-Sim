using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genes
{
    public float speed;
    public float detRad;
    public float metabolism;
    public float gestPer;
    public float size;
    public float bite;
    public float carnivorous;

    public void AssignGenes(float[] _genes)
    {
        _genes[0] = speed;
        _genes[1] = detRad;
        _genes[2] = metabolism;
        _genes[3] = gestPer;
        _genes[4] = bite;
        _genes[5] = size;
        _genes[6] = carnivorous;
    }

    public void Merge(Genes _first, Genes _second)
    {
        speed =         Mathf.Lerp(_first.speed, _second.speed, UnityEngine.Random.value);
        detRad =        Mathf.Lerp(_first.detRad, _second.detRad, UnityEngine.Random.value);
        metabolism =    Mathf.Lerp(_first.metabolism, _second.metabolism, UnityEngine.Random.value);
        gestPer =       Mathf.Lerp(_first.gestPer, _second.gestPer, UnityEngine.Random.value);
        size =          Mathf.Lerp(_first.size, _second.size, UnityEngine.Random.value);
        bite =          Mathf.Lerp(_first.bite, _second.bite, UnityEngine.Random.value);
        carnivorous =   Mathf.Lerp(_first.carnivorous, _second.carnivorous, UnityEngine.Random.value);
    }

    public float[] GetFloat()
    {
        float[] temp = new float[7];
        temp[0] = speed;
        temp[1] = detRad;
        temp[2] = metabolism;
        temp[3] = gestPer;
        temp[4] = size;
        temp[5] = bite;
        temp[6] = carnivorous;
        return temp;
    }

    public void Random()
    {

    }

    public float Similarity(Genes _compare)
    {
        float meanSpeed = (speed + _compare.speed) / 3f;
        float meanDet = (detRad + _compare.detRad) / 31f;
        float meanMeta = (metabolism + _compare.metabolism) / 20f;
        float meanGest = (gestPer + _compare.gestPer) / 200f;
        float meanSize = (size + _compare.size) / 4f;
        float meanBite = (bite + _compare.bite) / 50f;
        float meanCarn = (carnivorous + _compare.carnivorous) / 2f;

        return (meanSpeed + meanDet + meanMeta + meanGest + meanSize + meanBite + meanCarn) / 7;
    }

}
