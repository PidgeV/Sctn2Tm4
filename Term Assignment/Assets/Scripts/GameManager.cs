using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private FollowPointCameraController camera;

	[SerializeField] private GameObject advancedEnemy;
	[SerializeField] private GameObject towerEnemy;
	[SerializeField] private GameObject enemy;
	[SerializeField] private GameObject bossEnemy;

	private GameObject[] enemySpawnPoints = new GameObject[0];
	private GameObject[] towerSpawnPoints = new GameObject[0];

	/// <summary> Singleton for the GameManager script </summary>
	public static GameManager Instance;

	/// <summary> What stage of the game we are on </summary>
	private int currentStage = 3;
	private int enemyCount = 0;

	/// <summary> Do we want the game in hardmode? </summary>
	[SerializeField] private bool hardMode = false;

	public bool playing = false;

	// Properties
	/// <summary> Returns if the game is finished </summary>
	public bool GameOver { get { return enemyCount <= 0; } }


	//set hard mode (from the menu)  --- svitlana
	public void SetHardMode(bool mode)
	{
		hardMode = mode;
	}

	// Start is called before the first frame update
	IEnumerator Start()
	{
		Instance = this;

		yield return new WaitForSecondsRealtime(2f);

		towerSpawnPoints = GameObject.FindGameObjectsWithTag("TowerSpawnPoint");
		enemySpawnPoints = GameObject.FindGameObjectsWithTag("Waypoints");

		// Start the Initialization for the beginning stage of the game
		yield return InitializeStage();
	}

	/// <summary>
	/// Initialize a stage of the game
	/// Load enemy, clear bullets etc 
	/// </summary>
	IEnumerator InitializeStage()
	{
		camera.FollowPlayer = false;
		HidePlayer();

		// Wait a frame before starting
		yield return null;

		enemyCount = 0;

		GameManager.Instance.playing = false;

		// If we want to limit the enemy count when were..
		// NOT in Hard Mode
		int enemyLimit = 1;

		int enemyCounter = 0;

		//Spawn Enemies
		foreach (GameObject go in enemySpawnPoints)
		{
			GameObject lastenemy = null;

			enemyCounter++;
			if (!hardMode)
			{
				// Default Mode
				lastenemy = GameObject.Instantiate(enemy, go.transform.position, go.transform.rotation);

				// If were over our enemy cap we stone spawning
				if (enemyCounter > enemyLimit) break;
			}
			else
			{
				// Hard Mode
				lastenemy = GameObject.Instantiate(advancedEnemy, go.transform.position, go.transform.rotation);
			}

			yield return new WaitForSeconds(0.2f);

			// Give random life
			if (lastenemy && lastenemy.TryGetComponent<Health>(out Health health))
			{
				health.currentLife = Random.Range(1, 10) <= 2 ? 1 : 2;
			}
		}

		// Spawn Towers
		if (currentStage == 2)
		{
			foreach (GameObject go in towerSpawnPoints)
			{
				enemyCounter++;

				// Instantiate a tower
				GameObject.Instantiate(towerEnemy, go.transform.position, go.transform.rotation);
				yield return new WaitForSeconds(0.2f);
			}
		}

		// Spawn the BOSS
		if (currentStage >= 3)
		{
			GameObject.Instantiate(bossEnemy, Vector3.zero, Quaternion.identity);
			enemyCounter++;
		}

		enemyCount = enemyCounter;

		// Wait a frame before ending
		yield return new WaitForSeconds(0.5f);

		ShowPlayer();
		camera.FollowPlayer = true;
		GameManager.Instance.playing = true;
	}

	/// <summary>
	/// Called by each enemy in Death
	/// </summary>
	public void OnEnemyDeath()
	{
		enemyCount--;
		if (enemyCount <= 0)
		{
			currentStage++;
			StartCoroutine(InitializeStage());
		}
	}

	private void OnGUI()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		GUILayout.Box("Stage: " + currentStage + "\n Enemies Remaining: " + enemyCount);
	}

	private Vector3 HidePlayer()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		Vector3 playerPos = player.transform.position;

		player.transform.position = new Vector3(0, -100, 0);

		return playerPos;
	}

	private void ShowPlayer()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = new Vector3(0, 0, -65);
		player.transform.rotation = Quaternion.identity;
	}
}
