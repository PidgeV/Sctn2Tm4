using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
	[SerializeField] private GameObject target;

	// Update is called once per frame
	void LateUpdate()
	{
		transform.position = target.transform.position + new Vector3(0, 50, 0);
	}
}
