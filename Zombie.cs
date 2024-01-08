using System;

namespace RoguelikeFreelance
{
    internal class Zombie : Enemy
    {
        public Zombie(MazeGenerator mazeGenerator, Player player, Random random) : base(mazeGenerator, player, random) 
        {
            enemySymbol = 'Z';
        }

        #region Методы 

        /// <summary>
        /// Проверяем если позиция зомби равна позиции игрока
        /// то тогда мы атакуем
        /// </summary>
        public override void CanAttack()
        {
            if (currentPosition[0] == player.currentPosition[0] &&
                   currentPosition[1] == player.currentPosition[1])
                Attack();
        }
        #endregion
    }
}
