using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellStructure {None, Element, Projectile}
public enum Element {None, Fire, Ice}
public enum Projectile {None, Bolt, Blast}
[CreateAssetMenu]
public class Spell : ScriptableObject
{
    [SerializeField]
    private Sprite _projectileShape;
    public Sprite ProjectileShape { get => _projectileShape; }

    [SerializeField]
    private string _spellPart;
    public string SpellPart { get => _spellPart;}

    [SerializeField]
    private ParticleSystem _particleSystem;
    public ParticleSystem ParticleSystem { get => _particleSystem; }

    [SerializeField]
    private SpellStructure _spellStructure;
    public SpellStructure SpellStructure { get => _spellStructure; }

    [SerializeField]
    private Element _element;
    public Element Element { get => _element; }

    [SerializeField]
    private Projectile _projectile;
    public Projectile Projectile { get => _projectile; }
}
