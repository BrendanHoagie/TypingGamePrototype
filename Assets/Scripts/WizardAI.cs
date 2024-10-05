using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WizardAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopRadius = 10f; 
    public List<string> correctCombination = new List<string>();

    private Transform target;
<<<<<<< Updated upstream
    private PlayerController playerController;
=======
    [SerializeField] private NavMeshAgent wizardAgent;
>>>>>>> Stashed changes
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Awake()
    {
        // Get NavMesh Agent component and disable rotation and axis updates
        wizardAgent = GetComponent<NavMeshAgent>();
        wizardAgent.updateRotation = false;
        wizardAgent.updateUpAxis = false;
    }

    void Update()
    {
        if (target)
        {
<<<<<<< Updated upstream
            // AI Pathfinding movement
            wizardAgent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
=======
            float distanceToPlayer = Vector3.Distance(target.position, transform.position);

            if (distanceToPlayer > stopRadius)
            {
                // AI Pathfinding movement
                wizardAgent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
            } else
            {
                wizardAgent.ResetPath();
                wizardAgent.velocity = Vector3.zero;
            }
            
>>>>>>> Stashed changes
        }
    }

    public void setCorrectCombination(string input)
    {
        correctCombination.Add(input);
    }

    public void HitBySpell(string input)
    {
        if (correctCombination.Contains(input))
        {
            Destroy(gameObject);
            Debug.Log("Enemy with combination " + input + " destroyed!");
        }
    }

    // currently a 1-hit kill
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Destroy(collision.gameObject);
            playerController.TakeDamage();

        } 
        if (collision.gameObject.tag == "Spell")
        {
            HitBySpell(collision.gameObject.GetComponent<ProjectileController>().GetIncantation());
        }
    }
}
