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
    private Vector2 moveDirection;
    private Vector2 mousePosition;

    // typing
    [SerializeField] private float textHoverDistance = 2f;
    private string buffer = "";
    [SerializeField] private GameObject floatingTextPrefab;
    private GameObject floatingText = null;

    // casting
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private SpellMaker spellMaker;


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
        // Old keyboard based movement
        // moveDirection = new Vector2(Input.GetAxisRaw("HorizontalArrows"), Input.GetAxisRaw("VerticalArrows")).normalized;

        // Point and click movement
        SetTargetPosition();
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RunTyping();
    }

    private void FixedUpdate()
    {
        // Old keyboard based movement
        // rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    void SetTargetPosition()
    {
        if (Input.GetMouseButtonDown(1))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void RunTyping()
    {
        // cast spell
        if (Input.GetMouseButtonUp(0))
        {
            if(GameObject.Find("Game Manager").GetComponent<SpellMaker>().ValidateString(buffer))
            {
                print("Cast Successful!");
                spellMaker.CreateSpell(buffer, spawnPoint, projectileSpeed);
                /*GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
                projectile.GetComponent<Rigidbody2D>().AddForce(spawnPoint.up * projectileSpeed, ForceMode2D.Impulse);*/

                // todo: Fix hit system
                // see comment in ProjectileController's OnCollisionEnter2D method
                //projectile.GetComponent<ProjectileController>().SetIncantation(buffer);
            }
            // todo: take damage here
            else
            {
                print("Cast fail!");
            }

            buffer = "";
        }

        // delete is removed and Right Mouse button is replaced for movement
        // else if (Input.GetMouseButtonDown(1)) buffer = "";

        // build buffer
        foreach (char c in Input.inputString)
        {
            switch (c)
            {
                // disabled keys -> should probably put all escape sequences here
                // and possibly space + numbers? tbd.
                case '\n':
                case '\r':
                case '\b':
                    break;

                default:
                    buffer += char.ToLower(c);
                    break;
            }
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
}
