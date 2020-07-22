using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorBrain : MonoBehaviour
{
    public int DNALength = 1;
    public float timeAlive;
    public PredatorDNA dna;

    public bool alive = true;

     public void Init()
    {
        // init DNA
        // 0-5 speed of predator
        dna = new PredatorDNA(DNALength, 6);
        timeAlive = 0;
        alive = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (alive)
            timeAlive += Time.deltaTime;
    }
}
