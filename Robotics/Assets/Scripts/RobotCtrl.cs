using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;

public class RobotCtrl : MonoBehaviour
{
	public float rotationSpeed;
	public GameObject lw;//Левое колесо
	public GameObject rw;//Правое колесо
	public GameObject leftSensor;
	public GameObject rightSensor;
	public GameObject lsEnd;
	public GameObject rsEnd;
	public float leftDist;
	public float rightDist;

	private Vector3 prevPos;

	private float lp;
	/// <summary>
	/// Мощность левого колеса
	/// </summary>
	public float Lp
	{
		get { return lp; }
		set { lp = Mathf.Clamp(value, -1.0f, 1.0f); }
	}

	private float rp;
	/// <summary>
	/// Мощность правого колеса
	/// </summary>
	public float Rp
	{
		get { return rp; }
		set { rp = Mathf.Clamp(value, -1.0f, 1.0f); }
	}

	public bool collision;//Есть ли столкновение с препятствием?
	private bool odd;//Четный шаг выполнения движения
	private Action move;

	void Start()
	{
		odd = false;
		collision = false;
		prevPos = transform.position;
	}

	void Update()
	{
		//Обработка датчиков
		RaycastHit hit;
		Ray leftRay = new Ray(leftSensor.transform.position, lsEnd.transform.position - leftSensor.transform.position);
		Ray rightRay = new Ray(rightSensor.transform.position, rsEnd.transform.position - rightSensor.transform.position);
		if (Physics.Raycast(leftRay, out hit, 6) && !hit.collider.CompareTag("Finish"))
		{
			leftDist = Vector3.Distance(hit.point, leftSensor.transform.position);
		}
		else
		{
			leftDist = 1000;
		}
		if (Physics.Raycast(rightRay, out hit, 6) && !hit.collider.CompareTag("Finish"))
		{
			rightDist = Vector3.Distance(hit.point, rightSensor.transform.position);
		}
		else
		{
			rightDist = 1000;
		}

		//Если врезался
		if (collision)
		{
			//Юнити не может в Invoke, не оптимизировать!
			if (move != null)
				move();
		}
		if (prevPos != transform.position)
			prevPos = transform.position;
	}

	void OnTriggerEnter(Collider oth)
	{
		if (collision || oth.CompareTag("Finish")) return;
		collision = true;
	}

	void OnTriggerExit(Collider col)
	{
		if (!collision) return;
		collision = false;
	}

	/// <summary>
	/// Ехать вперед левым колесом
	/// </summary>
	public void MoveLF()
	{
		if (!collision)
			move = MoveLB;
		MoveLw(lp);
	}
	/// <summary>
	/// Ехать вперед правым колесом
	/// </summary>
	public void MoveRF()
	{
		if (!collision)
			move = MoveRB;
		MoveRw(rp);
	}
	/// <summary>
	/// Ехать назад левым колесом
	/// </summary>
	public void MoveLB()
	{
		if (!collision)
			move = MoveLF;
		MoveLw(-lp);
	}
	/// <summary>
	/// Ехать назад правым колесом
	/// </summary>
	public void MoveRB()
	{
		if (!collision)
			move = MoveRF;
		MoveRw(-rp);
	}
	/// <summary>
	/// Ехать вперед
	/// </summary>
	public void MoveF()
	{
		if (rotationSpeed <= 10)
		{
			if (!collision)
				move = MoveB;
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
		else
		{
			//Выпрямление траектории
			rotationSpeed /= 20f;
			for (int i = 0; i < 20; ++i)
			{
				if (!collision)
					move = MoveB;
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
			rotationSpeed *= 20f;
		}
	}
	/// <summary>
	/// Ехать назад
	/// </summary>
	public void MoveB()
	{
		if (rotationSpeed <= 10)
		{
			if (!collision)
				move = MoveF;
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
		else
		{
			//Выпрямление траектории
			rotationSpeed /= 20f;
			for (int i = 0; i < 20; ++i)
			{
				if (!collision)
					move = MoveF;
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
			rotationSpeed *= 20f;
		}
	}

	private void MoveLw(float l)
	{
		transform.RotateAround(rw.transform.position, new Vector3(0, 1, 0), rotationSpeed * l);
	}

	private void MoveRw(float r)
	{
		transform.RotateAround(lw.transform.position, new Vector3(0, 1, 0), -rotationSpeed * r);
	}
}