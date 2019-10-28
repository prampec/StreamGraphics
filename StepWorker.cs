using System;
using System.Collections.Generic;

namespace StreamGraphics
{
    public class StepWorker : GraphicsWorker
    {
        public static Random rnd = new Random();

        public void run()
        {
            StreamGraphics.setBufferSize(2000);
            StreamGraphics.clear();
            List<Ball> balls = new List<Ball>();
            for (int i=0; i<30; i++)
            {
                Ball ball = createBall();
                balls.Add(ball);
            }

            while(true)
            {
                foreach(Ball ball in balls)
                {
                    ball.step();
                }
                StreamGraphics.step();
            }
        }

        public Ball createBall()
        {
            int x = rnd.Next(StreamGraphics.width);
            int y = rnd.Next(StreamGraphics.height);
            int vx = rnd.Next(10) + 2;
            int vy = rnd.Next(10) + 2;
            string id = StreamGraphics.drawCircle(
                x, y, 20, Color.randomLight(rnd));

            return new Ball(id, x, y, vx, vy);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////
    
    public class Ball
    {
        string id;
        int x;
        int y;
        double vx;
        double vy;

        public Ball(string id, int x, int y, double vx, double vy)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public void step()
        {
            x += (int)vx;
            y += (int)vy;

            if (x > StreamGraphics.width)
            {
                x = StreamGraphics.width - (x - StreamGraphics.width);
                vx = -vx;
                vy += StepWorker.rnd.Next(3) - 1;
            }
            if (x < 0)
            {
                x = -x;
                vx = -vx;
                vy += StepWorker.rnd.Next(3) - 1;
            }
            if (y > StreamGraphics.height)
            {
                y = StreamGraphics.height - (y - StreamGraphics.height);
                vy = -vy;
                vx += StepWorker.rnd.Next(3) - 1;
            }
            if (y < 0)
            {
                y = -y;
                vy = -vy;
                vx += StepWorker.rnd.Next(3) - 1;
            }
            StreamGraphics.moveTo(id, x, y);
        }
    }
}