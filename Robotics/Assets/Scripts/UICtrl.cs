using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
	public GameObject Robot;
	private bool odd = false;
	public Button lf;
	public Button rf;
	public Button lb;
	public Button rb;
	public Button f;
	public Button b;

	private bool LockPower;
	private bool LockDirection;

	public Button lp;
	public Button ld;

	public Slider ls;
	public Slider rs;
	public Text lt;
	public Text rt;

	public Sprite lockedPic;
	public Sprite unLockedPic;

	public EventSystem eventSys;

	public void Start()
	{
		f.gameObject.SetActive(false);
		b.gameObject.SetActive(false);
		lt.text = ls.value.ToString("0.0");
		rt.text = rs.value.ToString("0.0");

		Robot.GetComponent<RobotCtrl>().Lp = ls.value;
		Robot.GetComponent<RobotCtrl>().Rp = rs.value;
	}
	
	public void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == lf.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveLF();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == rf.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveRF();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == lb.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveLB();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == rb.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveRB();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == f.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveF();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == b.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveB();
			}
		}

	}

	public void LeftSliderChanged()
	{
		Robot.GetComponent<RobotCtrl>().Lp = ls.value;
		lt.text = ls.value.ToString("0.0", CultureInfo.InvariantCulture);
		if (LockPower)
			rs.value = ls.value;
	}

	public void RightSliderChanged()
	{
		Robot.GetComponent<RobotCtrl>().Rp = rs.value;
		rt.text = rs.value.ToString("0.0", CultureInfo.InvariantCulture);
		if (LockPower)
			ls.value = rs.value;
	}

	public void lpClick()
	{
		if (LockPower)
		{
			lp.GetComponent<Image>().sprite = unLockedPic;
			LockPower = false;
		}
		else
		{
			lp.GetComponent<Image>().sprite = lockedPic;
			LockPower = true;
		}
	}

	public void ldClick()
	{
		if (LockDirection)
		{
			ld.GetComponent<Image>().sprite = unLockedPic;
			LockDirection = false;
			lf.gameObject.SetActive(true);
			f.gameObject.SetActive(false);
			rf.gameObject.SetActive(true);
			lb.gameObject.SetActive(true);
			b.gameObject.SetActive(false);
			rb.gameObject.SetActive(true);
		}
		else
		{
			ld.GetComponent<Image>().sprite = lockedPic;
			LockDirection = true;
			lf.gameObject.SetActive(false);
			f.gameObject.SetActive(true);
			rf.gameObject.SetActive(false);
			lb.gameObject.SetActive(false);
			b.gameObject.SetActive(true);
			rb.gameObject.SetActive(false);
		}
	}
}
