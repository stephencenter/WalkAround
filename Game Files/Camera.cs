using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkAround
{
    public static class Camera
    {
        public static int x_offset = 0;
        public static int y_offset = 0;
        public static CameraState camera_state = CameraState.follow_player;

        public enum CameraState
        {
            follow_player,
            free_roam
        }

        public static void UpdateCamera(GraphicsDeviceManager graphics)
        {
            x_offset = -EntityManager.player.PosX + graphics.PreferredBackBufferWidth/2;
            y_offset = -EntityManager.player.PosY + graphics.PreferredBackBufferHeight/2;
        }

    }
}
