using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

/// <summary>
/// It is the controller of the character
/// which sits between the character and the DNA.
/// Reads the DNA and determines what to do and then tells the actua character what to do
/// Here the character can be an agent which is controlled by a brain
/// </summary>
[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour
{
    //For this case we'll use a dna of length only one for testing
    public int DNALength = 1;

    //Time the character was alive for
    public float timeAlive;

    public float distanceTravelled = 0;
    public Vector3 startPosition;

    //It's DNA sequesnce
    public DNA dna;

    //Character ref
    private ThirdPersonCharacter m_Character;

    //For the movement
    private Vector3 m_Move;

    //To jump or not
    private bool m_Jump;

    //Is alive or not
    bool alive = true;

    /// <summary>
    /// Checks if the character has collided with a collider dead to set <see cref="alive"/>
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "dead")
        {
            alive = false;
        }
    }

    /// <summary>
    /// To be called when we create the prefab rfom the Population Manager
    /// </summary>
    public void Init()
    {
        ///<see cref="DNA.genes"/>
        //initialise DNA
        // 0 forward
        // 1 back
        // 2 left
        // 3 right
        // 4 jump
        // 5 crouch

        dna = new DNA(DNALength, 6);

        m_Character = GetComponent<ThirdPersonCharacter>();
        startPosition = transform.position;
        timeAlive = 0;
        alive = true;
    }

    private void FixedUpdate()
    {
        //read DNA
        float h = 0;
        float v = 0;

        bool crouch = false;

        if (dna.GetGene(0) == 0)
            v = 1;
        else if (dna.GetGene(0) == 1)
            v = -1;
        else if (dna.GetGene(0) == 2)
            h = -1;
        else if (dna.GetGene(0) == 3)
            h = 1;
        else if (dna.GetGene(0) == 4)
            m_Jump = true;
        else if (dna.GetGene(0) == 5)
            crouch = true;

        //Make the Vector for the movement
        m_Move = v * Vector3.forward + h * Vector3.right;

        //Submit the movement , crouch and Jump input to the character
        m_Character.Move(m_Move, crouch, m_Jump);

        //Reset
        m_Jump = false;

        //Add Time
        if (alive)
        {
            timeAlive += Time.deltaTime;
            distanceTravelled = Vector3.Distance(this.transform.position, startPosition);
        }
    }


}
