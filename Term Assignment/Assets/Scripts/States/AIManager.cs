using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
	public static AIManager Instance;
	public bool AdvancedAI = true;
	public bool playerFound = false;

	Queue<Transform> guardPoints = new Queue<Transform>();

	bool guard = false;

	[SerializeField] private Transform guardPos;
	[SerializeField] private Transform AmbushPos;

	public enum EnemyJob { NONE, GUARD, ATTACKER }

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
	}

	public void StartAdvancedStates()
	{
		guardPoints.Clear();
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("GuardPoint"))
		{
			guardPoints.Enqueue(go.transform);
		}

		guard = true;
		playerFound = true;
	}

	public EnemyJob GetJob()
	{
		EnemyJob job = EnemyJob.NONE;
		if (guard && guardPoints.Count > 0)
		{
			job = EnemyJob.GUARD;
		}
		else
		{
			job = EnemyJob.ATTACKER;
		}

		guard = !guard;
		return job;
	}

	public bool GetTarget(ref Transform guardPos)
	{
		if (guardPoints.Count > 0)
		{
			guardPos = guardPoints.Dequeue();
			return true;
		}

		return false;
	}
}
