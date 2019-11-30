using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	float life = 5;
	float counter = 0;

	// Update is called once per frame
	void Update()
	{
		counter += Time.deltaTime;
		if (counter > life)
		{
			Destroy(gameObject);
		}
	}
}
