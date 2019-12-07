using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class Player : MonoBehaviour
{
	[SerializeField] private Transform cameraHelper;
	[SerializeField] private GameObject camera;

	[SerializeField] private Transform barrel;
	[SerializeField] private GameObject bullet;
    
    public GameObject landMinePrefab;
    public List<GameObject> landMineList = new List<GameObject>();
    public Transform landMineSpawnPosition;
    public int maxLandmineCount = 3;

	public Animator rightTread;
	public Animator leftTread;

	public float speed = 15f;
	public float rotation = 55f;

	private float currentSpeed;
	private float currentRotation;

	public float shootCooldown = 1.0f;
	public float shootInterval = 0.0f;

	public VisualEffect smokeEffect;

	bool wait = true;

	// Start is called before the first frame update
	IEnumerator Start()
	{
		currentSpeed = speed;
		currentRotation = rotation;
		shootInterval = 0.0f;

		yield return new WaitForSecondsRealtime(0.5f);
		wait = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (wait)
		{
			return;
		}

		shootInterval += Time.deltaTime;
		if (shootInterval > shootCooldown)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				smokeEffect.SendEvent("EmitSmoke");
				GameObject shot = GameObject.Instantiate(bullet);
				shot.transform.position = barrel.position;

				if (shot.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
				{
					rigidbody.velocity = transform.forward * 100;
				}

				shootInterval = 0.0f;
			}
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			currentSpeed = Mathf.Lerp(currentSpeed, speed * 3.0f, 0.01f);
			currentRotation = Mathf.Lerp(currentRotation, rotation * 0.2f, 0.01f);
		}
		else
		{
			currentSpeed = Mathf.Lerp(currentSpeed, speed, 0.01f);
			currentRotation = Mathf.Lerp(currentRotation, rotation, 0.01f);
		}

		Vector3 targetPos = transform.position + (transform.forward * -7.0f) + new Vector3(0, 7, 0);

		float x = Input.GetAxis("Horizontal") * Time.deltaTime * currentRotation;
		float y = Input.GetAxis("Vertical") * Time.deltaTime * currentSpeed;

		y = y < 0 ? y *= 0.4f : y;

		transform.Translate(0, 0, y);
		transform.Rotate(0, x, 0);

		camera.transform.position = Vector3.Lerp(camera.transform.position, targetPos, 0.1f);
		camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, cameraHelper.rotation, 0.1f);

		SetAnimators(x, y);

        if(Input.GetMouseButtonDown(0) && landMineList.Count < maxLandmineCount)
        {
            PlaceLandmine();
        }
	}

	/// <summary>
	/// This will set the speed for the animations
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	void SetAnimators(float x, float y)
	{
		float rSpeed = 0;
		float lSpeed = 0;

		if (x > 0)
		{
			rightTread.SetFloat("Speed", rSpeed);
			leftTread.SetFloat("Speed", lSpeed);
		}
		else if (x < 0)
		{

		}
		else
		{

		}
	}

    void PlaceLandmine()
    {
        GameObject landmine = Instantiate(landMinePrefab, landMineSpawnPosition);
        landmine.transform.parent = null;

        landMineList.Add(landmine);
    }
}
