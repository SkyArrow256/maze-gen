using System.Drawing;

class Program
{
	static void Main()
	{
		var width = 37;
		var height = 37;
		var start = new Point(2, 2);
		var speed = TimeSpan.FromMilliseconds(10);

		var maze = new Maze(width, height, start, speed);
		maze.Generate();
	}
}