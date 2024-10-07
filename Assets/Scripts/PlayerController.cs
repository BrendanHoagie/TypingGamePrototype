using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    // movement
    [SerializeField] private float moveSpeed;
    [SerializeField] private NavMeshAgent agent;
    private Vector3 target;
    private Rigidbody2D rb;
    private Vector2 mousePosition;

    // health
    private int hitPoints = 3;

    // typing
    [SerializeField] private float textHoverDistance = 2f;
    private string buffer = "";
    [SerializeField] private GameObject floatingTextPrefab;
    private GameObject floatingText = null;

    // casting
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float projectileSpeed;
    private SpellMaker spellMaker;
    [SerializeField] private float spellLockoutTime = 0f;
    private float currentTime = 0f;


    public float GetLockoutTime()
    {
        return spellLockoutTime;
    }
    public float GetCurrentTime()
    {
        return currentTime;
    }

    public int GetHP()
    {
        return hitPoints;
    }

    public void TakeDamage()
    {
        hitPoints -= 1;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>().startRoutine = true;
        if (hitPoints == 0)
        {
            Destroy(gameObject);
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spellMaker = GameObject.Find("Game Manager").GetComponent<SpellMaker>();
    }

    private void Awake()
    {
        // Get NavMesh Agent component and disable rotation and axis updates
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    void Update()
    {
        // Point and click movement
        if (Input.GetMouseButtonDown(1))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));

        // aiming
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // reset sprite so it doesn't turn with the rigid body
        gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, 0);
        RunTyping();
    }

    private void FixedUpdate()
    {
        Vector2 aimDirection = mousePosition - rb.position;
        rb.rotation = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
    }


    void RunTyping()
    {
        // punishment for miscasting -> can't type for a lockout time
        if (currentTime < spellLockoutTime)
        {
            currentTime += Time.deltaTime;
            DisplayText(buffer);
            return;
        }
        currentTime = spellLockoutTime;

        // cast spell
        if (Input.GetMouseButtonUp(0))
        {
            spellMaker.CreateSpell(buffer, spawnPoint, projectileSpeed);
            buffer = "";
            currentTime = 0;
        }

        // build buffer, only include A-Z & a-z
        foreach (char c in Input.inputString)
        {
            int asciiVersion = (int) c;
            if ((asciiVersion >= 65 && asciiVersion <= 90) || (asciiVersion >= 97 && asciiVersion <= 122)) buffer += char.ToLower(c);
        }
        DisplayText(buffer);
    }

    void DisplayText(string text)
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
            floatingText.GetComponentInChildren<TextMesh>().text = text;
        }
    }

    // Only triggers when a projectile hits it- weird?
    // If this would work with the enemy collider we could rip
    // out a lot of the stuff in WizardAI
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("collision detected- " + collision.ToString());
        if (collision.gameObject.tag == "Enemy Spell")
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }
    }
    
}
