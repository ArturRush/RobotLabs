using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{
	public GameObject Robot;
	public Button lfButton;
	public Button rfButton;
	public Button lbButton;
	public Button rbButton;
	public Button fButton;
	public Button bButton;
	public Text aiText;
	public GameObject AI;

	private bool LockPower;
	private bool LockDirection;

	public Button LockPowerButton;
	public Button LockDirectionButton;

	public Slider LeftPowerSlider;
	public Slider RightPowerSlider;
	public Text LeftPowerText;
	public Text RightPowerText;

	public Sprite lockedPic;
	public Sprite unLockedPic;

	public EventSystem eventSys;

	public void Start()
	{
		fButton.gameObject.SetActive(false);
		bButton.gameObject.SetActive(false);
		LeftPowerText.text = LeftPowerSlider.value.ToString("0.0");
		RightPowerText.text = RightPowerSlider.value.ToString("0.0");

		Robot.GetComponent<RobotCtrl>().LeftPower = LeftPowerSlider.value;
		Robot.GetComponent<RobotCtrl>().RightPower = RightPowerSlider.value;
	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == lfButton.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveLeftForward();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == rfButton.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveRightForward();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == lbButton.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveLeftBack();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == rbButton.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveRightBack();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == fButton.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveForward();
			}
			if (eventSys.GetComponent<EventSystem>().currentSelectedGameObject == bButton.gameObject)
			{
				Robot.GetComponent<RobotCtrl>().MoveBack();
			}
		}

	}

	public void LeftSliderChanged()
	{
		Robot.GetComponent<RobotCtrl>().LeftPower = LeftPowerSlider.value;
		LeftPowerText.text = LeftPowerSlider.value.ToString("0.0", CultureInfo.InvariantCulture);
		if (LockPower)
			RightPowerSlider.value = LeftPowerSlider.value;
	}

	public void RightSliderChanged()
	{
		Robot.GetComponent<RobotCtrl>().RightPower = RightPowerSlider.value;
		RightPowerText.text = RightPowerSlider.value.ToString("0.0", CultureInfo.InvariantCulture);
		if (LockPower)
			LeftPowerSlider.value = RightPowerSlider.value;
	}

	public void lpClick()
	{
		if (LockPower)
		{
			LockPowerButton.GetComponent<Image>().sprite = unLockedPic;
			LockPower = false;
		}
		else
		{
			LockPowerButton.GetComponent<Image>().sprite = lockedPic;
			LockPower = true;
			RightPowerSlider.value = LeftPowerSlider.value;
		}
	}

	public void ldClick()
	{
		if (LockDirection)
		{
			LockDirectionButton.GetComponent<Image>().sprite = unLockedPic;
			LockDirection = false;
			lfButton.gameObject.SetActive(true);
			fButton.gameObject.SetActive(false);
			rfButton.gameObject.SetActive(true);
			lbButton.gameObject.SetActive(true);
			bButton.gameObject.SetActive(false);
			rbButton.gameObject.SetActive(true);
		}
		else
		{
			LockDirectionButton.GetComponent<Image>().sprite = lockedPic;
			LockDirection = true;
			lfButton.gameObject.SetActive(false);
			fButton.gameObject.SetActive(true);
			rfButton.gameObject.SetActive(false);
			lbButton.gameObject.SetActive(false);
			bButton.gameObject.SetActive(true);
			rbButton.gameObject.SetActive(false);
		}
	}

	public void AiButtonClick()
	{
		aiText.text = AI.GetComponent<AIScript>().working ? "Start AI" : "Stop AI";
		AI.GetComponent<AIScript>().working = !AI.GetComponent<AIScript>().working;
	}
}
