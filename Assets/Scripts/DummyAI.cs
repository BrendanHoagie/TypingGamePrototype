using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAI : MonoBehaviour
{
    public List<string> correctCombination = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitBySpell(string input)
    {
        if (correctCombination.Contains(input))
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().startRoutine = true;
            Destroy(gameObject);
            Debug.Log("Enemy with combination " + input + " destroyed!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spell")
        {
            HitBySpell(collision.gameObject.GetComponent<ProjectileController>().GetIncantation());
        }
    }
}
