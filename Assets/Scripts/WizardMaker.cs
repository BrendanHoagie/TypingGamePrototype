using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

enum hats
{
    wizard = 0,
    witch = 1,
    crown = 2,
    // UPSIDE_DOWN_CROOKED = 3  Not using 4th case yet
}

public class WizardMaker : MonoBehaviour
{
    public List<Wizards> colors;  
    public List<Wizards> staffShapes;
    public List<Wizards> hatShapes;

    private Wizards currentColor;  
    private Wizards currentStaff;
    private int numVariants = Enum.GetNames(typeof(hats)).Length;
    private hats currentVariant;

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
    }

    public void GenerateRandomCombination()
    {
        currentColor = colors[UnityEngine.Random.Range(0, colors.Count)];
        currentStaff = staffShapes[UnityEngine.Random.Range(0, staffShapes.Count)];
        currentVariant = (hats)UnityEngine.Random.Range(0, numVariants);
    }

    private void SetupShapes(GameObject enemy)
    {
        Transform robeTransform = enemy.transform.Find("Robe");
        Transform staffTransform = enemy.transform.Find("Staff");
        Transform hatTransform = enemy.transform.Find("Hat");

        SpriteRenderer robeRenderer = robeTransform.GetComponent<SpriteRenderer>();
        robeRenderer.color = currentColor.Color;

        SpriteRenderer staffRenderer = staffTransform.GetComponent<SpriteRenderer>();
        staffRenderer.sprite = currentStaff.Shape;
        staffRenderer.color = currentColor.Color;

        SpriteRenderer hatRenderer = hatTransform.GetComponent<SpriteRenderer>();
        hatRenderer.color = currentColor.Color;

        // debug
        print("Current variant is " + Enum.GetNames(typeof(hats))[(int) currentVariant]);

        // move hat (square) depending on variant
        switch (currentVariant)
        {
            // keep defaults if base
            case hats.wizard:
                hatRenderer.sprite = hatShapes[0].Shape;
                break;

            // rotate right 90 degrees
            case hats.witch:
                // Switch hate sprite instead of rotating
                hatRenderer.sprite = hatShapes[1].Shape;
                // robeRenderer.transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                break;

            // flip upside down
            case hats.crown:
                // Switch hate sprite instead of rotating
                hatRenderer.sprite = hatShapes[2].Shape;
                // robeRenderer.transform.localRotation = Quaternion.Euler(-180f, 0f, 0f);
                break;
            
            /*
             * Not using fourth case yet
            // flip upside down and rotate right 90 degrees
            case variants.UPSIDE_DOWN_CROOKED:
                robeRenderer.transform.localRotation = Quaternion.Euler(-180f, 0f, 90f);
                break;
            */
        }

        // Adjustments to height no longer needed with new sprite system
        // Adjust triangle position to be above the square
        // float squareHeight = robeRenderer.bounds.size.y;
        // staffTransform.localPosition = new Vector3(0, squareHeight / 2, 0);
    }

    private void StoreCombination(GameObject enemy)
    {
        foreach (string squareWord in currentColor.Word)
        {
            foreach (string triangleWord in currentStaff.Word)
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
    
    private string ConvertSpell(string prefix, string suffix, hats variant)
    {
        switch (variant)
        {

            // swap prefix & suffix
            case hats.witch:
                return suffix + prefix;

            // reverse prefix + reverse suffix
            case hats.crown: 
                return ReverseString(prefix) + ReverseString(suffix);
            
            /*
             * Not using 4th case yet
            // reverse prefix + reverse suffix and swap them
            case hats.UPSIDE_DOWN_CROOKED:
                return ReverseString(suffix) + ReverseString(prefix);
            */

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
