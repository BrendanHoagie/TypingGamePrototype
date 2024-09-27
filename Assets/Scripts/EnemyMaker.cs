using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    public List<Wizards> squares;
    public List<Wizards> triangles;

    private Wizards currentSquare;
    private Wizards currentTriangle;

    [SerializeField] private Vector3 spawnPos = new Vector3(0, 0, 0);
    [SerializeField] private Transform enemyParent;

    private Dictionary<string, GameObject> enemyCombinations = new Dictionary<string, GameObject>();

    void Start()
    {
        GenerateRandomCombination();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SpawnEnemy();
        }
    }

    private void GenerateRandomCombination()
    {
        currentSquare = squares[UnityEngine.Random.Range(0, squares.Count)];
        currentTriangle = triangles[UnityEngine.Random.Range(0, triangles.Count)];
    }

    public bool CheckCombination(string input)
    {
        foreach (string squareWord in currentSquare.Word)
        {
            foreach (string triangleWord in currentTriangle.Word)
            {
                string correctCombination = squareWord + triangleWord;

                if (input == correctCombination)
                {
                    return true;
                }
            }
        }

        return false;

    }

    public void SpawnEnemy()
    {
        GenerateRandomCombination();

        // Create a parent GameObject for the enemy
        GameObject enemy = new GameObject("Enemy");
        enemy.transform.position = spawnPos;

        // Set parent if provided
        if (enemyParent != null)
        {
            enemy.transform.parent = enemyParent;
        }

        // Spawn the square and triangle as children of the enemy
        GameObject squareObject = CreateShapeObject(currentSquare, enemy.transform);
        GameObject triangleObject = CreateShapeObject(currentTriangle, enemy.transform);
        StoreCombination(enemy);

        // Delete this later 
        Debug.Log("Enemy spawned with square and triangle!");
        foreach (string squareWord in currentSquare.Word)
        {
            foreach (string triangleWord in currentTriangle.Word)
            {
                string correctCombination = squareWord + triangleWord;
                print(correctCombination);
            }
        }

    }

    // Helper method to create the shape GameObject
    private GameObject CreateShapeObject(Wizards shapeData, Transform parent)
    {
        GameObject shapeObject = new GameObject(shapeData.ShapeType.ToString());
        shapeObject.transform.parent = parent;

        SpriteRenderer renderer = shapeObject.AddComponent<SpriteRenderer>();
        renderer.sprite = shapeData.Shape;
        renderer.color = shapeData.Color;

        if (shapeData.ShapeType == ShapeType.Square)
        {
            shapeObject.transform.localPosition = Vector3.zero;
        }
        else if (shapeData.ShapeType == ShapeType.Triangle)
        {
            SpriteRenderer squareRenderer = parent.Find("Square").GetComponent<SpriteRenderer>();
            float squareHeight = squareRenderer.bounds.size.y;

            float triangleHeight = renderer.bounds.size.y;
            shapeObject.transform.localPosition = new Vector3(0, squareHeight / 2, 0);
        }

        return shapeObject;
    }

    private void StoreCombination(GameObject enemy)
    {
        foreach (string squareWord in currentSquare.Word)
        {
            foreach (string triangleWord in currentTriangle.Word)
            {
                string correctCombination = squareWord + triangleWord;
                enemyCombinations[correctCombination] = enemy; // Store the enemy with its combination
            }
        }
    }

    public void RemoveCombinedObject(string combination)
    {
        if (enemyCombinations.TryGetValue(combination, out GameObject enemyToRemove))
        {
            Destroy(enemyToRemove);
            enemyCombinations.Remove(combination); // Remove from dictionary after deletion
            Debug.Log("Enemy with combination " + combination + " destroyed!");
        }
        else
        {
            Debug.Log("No enemy found with combination: " + combination);
        }
    }
}
