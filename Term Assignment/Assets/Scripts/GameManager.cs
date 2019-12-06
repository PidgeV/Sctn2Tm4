using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject enemy;
	[SerializeField] private GameObject advancedEnemy;
	[SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();

	/// <summary> Singleton for the GameManager script </summary>
	public static GameManager Instance;

	private int enemyCount;
	private int currentStage;
	private bool gameOver;
	public bool hardMode = false;

	// Properties
	/// <summary> Returns if the game is finished </summary>
	public bool GameOver { get { return gameOver; } }

	// Start is called before the first frame update
	IEnumerator Start()
	{
		Instance = this;
		gameOver = false;
		currentStage = 1;
		enemyCount = -1;
		yield return new WaitForSecondsRealtime(6f);
		StartCoroutine(InitializeStage());
	}

	/// <summary>
	/// Initialize a stage of the game
	/// Load enemy, clear bullets etc 
	/// </summary>
	IEnumerator InitializeStage()
	{
		// Wait a frame before starting
		yield return null;

		int counter = 0;

		// TODO: Spawn different things for each stage
		// Spawns some target dummies 
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Waypoints"))
		{
			if (counter >= 8 && !hardMode)
			{
				break;
			}

			yield return new WaitForSecondsRealtime(3.5f);
			if (hardMode)
			{
				GameObject.Instantiate(advancedEnemy, go.transform.position, go.transform.rotation);
			}
			else
			{
				GameObject.Instantiate(enemy, go.transform.position, go.transform.rotation);
			}
			enemyCount++;
			counter++;
		}

		// Wait a frame before ending
		yield return null;
	}

	// TODO: Add this to the enemy scripts
	/// <summary>
	/// Called by each enemy in OnDisable
	/// </summary>
	public void OnEnemyDeath()
	{
		if (enemyCount <= 0)
		{
			StopCoroutine(InitializeStage());
			StartCoroutine(InitializeStage());
			gameOver = true;
			currentStage++;
		}
		else
		{
			enemyCount--;
		}
	}
}
