using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the Player
/// </summary>
public class Player : LifeForm
{

    public float movementSpeed = 15f;
    public float fireRate = 0.4f;

    /// <summary>
    /// Defines the space on top of the mosue to position the player when dragged
    /// </summary>
    public float mouseDeltaZPosition = 1f;
    public static Player instance;

    public Transform bulletPosition;
    public GameObject bulletObject;

    PlayerState playerState = PlayerState.STATIONARY;

	/// <summary>
	/// Defines a delta space for the extents of the player (a bit of space at the sides, and the front/back).
	/// This is used to calculate the real size of the player, and then to calculate if the player is off the screen.
	/// </summary> 
	private Vector3 BOUND_DELTA_POSITION = new Vector3(0.05f, 0, 0.12f);
    private Vector3 playerExtents;


    public void Awake()
    {
        instance = this;
        // Stores the extents of the player (get its real size)
        playerExtents = this.transform.GetChild(0).GetComponent<Renderer>().bounds.extents + BOUND_DELTA_POSITION; 

        //Runs the autofire
        StartCoroutine(AutoFire());
	}

    /// <summary>
    /// Defines the States of the player
    /// </summary>
    public enum PlayerState 
    {
        STATIONARY,
        MOVING
    }


    private void Update()
    {
        CheckInput();
        CheckMovement();
    }

	/// <summary>
	/// Makes the player shoot a bullet based on the fire Rate defined 
	/// </summary>
	/// <returns>The fire.</returns>
	IEnumerator AutoFire()
    {
        yield return new WaitForSeconds(fireRate);
        Instantiate(bulletObject,bulletPosition.position,bulletObject.transform.rotation);
        StartCoroutine(AutoFire());
    }

    /// <summary>
    /// Performs the movement if the current State of the player is moving
    /// </summary>
    void CheckMovement()
    {
		switch (playerState)
		{
			case PlayerState.MOVING:
				DragPlayer();
				break;

		}

    }

    /// <summary>
    /// Sets the state of the player to moving when the mouse is down, and to stationary if its being lifted
    /// </summary>
    void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerState = PlayerState.MOVING;
        }

		if (Input.GetMouseButtonUp(0))
		{
            playerState = PlayerState.STATIONARY;

		}
    }

    /// <summary>
    /// Informs the Game Manager to restart the game and make the player die
    /// </summary>
    public override void Die()
    {

        GameManager.instance.SetRestartOfGame();
        base.Die();
    }



    /// <summary>
    /// Moves the player on top of the mouse position and drags him along the mouse movement
    /// </summary>
    void DragPlayer()
    {
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 mouseFixedPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, playerScreenPos.z);
        Vector3 newPlayerPosition = Camera.main.ScreenToWorldPoint(mouseFixedPosition) + this.transform.forward * mouseDeltaZPosition;
        newPlayerPosition=  FixBounds(newPlayerPosition);
        this.transform.position = Vector3.Lerp(this.transform.position, newPlayerPosition, Time.deltaTime * movementSpeed);

    }

    /// <summary>
    /// Mantains the player inside the Screen (compatible with different screen sizes)
    /// </summary>
    /// <returns>The fixed position of the player</returns>
    /// <param name="aPosition">The current desired position of the player</param>
    Vector3 FixBounds(Vector3 aPosition)
    {
        //Stores the position of the Four extents of the player (right, left, front and back) in Viewport format
        float playerViewportPositionMinX = Camera.main.WorldToViewportPoint(aPosition - this.transform.right*playerExtents.x).x;
        float playerViewportPositionMaxX = Camera.main.WorldToViewportPoint(aPosition + this.transform.right * playerExtents.x).x;
        float playerViewportPositionMinY = Camera.main.WorldToViewportPoint(aPosition - this.transform.forward * playerExtents.z).y;
        float playerViewportPositionMaxY = Camera.main.WorldToViewportPoint(aPosition + this.transform.forward * playerExtents.z).y;

        Vector3 playerNewPosition = aPosition;

        // Checks of the sides of the player are out of the screen, and if so, fixes the position
        if(playerViewportPositionMinX<0)
        {

            Vector3 playerViewPortPositionCenter = Camera.main.WorldToViewportPoint(playerNewPosition);
            Vector3 fixedPlayerPos= Camera.main.ViewportToWorldPoint(new Vector3(0, playerViewPortPositionCenter.y, playerViewPortPositionCenter.z));
            playerNewPosition = fixedPlayerPos+transform.right*playerExtents.x;
        }

		if (playerViewportPositionMaxX > 1)
		{

			Vector3 playerViewPortPositionCenter = Camera.main.WorldToViewportPoint(playerNewPosition);
			Vector3 fixedPlayerPos = Camera.main.ViewportToWorldPoint(new Vector3(1, playerViewPortPositionCenter.y, playerViewPortPositionCenter.z));
			playerNewPosition = fixedPlayerPos - transform.right *playerExtents.x;
		}

		// Checks of the Front and Back part of the player are out of the screen, and if so, fixes the position
		if (playerViewportPositionMinY < 0)
		{

			Vector3 playerViewPortPositionCenter = Camera.main.WorldToViewportPoint(playerNewPosition);
			Vector3 fixedPlayerPos = Camera.main.ViewportToWorldPoint(new Vector3(playerViewPortPositionCenter.x,0, playerViewPortPositionCenter.z));
            playerNewPosition = fixedPlayerPos + transform.forward * playerExtents.z ;
		}

		if (playerViewportPositionMaxY > 1)
		{

			Vector3 playerViewPortPositionCenter = Camera.main.WorldToViewportPoint(playerNewPosition);
			Vector3 fixedPlayerPos = Camera.main.ViewportToWorldPoint(new Vector3( playerViewPortPositionCenter.x,1, playerViewPortPositionCenter.z));
            playerNewPosition = fixedPlayerPos - transform.forward * playerExtents.z;
		}

        // Returns the fixed position
        return playerNewPosition;

    }
}