using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class TypingController : MonoBehaviour
{
    public float distance = 0f;
    private string[] words = { "time", "year", "people", "way", "day", "man", "thing", "woman", "life", "child", "world", "school", "state", "family", "student", "group", "country", "problem", "hand", "part", "place", "case", "week", "company", "system", "program", "question", "work", "government", "number", "night", "point", "home", "water", "room", "mother", "area", "money", "story", "fact", "month", "lot", "right", "study", "book", "eye", "job", "word", "business", "issue", "side", "kind", "head", "house", "service", "friend", "father", "power", "hour", "game", "line", "end", "member", "law", "car", "city", "community", "name", "president", "team", "minute", "idea", "kid", "body", "information", "back", "parent", "face", "others", "level", "office", "door", "health", "person", "art", "war", "history", "party", "result", "change", "morning", "reason", "research", "girl", "guy", "moment", "air", "teacher", "force", "education" };
    private string buffer = "";
    [SerializeField] private GameObject spell;
    private GameObject floatingText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            switch (c)
            {
                // cast
                case '\n':
                case '\r':

                    // placeholders for now, should fire a particle or decrease health or whatever
                    if (wordCompare(buffer))
                    {
                        print("Success!");
                    } else
                    {
                        print("Fail!");
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
        print("buffer is " + buffer);
    }

    private bool wordCompare(string word)
    {
        foreach(string curWord in words)
        {
            if(word == curWord) return true;
        }
        return false;
    }

    void DisplayText(string text)
    {
        if (spell)
        {
            if (!floatingText)
            {
                floatingText = Instantiate(spell, new Vector3(transform.position.x, transform.position.y + distance, transform.position.z), Quaternion.identity);
            }
            else
            {
                floatingText.transform.position = new Vector3(transform.position.x, transform.position.y + distance, transform.position.z);
            }
            floatingText.GetComponentInChildren<TextMesh>().text = text;
        }
    }
}
