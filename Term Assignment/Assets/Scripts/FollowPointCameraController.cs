using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPointCameraController : MonoBehaviour
{
	[SerializeField] private GameObject idle;
	[SerializeField] private GameObject player;

	// The point to move to
	[SerializeField]  private GameObject target;

	private bool active = true;

	// Set the cameras target
	public bool FollowPlayer
	{
		get { return active; }
		set
		{
			if (value == true)
			{
				active = true;
				target = player;
			}
			else
			{
				active = false;
				target = idle;
			}
		}
	}

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.1f);
		transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, 0.1f);
	}
}
