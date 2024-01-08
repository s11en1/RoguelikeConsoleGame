using System;
using System.Collections.Generic;
using System.Threading;

namespace RoguelikeFreelance
{
    // Перечисление возможных исходов игры
    public enum GameResult
    {
        Win,
        Lose
    }

    internal class Program
    {
        #region Переменные
        // [Генератор лабиринта]
        private static int mapWidth;
        private static int mapHeight;
        private static MazeGenerator mazeGenerator;

        // [Контроль игры]
        private static bool isGameOver;
        private static int frameUpdateTime = 500;
        private static Random random = new Random();

        // [Игрок]
        private static Player player;

        // [Враги]
        private static List<Enemy> enemies;
        #endregion

        #region Методы инициализации

        /// <summary>
        /// Инициализация игры
        /// </summary>
        private static void InitializeGame()
        {
            isGameOver = false;
            Console.CursorVisible = false;

            InitializeMazeGenerator();
            InitializePlayer();
            InitializeEnemies();
        }

        /// <summary>
        /// Получение ввода для настройки размеров лабиринта
        /// </summary>
        private static void GetInput()
        {
            Console.Write("Введите ширину лабиринта (по умолчанию 10): ");
            if (int.TryParse(Console.ReadLine(), out mapWidth) == false)
                mapWidth = 10;

            Console.Write("Введите высоту лабиринта (по умолчанию 10): ");
            if (int.TryParse(Console.ReadLine(), out mapHeight) == false)
                mapHeight = 10;
        }

        /// <summary>
        /// Инициализация генератора лабиринта
        /// </summary>
        private static void InitializeMazeGenerator()
        {
            GetInput();
            mazeGenerator = new MazeGenerator(mapWidth, mapHeight, random);
            mazeGenerator.GenerateMaze();
        }

        /// <summary>
        /// Инициализация игрока
        /// </summary>
        private static void InitializePlayer() => player = new Player(mazeGenerator);

        /// <summary>
        /// Инициализация врагов
        /// </summary>
        private static void InitializeEnemies()
        {
            enemies = new List<Enemy>()
            {
                new Shooter(mazeGenerator,player,random),
                new Shooter(mazeGenerator,player,random),
                new Zombie(mazeGenerator,player,random),
                new Zombie(mazeGenerator,player,random),
            };
        }
        #endregion

        #region Методы состояния игры

        /// <summary>
        /// Перезапуск игры
        /// </summary>
        private static void RestartGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            InitializeGame();
        }

        /// <summary>
        /// Остановка игры
        /// </summary>
        /// <param name="gameResult"></param>
        private static void EndGame(GameResult gameResult)
        {
            Console.Clear();

            player.StopHandleInput();
            isGameOver = true;

            if (gameResult == GameResult.Win)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ты добрался до выхода, поздравляем с победой!!!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ты не добрался до выхода, это поражение");
            }

            Console.WriteLine("Нажми любую клавишу для продолжения...");
            Console.ReadKey();
            RestartGame();
        }
        #endregion

        #region Методы обновления

        /// <summary>
        /// Очистка кадра
        /// </summary>
        private static void ClearFrame()
        {
            Thread.Sleep(frameUpdateTime);
            Console.Clear();
        }

        /// <summary>
        /// Обновление врагов
        /// </summary>
        private static void UpdateEnemies()
        {
            foreach (Enemy enemy in enemies)
                enemy.Update();
        }

        /// <summary>
        /// Проверка на атаку
        /// </summary>
        private static void CheckDamage()
        {
            foreach (Enemy enemy in enemies)
                enemy.CanAttack();
        }

        /// <summary>
        /// Обновление кадра
        /// </summary>
        private static void UpdateFrame()
        {
            // Рисуем лабиринт
            mazeGenerator.PrintMaze(); 

            // Обновляем позицию и
            // перерисовываем врагов и игрока
            UpdateEnemies();
            player.Update();

            // Проверяем нанесли ли игроку урон и
            // выводим остальную информацию
            CheckDamage();
            player.PrintOtherInformation();

            // Пробуем получить результат игры
            TryToGetResultOfGame();
        }

        /// <summary>
        /// Попытка получить результат игры
        /// </summary>
        private static void TryToGetResultOfGame()
        {
            if (player.healthCount <= 0)
                EndGame(GameResult.Lose);
            else if (player.TryToWin())
                EndGame(GameResult.Win);
        }
        #endregion

        /// <summary>
        /// Точка входа мейн в которой все происходит
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            InitializeGame();

            while (isGameOver == false)
            {
                ClearFrame();

                UpdateFrame();
            }
        }
    }
}
