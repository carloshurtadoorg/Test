using UnityEngine;

/// <summary>
/// Manages the creation of enemies
/// </summary>
public class EnemyManager : MonoBehaviour
{
    

    //Range of the spawning time between enemies, it decrements overs the time, and has a minimun possible value
	public Range spawnTimeRange = new Range(0.4f, 1f);
	public float minSpawnTime = 0.2f;
	public float spawnTimeDecrement = 0.01f;


	//Range of the size of the enemies, it increments overs the time, and has a maximum possible value
	public Range sizeRange = new Range(1f, 1.5f);
    public float maxSize = 6f;
	public float sizeIncrement = 0.04f;

    //Dictates how much health an enemy should have when it has an specific size. The health of all the enemeties is obtained in a proportion in regards to this value
    public EnemyHealthAtSize enemyHealthAtSize;

    //Timer for respawn the enemies
    private float spawnTimer;


    public GameObject enemyObjectToSpawn;
    private float spawnPosZ;

    float randomSpawnTime;

	void Start () {
        enemyObjectToSpawn.GetComponent<Enemy>().enemyHealthAtSize = enemyHealthAtSize;

		spawnPosZ = Camera.main.WorldToViewportPoint(Player.instance.transform.position).z;

        randomSpawnTime=Random.Range(spawnTimeRange.min,spawnTimeRange.max);
	}
	

    void Update()
    {

        ManageEnemySpawn();

    }

    /// <summary>
    /// Manages the spawn of enemies
    /// </summary>
    void ManageEnemySpawn()
    {
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= randomSpawnTime)
		{
			float randomXPosition = Random.Range(0.15f, 0.95f);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(randomXPosition, 1.1f, spawnPosZ));

            // Calculates the size of the name, based on the size Increment defined, the current time and the max possible size
			float sizeFactor = GameManager.instance.currentTime * sizeIncrement;
			if (sizeRange.max + sizeFactor > maxSize)
			{
				sizeFactor = maxSize - sizeRange.max;

			}
			float size = Random.Range(sizeRange.min, sizeRange.max) + sizeFactor;
			GameObject obstacle = (GameObject)Instantiate(enemyObjectToSpawn, spawnPosition, enemyObjectToSpawn.transform.rotation);
            obstacle.transform.localScale = Vector3.one * size;

            // Calculates the time for spawn the next enemie, based on the spawn time decrement defined, the current time and the minimum spawn time possible
            float spawnFactor = GameManager.instance.currentTime * spawnTimeDecrement;
			if (spawnTimeRange.min - spawnFactor < minSpawnTime)
			{
				spawnFactor = spawnTimeRange.min - minSpawnTime;

			}
			randomSpawnTime = Random.Range(spawnTimeRange.min, spawnTimeRange.max) - spawnFactor;

            //Resets the timer
			spawnTimer = 0;

		}

    }


    /// <summary>
    /// Struct to Storage the information of the desired health of an Enemy at a particular Size
    /// </summary>
    [System.Serializable]
    public struct EnemyHealthAtSize
    {
        public int health;
        public float size;

    }

    /// <summary>
    /// Struct to storage range of values 
    /// </summary>
	[System.Serializable]
	public struct Range
	{
		public float min;
		public float max;
        public Range(float aMin, float aMax)
        {
            min = aMin;
            max = aMax;
        }

	}
}
