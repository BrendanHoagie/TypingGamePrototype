using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShapeType {Square, Triangle}
[CreateAssetMenu]
public class Wizards : ScriptableObject
{

    [SerializeField]
    private Sprite _shape;
    public Sprite Shape { get => _shape; }

    [SerializeField]
    private string[] _word;
    public string[] Word { get => _word; }

    [SerializeField]
    private Color _color;
    public Color Color { get => _color; }

    [SerializeField]
    private ShapeType _shapeType;
    public ShapeType ShapeType { get => _shapeType; }

    [SerializeField]
    private float _stopRadius;
    public float StopRadius { get => _stopRadius;}
}
