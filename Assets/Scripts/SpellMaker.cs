using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpellMaker : MonoBehaviour
{
    public List<Spell> elements;
    public List<Spell> projectiles;

    private Spell currentElement;
    private Spell currentProjectile;

    [SerializeField] private GameObject projectilePrefab;

    public void CreateSpell(string spellInput, Transform spawnPoint, float speed)
    {
        foreach (Spell element in elements)
        {
            if (spellInput.Contains(element.SpellPart))
            {
                currentElement = element;
                break;
            }
        }

        foreach(Spell projectile in projectiles)
        {
            if (spellInput.Contains(projectile.SpellPart))
            {
                currentProjectile = projectile;
                break;
            }
        }

        // spell type or element that doesn't exist
        // todo: ideally this would shoot a generic crap spell, to be added later
        if (!currentProjectile || !currentElement)
        {
            print("Failed cast- projectile: " + (!currentProjectile ? "none" : currentProjectile.name) + " element: " + (!currentElement ? "none" : currentElement.name));
            return;
        }

        GameObject spellProjectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        spellProjectile.GetComponent<ProjectileController>().SetIncantation(spellInput);
        ConstructSpell(spellProjectile, spawnPoint, speed);
    }

    private void ConstructSpell(GameObject spellProjectile, Transform spawnPoint, float speed)
    {
        SpriteRenderer render = spellProjectile.GetComponent<SpriteRenderer>();
        render.sprite = currentProjectile.ProjectileShape;

        render.color = GetElementColor(currentElement.Element);

        if (currentProjectile.Projectile == Projectile.Bolt)
        {
            Rigidbody2D rb = spellProjectile.GetComponent<Rigidbody2D>();
            spellProjectile.GetComponent<Rigidbody2D>().AddForce(spawnPoint.up * speed, ForceMode2D.Impulse);
        } 
        else if (currentProjectile.Projectile == Projectile.Blast)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spellProjectile.transform.position = mousePos;
            spellProjectile.transform.localScale = new Vector3(2f, 2f, 2f);
            Destroy(spellProjectile, 0.5f); //Despawn after a set time regarldess of hit or not
        }
        else if (currentProjectile.Projectile == Projectile.Barrage)
        {
            Rigidbody2D rb = spellProjectile.GetComponent<Rigidbody2D>();
            spellProjectile.GetComponent<Rigidbody2D>().AddForce(spawnPoint.up * speed, ForceMode2D.Impulse);
        }
    }

    public Color GetElementColor(Element element)
    {
        switch (element)
        {
            case Element.Fire:
                return Color.red;
            case Element.Ice: 
                return Color.cyan;
            case Element.Rock:
                return Color.gray;
            default:
                return Color.white;
        }
    }
}
