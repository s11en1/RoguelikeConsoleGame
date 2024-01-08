using System;

namespace RoguelikeFreelance
{
    internal class Shooter : Enemy
    {
        #region Переменные
        // [Настройки снаряда стрелка]
        private char bullet = '+'; // Символ пули
        private int[] bulletPosition = new int[2]; // Позиция пули
        private int bulletX; // Координата X пули
        private int bulletY; // Координата Y пули
        private int[] shootDirection = new int[2]; // Направление выстрела
        private int currentBulletDistance; // Текущая дистанция пули т.е. сколько она уже пролетела
        private int maxBulletDistance = 5; // Максимальная дистанция полета пули
        private bool thereIsAbullet = false; // Булевая переменная существует ли пуля в данный момент
        #endregion

        public Shooter(MazeGenerator mazeGenerator, Player player, Random random) : base(mazeGenerator, player, random)
        {
            enemySymbol = 'S';
        }

        #region Переопределенные методы

        public override void CanAttack()
        {
            if (thereIsAbullet &&
                bulletPosition[0] == player.currentPosition[0] &&
                    bulletPosition[1] == player.currentPosition[1])
                Attack();
        }

        public override void Update()
        {
            base.Update();

            if (CanShoot())
                Shoot();
            UpdateBullet();
        }
        #endregion

        #region Управление стрельбой

        /// <summary>
        /// Метод, который показывает можем ли мы выстрелить
        /// </summary>
        /// <returns></returns>
        private bool CanShoot() => !thereIsAbullet;

        /// <summary>
        /// Метод выстрела
        /// </summary>
        private void Shoot()
        {
            GetShootDirection();
            ResetBullet();
        }

        /// <summary>
        /// Получение направления выстрела
        /// </summary>
        private void GetShootDirection() => shootDirection = directions[random.Next(directions.Count)];

        /// <summary>
        /// Пересоздание пули
        /// </summary>
        private void ResetBullet()
        {
            bulletPosition = currentPosition;
            UpdateBulletPosition();
            currentBulletDistance = 0;
            thereIsAbullet = true;
        }

        /// <summary>
        /// Перемещение пули
        /// </summary>
        private void ChangeBulletPosition()
        {
            int tempX = bulletPosition[0] + shootDirection[0];
            int tempY = bulletPosition[1] + shootDirection[1];

            if (mazeGenerator.IsInBounds(tempY, tempX) && mazeGenerator.IsWall(tempY, tempX) == false)
            {
                bulletPosition = new int[] { tempX, tempY };
                UpdateBulletPosition();
                currentBulletDistance++;
            }
            else
                DestroyBullet();
        }

        /// <summary>
        /// Обновление позиции пули
        /// </summary>
        private void UpdateBulletPosition()
        {
            bulletX = bulletPosition[0];
            bulletY = bulletPosition[1];
        }

        /// <summary>
        /// Отрисовка пули
        /// </summary>
        private void DrawBullet()
        {
            Console.SetCursorPosition(bulletX, bulletY);
            Console.Write(bullet);
        }

        /// <summary>
        /// Обновление пули на кадре
        /// </summary>
        private void UpdateBullet()
        {
            if (currentBulletDistance >= maxBulletDistance)
                DestroyBullet();

            ChangeBulletPosition();
            if (thereIsAbullet)
                DrawBullet();
        }

        /// <summary>
        /// Уничтожение пули
        /// </summary>
        private void DestroyBullet() => thereIsAbullet = false;
        #endregion
    }
}
