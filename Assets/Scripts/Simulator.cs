using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject[] agents;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.S))
      {
          int type = Random.Range(0,prefabs.Length);
          Vector3 spawnPos = new Vector3(Random.Range(-5,5),0,Random.Range(-5,5));
          Instantiate(prefabs[type],spawnPos,prefabs[type].transform.rotation);
      }  
    }
}
