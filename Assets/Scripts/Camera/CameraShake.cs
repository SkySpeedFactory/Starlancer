using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] private float durationOfShake = 10f;
	[SerializeField] private float decreaseFactor = 0.025f;

	[SerializeField] private float amountOfShake = 0.7f;
	

	Vector3 startingPosition;

	void Start()
	{
		startingPosition = transform.localPosition;
	}

	void Update()
	{
		if (durationOfShake > 0)
		{
			transform.localPosition = startingPosition + Random.insideUnitSphere * amountOfShake;
			durationOfShake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			durationOfShake = 0f;
			transform.localPosition = startingPosition;
		}
	}
}
