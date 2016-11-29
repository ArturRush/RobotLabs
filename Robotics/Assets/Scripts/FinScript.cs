using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FinScript : MonoBehaviour
{
	//public Text finText;
	public void OnTriggerEnter(Collider rob)
	{
		if (rob.CompareTag("Robot"))
		{
			Debug.Log("Triggered");
			//var fin = (Text) Instantiate(finText, transform);
			FindObjectOfType<Text>().text = "Лабиринт пройден";
		}
	}
}
