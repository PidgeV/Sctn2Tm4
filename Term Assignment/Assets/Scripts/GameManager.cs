using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject targetDummy;
	[SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();

	// Start is called before the first frame update
	void Start()
	{
		StartStage();
	}

	/// <summary>
	/// Initialize a stage of the game
	/// Load enemy, clear bullets etc 
	/// </summary>
	void StartStage()
	{
		foreach (Transform pos in enemySpawnPoints) {
			GameObject.Instantiate(targetDummy, pos.position, pos.rotation);
		}
	}
}
