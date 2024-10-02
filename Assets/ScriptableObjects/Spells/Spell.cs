using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Spell : ScriptableObject
{
    [SerializeField]
    private Sprite _projectileType;
    public Sprite ProjectileType { get => _projectileType; }

    [SerializeField]
    private string _spellPart;
    public string SpellPart { get => _spellPart;}

    [SerializeField]
    private ParticleSystem _particleSystem;
    public ParticleSystem ParticleSystem { get => _particleSystem; }

    [SerializeField]
    private Color _element;
    public Color Element { get => _element; }

    public enum SpellStructure { Element, Projectile }
}
