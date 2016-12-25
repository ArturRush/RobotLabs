using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
	public GameObject Robot;
	public bool working;
	private RobotCtrl robot;
	void Start()
	{
		working = false;
		robot = Robot.GetComponent<RobotCtrl>();
		robot.LeftPower = 1;
		robot.RightPower = 1;
	}
	
	void Update()
	{
		if (!working) return;
		robot.LeftPower = 1;
		robot.RightPower = 1;
		if (robot.rightDist > 6 && !robot.collision)
		{
			robot.MoveLeftForward();
		}
		else
		{
			robot.MoveRightForward();
			robot.MoveRightForward();
		}
	}
}
