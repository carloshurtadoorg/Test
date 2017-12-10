
using UnityEngine;

/// <summary>
/// Manages the behaviour of an enemy 
/// </summary>
public class Enemy : LifeForm
{
    public float enemySpeed = 7;
    public EnemyManager.EnemyHealthAtSize enemyHealthAtSize;

    private void Start()
    {
        CalculateHealth();      
    }

    /// <summary>
    /// Calculates the health of the enemy, based on its size, and the parameters defined at the enemies Manager
    /// </summary>
    void CalculateHealth()
    {
		health = Mathf.RoundToInt((this.transform.localScale.x * enemyHealthAtSize.health) / enemyHealthAtSize.size);
		health = Mathf.Max(1, health);

    }

    void Update()
	{
        DoMovement();
        CheckBounds();
	}

    /// <summary>
    /// Performs a straigth line movement (down the screen) based on the speed defined
    /// </summary>
    void DoMovement(){

        this.transform.position = this.transform.position - Vector3.forward * Time.deltaTime * enemySpeed;
    }

    /// <summary>
    /// Checks if the enemy went ouf of the screen, and if so, it destroys it.
    /// </summary>
	void CheckBounds()
	{

		Vector3 viewportPosition = Camera.main.WorldToViewportPoint(this.transform.position);
		if (viewportPosition.y < -0.2f)
		{
			Destroy(this.gameObject);
		}

	}

    /// <summary>
    /// Increments the score of the player when the enemy dies (was killed by the player)
    /// </summary>
    public override void Die()
    {
        GameManager.instance.IncrementScore();
        base.Die();

    }
}
