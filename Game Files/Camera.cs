using Microsoft.Xna.Framework;

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
            x_offset = (int)(-EntityManager.player.PosX + (0.5*graphics.PreferredBackBufferWidth/WalkAround.scaling_factor));
            y_offset = (int)(-EntityManager.player.PosY + (0.5*graphics.PreferredBackBufferHeight/WalkAround.scaling_factor));
        }
    }
}
