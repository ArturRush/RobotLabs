using System;
using UnityEngine;

public class RobotCtrl : MonoBehaviour
{
	public float rotationSpeed;
	public GameObject lw;
	public GameObject rw;
	public float lp;
	public float rp;
	private bool collision;
	public float moveBackTime;
	private float mbTime;
	void Start()
	{
		collision = false;
		mbTime = moveBackTime;
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.A))
		{
			MoveLW(-lp);
		}
		if (Input.GetKey(KeyCode.Q))
		{
			MoveLW(lp);
		}
		if (Input.GetKey(KeyCode.E))
		{
			MoveRW(rp);
		}
		if (Input.GetKey(KeyCode.D))
		{
			MoveRW(-rp);
		}
		if (Input.GetKey(KeyCode.W))
		{
			MoveLW(lp);
			MoveRW(rp);
		}
		if (Input.GetKey(KeyCode.S))
		{
			MoveLW(-lp);
			MoveRW(-rp);
		}
		if (collision)
		{
			mbTime -= Time.deltaTime;
			if (mbTime <= 0)
			{
				lp = 0;
				rp = 0;
				collision = false;
				mbTime = moveBackTime;
			}
		}
	}

	void OnTriggerEnter(Collider oth)
	{
		if (collision) return;
		lp *= -1;
		rp *= -1;
		collision = true;
	}

	public void MoveLW(float l)
	{
		transform.RotateAround(rw.transform.position, new Vector3(0, 1, 0), rotationSpeed * l);
		
	}

	public void MoveRW(float r)
	{
		transform.RotateAround(lw.transform.position, new Vector3(0, 1, 0), -rotationSpeed * r);
	}

}