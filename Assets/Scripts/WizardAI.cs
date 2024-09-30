using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WizardAI : MonoBehaviour
{
    public float moveSpeed = 2f;
    public List<string> correctCombination = new List<string>();
    private Transform target;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
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
