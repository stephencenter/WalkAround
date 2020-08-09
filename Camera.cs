using Microsoft.Xna.Framework;
using System;

namespace WalkAround
{
    public static class Camera
    {
        // How far horizontally (x) or vertically (y) to draw all the screen objects away
        // from their normal position
        public static int x_offset = 0;
        public static int y_offset = 0;

        // The deadzone is a box in the center of the screen. If the player is inside this
        // box then the camera will not update. This prevents the camera from getting
        // jittery when quickly alternating left/right or up/down.
        private const int deadzone_size = 15;

        // This is the player's position inside the deadzone
        public static int deadzone_x = 0;
        public static int deadzone_y = 0;

        // The current camera mode, determines camera behavior
        public static CameraState camera_state = CameraState.follow_player;

        public enum CameraState
        {
            follow_player,
            free_roam,
            fixed_position
        }

        // Update the camera offsets so the camera is centered on the player
        public static void UpdateCamera(GameObject camera_target, GraphicsDeviceManager graphics)
        {
            // Clamp the player's current position inside the deadzone
            deadzone_x = Math.Min(deadzone_size, Math.Max(-deadzone_size, deadzone_x));
            deadzone_y = Math.Min(deadzone_size, Math.Max(-deadzone_size, deadzone_y));

            // Only update the x_offset if the player isn't in the deadzone
            if (!(-deadzone_size < deadzone_x && deadzone_x < deadzone_size))
            {
                x_offset = (int)
                    (-camera_target.PosX +
                    (0.5*graphics.PreferredBackBufferWidth) +
                    deadzone_x);
            }

            // Only update the y_offset if the player isn't in the deadzone
            if (!(-deadzone_size < deadzone_y && deadzone_y < deadzone_size))
            {
                y_offset = (int)
                    (-camera_target.PosY +
                    (0.5*graphics.PreferredBackBufferHeight) +
                    deadzone_y);
            }
        }
    }
}
