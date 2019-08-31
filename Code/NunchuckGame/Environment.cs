
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
        Rectangle srcRec;
        int nooFRec;
        bool flip = false;
        Rectangle[] desRec;
        int w = 0, h = 0;
        Viewport screenSize;


        public void Intiliaze(Texture2D background, Viewport screenSize)
        {
            Background = background;
            this.screenSize = screenSize;
            

        }
        public void  Blocks()
        {
            nooFRec = ((int)(screenSize.Width / (int)(2.75f* Background.Width)+1) * ((int)screenSize.Height / (int)(2.75f * Background.Height)+1));
            srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
            desRec = new Rectangle[nooFRec];
            int i = 0;
            while (h <= screenSize.Height)
            {

                while (w <= screenSize.Width)
                {
                    desRec[i] = new Rectangle(w, h, (int)(Background.Width * 2.75f), (int)(Background.Height * 2.75f));
                    i++;
                    w += (int)(Background.Width * 2.75f);
                }
                w = 0;
                h +=(int)( Background.Height * 2.75f);
            }
        }
        public void fences(int x, int y)
        {
            nooFRec = (int)(screenSize.Width / ( Background.Width) + 1);
            srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
            desRec = new Rectangle[nooFRec];
            h = y;
            w = x;
            int i = 0;
            while (w < screenSize.Width)
            {
                desRec[i] = new Rectangle(w, h, Background.Width*2, Background.Height*2);
                i++;
                w += Background.Width*2;
            }
        }
        public void walls( int x, int y)
        {
            nooFRec = (int)(screenSize.Width / (2 * Background.Width) + 1);
            srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
            desRec = new Rectangle[nooFRec];
            h = y;
            w = x;
            int i = 0;
                while (w < screenSize.Width)
                {
                    desRec[i] = new Rectangle(w, h, Background.Width*2 , Background.Height*2);
                    i++;
                    w += Background.Width * 2;
                }
               
            
        }
        public void draw(SpriteBatch spriteBatch)
        {
            SpriteEffects test;
            if (flip)
            {
               test = SpriteEffects.FlipVertically;
            }
            else
            {
                test = SpriteEffects.None;
            }
          
            for(int i=0; i< nooFRec; i++)
            {
                spriteBatch.Draw(Background, desRec[i], srcRec,Color.White, 0f, new Vector2(0, 0), test, 0f);
                
            }
            


        }

    }
}
