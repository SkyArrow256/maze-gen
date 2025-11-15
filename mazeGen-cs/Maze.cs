using System.Drawing;
using System.Runtime.InteropServices;

class Maze
{
	/// <summary>
	/// 迷路盤
	/// </summary>
	private readonly List<List<Cell>> field;
	/// <summary>
	/// 迷路の開始地点
	/// </summary>
	private readonly Point start;
	/// <summary>
	/// 迷路の生成スピード
	/// </summary>
	private readonly TimeSpan speed;

	/// <summary>
	/// 迷路の盤を作成する
	/// </summary>
	/// <param name="width">盤の幅。奇数である必要があります</param>
	/// <param name="height">盤の高さ。奇数である必要があります</param>
	/// <param name="start">スタート地点。偶数である必要があります</param>
	/// <param name="speed">迷路の生成スピード</param>
	public Maze(int width, int height, Point start, TimeSpan speed)
	{
		field = new List<List<Cell>>(height);

		var topAndBottom = new List<Cell>(width);
		for (int i = 0; i < width; ++i)
		{
			topAndBottom.Add(Cell.Route);
		}

		var other = new List<Cell>(width);
		other.Add(Cell.Route);
		for (int i = 1; i < width - 1; ++i)
		{
			other.Add(Cell.Block);
		}
		other.Add(Cell.Route);

		field.Add(topAndBottom);
		for (int i = 1; i < height - 1; ++i)
		{
			field.Add([.. other]);
		}
		field.Add([.. topAndBottom]);

		field[start.Y][start.X] = Cell.Route;

		this.start = start;
		this.speed = speed;
	}
	/// <summary>
	/// 迷路を生成する
	/// </summary>
	public void Generate()
	{
		Drive(start);
	}
	/// <summary>
	/// 迷路を掘る
	/// </summary>
	/// <param name="current">現在の探索基準場所を示す。</param>
	private void Drive(Point current)
	{
		Display();
		Thread.Sleep(speed);

		List<Point> dirs = [new Point(0, -2), new Point(-2, 0), new Point(0, 2), new Point(2, 0)];
		Random.Shared.Shuffle(CollectionsMarshal.AsSpan(dirs));

		foreach (var dir in dirs)
		{
			switch (field[current.Y + dir.Y][current.X + dir.X])
			{
				case
					Cell.Block:
					// 穴を2つ開ける
					field[current.Y + dir.Y / 2][current.X + dir.X / 2] = Cell.Route;
					field[current.Y + dir.Y][current.X + dir.X] = Cell.Route;

					//次のステップへ
					Drive(new Point(current.X + dir.X, current.Y + dir.Y));
					break;
				case
					Cell.Route:
					continue;
			}
		}
	}
	/// <summary>
	/// 迷路を表示する
	/// </summary>
	private void Display()
	{
		foreach (var line in field)
		{
			foreach (var cell in line)
			{
				switch (cell)
				{
					case Cell.Block:
						Console.Write("■ ");
						break;
					case Cell.Route:
						Console.Write("  ");
						break;
				}
			}
			Console.WriteLine();
		}
	}
}

/// <summary>
/// セルの種類
/// </summary>
enum Cell
{
	/// <summary>
	/// 壁
	/// </summary>
	Block,
	/// <summary>
	/// 通路
	/// </summary>
	Route,
}