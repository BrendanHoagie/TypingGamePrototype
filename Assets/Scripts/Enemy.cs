using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private string[] words;
    private string buffer = "";

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
                        print("Correct Spell");
                        Destroy(gameObject);
                    }
                    else
                    {
                        print("Wrong Spell");
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
    }

    private bool wordCompare(string word)
    {
        foreach (string curWord in words)
        {
            if (word == curWord) return true;
        }
        return false;
    }
}
