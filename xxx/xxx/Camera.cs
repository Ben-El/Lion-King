using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace xxx
{
    class Camera
    {
        #region Data

        public Matrix Mat { get; private set; }
        public Ifocus Focus { get; private set; }
        public Vector2 Zoom { get; private set; }
        public Viewport View { get; private set; }
        public Vector2 Pos { get; private set; }

        #endregion

        /// <summary>
        /// Initializing the parameters
        /// </summary>
        /// <param name="focus"></param>
        /// <param name="zoom"></param>
        /// <param name="view"></param>
        public Camera(Ifocus focus, Vector2 zoom, Viewport view)
        {
            Game1.UPDATE_EVENT += this.UpdateMat;

            Focus = focus;
            Zoom = zoom;
            View = view;
            Pos = Focus.Pos;
        }

        /// <summary>
        /// Updating the cam's data
        /// </summary>
        public void UpdateMat()
        {
            Mat = Matrix.CreateTranslation(-Pos.X, -Pos.Y, 0) *
                  Matrix.CreateScale(Zoom.X, Zoom.Y, 1) *
                  Matrix.CreateTranslation(View.Width / 2, View.Height / 2, 0);

            // -------------------------------------------------------------------------------------------------

            // :אם המשחק רץ במחשב הנייח - אז נוציא מהערה את קטע הקוד הזה

            if (Level.LevelNumber == 1)
            {
                Pos = Vector2.Lerp(new Vector2(
                    MathHelper.Clamp(Focus.Pos.X, View.Width / 2, 3350),
                    MathHelper.Clamp(Focus.Pos.Y, -510 + View.Height, 3200 - View.Height / 2)), Pos, 0.85f);
            }

            if (Level.LevelNumber == 2)
            {
                Pos = Vector2.Lerp(new Vector2(
                    MathHelper.Clamp(Focus.Pos.X, (View.Width / 2), 3360),
                    MathHelper.Clamp(Focus.Pos.Y, -1000 + View.Height, 4000 - View.Height / 2)), Pos, 0.85f);
            }

            // -------------------------------------------------------------------------------------------------

            // :אם המשחק רץ בלפטופ - אז נוציא מהערה את קטע הקוד הזה

            //if (Level.LevelNumber == 1)
            //{
            //    Pos = Vector2.Lerp(new Vector2(
            //        MathHelper.Clamp(Focus.Pos.X, View.Width / 2, 3300),
            //        MathHelper.Clamp(Focus.Pos.Y, -385 + View.Height, 3200 - View.Height / 2)), Pos, 0.85f);
            //}

            //if (Level.LevelNumber == 2)
            //{
            //    Pos = Vector2.Lerp(new Vector2(
            //        MathHelper.Clamp(Focus.Pos.X, View.Width / 2, 3320),
            //        MathHelper.Clamp(Focus.Pos.Y, -450 + View.Height, 4000 - View.Height / 2)), Pos, 0.85f);
            //}
        }
    }
}