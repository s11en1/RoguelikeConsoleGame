using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RoguelikeFreelance
{
    internal class Player
    {
        #region Переменные
        // [Настройки Игрока]
        public const char playerSymbol = '@';
        public int[] currentPosition { get; private set; }
        private int currentX;
        private int currentY;

        // [Лабиринт]
        private MazeGenerator maze;

        // [Настройки Управления]
        private List<int[]> directions = new List<int[]>() // Создаем список направлений
        {
            new int[] {1, 0}, // Право
            new int[] {-1, 0}, // Лево
            new int[] {0, 1}, // Низ
            new int[] {0, -1} // Верх
        };
        private int[] currentDirection;
        private ConsoleKeyInfo pressedKey;
        private bool canHandleInput;

        // [Контроллер Здоровья]
        public int healthCount { get; private set; }
        #endregion

        /// <summary>
        /// Конструктор игрока в котором 
        /// инициализируем лабиринт 
        /// устанавливаем позицию игрока и 
        /// задаем начальное направление движения
        /// </summary>
        /// <param name="maze"></param>
        public Player(MazeGenerator maze)
        {
            this.maze = maze;

            Spawn();
            currentDirection = directions[0];
            canHandleInput = true;

            healthCount = 3;

            StartHandleInput();
        }


        /// <summary>
        /// Метод, который будет вызываться каждый ипровизированный кадр
        /// </summary>
        public void Update()
        {
            Move();
            PrintPlayer();
        }

        #region Методы для отрисовки
        /// <summary>
        /// Метод отрисовки игрока
        /// </summary>
        private void PrintPlayer()
        {
            // Устанавливаем цвет cтавим курсор
            // в текущую позицию игрока
            // и рисуем там игрока
            SetPlayerColor();
            SetCursorOnPlayerPosition();
            DrawPlayer();
        }

        /// <summary>
        /// Вывод другой информации
        /// </summary>
        public void PrintOtherInformation()
        {
            // Методы отрисовки информации об игроке
            PrintHealthCount();
            PrintLastPressedKey();
        }

        /// <summary>
        /// Устанавливаем цвет для отрисовки игрока в консоли
        /// </summary>
        private void SetPlayerColor() => Console.ForegroundColor = ConsoleColor.Yellow;

        /// <summary>
        /// Устанавливаем курсор на текущую позицию игрока
        /// </summary>
        private void SetCursorOnPlayerPosition() => Console.SetCursorPosition(currentX, currentY);

        /// <summary>
        /// Выводим последнюю нажатую кнопку
        /// </summary>
        private void PrintLastPressedKey()
        {
            Console.SetCursorPosition(maze.height + 1, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Последняя нажатая кнопка: {pressedKey.KeyChar}");
        }

        /// <summary>
        /// Метод для вывода количества здоровья
        /// </summary>
        private void PrintHealthCount()
        {
            Console.SetCursorPosition(maze.height + 1, 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Количество здоровья: {healthCount}");
        }

        /// <summary>
        /// Рисуем игрока
        /// </summary>
        private void DrawPlayer() => Console.Write(playerSymbol);
        #endregion

        #region Методы для управления персонажем

        /// <summary>
        /// Метод получения позиции игрока
        /// </summary>
        private void UpdatePlayerPosition()
        {
            // Записываем координаты игрока
            currentX = currentPosition[0];
            currentY = currentPosition[1];
        }

        /// <summary>
        /// Передвижение персонажа
        /// </summary>
        private void Move()
        {
            // Проверка не сменилось ли направление
            ChangeDirection();

            // Запись новых координат игрока
            int newX = currentPosition[0] + currentDirection[0];
            int newY = currentPosition[1] + currentDirection[1];

            // Проверяем если данная точка в пределах лабиринта и
            // там не стенка то можно туда переместить игрока
            if (maze.IsInBounds(newY, newX) && maze.IsWall(newY, newX) == false)
            {
                currentPosition = new int[] { newX, newY };
                UpdatePlayerPosition();
            }
                
        }

        private void Spawn()
        {
            currentPosition = new int[] { 0, 1 };
            UpdatePlayerPosition();
        }

        /// <summary>
        /// Метод который запускает обработку ввода в другом потоке
        /// 
        /// </summary>
        public void StartHandleInput()
        {
            Task.Run(() =>
            {
                while (canHandleInput)
                {
                    pressedKey = Console.ReadKey();
                }
            });
        }

        /// <summary>
        /// Метод для остановки обработки вывода
        /// </summary>
        public void StopHandleInput() => canHandleInput = false;

        /// <summary>
        /// Смена направления в зависимости от нажатой кнопки
        /// </summary>
        private void ChangeDirection()
        {
            switch (pressedKey.Key)
            {
                case ConsoleKey.W:
                    currentDirection = directions[3];
                    break;
                case ConsoleKey.A:
                    currentDirection = directions[1];
                    break;
                case ConsoleKey.S:
                    currentDirection = directions[2];
                    break;
                case ConsoleKey.D:
                    currentDirection = directions[0];
                    break;
            }
        }

        /// <summary>
        /// Проверяем можем ли мы закончить игру
        /// </summary>
        /// <returns></returns>
        public bool TryToWin() => maze.IsExit(currentY, currentX);
        #endregion

        #region Методы контроллера здоровья
        public void GetDamage() => healthCount--;
        #endregion
    }
}
