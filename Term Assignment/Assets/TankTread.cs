﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TankTread : MonoBehaviour
{
	Animator animator;

	// Start is called before the first frame update
	void Start()
	{
		animator = gameObject.GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{

	}
}