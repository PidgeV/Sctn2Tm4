using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
	public static MusicController Instance;

	AudioSource audioSource;

	public List<AudioClip> sounds = new List<AudioClip>();

	// Start is called before the first frame update
	void Start()
	{
		Instance = this;
		audioSource = GetComponent<AudioSource>();
	}

	public void Shoot()
	{
		audioSource.PlayOneShot(sounds[0]);
	}

	public void Hit()
	{
		audioSource.PlayOneShot(sounds[1]);
	}
}
