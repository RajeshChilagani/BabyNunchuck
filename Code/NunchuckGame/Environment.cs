
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NunchuckGame
{
    class Environment
    {
        Texture2D Background;
        Vector2 BackgroundWidth= new Vector2(0) ;
        Rectangle srcRec;
        int nooFRec;
        Rectangle[] desRec;
        int w = 0, h = 0;
        Viewport screenSize;


        public void Intiliaze(Texture2D background, Viewport screenSize)
        {
            Background = background;
            this.screenSize = screenSize;
            nooFRec = (int)(screenSize.Width / Background.Width * screenSize.Height / Background.Height);
            nooFRec += 4;
            srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
            desRec = new Rectangle[nooFRec];
            int i = 0;
            while(h<screenSize.Height)
            {
               
                while(w<screenSize.Width)
                {
                    desRec[i] = new Rectangle(w, h, Background.Width, Background.Height);
                    i++;
                    w += Background.Width;
                }
                w = 0;
                h += Background.Height;
            }

        }
        public void draw(SpriteBatch spriteBatch)
        {
                  
            for(int i=0; i< nooFRec; i++)
            {
                spriteBatch.Draw(Background, desRec[i], srcRec, Color.Wheat);
            }
               

        }

    }
}
