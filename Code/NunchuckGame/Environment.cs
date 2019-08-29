
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
            nooFRec = (int)(screenSize.Width / (2 * Background.Width) * screenSize.Height / (2 * Background.Height));
            nooFRec += 200;
            srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
            desRec = new Rectangle[nooFRec];
            int i = 0;
            while (h < screenSize.Height)
            {

                while (w < screenSize.Width)
                {
                    desRec[i] = new Rectangle(w, h, Background.Width * 2, Background.Height * 2);
                    i++;
                    w += Background.Width * 2;
                }
                w = 0;
                h += Background.Height * 2;
            }
        }
        public void walls( int start )
        {
            nooFRec = (int)(screenSize.Width / Background.Width);
            srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
            desRec = new Rectangle[nooFRec];
            int i = 0;
            if(start==0)
            {
                flip = true;
            }
            h = start;
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
