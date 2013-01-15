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
    class Building
    {
        public int height;
        public Texture2D texture;
        public Texture2D roof;
        public Boolean exist = false;

        public Building(int height, Texture2D wallTexture, Texture2D roofTexture, Boolean doesExist)
        {
            this.height = height;
            texture = wallTexture;
            roof = roofTexture;
            exist = doesExist;
        }

        public Building(int height, Boolean doesExist)
        {
            this.height = height;
            exist = doesExist;
        }

        public Building(Boolean doesExist)
        {
            exist = doesExist;
        }

        public int getHeight()
        {
            return height;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public Texture2D getRoof()
        {
            return roof;
        }

        public Boolean exists()
        {
            return exist;
        }
    }
}
