using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Wizards> squares;
    [SerializeField] private List<Wizards> triangles;
    [SerializeField] private Vector3 spawnPos = new Vector3(0,0,0);
    [SerializeField] private Transform enemyParent; 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Ensure both lists are populated
        if (squares.Count > 0 && triangles.Count > 0)
        {
            // Randomly pick a square and a triangle from their respective lists
            Wizards randomSquare = squares[Random.Range(0, squares.Count)];
            Wizards randomTriangle = triangles[Random.Range(0, triangles.Count)];

            // Create GameObject to represent the enemy
            GameObject enemy = new GameObject("Enemy");

            // Set the position and parent of the enemy
            enemy.transform.position = spawnPos;
            if (enemyParent != null)
            {
                enemy.transform.parent = enemyParent;
            }

            // Spawn the square and triangle as children of the enemy
            GameObject squareObject = CreateShapeObject(randomSquare, enemy.transform);
            GameObject triangleObject = CreateShapeObject(randomTriangle, enemy.transform);

            Debug.Log("Enemy spawned with square and triangle!");
        }
        else
        {
            Debug.LogError("No squares or triangles available to spawn!");
        }
    }

    private GameObject CreateShapeObject(Wizards shapeData, Transform parent)
    {
        GameObject shapeObject = new GameObject(shapeData.ShapeType.ToString());
        shapeObject.transform.parent = parent;

        // Add a SpriteRenderer component to display the shape
        SpriteRenderer renderer = shapeObject.AddComponent<SpriteRenderer>();
        renderer.sprite = shapeData.Shape;
        renderer.color = shapeData.Color;

        // Set shape size and positioning as needed
        shapeObject.transform.localPosition = Vector3.zero;

        return shapeObject;
    }
}
