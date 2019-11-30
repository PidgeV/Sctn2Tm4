using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		Destroy(collision.gameObject);
		Destroy(gameObject);
	}

	private void OnDisable()
	{
		GameManager.Instance.OnEnemyDeath();
	}
}
