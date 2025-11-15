use maze_gen::Maze;
use std::time::Duration;

fn main() {
    let width = 31;
    let height = 31;
    let start = (2, 2).into();

    let mut maze = Maze::new(width, height, start, Duration::from_millis(10));
    maze.generate();
}
