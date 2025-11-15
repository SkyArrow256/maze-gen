using System.Drawing;

class Program
{
	static void Main()
	{
		var width = 37;
		var height = 37;
		var start = new Point(2, 2);

		var maze = new Maze(width, height, start, 100);
		maze.Generate();
	}
}