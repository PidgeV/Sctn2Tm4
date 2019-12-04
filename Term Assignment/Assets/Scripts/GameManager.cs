using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject targetDummy;
	[SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();

	/// <summary> Singleton for the GameManager script </summary>
	public static GameManager Instance;

	private int enemyCount;
	private int currentStage;
	private bool gameOver;

	// Properties
	/// <summary> Returns if the game is finished </summary>
	public bool GameOver { get { return gameOver; } }

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
		gameOver = false;
		currentStage = 1;
		enemyCount = -1;
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

		// TODO: Spawn different things for each stage
		// Spawns some target dummies 
		foreach (Transform pos in enemySpawnPoints)
		{
			yield return new WaitForSecondsRealtime(0.5f);
			//GameObject.Instantiate(targetDummy, pos.position, pos.rotation);
			enemyCount++;
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
