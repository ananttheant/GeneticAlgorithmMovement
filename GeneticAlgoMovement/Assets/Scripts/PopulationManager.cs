#define BasedOnDistace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour
{
    // Prfab to be instantiated
    public GameObject BotPrefab;

    //Size of the population
    public int populationSize = 50;

    //Container for the population
    List<GameObject> population = new List<GameObject>();

    //Time that has elapsed
    public static float elapsed = 0;

    //Time remaining for the current generation
    public float trialTime = 5;

    //Generation number
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();

    /// <summary>
    /// Set the Ui to see the progress
    /// </summary>
    private void OnGUI()
    {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.cyan;

        GUI.BeginGroup(new Rect(10, 10, 250, 150));
        GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
        GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
        GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}",elapsed), guiStyle);
        GUI.Label(new Rect(10, 75, 200, 30), "Population:" + populationSize, guiStyle);
        GUI.EndGroup();
    }

    private void Start()
    {
        //Create the initial popuplation Gen1
        for (int i = 0; i < populationSize; i++)
        {
            Vector3 startpos = GetRandomStartPos();

            GameObject go = Instantiate(BotPrefab, startpos, this.transform.rotation);
            go.GetComponent<Brain>().Init();

            population.Add(go);
        }
    }

    /// <summary>
    /// Gets a random start point based on this transforms position
    /// </summary>
    /// <returns></returns>
    Vector3 GetRandomStartPos()
    {
        return new Vector3(this.transform.position.x + Random.Range(-2, 2),
                                            this.transform.position.y,
                                            this.transform.position.z + Random.Range(-2, 2));
    }

    /// <summary>
    /// Checks if the time has elapsed for the current generation and breeds new population
    /// </summary>
    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }

    /// <summary>
    /// Breeds new population
    /// </summary>
    /// <remarks> 
    ///  1. Makes a sorted list by ordering the current population based on <see cref="Brain.timeAlive"/>
    ///  2. Then splits the population into half, and starts breeding them against each other
    ///  3. Destorys all the previous generation
    ///  4. Updates the generation
    /// </remarks>
    void BreedNewPopulation()
    {
#if !BasedOnDistace
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().timeAlive).ToList();
#else
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().distanceTravelled).ToList();
#endif


        population.Clear();

        //Breed better half of sorted list
        for (int i = (int)(sortedList.Count / 2f) - 1; i < sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        //Destory all parents and previous population
        for (int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    /// <summary>
    /// Takes both the parents, and breeds them i.e mutate or combine the dna
    /// </summary>
    /// <param name="parent1"></param>
    /// <param name="parent2"></param>
    /// <returns> The offspring of the parents provided</returns>
    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 startingPos = GetRandomStartPos();

        GameObject offspring = Instantiate(BotPrefab, startingPos, this.transform.rotation);

        Brain _brain = offspring.GetComponent<Brain>();

        _brain.Init();
        if (Random.Range(0, 100) == 1)
        {          
            _brain.dna.Mutate();
        }
        else
        {            
            _brain.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return offspring;
    }   
}
