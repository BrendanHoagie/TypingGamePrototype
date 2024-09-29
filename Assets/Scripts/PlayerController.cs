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
    [SerializeField] private float textHoverDistance = 0f;
    private string[] words = { "time", "year", "people", "way", "day", "man", "thing", "woman", "life", "child", "world", "school", "state", "family", "student", "group", "country", "problem", "hand", "part", "place", "case", "week", "company", "system", "program", "question", "work", "government", "number", "night", "point", "home", "water", "room", "mother", "area", "money", "story", "fact", "month", "lot", "right", "study", "book", "eye", "job", "word", "business", "issue", "side", "kind", "head", "house", "service", "friend", "father", "power", "hour", "game", "line", "end", "member", "law", "car", "city", "community", "name", "president", "team", "minute", "idea", "kid", "body", "information", "back", "parent", "face", "others", "level", "office", "door", "health", "person", "art", "war", "history", "party", "result", "change", "morning", "reason", "research", "girl", "guy", "moment", "air", "teacher", "force", "education" };
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
        print("(" + aimDirection.x + ", " + aimDirection.y + ") angle = " + aimAngle);
        rb.rotation = aimAngle;
    }

    void RunTyping()
    {
        foreach (char c in Input.inputString)
        {
            switch (c)
            {
                // cast
                case '\n':
                case '\r':
                    if (wordCompare(buffer))
                    {
                        print("Cast Successful!");
                        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
                        projectile.GetComponent<Rigidbody2D>().AddForce(spawnPoint.up * projectileSpeed, ForceMode2D.Impulse);
                    }
                    else
                    {
                        print("Cast Fail!");
                    }
                    buffer = "";
                    break;

                // delete
                case '\b':
                    buffer = "";
                    break;

                default:
                    buffer += c;
                    break;
            }
        }
        DisplayText(buffer);
    }

    private bool wordCompare(string word)
    {
        foreach (string curWord in words) 
        {
            if (word == curWord) return true;
        }
        return false;
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
