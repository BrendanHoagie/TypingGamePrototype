using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // movement
    [SerializeField] private float moveSpeed;
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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        moveDirection = new Vector2(Input.GetAxisRaw("HorizontalArrows"), Input.GetAxisRaw("VerticalArrows")).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RunTyping();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    void RunTyping()
    {
        // cast spell
        if (Input.GetMouseButtonUp(0))
        {
            if(GameObject.Find("EnemyManager").GetComponent<WizardMaker>().ValidateString(buffer))
            {
                print("Cast Successful!");
                GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
                projectile.GetComponent<Rigidbody2D>().AddForce(spawnPoint.up * projectileSpeed, ForceMode2D.Impulse);

                // todo: Fix hit system
                // see comment in ProjectileController's OnCollisionEnter2D method
                projectile.GetComponent<ProjectileController>().SetIncantation(buffer);
            }
            // todo: take damage here
            else
            {
                print("Cast fail!");
            }

            buffer = "";
        }

        // delete
        else if (Input.GetMouseButtonDown(1)) buffer = "";

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
