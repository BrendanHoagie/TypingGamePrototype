using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

enum variants
{
    BASE = 0,
    CROOKED = 1,
    UPSIDE_DOWN = 2,
    UPSIDE_DOWN_CROOKED = 3
}

public class WizardMaker : MonoBehaviour
{
    public List<Wizards> squares;  
    public List<Wizards> triangles; 

    private Wizards currentSquare;  
    private Wizards currentTriangle;
    private int numVariants = Enum.GetNames(typeof(variants)).Length;
    private variants currentVariant;

    private Dictionary<string, GameObject> wizardCombinations = new Dictionary<string, GameObject>();
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float spawnTime = 5f;
    private float curTime = 0f;


    void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > spawnTime)
        {
            SpawnWizard();
            curTime = 0f;
        }

        // for debugging
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SpawnWizard();
        }
    }


    public void SpawnWizard()
    {
        GenerateRandomCombination();

        GameObject currentWizard = Instantiate(enemyPrefab);
        currentWizard.transform.position = Vector3.zero; 

        SetupShapes(currentWizard);
        StoreCombination(currentWizard);

        Debug.Log("Enemy spawned with dynamically set square and triangle!");
    }

    public void GenerateRandomCombination()
    {
        currentSquare = squares[UnityEngine.Random.Range(0, squares.Count)];
        currentTriangle = triangles[UnityEngine.Random.Range(0, triangles.Count)];
        currentVariant = (variants)UnityEngine.Random.Range(0, numVariants);
    }

    private void SetupShapes(GameObject enemy)
    {
        Transform squareTransform = enemy.transform.Find("Square");
        Transform triangleTransform = enemy.transform.Find("Triangle");

        SpriteRenderer squareRenderer = squareTransform.GetComponent<SpriteRenderer>();
        squareRenderer.sprite = currentSquare.Shape;
        squareRenderer.color = currentSquare.Color;

        SpriteRenderer triangleRenderer = triangleTransform.GetComponent<SpriteRenderer>();
        triangleRenderer.sprite = currentTriangle.Shape;
        triangleRenderer.color = currentTriangle.Color;

        // debug
        print("Current variant is " + Enum.GetNames(typeof(variants))[(int) currentVariant]);

        // move hat (square) depending on variant
        switch (currentVariant)
        {
            // keep defaults if base
            case variants.BASE:
                break;

            // rotate right 90 degrees
            case variants.CROOKED:
                squareRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                break;

            // flip upside down
            case variants.UPSIDE_DOWN:
                squareRenderer.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
                break;

            // flip upside down and rotate right 90 degrees
            case variants.UPSIDE_DOWN_CROOKED:
                squareRenderer.transform.localRotation = Quaternion.Euler(-180f, 0f, 90f);
                break;
        }

        // Adjust triangle position to be above the square
        float squareHeight = squareRenderer.bounds.size.y;
        triangleTransform.localPosition = new Vector3(0, squareHeight / 2, 0);
    }

    private void StoreCombination(GameObject enemy)
    {
        foreach (string squareWord in currentSquare.Word)
        {
            foreach (string triangleWord in currentTriangle.Word)
            {
                string correctCombination = ConvertSpell(squareWord, triangleWord, currentVariant);
                enemy.GetComponent<WizardAI>().setCorrectCombination(correctCombination);
                Debug.Log(correctCombination);
                wizardCombinations[correctCombination] = enemy; 
            }
        }
    }

    public void RemoveCombinedObject(string combination)
    {
        if (wizardCombinations.TryGetValue(combination, out GameObject enemyToRemove))
        {
            Destroy(enemyToRemove);  
            wizardCombinations.Remove(combination); 
            Debug.Log("Enemy with combination " + combination + " destroyed!");
        }
        else
        {
            Debug.Log("No enemy found with combination: " + combination);
        }
    }
    
    private string ConvertSpell(string prefix, string suffix, variants variant)
    {
        switch (variant)
        {

            // swap prefix & suffix
            case variants.CROOKED:
                return suffix + prefix;

            // reverse prefix + reverse suffix
            case variants.UPSIDE_DOWN: 
                return ReverseString(prefix) + ReverseString(suffix);

            // reverse prefix + reverse suffix and swap them
            case variants.UPSIDE_DOWN_CROOKED:
                return ReverseString(suffix) + ReverseString(prefix);

            // variants.BASE - no change
            default:
                return prefix + suffix;

        }
    }

    private string ReverseString(string str) 
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

}
