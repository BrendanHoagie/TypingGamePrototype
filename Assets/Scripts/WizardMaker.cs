using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WizardMaker : MonoBehaviour
{
    public List<Wizards> squares;  
    public List<Wizards> triangles; 

    private Wizards currentSquare;  
    private Wizards currentTriangle; 

    private Dictionary<string, GameObject> wizardCombinations = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> validCombinations = new Dictionary<string, bool>();
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private float spawnTime = 5f;
    private float curTime = 0f;

    void Start()
    {
        // populate dictionary of all possible combinations for checking firing
        // I am sorry for what I've done here
        for (int i = 0; i < squares.Count; i++)
        {
            Wizards curSquare = squares[i];
            for (int j = 0; j < triangles.Count; j++)
            {
                Wizards curTriangle = triangles[j];
                foreach (string squareWord in curSquare.Word)
                {
                    foreach (string triangleWord in curTriangle.Word)
                    {
                        string newWord = squareWord + triangleWord;
                        if (validCombinations.ContainsKey(newWord)) continue;
                        validCombinations[newWord] = true;
                    }
                }
            }
        }
    }

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
        currentSquare = squares[Random.Range(0, squares.Count)];
        currentTriangle = triangles[Random.Range(0, triangles.Count)];
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
                string correctCombination = squareWord + triangleWord;
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

    public bool ValidateString(string str)
    {
        try
        {
            return validCombinations[str];
        }
        catch
        {
            return false;
        }
        
    }
}
