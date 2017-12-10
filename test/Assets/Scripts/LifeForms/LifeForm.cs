using UnityEngine;

/// <summary>
/// Base class for all the Objects of the game that possess health
/// </summary>
public class LifeForm : MonoBehaviour 
{

    public int health;
    public GameObject deathEffect;

    /// <summary>
    /// Makes the the health decrease by one. If the health is zero, make the object die.
    /// </summary>
    public virtual void TakeDamage()
    {
        
        health--;
        if (health == 0) 
        {
            Die();

        }
    }

    /// <summary>
    /// Takes Damage every time the object collides with another
    /// </summary>
    /// <param name="collision">Collision.</param>
	private void OnTriggerEnter(Collider collision)
	{
		TakeDamage();
	}

    /// <summary>
    /// Instantiates an explosion effect and Destroys the object
    /// </summary>
    public virtual void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect,this.transform.position,deathEffect.transform.rotation);
        }
        Destroy(this.gameObject);

    }


     

}
