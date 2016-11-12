using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MazeCtrl : MonoBehaviour
{
	private List<List<int>> maze;
	public GameObject block;
	private GameObject floor;
	public GameObject cam;
	public GameObject robot;

	// Use this for initialization
	void Start()
	{
		string fileName = "C:\\lessons\\robots\\robotics\\test.txt";
		maze = ReadMaze(fileName);
		floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
		floor.transform.localScale = new Vector3(maze.Count / 10f, 1, maze[0].Count / 10f);
		floor.transform.position = new Vector3(maze.Count / 2f, 0, maze[0].Count / 2f);
		var c = cam.GetComponent<Camera>();
		c.orthographicSize = (maze.Count<maze[0].Count ? maze.Count : maze[0].Count)/2f;
		c.rect = new Rect(new Vector2(0, 0), new Vector2(1, floor.transform.localScale.x / floor.transform.localScale.z));
		cam.transform.position = new Vector3(maze.Count/2f, 10, maze[0].Count/2f);
		robot.transform.position = new Vector3(maze.Count / 2f, 0.6f, maze[0].Count / 2f);
		Vector2 size = new Vector2(0, 0);
		for (int i = 0; i < maze.Count; ++i)
		{
			for (int j = 0; j < maze[i].Count; ++j)
			{
				if (maze[i][j] == 1)
				{
					size.x = 0;
					size.y = 0;
					for (int k = j; k < maze[0].Count; ++k)
					{
						maze[i][k] = 2;
						if (k + 1 == maze[0].Count || maze[i][k + 1] != 1)
						{
							size.y = k + 1 - j;
							size.x = 1;
							break;
						}
					}
					bool r = true;//Не знаю как назвать, но означает, что следующая строка тоже подходит
					while (r)
					{
						if (i + (int) size.x == maze.Count) break;
						for (int k = j; k < j + size.y; ++k)
						{
							if (maze[i + (int)size.x][k] != 1)
							{
								r = false;
								break;
							}
						}
						if (!r) break;
						for (int k = j; k < j + size.y; ++k)
						{
							maze[i + (int)size.x][k] = 2;
						}
						size.x += 1;
					}
					var b = (GameObject)Instantiate(block, new Vector3(i + size.x / 2f, maze[i][j] / 4f, j + size.y / 2f), Quaternion.Euler(0, 0, 0));
					b.transform.parent = transform;
					b.transform.localScale = new Vector3(size.x, maze[i][j]/2f, size.y);
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	List<List<int>> ReadMaze(string fileName)
	{
		List<string> tmp = File.ReadAllLines(fileName).ToList();
		tmp.RemoveAt(0);
		List<List<int>> res = new List<List<int>>();
		//Чтение файла с пробелами
		//foreach (var s in tmp)
		//{
		//	string[] a = s.Split(new []{' '},StringSplitOptions.RemoveEmptyEntries);
		//	res.Add(new List<int>());
		//	foreach (var s1 in a)
		//	{
		//		res[res.Count-1].Add(Int32.Parse(s1));
		//	}
		//}

		//Чтение файла без пробелов
		foreach (var s in tmp)
		{
			res.Add(new List<int>());
			for (int i = 0; i < s.Length; i++)
			{
				res[res.Count - 1].Add(Int32.Parse(s[i].ToString()));
			}
		}
		return res;
	}
}
