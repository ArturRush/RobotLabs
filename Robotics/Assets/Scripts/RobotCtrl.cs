using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RobotCtrl : MonoBehaviour
{
	public float rotationSpeed;
	public GameObject lw;//Левое колесо
	public GameObject rw;//Правое колесо

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

	private bool collision;//Есть ли столкновение с препятствием?
	private bool odd;//Четный шаг выполнения движения
	private Action move;
	
	void Start()
	{
		odd = false;
		collision = false;
	}

	void Update()
	{
		if (collision)
		{
			move();
		}

		//Управление с клевиатуры
		if (Input.GetKey(KeyCode.A))
		{
			MoveLB();
			return;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			MoveLF();
			return;
		}
		if (Input.GetKey(KeyCode.E))
		{
			MoveRF();
			return;
		}
		if (Input.GetKey(KeyCode.D))
		{
			MoveRB();
			return;
		}
		if (Input.GetKey(KeyCode.W))
		{
			MoveF();
			return;
		}
		if (Input.GetKey(KeyCode.S))
		{
			MoveB();
			return;
		}
	}

	void OnTriggerEnter(Collider oth)
	{
		if (collision) return;
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
	/// <summary>
	/// Ехать назад
	/// </summary>
	public void MoveB()
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

	private void MoveLw(float l)
	{
		transform.RotateAround(rw.transform.position, new Vector3(0, 1, 0), rotationSpeed * l);
	}

	private void MoveRw(float r)
	{
		transform.RotateAround(lw.transform.position, new Vector3(0, 1, 0), -rotationSpeed * r);
	}

}