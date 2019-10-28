/*
  SimpleWorker.cs -- StreamGraphics is a cross platform C# DotNet
  solution for visualizing basic graphics in a web browser.
    https://github.com/prampec/StreamGraphics
 
  Copyright (C) 2019 Balazs Kelemen <prampec@gmail.com>
 
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Threading;

namespace StreamGraphics
{
    public class SimpleWorker : GraphicsWorker
    {
        Random rnd = new Random();

        public void run()
        {
            randomLines();
        }

        public void randomLines()
        {
            for (int i = 0; i<5000; i++)
            {
                StreamGraphics.drawLine(
                    rnd.Next(800),
                    rnd.Next(600),
                    rnd.Next(800),
                    rnd.Next(600),
                    new Color(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                StreamGraphics.step();
            }
        }

        public void redNoise()
        {
            StreamGraphics.setStepDelayMs(50);
            StreamGraphics.setBufferSize(60*StreamGraphics.width/20*StreamGraphics.height/20);
            while (true)
            {
                StreamGraphics.clear();
                for (int x = 0; x<StreamGraphics.width; x+=20)
                {
                    for (int y = 0; y<StreamGraphics.height; y+=20)
                    {
                        StreamGraphics.drawRect(
                            x, y, 20, 20,
                            new Color(rnd.Next(265), 0, 0));
                    }
                }
                StreamGraphics.step();
            }
        }
    }
}