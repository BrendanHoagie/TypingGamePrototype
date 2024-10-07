using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public enum WizardBehavior 
{ 
    NEUTRAL = 0, // In theory never used, I like having a debug state
    CHASING = 1,
    CASTING = 2
}

public class WizardAI : MonoBehaviour
{
    System.Random random = new System.Random();

    public float moveSpeed = 2f;
    public float playerDamageRadius = 2.0f;
    public List<string> correctCombination = new List<string>();

    // casting spells & chasing -> needs a refactor BADLY
    public int aiChaseBias = 45;
    private float curTime = 0f;
    [SerializeField] private float aiDecisionTime = 5f;
    private WizardBehavior behavior;
    private bool currentlyCasting = false;
   
    // this part is bad (should pull from children in setup and store as array) but I can't get to work any other way
    public Transform hat;
    public Transform staff;
    public Transform body;
    public Transform robe;
    
    [SerializeField] private Transform spellSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private GameObject floatingTextPrefab;
    private GameObject floatingText = null;
    private float textHoverDistance = 2f;
    private string spellText;
    private SpellMaker spellMaker;

    [SerializeField] private float playerIFrameLength = 3f;
    private float playerIFrameTimer;
    private bool playerCanBeHit = true;

    private Transform target;
    private PlayerController playerController;
    [SerializeField] private NavMeshAgent wizardAgent;

void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spellMaker = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<SpellMaker>();
        spellText = "";
        playerIFrameTimer = playerIFrameLength;
        behavior = WizardBehavior.CHASING;
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
        // move to destination, reset rotation of sprites
        wizardAgent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
        hat.transform.rotation = Quaternion.Euler(0, 0, 0);
        staff.transform.rotation = Quaternion.Euler(0, 0, 0);
        body.transform.rotation = Quaternion.Euler(0, 0, 0);
        robe.transform.rotation = Quaternion.Euler(0, 0, 0);

        // in the process of a decision
        if (curTime < aiDecisionTime)
        {
            curTime += Time.deltaTime;
            DisplayText();
            // while in the decision lockout period, continue to run what was happening
            switch (behavior)
            {
                case WizardBehavior.CHASING:
                    wizardAgent.SetDestination(new Vector3(target.position.x, target.position.y, transform.position.z));
                    break;
                case WizardBehavior.CASTING:
                    CastSpell();
                    wizardAgent.ResetPath();
                    wizardAgent.velocity = Vector3.zero;
                    break;
            }

            /* // check for collision manually since colliders aren't working
             float distanceToPlayer = Vector3.Distance(target.position, transform.position);
             if (distanceToPlayer < playerDamageRadius)
             {
                 // no i frames
                 if (playerCanBeHit)
                 {
                     playerController.TakeDamage();
                     playerIFrameTimer = playerIFrameLength;
                     playerCanBeHit = false;
                     return;
                 }

                 // check if still in i frames
                 playerIFrameTimer -= Time.deltaTime;
                 if (playerIFrameTimer <= 0f) playerCanBeHit = true;
             }*/
            return;
        }

        curTime = 0f;
        behavior = random.Next(0, 100) <= aiChaseBias ? WizardBehavior.CHASING : WizardBehavior.CASTING;
        currentlyCasting = false;
    }

    private void FixedUpdate()
    {
        Vector2 aimDirection = target.position - transform.position;
        gameObject.GetComponent<Rigidbody2D>().rotation = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    }

    void CastSpell()
    {
        // don't start a cast every frame
        if (currentlyCasting) return;
        currentlyCasting = true;
        StartCoroutine(Casting());
    }

    IEnumerator Casting()
    {
        for (int i = 0; i < 8; i++)
        {
            spellText += (char)random.Next(65, 90);
            DisplayText();
            yield return new WaitForSeconds(aiDecisionTime / 8f);
        }

        // ready to fire
        GameObject spellProjectile = Instantiate(projectilePrefab, spellSpawnPoint.position, spellSpawnPoint.rotation);
        spellProjectile.GetComponent<ProjectileController>().SetIncantation(spellText);
        spellProjectile.GetComponent<Rigidbody2D>().AddForce(spellSpawnPoint.up * projectileSpeed, ForceMode2D.Impulse);
        // this is really terrible encapsulation
        Spell element = spellMaker.elements[random.Next(0, spellMaker.elements.Count - 1)];
        Spell projectile = spellMaker.projectiles[random.Next(0, spellMaker.projectiles.Count - 1)];
        SpriteRenderer render = spellProjectile.GetComponent<SpriteRenderer>();
        render.sprite = projectile.ProjectileShape;
        render.color = spellMaker.GetElementColor(element.Element);
        Destroy(spellProjectile, 0.8f); //Despawn after a set time regarldess of hit or not
        spellText = "";
        yield break;
    }

    void DisplayText()
    {
        if (floatingTextPrefab)
        {
            if (!floatingText)
            {
                floatingText = Instantiate(floatingTextPrefab, new Vector3(transform.position.x, transform.position.y + textHoverDistance, transform.position.z), Quaternion.identity);
            }
            else
            {
                floatingText.transform.position = new Vector3(transform.position.x, transform.position.y + textHoverDistance, transform.position.z);
            }
            floatingText.GetComponentInChildren<TextMesh>().text = spellText;
            floatingText.GetComponentInChildren<TextMesh>().color = Color.red;
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
