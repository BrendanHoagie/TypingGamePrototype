using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WizardAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    [SerializeField] private NavMeshAgent wizardAgent;
    public List<string> correctCombination = new List<string>();
    private Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
            // Old vector based movement
            //transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // AI Pathfinding movement
            wizardAgent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
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
        } 
        if (collision.gameObject.tag == "Spell")
        {
            HitBySpell(collision.gameObject.GetComponent<ProjectileController>().GetIncantation());
        }
    }
}
