use std::{ops::Add, thread, time::Duration};

/// 迷路
pub struct Maze {
    /// 迷路盤
    field: Vec<Vec<Cell>>,
    /// 開始地点
    start: Point,
    /// 生成スピード
    speed: Duration,
}

impl Maze {
    /// 迷路の盤を作成する
    pub fn new(width: usize, height: usize, start: Point, speed: Duration) -> Self {
        let mut field = Vec::with_capacity(height);

        let mut line = vec![Cell::Block; width];
        line[0] = Cell::Route;
        line[width - 1] = Cell::Route;

        field.push(vec![Cell::Route; width]);
        (1..height - 1).for_each(|_| field.push(line.clone()));
        field.push(vec![Cell::Route; width]);

        field[start.y as usize][start.x as usize] = Cell::Route;

        Self {
            field,
            start,
            speed,
        }
    }
    /// 迷路を生成する
    pub fn generate(&mut self) {
        self.drive(self.start);
    }
    ///　迷路を掘る
    fn drive(&mut self, current: Point) {
        self.display();
        thread::sleep(self.speed);

        let mut dirs = [
            Point { x: 0, y: -2 },
            Point { x: -2, y: 0 },
            Point { x: 0, y: 2 },
            Point { x: 2, y: 0 },
        ];
        fastrand::shuffle(&mut dirs);

        for dir in dirs {
            match self.field[(current.y + dir.y) as usize][(current.x + dir.x) as usize] {
                Cell::Block => {
                    // 穴を2つ開ける
                    self.field[(current.y + dir.y / 2) as usize]
                        [(current.x + dir.x / 2) as usize] = Cell::Route;
                    self.field[(current.y + dir.y) as usize][(current.x + dir.x) as usize] =
                        Cell::Route;

                    // 次のステップへ
                    self.drive(current + dir);
                }
                Cell::Route => continue,
            }
        }
    }
    /// 迷路を表示する
    fn display(&self) {
        for line in &self.field {
            for cell in line {
                match cell {
                    Cell::Block => print!("■ "),
                    Cell::Route => print!("  "),
                }
            }
            println!();
        }
    }
}

/// セルの種類
#[derive(Clone)]
enum Cell {
    /// 壁
    Block,
    /// 通路
    Route,
}

/// 直行座標
#[derive(Clone, Copy)]
pub struct Point {
    x: isize,
    y: isize,
}

impl From<(isize, isize)> for Point {
    fn from(p: (isize, isize)) -> Self {
        Point { x: p.0, y: p.1 }
    }
}

impl Add for Point {
    type Output = Point;

    fn add(self, rhs: Self) -> Self::Output {
        Self {
            x: self.x + rhs.x,
            y: self.y + rhs.y,
        }
    }
}
