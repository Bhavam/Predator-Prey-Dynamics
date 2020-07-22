using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBrain : MonoBehaviour
{
    public int DNALength = 1;
    public float timeHungry;
    public PredatorDNA dna;

    public bool hungry = true;

     public void Init()
    {
        // init DNA
        // 0-5 speed of predator
        dna = new PredatorDNA(DNALength, 10);
        timeHungry = 0;
        hungry = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (hungry)
            timeHungry += Time.deltaTime;
    }
}
