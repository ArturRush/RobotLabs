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
			int freeWidth = 10;
			int wallWidth = 5;
			int mazeWidth = 500;
			int mazeHeigth = 500;
			tmpN = mazeHeigth / (freeWidth + wallWidth);
			tmpM = mazeWidth / (freeWidth + wallWidth);
			N = 2 * tmpN + 1;
			M = 2 * tmpM + 1;
			int a = tmpN - 1;
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
			SaveToRoboLabs(freeWidth, wallWidth);
			//Console.WriteLine("Укажите путь к файлу лабиринта");
			//mazeFromFilePath = Console.ReadLine();
			//MazeFromFile(mazeFromFilePath);
			//SaveAsPicture(maze);
			//SaveAsText(maze);
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
			maze[start.X][start.Y] = 2;
			maze[fin.X][fin.Y] = 5;
		}

		static void PrintMaze()
		{
			for (int i = 0; i < N; ++i)
			{
				for (int j = 0; j < M; ++j)
					Console.Write(maze[i][j]);
				Console.Write("\n");
			}
		}

		static void SaveAsPicture(List<List<int>> mz)
		{
			Bitmap pic = new Bitmap(mz[0].Count, mz.Count);
			using (var g = Graphics.FromImage(pic))
				g.Clear(Color.White);

			for (int i = 0; i < mz.Count; ++i)
			{
				for (int j = 0; j < mz[0].Count; ++j)
				{
					switch (mz[i][j])
					{
						case 1:
							pic.SetPixel(j, i, Color.Black);
							break;
						case 2:
							pic.SetPixel(j, i, Color.GreenYellow);
							break;
						case 3:
							pic.SetPixel(j, i, Color.Red);
							break;
					}
				}
			}
			pic.Save(filePath + ".png", ImageFormat.Png);
		}

		static void SaveAsText(List<List<int>> mz)
		{
			using (StreamWriter f =
			new StreamWriter(filePath + ".txt"))
			{
				f.Write(tmpN + " " + tmpM + "\n");
				for (int i = 0; i < mz.Count; ++i)
				{
					for (int j = 0; j < mz[0].Count; ++j)
						f.Write(mz[j][i]);
					f.Write("\n");
				}
			}
		}

		/// <summary>
		/// Класс для сохранения лабиринта по спецификации лаб
		/// </summary>
		/// <param name="freeWidth">ширина прохода</param>
		/// <param name="filledWidth">ширина стен</param>
		static void SaveToRoboLabs(int freeWidth, int filledWidth)
		{
			List<List<int>> res = new List<List<int>>();

			for (int i = 0; i < N; ++i)
			{
				res.Add(new List<int>());
				for (int j = 0; j < M; ++j)
				{
					for (int k = 0; k < (j % 2 == 1 ? freeWidth : filledWidth); ++k)
						res[res.Count - 1].Add(maze[i][j]);
				}
				for (int k = 0; k < (i % 2 == 1 ? freeWidth - 1 : filledWidth - 1); ++k)
				{
					res.Add(res[res.Count - 1]);
				}
			}
			SaveAsPicture(res);
			SaveAsText(res);
		}

		static bool ml()
		{
			var tmp = new Point(cur.X - 2, cur.Y);
			maze[cur.X - 1][cur.Y] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[tmp.X - 2][tmp.Y] = 3;
				return false;
			}
			cur = tmp;
			return true;
		}

		static bool mr()
		{
			var tmp = new Point(cur.X + 2, cur.Y);
			maze[cur.X + 1][cur.Y] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[cur.X + 2][cur.Y] = 3;
				return false;
			}
			cur = tmp;
			return true;
		}

		static bool mu()
		{
			var tmp = new Point(cur.X, cur.Y - 2);
			maze[cur.X][cur.Y - 1] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[cur.X][cur.Y - 2] = 3;
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
			maze[cur.X][cur.Y + 1] = 0;
			MadeVisited(tmp);
			if (tmp == fin)
			{
				maze[cur.X][cur.Y + 2] = 3;
				return false;
			}
			cur = tmp;
			return true;
		}

		static void GetNeigh()
		{
			if (cur.X - 2 > 0 && !CheckVisit(new Point(cur.X - 2, cur.Y)))
				neigh.Add(Directions.Left);
			if (cur.X + 2 < N && !CheckVisit(new Point(cur.X + 2, cur.Y)))
				neigh.Add(Directions.Right);
			if (cur.Y - 2 > 0 && !CheckVisit(new Point(cur.X, cur.Y - 2)))
				neigh.Add(Directions.Up);
			if (cur.Y + 2 < M && !CheckVisit(new Point(cur.X, cur.Y + 2)))
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
			return t == 0 || t == 2 || t == 3;
		}
	}
}
