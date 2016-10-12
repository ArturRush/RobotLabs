using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MazeGenerator
{
	class Program
	{
		private static List<List<int>> maze;
		private static List<Point> visited = new List<Point>();
		private static Stack<Point> path = new Stack<Point>();
		private static List<Directions> neigh;
		private static int N;//Высота
		private static int M;//Ширина
		private static Point cur;//Текущая позиция(ширина;высота)
		private static Point start;
		private static Point fin;
		private static string filePath;
		static void Main(string[] args)
		{
			Console.WriteLine("Введите высоту лабиринта");
			Int32.TryParse(Console.ReadLine(), out N);
			N = 2 * N + 1;
			Console.WriteLine("Введите ширину лабиринта");
			Int32.TryParse(Console.ReadLine(), out M);
			M = 2 * M + 1;
			int a;
			Console.WriteLine("Введите стартовую точку");
			Int32.TryParse(Console.ReadLine(), out a);
			start.X = a * 2 + 1;
			Int32.TryParse(Console.ReadLine(), out a);
			start.Y = a * 2 + 1;
			Console.WriteLine("Введите конечную точку");
			Int32.TryParse(Console.ReadLine(), out a);
			fin.X = a * 2 + 1;
			Int32.TryParse(Console.ReadLine(), out a);
			fin.Y = a * 2 + 1;
			cur = start;
			visited.Add(cur);
			neigh = new List<Directions>();
			GenerateMaze();
			//PrintMaze();
			Console.WriteLine("Укажите имя лабиринта");
			filePath = Console.ReadLine();
			SaveAsPicture();
			//SaveAsText();
		}

		static void GenerateMaze()
		{
			FillMaze();

			path.Push(cur);
			Random r = new Random();
			float accum = 0;
			float size = ((M - 1)/2)*((N - 1)/2);
			while (path.Count > 0)
			{
				GetNeigh();
				if (neigh.Count > 0)
				{
					Directions next = neigh[r.Next(0, neigh.Count)];
					switch (next)
					{
						case Directions.Left:
							if (!ml())
								cur = path.Pop();
							break;
						case Directions.Right:
							if (!mr())
								cur = path.Pop();
							break;
						case Directions.Up:
							if (!mu())
								cur = path.Pop();
							break;
						case Directions.Down:
							if (!md())
								cur = path.Pop();
							break;
					}
					neigh.Clear();

					++accum;
					Console.Write("\r{0}%", 100 * (accum / size));
				}
				else
					cur = path.Pop();
			}
		}

		static void FillMaze()
		{
			maze = new List<List<int>>(N);
			for (int i = 0; i < N; ++i)
			{
				maze.Add(new List<int>(M));
				for (int j = 0; j < M; ++j)
					maze[i].Add(i % 2 == 0 || j % 2 == 0 ? 1 : 0);
			}
			maze[start.Y][start.X] = 2;
			maze[fin.Y][fin.X] = 3;
		}

		static void PrintMaze()
		{
			for (int i = 0; i < N; ++i)
			{
				for (int j = 0; j < M; ++j)
					Console.Write(maze[j][i]);
				Console.Write("\n");
			}
		}

		static void SaveAsPicture()
		{
			Bitmap pic = new Bitmap(N, M);
			using (var g = Graphics.FromImage(pic))
				g.Clear(Color.White); 

			for (int i = 0; i < N; ++i)
			{
				for (int j = 0; j < M; ++j)
				{
					switch (maze[i][j])
					{
						case 1:
							pic.SetPixel(i, j, Color.Black);
							break;
						case 2:
							pic.SetPixel(i, j, Color.GreenYellow);
							break;
						case 3:
							pic.SetPixel(i, j, Color.Red);
							break;
					}
				}
			}
			pic.Save(filePath + ".png", ImageFormat.Png);
		}

		static void SaveAsText()
		{
			using (StreamWriter f =
			new StreamWriter(filePath+".txt"))
			{
				for (int i = 0; i < N; ++i)
				{
					for (int j = 0; j < M; ++j)
						f.Write(maze[j][i]);
					f.Write("\n");
				}
			}
		}

		static bool ml()
		{
			var tmp = new Point(cur.X - 2, cur.Y);
			if (tmp == fin)
			{
				maze[cur.Y][cur.X - 1] = 0;
				visited.Add(fin);
				return false;
			}
			maze[cur.Y][cur.X - 1] = 0;
			path.Push(cur);
			cur = tmp;
			visited.Add(cur);
			return true;
		}

		static bool mr()
		{
			var tmp = new Point(cur.X + 2, cur.Y);
			if (tmp == fin)
			{
				maze[cur.Y][cur.X + 1] = 0;
				visited.Add(fin);
				return false;
			}
			maze[cur.Y][cur.X + 1] = 0;
			path.Push(cur);
			cur = tmp;
			visited.Add(cur);
			return true;
		}

		static bool mu()
		{
			var tmp = new Point(cur.X, cur.Y - 2);
			if (tmp == fin)
			{
				maze[cur.Y - 1][cur.X] = 0;
				visited.Add(fin);
				return false;
			}
			maze[cur.Y - 1][cur.X] = 0;
			path.Push(cur);
			cur = tmp;
			visited.Add(cur);
			return true;
		}

		static bool md()
		{
			var tmp = new Point(cur.X, cur.Y + 2);
			if (tmp == fin)
			{
				maze[cur.Y + 1][cur.X] = 0;
				visited.Add(fin);
				return false;
			}
			maze[cur.Y + 1][cur.X] = 0;
			path.Push(cur);
			cur = tmp;
			visited.Add(cur);
			return true;
		}

		static void GetNeigh()
		{
			if (cur.X - 2 > 0 && !visited.Contains(new Point(cur.X - 2, cur.Y)))
				neigh.Add(Directions.Left);
			if (cur.X + 2 < M && !visited.Contains(new Point(cur.X + 2, cur.Y)))
				neigh.Add(Directions.Right);
			if (cur.Y - 2 > 0 && !visited.Contains(new Point(cur.X, cur.Y - 2)))
				neigh.Add(Directions.Up);
			if (cur.Y + 2 < N && !visited.Contains(new Point(cur.X, cur.Y + 2)))
				neigh.Add(Directions.Down);
		}

		enum Directions
		{
			Left,
			Right,
			Up,
			Down
		}
	}
}
