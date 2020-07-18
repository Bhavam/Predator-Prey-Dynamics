using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject[] prefabsAgents;
    List<GameObject> restObjects;
    public GameObject[] restPrefabs;

    struct RestLocation
    {
      public GameObject restPrefab;
      public Vector3 location;
    }
    void Awake()
    {
      InitialiseWorld();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.S)) //Spawn Predator/Prey
      {
          int type = Random.Range(0,prefabsAgents.Length);
          Vector3 spawnPos = new Vector3(Random.Range(-5,5),1,Random.Range(-5,5)); //Hardcoded bounds
          Instantiate(prefabsAgents[type],spawnPos,prefabsAgents[type].transform.rotation);
      }  
      else if(Input.GetKeyDown(KeyCode.R)) //reset world
      {
        for(int i =0 ;i<restObjects.Count;i++)
        {
          Destroy(restObjects[i]);
        }
        restObjects.Clear();
        InitialiseWorld();
      }
    }

    public void InitialiseWorld()
    {
      restObjects = new List<GameObject>(); //List to keep track of rest objects
      //New Random rest object declaration
      

      for(int i = 0;i < 20;i++) //hardcoded bound
      {
        RestLocation restRandom = new RestLocation();
        restRandom.restPrefab = restPrefabs[Random.Range(0,restPrefabs.Length)];
        restRandom.location = new Vector3(Random.Range(-12,12),0,Random.Range(-12,12)) ;//Hardcoded bounds
        GameObject temp =(GameObject)Instantiate(restRandom.restPrefab,restRandom.location,Quaternion.identity);
        temp.transform.parent = transform; //All rest Objects are parented to ForestEnvironment
        restObjects.Add(temp);
      }
    }
}
