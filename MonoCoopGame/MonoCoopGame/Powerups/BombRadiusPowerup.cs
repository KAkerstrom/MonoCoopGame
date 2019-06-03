using Microsoft.Xna.Framework;

namespace monoCoopGame.Powerups
{
    class BombRadiusPowerup : Powerup
    {
        public BombRadiusPowerup(Point gridPos) : base(new Sprite("explosion0"), gridPos)
        {
        }

        public override void Activate(GameState gameState, Player player)
        {
            player.BombPower++;
        }
    }
}
