using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] private FollowPointCameraController camera;
	
	[SerializeField] private GameObject advancedEnemy;
	[SerializeField] private GameObject towerEnemy;
	[SerializeField] private GameObject enemy;

	private GameObject[] enemySpawnPoints = new GameObject[0];
	private GameObject[] towerSpawnPoints = new GameObject[0];
    private Button[] buttons = new Button[0];

	/// <summary> Singleton for the GameManager script </summary>
	public static GameManager Instance;

	/// <summary> What stage of the game we are on </summary>
	private int currentStage = 3;
	private int enemyCount = 0;

	/// <summary> Do we want the game in hardmode? </summary>
	[SerializeField] private bool hardMode = false;

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
		yield return new WaitForSecondsRealtime(2f);

		towerSpawnPoints = GameObject.FindGameObjectsWithTag("TowerSpawnPoint");
		enemySpawnPoints = GameObject.FindGameObjectsWithTag("Waypoints");
        buttons = FindObjectsOfType<Button>();


		// Start the Initialization for the beginning stage of the game
	    //	yield return InitializeStage();    -------- Initializing from the Start Button/Hard Mode function now (Svitlana)
	}

	/// <summary>
	/// Initialize a stage of the game
	/// Load enemy, clear bullets etc 
	/// </summary>
	IEnumerator InitializeStage()
	{
		// Wait a frame before starting
		yield return null;

		//camera.FollowPlayer = false;

		// If we want to limit the enemy count when were..
		// NOT in Hard Mode
		int enemyLimit = 12;

		int enemyCounter = 0;

		//Spawn Enemies
		foreach (GameObject go in enemySpawnPoints)
		{
			if (!hardMode)
			{
				// Default Mode
				GameObject.Instantiate(enemy, go.transform.position, go.transform.rotation);

				// If were over our enemy cap we stone spawning
				if ( enemyCounter > enemyLimit) break;
			}
			else
			{
				// Hard Mode
				GameObject.Instantiate(advancedEnemy, go.transform.position, go.transform.rotation);
			}
			enemyCounter++;
		}
        

		// Spawn Towers
		if (currentStage >= 2)
		{
			foreach (GameObject go in towerSpawnPoints)
			{
				// Instantiate a tower
				GameObject.Instantiate(towerEnemy, go.transform.position, go.transform.rotation);
			}
		}

		// Spawn the BOSS
		if (currentStage >= 3)
		{
		}

		// Wait a frame before ending
		yield return new WaitForSecondsRealtime(3f);

		camera.FollowPlayer = true;
	}
	
	/// <summary>
	/// Called by each enemy in Death
	/// </summary>
	public void OnEnemyDeath()
	{
		if (enemyCount <= 0)
		{
			// The Game is over
			Debug.Log("Game is over!");
		}
		else
		{
			enemyCount--;
		}
	}

    public void OnClickStartButton()
    {
        hardMode = false;
        foreach(Button b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        StartCoroutine(InitializeStage());

    }

    /// <summary>
    /// Menu buttons functions
    /// </summary>
    public void OnClickHardModeButton()
    {
        hardMode = true;
        foreach (Button b in buttons)
        {
            b.gameObject.SetActive(false);
           
        }
        StartCoroutine(InitializeStage());

    }

    public void OnClickExitButton()
    {
        UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }


}
