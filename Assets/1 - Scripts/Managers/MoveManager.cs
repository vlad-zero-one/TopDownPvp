using Game.UI.Abstract;
using Game.Views;
using System;
using UnityEngine;

namespace Game.Managers
{
    /// <summary>
    /// Manager where moving must be implemented
    /// </summary>
    public class MoveManager: IDisposable
    {
        private readonly IMoveController moveController;
        private readonly PlayerView playerView;

        public MoveManager(IMoveController moveController, PlayerView playerView)
        {
            this.moveController = moveController;
            this.playerView = playerView;

            moveController.Init();

            moveController.MoveDirective += MovePlayer;
            moveController.StopDirective += StopPlayer;
        }

        public void Dispose()
        {
            moveController.MoveDirective -= MovePlayer;
            moveController.StopDirective -= StopPlayer;
        }

        private void MovePlayer(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                playerView.StartMove(direction);
            }
        }

        private void StopPlayer()
        {
            playerView.StopMove();
        }
    }
}