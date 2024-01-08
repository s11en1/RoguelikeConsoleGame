using System;
using System.Collections.Generic;

namespace RoguelikeFreelance
{
    class MazeGenerator
    {
        #region Переменные
        private char[,] maze; // Сам лабиринт

        // Размеры лабиринта сделаны как публичные
        // свойства для отрисовки некоторой информации
        public int width { get; private set; } // Ширина лабиринта
        public int height { get; private set; } // Высота лабиринта

        // Константные символы для обозначений лабиринта
        public const char wall = '#';
        public const char ground = '·';
        public const char exit = 'X';
        public const char enter = 'O';

        private List<int[]> controlPoints;
        private int maxWallLength = 3;

        private List<int[]> directions = new List<int[]>() // Создаем список направлений
        {
            new int[] {0, 1}, // Право
            new int[] {0, -1}, // Лево
            new int[] {1, 0}, // Низ
            new int[] {-1, 0} // Верх
        };
        private Random random;
        #endregion

        /// <summary>
        /// Это конструктор в котором мы инициализируем размеры лабиринта
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public MazeGenerator(int width, int height, Random random)
        {
            this.width = width;
            this.height = height;
            this.random = random;
        }

        #region Методы проверки

        /// <summary>
        /// В этом методе мы проверяем находимся ли мы в границах лабиринта
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsInBounds(int x, int y) => x >= 0 && x < width && y >= 0 && y < height;

        /// <summary>
        /// Проверка на то стена ли в данной координате
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsWall(int x, int y) => maze[x, y] == wall;

        /// <summary>
        /// Проверка на то выход ли в данной координате
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsExit(int x, int y) => maze[x,y] == exit;
        #endregion

        #region Методы инициализации и генерации лабиринта
        /// <summary>
        /// В этом методе мы по порядку вызываем методы инициалиции
        /// и генерации и возвращаем наш лабиринт
        /// </summary>
        public void GenerateMaze()
        {
            InitializeMaze();
            FillMaze();
            DrawBounds();
            GetControlPoints();
            DrawWalls();
            DrawEntranceAndExit();
        }

        /// <summary>
        /// В этом методе мы инициализируем новый лабиринт
        /// </summary>
        private void InitializeMaze() => maze = new char[width, height];

        /// <summary>
        /// Заполнение лабиринта пустотой
        /// </summary>
        private void FillMaze()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    maze[i, j] = ground;
        }

        /// <summary>
        /// Обрисовка стен лабиринта
        /// </summary>
        private void DrawBounds()
        {
            for (int i = 0; i < height; i++)
                maze[0, i] = wall;

            for (int i = 0; i < width; i++)
                maze[i, 0] = wall;

            for (int i = 0; i < height; i++)
                maze[width - 1, i] = wall;

            for (int i = 0; i < width; i++)
                maze[i, height - 1] = wall;
        }

        /// <summary>
        /// Получаем контрольные точки по которым будем отрисовывать стены
        /// </summary>
        private void GetControlPoints()
        {
            controlPoints = new List<int[]>();

            for (int i = 2; i < width - 2; i += 2)
                for (int j = 2; j < height - 2; j += 2)
                    controlPoints.Add(new int[] { i, j });
        }

        /// <summary>
        /// Зарисовка стен внутри лабиринта
        /// </summary>
        private void DrawWalls()
        {
            // Проходим по листу контрольных точек чтобы от каждой точки отрисовать стену
            foreach (int[] controlPoint in controlPoints)
            {
                // Берем координаты нашей контрольной точки
                int x = controlPoint[0];
                int y = controlPoint[1];

                // Проверка на то что на нашей контрольной точке уже не стоит стена
                if (maze[x, y] == wall)
                    continue;

                // Выбираем случайное направление от контрольной точки и записываем вектор направления
                int[] direction = directions[random.Next(directions.Count)];

                int dirX = direction[0];
                int dirY = direction[1];

                /*В цикле проходимся от нашей контрольной точки до максимальной
                длины стены по выбраному направлению и если выбранная координата
                находится в рамках лабиринта то ставим стену*/

                for (int i = 0; i < maxWallLength; i++)
                {
                    if (IsInBounds(x, y))
                        maze[x, y] = wall;

                    x += dirX;
                    y += dirY;
                }
            }
        }

        /// <summary>
        /// Отрисовка входа и выхода
        /// </summary>
        private void DrawEntranceAndExit()
        {
            // Отрисовываем вход в левом верхнем углу
            maze[1, 0] = enter;

            // Отрисовываем выход в правом нижнем углу
            maze[width - 2, height - 1] = exit;
        }
        #endregion

        #region Методы для отрисовки лабиринта

        /// <summary>
        /// Добавим чуть цветной графики
        /// </summary>
        /// <param name="x">Координата по Х</param>
        /// <param name="y">Координата по Y</param>
        /// <returns>Цвет в который будет покрашена клетка</returns>
        private ConsoleColor GetConsoleColor(int x, int y)
        {
            ConsoleColor color = ConsoleColor.White;

            switch (maze[x, y])
            {
                case wall:
                    color = ConsoleColor.Blue;
                    break;
                case enter:
                case exit:
                    color = ConsoleColor.Green;
                    break;
            }
            return color;
        }

        /// <summary>
        /// В этом методе мы рисуем наш лабиринт в консоль
        /// </summary>
        public void PrintMaze()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Console.ForegroundColor = GetConsoleColor(i, j); // Получаем цвет символа
                    Console.Write(maze[i, j]); // Печатаем наш символ
                }
                Console.WriteLine();
            }
        }
        #endregion
    }
}
