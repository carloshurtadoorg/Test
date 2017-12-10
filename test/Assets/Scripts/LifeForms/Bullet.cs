using UnityEngine;

/// <summary>
/// Manage the behaviour of the bullets shot by the player
/// </summary>
public class Bullet : LifeForm  
{
    public float bulletSpeed = 7;

    void Update () 
    {
        DoMovement();
        CheckBounds();
	}

	/// <summary>
    /// Performs a straigth line movement (up the screen) based on the speed defined
	/// </summary>
	void DoMovement()
    {
        this.transform.position = this.transform.position + this.transform.forward * Time.deltaTime *bulletSpeed;

    }

	/// <summary>
	/// Checks if the bullet went ouf of the screen, and if so, it destroys it.
	/// </summary>
	void CheckBounds()
    {

        Vector3 viewportPosition =Camera.main.WorldToViewportPoint(this.transform.position);
        if (viewportPosition.y > 1f){

            Destroy(this.gameObject);
        }

    }


}
