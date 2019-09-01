
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
        public Texture2D Background;
        Rectangle srcRec;
        int nooFRec;
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
        public void fences(int x, int y, float scale, bool isWidth, int Limit)
        {
            if(isWidth)
            {
                nooFRec = (int)(screenSize.Width / (int)(Background.Width * scale) + 1);
                srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
                desRec = new Rectangle[nooFRec];
                h = y;
                w = x;
                int i = 0;
                while (w < Limit)
                {
                    desRec[i] = new Rectangle(w, h, (int)(Background.Width * scale), (int)(Background.Height * scale));
                    i++;
                    w += (int)(Background.Width * scale);
                }
            }
            else
            {
                nooFRec = (int)(screenSize.Height / (int)(Background.Height * scale) + 1);
                srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
                desRec = new Rectangle[nooFRec];
                h = y;
                w = x;
                int i = 0;
                while (h < Limit)
                {
                    desRec[i] = new Rectangle(w, h, (int)(Background.Width * scale), (int)(Background.Height * scale));
                    i++;
                    h += (int)(Background.Height * scale);
                }
            }
          
        }
        public void walls( int x, int y ,float scale, bool isWidth, int Limit)
        {
           
            h = y;
            w = x;
            if (isWidth)
            {
                
                nooFRec = (int)(screenSize.Width / (scale * Background.Width) + 1);
                srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
                desRec = new Rectangle[nooFRec];

                int i = 0;
                while (w < Limit)
                {
                    desRec[i] = new Rectangle(w, h, (int)(Background.Width * scale), (int)(Background.Height * scale));
                    i++;
                    w += (int)(Background.Width * scale);
                }
            }
            else
            {
                nooFRec = (int)(screenSize.Height / (scale * Background.Height) + 1);
                srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
                desRec = new Rectangle[nooFRec];

                int i = 0;
                while (h < Limit)
                {
                    desRec[i] = new Rectangle(w, h, (int)(Background.Width * scale), (int)(Background.Height * scale));
                    i++;
                    h+= (int)(Background.Height * scale);
                }
            }
           
               
            
        }
        public void draw(SpriteBatch spriteBatch, float scale, int x, int y, float depth=0f)
        {
            w = x;
            h = y;
            srcRec = new Rectangle(0, 0, Background.Width, Background.Height);
            Rectangle desRec = new Rectangle(w,h,(int)(Background.Width * scale),(int)(Background.Height * scale));
            spriteBatch.Draw(Background, desRec, srcRec,Color.White,0f,Vector2.Zero,SpriteEffects.None,depth);
        }
        public void draw(SpriteBatch spriteBatch, float depth = 0f, string flip ="none")
        {
            SpriteEffects test;
            if (flip.Equals("hori"))
            {
               test = SpriteEffects.FlipHorizontally;
            }
            else if(flip.Equals("ver"))
            {
                test = SpriteEffects.FlipVertically;
            }
            else
            {
                test = SpriteEffects.None;
            }
          
            for(int i=0; i< nooFRec; i++)
            {
                spriteBatch.Draw(Background, desRec[i], srcRec,Color.White, 0f, new Vector2(0, 0), test, depth);
                
            }
            


        }

    }
}
