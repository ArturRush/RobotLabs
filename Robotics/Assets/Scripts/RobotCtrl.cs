using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;

public class RobotCtrl : MonoBehaviour
{
	public float rotationSpeed;
	public GameObject LeftWheel;//Левое колесо
	public GameObject RightWheel;//Правое колесо
	public GameObject leftSensor;
	public GameObject rightSensor;
	public GameObject lsEnd;//Позиция конца левого сенсора
	public GameObject rsEnd;//Позиция конца правого сенсора
	public float leftDist;
	public float rightDist;

	private Vector3 prevPos;

	private float _leftPower;
	/// <summary>
	/// Мощность левого колеса
	/// </summary>
	public float LeftPower
	{
		get { return _leftPower; }
		set { _leftPower = Mathf.Clamp(value, -1.0f, 1.0f); }
	}

	private float _rightPower;
	/// <summary>
	/// Мощность правого колеса
	/// </summary>
	public float RightPower
	{
		get { return _rightPower; }
		set { _rightPower = Mathf.Clamp(value, -1.0f, 1.0f); }
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
	public void MoveLeftForward()
	{
		if (!collision)
			move = MoveLeftBack;
		MoveLeftWheel(_leftPower);
	}
	/// <summary>
	/// Ехать вперед правым колесом
	/// </summary>
	public void MoveRightForward()
	{
		if (!collision)
			move = MoveRightBack;
		MoveRightWheel(_rightPower);
	}
	/// <summary>
	/// Ехать назад левым колесом
	/// </summary>
	public void MoveLeftBack()
	{
		if (!collision)
			move = MoveLeftForward;
		MoveLeftWheel(-_leftPower);
	}
	/// <summary>
	/// Ехать назад правым колесом
	/// </summary>
	public void MoveRightBack()
	{
		if (!collision)
			move = MoveRightForward;
		MoveRightWheel(-_rightPower);
	}
	/// <summary>
	/// Ехать вперед
	/// </summary>
	public void MoveForward()
	{
		if (rotationSpeed <= 10)
		{
			if (!collision)
				move = MoveBack;
			if (odd)
			{
				MoveRightWheel(_rightPower);
				MoveLeftWheel(_leftPower);
			}
			else
			{
				MoveLeftWheel(_leftPower);
				MoveRightWheel(_rightPower);
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
					move = MoveBack;
				if (odd)
				{
					MoveRightWheel(_rightPower);
					MoveLeftWheel(_leftPower);
				}
				else
				{
					MoveLeftWheel(_leftPower);
					MoveRightWheel(_rightPower);
				}
				odd = !odd;
			}
			rotationSpeed *= 20f;
		}
	}
	/// <summary>
	/// Ехать назад
	/// </summary>
	public void MoveBack()
	{
		if (rotationSpeed <= 10)
		{
			if (!collision)
				move = MoveForward;
			if (odd)
			{
				MoveRightWheel(-_rightPower);
				MoveLeftWheel(-_leftPower);
			}
			else
			{
				MoveLeftWheel(-_leftPower);
				MoveRightWheel(-_rightPower);
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
					move = MoveForward;
				if (odd)
				{
					MoveRightWheel(-_rightPower);
					MoveLeftWheel(-_leftPower);
				}
				else
				{
					MoveLeftWheel(-_leftPower);
					MoveRightWheel(-_rightPower);
				}
				odd = !odd;
			}
			rotationSpeed *= 20f;
		}
	}

	private void MoveLeftWheel(float l)
	{
		transform.RotateAround(RightWheel.transform.position, new Vector3(0, 1, 0), rotationSpeed * l);
	}

	private void MoveRightWheel(float r)
	{
		transform.RotateAround(LeftWheel.transform.position, new Vector3(0, 1, 0), -rotationSpeed * r);
	}
}