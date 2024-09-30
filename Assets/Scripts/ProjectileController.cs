using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private string incantation = "";

    public void SetIncantation(string newIncantation)
    {
        incantation = newIncantation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            /* todo: refactor the hit system
             * 
             * I do not love doing it this way (having an external manager) for multiple reasons,
             * most importantly if there are 2 wizards with the same hat & robe combination it will
             * pick one randomly to kill instead of the one that the projectile hits.
             * 
             * Wizards should store their own combinations and the projectile should store the incantation. 
             * On contact the projectile should check the wizard's list and decide what to do.
             * 
             * Additionally, we may want to think of what order things should go in.
             * Personally, body then hat seems a little backwards to me (if you read top to bottom you're probably gonna say hat first).
             * I think it makes sense lore wise as well, the robes is the armor they're wearing (heavy chain for a bolt, common robes for a blast, whatever)
             * which would be somewhat standard-issue, and the hat tells you what element they are. Decent argument either way, something to discuss
            */

            GameObject.Find("EnemyManager").GetComponent<EnemyMaker>().RemoveCombinedObject(incantation);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
