using UnityEngine;
using System.Collections;

public class RobotCtrl : MonoBehaviour
{
	public float speed;
	public float rotationSpeed;
	public GameObject lw;
	public GameObject rw;
	private bool rotateLeft;
	private bool rotateRight;
	void Start()
	{
		rotateLeft = true;
		rotateRight = true;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			GetComponent<Rigidbody>().velocity = -transform.forward * speed;
			//transform.position -= transform.forward * speed;
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			GetComponent<Rigidbody>().velocity = transform.forward * speed;
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		}
		if (Input.GetKey(KeyCode.A) && rotateLeft)
		{
			transform.RotateAround(lw.transform.position, new Vector3(0, 1, 0), -rotationSpeed);
			if(Input.GetKey(KeyCode.W))
				GetComponent<Rigidbody>().velocity = -transform.forward * speed;
		}
		if (Input.GetKey(KeyCode.D) && rotateRight)
		{
			transform.RotateAround(rw.transform.position, new Vector3(0, 1, 0), rotationSpeed);
			if (Input.GetKey(KeyCode.S))
				GetComponent<Rigidbody>().velocity = transform.forward * speed;
		}
	}

	void OnTriggerEnter(Collider oth)
	{
		if (Input.GetKey(KeyCode.A))
		rotateLeft = false;
		if (Input.GetKey(KeyCode.D))
			rotateRight = false;
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
	}

	void OnTriggerExit(Collider oth)
	{
		rotateLeft = true;
		rotateRight = true;
	}
}