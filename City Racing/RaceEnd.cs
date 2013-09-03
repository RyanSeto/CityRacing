using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace City_Racing
{
    class RaceEnd
    {
        public Vector2 Position;
        public Vector3 ThreeDPosition;
        public Rectangle MapLoc;
        int screenHeight;
        int x;
        int y;

        public RaceEnd(Vector2 pos, int theScreenHeight)
        {
            Position = pos;
            x = (int)pos.X;
            y = (int)pos.Y;
            screenHeight = theScreenHeight;
            ThreeDPosition = new Vector3(pos.X * 1.25f, 0, pos.Y * 1.25f);

            double xPos, zPos;
            xPos = ((pos.X / (99.75f)) * 150);// -0.25f;
            zPos = (pos.Y / (99)) * 150;
            MapLoc = new Rectangle((int)xPos - 2, (screenHeight - (((int)zPos) - 2)) - 4, 4, 4);
        }

        public Rectangle GetMapLoc()
        {
            return MapLoc;
        }

        public Vector3 GetPosition()
        {
            return ThreeDPosition;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }
   //     public SetS
    }
}
