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
    class Page
    {
        public List<Rectangle> rec { get; private set; }
        public List<Vector2> org { get; private set; }
        public List<Vector2> FlippedOrg { get; private set; }
        public Texture2D tex { get; private set; }
        public List<Circle> BigCircles;
        public List<Circle> SmallCircles;
        public List<List<Circle>> AllSmallCircles;
        public bool IsFound;
        public string name;

        /// <summary>
        /// Page's constructor
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="state"></param>
        public Page(Folders folder, States state)
        {
            BigCircles = new List<Circle>();
            AllSmallCircles = new List<List<Circle>>();
            rec = new List<Rectangle>();
            org = new List<Vector2>();
            FlippedOrg = new List<Vector2>();
            name = folder.ToString() + "/" + state.ToString();
            Processing(name);
            makeTrans();
        }

        /// <summary>
        /// Defining the slow of each State's frames
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static int DefineStatesSlows(States state)
        {
            if (state == States.Running)
            {
                return 3;
            }

            if (state == States.Running_Jump)
            {
                return 6;
            }

            if (state == States.Stand)
            {
                return 5;
            }

            if (state == States.Jump)
            {
                return 7;
            }

            if (state == States.Hanging)
            {
                return 4;
            }

            if (state == States.Roar)
            {
                return 12;
            }

            if (state == States.Slash)
            {
                return 3;
            }

            if (state == States.DoubleSlash)
            {
                return 4;
            }

            if (state == States.Climb)
            {
                return 7;
            }

            if (state == States.AfterFalling)
            {
                return 5;
            }

            if (state == States.GettingHurt)
            {
                return 5;
            }

            if (state == States.GettingSlashed)
            {
                return 3;
            }

            if (state == States.CrouchSlash)
            {
                return 3;
            }

            if (state == States.Crouch)
            {
                return 3;
            }

            if (state == States.ThrowingEnemy)
            {
                return 4;
            }

            if (state == States.TossedBySimba)
            {
                return 4;
            }

            if (state == States.Pouncing)
            {
                return 5;
            }

            if (state == States.Rolling)
            {
                return 5;
            }

            if (state == States.Dying)
            {
                return 5;
            }

            if (state == States.Swinging)
            {
                return 3;
            }

            return 0;
        }

        /// <summary>
        /// Processing texture's data
        /// </summary>
        /// <param name="name"></param>
        public void Processing(string name)
        {
            tex = S.cm.Load<Texture2D>(name);
            Color[] col = new Color[tex.Width];
            Color[] check = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(0, new Rectangle(0, tex.Height - 1, tex.Width, 1), col, 0, tex.Width);
            tex.GetData<Color>(check);

            List<int> pnt = new List<int>(); // מערך של כל הנקודות השחורות בסטריפ מסוים

            for (int i = 0; i < col.Length; i++)
            {
                if (col[i] == col[0])
                {
                    pnt.Add(i); // מוסיף את מיקומי הנקודות האלה כדי שאוכל להשתמש בהם בהמשך
                }
            }

            IsFound = false;

            for (int i = 1; i < pnt.Count; i += 2) // oringins - עובר על כל ה
            {
                for (int row = 0; row < tex.Height - 2; row++)
                {
                    if (check[pnt[i] + (row * tex.Width)] == col[0])
                    {
                        IsFound = true;
                        org.Add(new Vector2(pnt[i] - pnt[i - 1], row));
                        FlippedOrg.Add(new Vector2(pnt[i + 1] - pnt[i], row));

                        break;
                    }
                }

                if (!IsFound)
                {
                    org.Add(new Vector2(pnt[i] - pnt[i - 1], tex.Height - 1)); // פה אפשר גם בלי טקס הייט מינוס אחד
                    FlippedOrg.Add(new Vector2(pnt[i + 1] - pnt[i], tex.Height - 1));
                }

                rec.Add(new Rectangle(pnt[i - 1], 0, pnt[i + 1] - pnt[i - 1], tex.Height - 2));
            }


            for (int i = 0; i < rec.Count; i++)
            {
                BigCircles.Add(new Circle(Circle.find_center(rec[i]), rec[i], new Vector2(S.MapsScale, S.MapsScale), true));
                SmallCircles = new List<Circle>();
                SmallCircles.Add(new Circle(Circle.find_center(rec[i]), rec[i], new Vector2(S.MapsScale, S.MapsScale), false));
                SmallCircles.Add(new Circle(Circle.find_reva(rec[i]), rec[i], new Vector2(S.MapsScale, S.MapsScale), false));
                SmallCircles.Add(new Circle(Circle.find_shloshtreva(rec[i]), rec[i], new Vector2(S.MapsScale, S.MapsScale), false));
                AllSmallCircles.Add(SmallCircles);
            }
        }

        /// <summary>
        /// Making the animation strips transparent
        /// </summary>
        public void makeTrans()
        {
            Color[] data = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(data);
            Color trans = data[0];
            Color black = Color.Black;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == trans)
                {
                    data[i] = Color.Transparent;
                }

                // התנאי הזה הוא בשביל שלא יראו את נקודות האוריג'ין השחורות באמצע הגוף של סימבה
                // בזמן טיפוס על פלטפורמה
                if (data[i] == black && i < data.Length - 1)
                {
                    data[i] = data[i - 1];
                }
            }

            tex.SetData<Color>(data);
        }
    }
}