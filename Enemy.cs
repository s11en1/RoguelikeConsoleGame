using System;
using System.Collections.Generic;

namespace RoguelikeFreelance
{
    /// <summary>
    /// Класс шаблон для будующих врагов
    /// </summary>
    internal class Enemy
    {
        #region Переменные
        // [Настройки врага]
        protected char enemySymbol;
        protected ConsoleColor color = ConsoleColor.Red;

        // [Настройки Управления]
        protected List<int[]> directions = new List<int[]>() // Создаем список направлений
        {
            new int[] {1, 0}, // Право
            new int[] {-1, 0}, // Лево
            new int[] {0, 1}, // Низ
            new int[] {0, -1} // Верх
        };
        protected int[] currentDirection;
        protected int[] currentPosition;
        protected int currentX;
        protected int currentY;
        protected Random random;

        // [Прочие Компоненты]
        protected MazeGenerator mazeGenerator;
        protected Player player;
        #endregion

        public Enemy(MazeGenerator mazeGenerator, Player player,Random random)
        {
            this.random = random;
            this.mazeGenerator = mazeGenerator;
            this.player = player;

            Spawn();
            ChangeDirection();
        }


        /// <summary>
        /// Метод который будет вызываться каждый импровизированный кадр
        /// </summary>
        public virtual void Update()
        {
            Move();
            PrintEnemy();
        }

        #region Методы для отрисовки

        /// <summary>
        /// Отрисовка врага и его движение
        /// </summary>
        protected void PrintEnemy()
        {
            SetEnemyColor();
            SetCursorOnEnemyPosition();
            DrawEnemy();
        }

        /// <summary>
        /// Установка цвета врага
        /// </summary>
        protected void SetEnemyColor() => Console.ForegroundColor = color;

        /// <summary>
        /// Установка курсора на текущую позицию врага
        /// </summary>
        protected void SetCursorOnEnemyPosition() => Console.SetCursorPosition(currentX, currentY);

        /// <summary>
        /// Отрисовка врага
        /// </summary>
        protected void DrawEnemy() => Console.Write(enemySymbol);
        #endregion

        #region Методы для управления

        /// <summary>
        /// Метод для передвижения врага
        /// </summary>
        protected virtual void Move()
        {
            int newX = currentPosition[0] + currentDirection[0];
            int newY = currentPosition[1] + currentDirection[1];

            if (mazeGenerator.IsInBounds(newY, newX))
            {
                if (mazeGenerator.IsWall(newY, newX))
                    ChangeDirection();
                else
                {
                    currentPosition = new int[] { newX, newY };
                    UpdateEnemyPosition();
                }

            }
            else
                ChangeDirection();

        }

        /// <summary>
        /// Метод для спавна врага на карте
        /// </summary>
        public void Spawn()
        {
            while (true)
            {
                int spawnX = random.Next(mazeGenerator.width);
                int spawnY = random.Next(mazeGenerator.height);

                if (mazeGenerator.IsWall(spawnX, spawnY) == false)
                {
                    this.currentPosition = new int[] { spawnY, spawnX };
                    UpdateEnemyPosition();
                    return;
                }
            }
        }

        /// <summary>
        /// Смена направления движения
        /// </summary>
        protected virtual void ChangeDirection() => currentDirection = directions[random.Next(directions.Count)];

        /// <summary>
        /// Запись текущего положения врага
        /// </summary>
        protected void UpdateEnemyPosition()
        {
            currentX = currentPosition[0];
            currentY = currentPosition[1];
        }

        /// <summary>
        /// Аттака игрока
        /// </summary>
        public void Attack() => player.GetDamage();

        /// <summary>
        /// Проверка на то может ли враг атаковать игрока
        /// </summary>
        /// <returns></returns>
        public virtual void CanAttack()
        {
            if(true)
                Attack();
        }
        #endregion
    }
}
