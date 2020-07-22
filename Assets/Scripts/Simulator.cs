using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Simulator : MonoBehaviour
{
    public static float elapsed = 0;
    public float trialTime;
    int generation = 1;

    public GameObject[] predatorPrefabs;
    public int predatorPopulationSize;
    List<GameObject> predatorPopulation = new List<GameObject>();

    public GameObject[] preyPrefabs;
    public int preyPopulationSize;
    List<GameObject> preyPopulation = new List<GameObject>();

    public GameObject[] prefabsAgents;

    public GameObject[] restPrefabs;
    public int restPopulationSize;
    List<GameObject> restPopulation = new List<GameObject>(); 
    
    

    struct Rest
    {
      public GameObject restPrefab;
      public Vector3 location;
    }

    struct Predator
    {
      public GameObject predatorPrefab;
      public Vector3 location;
    }

    struct Prey
    {
      public GameObject preyPrefab;
      public Vector3 location;
    }

    void Awake()
    {
      InitialiseWorld();
    }

    void Update()
    {
      float sum=0;                
      for(int k=0;k<predatorPopulation.Count;k++)
            sum += predatorPopulation[k].GetComponent<PredatorBrain>().dna.GetGene(0);
      sum = sum/predatorPopulation.Count;
      Debug.Log("Final Speed "+sum);
    
      if(Input.GetKeyDown(KeyCode.S)) //Spawn Predator/Prey
      {
          int type = Random.Range(0,prefabsAgents.Length);
          Vector3 spawnPos = new Vector3(Random.Range(-5,5),1,Random.Range(-5,5)); //Hardcoded bounds
          Instantiate(prefabsAgents[1],spawnPos,prefabsAgents[1].transform.rotation);
      }  
      else if(Input.GetKeyDown(KeyCode.R)) //reset world
      {
        DestroyAllObjects(restPopulation);
      }

      elapsed += Time.deltaTime;
      if (elapsed >= trialTime)
        {
            BreedNewPredatorPopulation();
            elapsed = 0;
        }
    }

    public void DestroyAllObjects(List<GameObject> objectList)
    {
      for(int i =0 ;i<objectList.Count;i++)
        {
          Destroy(objectList[i]);
        }
        objectList.Clear();
        InitialiseWorld();
    }

    public void InitialiseWorld()
    {

      for(int i = 0;i < restPopulationSize;i++) //hardcoded bound
        {
          Rest restRandom = new Rest();
          restRandom.restPrefab = restPrefabs[Random.Range(0,restPrefabs.Length)];
          restRandom.location = new Vector3(Random.Range(-12,12),0,Random.Range(-12,12)) ;//Hardcoded bounds
          GameObject temp =(GameObject)Instantiate(restRandom.restPrefab,restRandom.location,Quaternion.identity);
          temp.transform.parent = transform; //All rest Objects are parented to ForestEnvironment
          restPopulation.Add(temp);
        }
      
      for(int i = 0;i < predatorPopulationSize;i++) //hardcoded bound
        {
          Predator predatorRandom = new Predator();
          predatorRandom.predatorPrefab = predatorPrefabs[Random.Range(0,predatorPrefabs.Length)];
          predatorRandom.location = new Vector3(Random.Range(-12,12),1,Random.Range(-12,12)) ;//Hardcoded bounds
          GameObject temp1 =(GameObject)Instantiate(predatorRandom.predatorPrefab,predatorRandom.location,Quaternion.identity);
          temp1.transform.parent = transform; //All rest Objects are parented to ForestEnvironment
          temp1.GetComponent<PredatorBrain>().Init();
          predatorPopulation.Add(temp1);
        } 
      
      for(int i = 0;i < preyPopulationSize;i++) //hardcoded bound
        {
          Prey preyRandom = new Prey();
          preyRandom.preyPrefab = preyPrefabs[Random.Range(0,preyPrefabs.Length)];
          preyRandom.location = new Vector3(Random.Range(-12,12),0.5f,Random.Range(-12,12)) ;//Hardcoded bounds
          GameObject temp2 =(GameObject)Instantiate(preyRandom.preyPrefab,preyRandom.location,Quaternion.identity);
          temp2.transform.parent = transform; //All prey Objects are parented to FopreyEnvironment
          preyPopulation.Add(temp2);
        }

    }
      
    GameObject BreedPredator(GameObject parent1,GameObject parent2)
    {
      Vector3 startingPos = new Vector3( parent1.transform.position.x,parent1.transform.position.y, parent1.transform.position.z);
      GameObject offspring = Instantiate(predatorPrefabs[0], startingPos, this.transform.rotation);
      PredatorBrain b = offspring.GetComponent<PredatorBrain>();
      if (Random.Range(0, 100) == 1)
        {
            b.Init();
            b.dna.Mutate();
        }
        else
        {
            b.Init();
            b.dna.Combine(parent1.GetComponent<PredatorBrain>().dna, parent2.GetComponent<PredatorBrain>().dna);

        }
        return offspring;
    }

    void BreedNewPredatorPopulation()
    {
        predatorPopulation.RemoveAll(item => item == null);
        List<GameObject> sortedList = predatorPopulation.OrderByDescending(o => o.GetComponent<PredatorBrain>().timeHungry).ToList();
        predatorPopulation.Clear();
        for(int i = (int)(sortedList.Count / 2.0f)-1; i < sortedList.Count - 2; i++)
        {
            
                predatorPopulation.Add(BreedPredator(sortedList[i], sortedList[i+1]));
                predatorPopulation.Add(BreedPredator(sortedList[i+1], sortedList[i]));
            
        }
  
        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }


}

