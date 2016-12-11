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
		robot.Lp = 1;
		robot.Rp = 1;
	}
	
	void Update()
	{
		if (!working) return;
		robot.Lp = 1;
		robot.Rp = 1;
		if (robot.rightDist > 6 && !robot.collision)
		{
			robot.MoveLF();
		}
		else
		{
			robot.MoveRF();
			robot.MoveRF();
		}
	}
}
