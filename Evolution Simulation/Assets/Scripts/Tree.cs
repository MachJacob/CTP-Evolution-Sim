using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private GameObject fruitPre;
    private int count;
    private float time;

    void Start()
    {
        time = 0.0f;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= 20 && count <= 4)
        {
            Vector3 point = Random.onUnitSphere + (Vector3.up * 1.5f);
            GameObject _fruit = Instantiate(fruitPre, point + transform.position, Quaternion.identity, transform);
            time = 0;
        }
    }
}
