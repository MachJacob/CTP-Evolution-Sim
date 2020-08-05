using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genes
{
    public List<int[]> chromosomes;
    public int numChromosomes = 10;
    public float speed;
    public float detRad;
    public float metabolism;
    public float gestPer;
    public float size;
    public float bite;
    public float carnivorous;

    public void RandomGenes()   //randomly generate generic traits
    {
        speed = Random.Range(0.5f, 1.5f);
        detRad = Random.Range(2.5f, 15.5f);
        metabolism = Random.Range(0f, 10f);
        gestPer = Random.Range(50f, 100f);
        size = Random.Range(0.5f, 2.0f);
        bite = Random.Range(0f, 25f);
        carnivorous = Random.value;
    }

    public void MergeGenes(Genes _first, Genes _second)     //standard lerping between parents
    {
        speed = Mathf.Lerp(_first.speed, _second.speed, Random.value);
        detRad = Mathf.Lerp(_first.detRad, _second.detRad, Random.value);
        metabolism = Mathf.Lerp(_first.metabolism, _second.metabolism, Random.value);
        gestPer = Mathf.Lerp(_first.gestPer, _second.gestPer, Random.value);
        size = Mathf.Lerp(_first.size, _second.size, Random.value);
        bite = Mathf.Lerp(_first.bite, _second.bite, Random.value);
        carnivorous = Mathf.Lerp(_first.carnivorous, _second.carnivorous, Random.value);
    }

    public void RandomChromosomes()    //randomly generate chromosomes
    {
        //string crom = "";

        chromosomes = new List<int[]>();
        for (int i = 0; i < numChromosomes; i++)
        {
            int[] list = new int[10];
            for(int j = 0; j < 10; j++)
            {
                list[j] = UnityEngine.Random.Range(0, 10);
                //crom += list[j].ToString();
                //crom += ", ";
            }
            chromosomes.Add(list);
        }

        //Debug.Log(crom);
    }

    public void MergeChromosomes(Genes _first, Genes _second)  //chromosonal crossover
    {
        //string crom = "";
        //string cromM = "";
        //string cromF = "";

        chromosomes = new List<int[]>();

        for(int i = 0; i < numChromosomes; i++)
        {
            int[] newChrom = new int[10];
            int p1 = UnityEngine.Random.Range(0, 10);
            int p2 = UnityEngine.Random.Range(0, 10);
            
            if (p2 <= p1)
            {
                p2 = 9;
            }
            int j = 0;
            while (j < p1)
            {
                newChrom[j] = _first.chromosomes[i][j];
                j++;
            }
            while (j <= p2)
            {
                newChrom[j] = _second.chromosomes[i][j];
                j++;
            }
            while (j < 10)
            {
                newChrom[j] = _first.chromosomes[i][j];
                j++;
            }
            chromosomes.Add(newChrom);
        }

        //for (int i = 0; i < 10; i++)
        //{
        //    crom += chromosomes[0][i].ToString();
        //    cromF += _second.chromosomes[0][i].ToString();
        //    cromM += _first.chromosomes[0][i].ToString();
        //}

        //Debug.Log(cromF);
        //Debug.Log(cromM);
        //Debug.Log(crom);
    }

    public void AssignChromosomes(List<int[]> _new)
    {
        chromosomes = _new;
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

    public int GetGene(int _ind1, int _ind2)
    {
        return chromosomes[_ind1][_ind2];
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
