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
		//private static List<Point> visited = new List<Point>();
		private static Stack<Point> path = new Stack<Point>();
		private static List<Directions> neigh;
		private static int N;//Высота
		private static int M;//Ширина
		static int tmpN;
		static int tmpM;
		private static Point cur;//Текущая позиция(ширина;высота)
		private static Point start;
		private static Point fin;
		private static string filePath;
		private static string mazeFromFilePath;
		static void Main(string[] args)
		{
			////==============Ввод с консоли===========================
			//Console.WriteLine("Введите высоту лабиринта");
			//Int32.TryParse(Console.ReadLine(), out tmpN);
			//N = 2 * tmpN + 1;
			//Console.WriteLine("Введите ширину лабиринта");
			//Int32.TryParse(Console.ReadLine(), out tmpM);
			//M = 2 * tmpM + 1;
			//int a;
			//Console.WriteLine("Введите стартовую точку");
			//Int32.TryParse(Console.ReadLine(), out a);
			//start.X = a * 2 + 1;
			//Int32.TryParse(Console.ReadLine(), out a);
			//start.Y = a * 2 + 1;
			//Console.WriteLine("Введите конечную точку");
			//Int32.TryParse(Console.ReadLine(), out a);
			//fin.X = a * 2 + 1;
			//Int32.TryParse(Console.ReadLine(), out a);
			//fin.Y = a * 2 + 1;
			////==============Хардкооооод!!!===========================
			tmpN = 100;
			tmpM = 100;
			N = 2 * tmpN + 1;
			M = 2 * tmpM + 1;
			int a = 0;
			int b = 0;
			int c = tmpN - 1;
			int d = tmpM - 1;
			start.X = a * 2 + 1;
			start.Y = b * 2 + 1;
			fin.X = c * 2 + 1;
			fin.Y = d * 2 + 1;
			cur = start;
			neigh = new List<Directions>();
			GenerateMaze();
			//PrintMaze();
			Console.WriteLine("Укажите имя лабиринта");
			filePath = Console.ReadLine();
			//Console.WriteLine("Укажите путь к файлу лабиринта");
			//mazeFromFilePath = Console.ReadLine();
			//MazeFromFile(mazeFromFilePath);
			SaveAsPicture();
			//SaveAsText();
		}

		static void GenerateMaze()
		{
			FillMaze();
			path.Push(cur);
			bool isStart = true;
			Random r = new Random();
			while (path.Count > 0)// && visited.Count < tmpN * tmpM)
			{
				if (isStart)
				{
					path.Pop();
					isStart = false;
				}
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
					//Console.Write("\r{0:##.000}% visited", 100 * ((float)visited.Count / (tmpN * tmpM)));
				}
				else
					cur = path.Pop();
			}
		}

		static void MazeFromFile(string f)
		{
			maze = File.ReadAllLines(f).Select(l => l.Select(i => Int32.Parse(i.ToString())).ToList()).ToList();
		}
		//Строит решетку, на основе которой создается лабиринт
		//Обозначения:
		//0 - посещенная свободная для прохода клетка
		//1 - стена
		//2 - старт
		//3 - финиш
		//5 - непосещенная свободная клетка
		static void FillMaze()
		{
			maze = new List<List<int>>(N);
			for (int i = 0; i < N; ++i)
			{
				maze.Add(new List<int>(M));
				for (int j = 0; j < M; ++j)
					maze[i].Add(i % 2 == 0 || j % 2 == 0 ? 1 : 5);
			}
			maze[start.Y][start.X] = 2;
			maze[fin.Y][fin.X] = 5;
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
			new StreamWriter(filePath + ".txt"))
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
			maze[cur.Y][cur.X - 1] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[tmp.Y][tmp.X - 2] = 3;
				return false;
			}
			cur = tmp;
			return true;
		}

		static bool mr()
		{
			var tmp = new Point(cur.X + 2, cur.Y);
			maze[cur.Y][cur.X + 1] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[cur.Y][cur.X + 2] = 3;
				return false;
			}
			cur = tmp;
			return true;
		}

		static bool mu()
		{
			var tmp = new Point(cur.X, cur.Y - 2);
			maze[cur.Y - 1][cur.X] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[cur.Y - 2][cur.X] = 3;
				return false;
			}
			if (tmp == fin)
				return false;
			cur = tmp;
			return true;
		}

		static bool md()
		{
			var tmp = new Point(cur.X, cur.Y + 2);
			maze[cur.Y + 1][cur.X] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[cur.Y + 2][cur.X] = 3;
				return false;
			}
			cur = tmp;
			return true;
		}

		static void GetNeigh()
		{
			if (cur.X - 2 > 0 && !CheckVisit(new Point(cur.X - 2, cur.Y)))
				neigh.Add(Directions.Left);
			if (cur.X + 2 < M && !CheckVisit(new Point(cur.X + 2, cur.Y)))
				neigh.Add(Directions.Right);
			if (cur.Y - 2 > 0 && !CheckVisit(new Point(cur.X, cur.Y - 2)))
				neigh.Add(Directions.Up);
			if (cur.Y + 2 < N && !CheckVisit(new Point(cur.X, cur.Y + 2)))
				neigh.Add(Directions.Down);
			if (neigh.Count > 1)
				path.Push(cur);
		}

		enum Directions
		{
			Left,
			Right,
			Up,
			Down
		}

		static void MadeVisited(Point p)
		{
			maze[p.X][p.Y] = 0;
		}

		static bool CheckVisit(Point p)
		{
			int t = maze[p.X][p.Y];
			return t == 0 || t == 2 || t ==3;
		}
	}
}
