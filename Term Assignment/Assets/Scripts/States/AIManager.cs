using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	public static AIManager Instance;
	public bool AdvancedAI = true;
	public bool playerFound = false;

	List<Vector3> guardPoints = new List<Vector3>();

	bool guard = false;

	[SerializeField] private Transform guardPos;
	[SerializeField] private Transform AmbushPos;

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("GuardPoint")) {
			guardPoints.Add(go.transform.position);
		}
	}

	public void StartAdvancedStates()
	{
		guard = true;
		playerFound = true;
	}

	/// <summary>
	/// Returns what job the asking AI should do
	/// </summary>
	/// <returns> [0 -> Guard] [1 -> Ambusher] </returns>
	public Transform GetTarget()
	{
		return null;
		//bool job = true; ;
		//if (guard)
		//{
		//	job = true;
		//}
		//else
		//{
		//	job = false;
		//}

		//guard = !guard;
		//return job;
	}
}
