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
	private bool odd;
	void Start()
	{
		odd = false;
		collision = false;
		mbTime = moveBackTime;
	}

	void Update()
	{
		if (collision)
		{
			mbTime -= Time.deltaTime;
			if (mbTime <= 0)
			{
				lp *= -1;
				rp *= -1;
				collision = false;
				mbTime = moveBackTime;
			}
		}
		if (Input.GetKey(KeyCode.A))
		{
			MoveLw(-lp);
		}
		if (Input.GetKey(KeyCode.Q))
		{
			MoveLw(lp);
		}
		if (Input.GetKey(KeyCode.E))
		{
			MoveRw(rp);
		}
		if (Input.GetKey(KeyCode.D))
		{
			MoveRw(-rp);
		}
		if (Input.GetKey(KeyCode.W))
		{
			if (odd)
			{
				MoveRw(rp);
				MoveLw(lp);
			}
			else
			{
				MoveLw(lp);
				MoveRw(rp);
			}
			odd = !odd;
		}
		if (Input.GetKey(KeyCode.S))
		{
			if (odd)
			{
				MoveRw(-rp);
				MoveLw(-lp);
			}
			else
			{
				MoveLw(-lp);
				MoveRw(-rp);
			}
			odd = !odd;
		}
	}

	void OnTriggerEnter(Collider oth)
	{
		if (collision) return;
		lp *= -1;
		rp *= -1;
		collision = true;
	}

	public void MoveLw(float l)
	{
		transform.RotateAround(rw.transform.position, new Vector3(0, 1, 0), rotationSpeed * l);
	}

	public void MoveRw(float r)
	{
		transform.RotateAround(lw.transform.position, new Vector3(0, 1, 0), -rotationSpeed * r);
	}

}