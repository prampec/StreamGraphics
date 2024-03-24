/*
  DemoWorker.cs -- StreamGraphics is a cross platform C# DotNet
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
    public class DemoWorker : InteractiveWorker
    {
        int pointerX = 0;
        int pointerY = 0;
        Stack<string> chars = new Stack<string>();

        public int robotX;
        private int robotY;
        private string? robotId;

        public void run()
        {
            StreamGraphics.clear();
            StreamGraphics.setStepDelayMs(10);
            for (int x = 0; x < 600; x += 10)
            {
                StreamGraphics.drawLine(
                    x,
                    0,
                    0,
                    600 - x,
                    Color.RED);
                StreamGraphics.step();
            }
            StreamGraphics.setStepDelayMs(500);
            StreamGraphics.drawText("Hello World!", 180, 300, 30, Color.YELLOW);
            StreamGraphics.step();
            StreamGraphics.drawText("Hello World!", 180, 300 + 40, 50, Color.MAGENTA);
            StreamGraphics.step();
            string bigtextId = StreamGraphics.drawText("Hello World!", 180, 300 + 40 + 50 + 20, 70, Color.CYAN);
            StreamGraphics.rotateTo(bigtextId, -4);
            StreamGraphics.step();
            StreamGraphics.drawText(
                "Try clicking with the mouse; use arrow keys; type something, use BACKSPACE to erase typed text!",
                40, 570, 12, Color.WHITE);

            robotX = 700;
            robotY = 300;
            robotId = StreamGraphics.drawSprite("images/robot.png", robotX, robotY);
            StreamGraphics.step();
            StreamGraphics.setStepDelayMs(10);
        }

        public void onPointerDown(int x, int y)
        {
            StreamGraphics.drawCircle(x, y, 20, Color.BLUE);
            StreamGraphics.drawRect(x, y, 20, 30, Color.GREEN);
            pointerX = x;
            pointerY = y;
            chars.Clear();
        }
        public void onKeyDown(string key)
        {
            if (key == "Backspace")
            {
                if (chars.Count > 0)
                {
                    string id = chars.Pop();
                    StreamGraphics.delete(id);
                    pointerX -= 20;
                }
                return;
            }
            else if (key == "ArrowUp")
            {
                robotY -= 10;
                StreamGraphics.moveTo(robotId!, robotX, robotY);
                return;
            }
            else if (key == "ArrowUp")
            {
                robotY -= 10;
                StreamGraphics.moveTo(robotId!, robotX, robotY);
                return;
            }
            else if (key == "ArrowDown")
            {
                robotY += 10;
                StreamGraphics.moveTo(robotId!, robotX, robotY);
                return;
            }
            else if (key == "ArrowLeft")
            {
                robotX -= 10;
                StreamGraphics.moveTo(robotId!, robotX, robotY);
                return;
            }
            else if (key == "ArrowRight")
            {
                robotX += 5;
                StreamGraphics.moveTo(robotId!, robotX, robotY);
                return;
            }
            if (key.Length > 1)
            {
                return;
            }

            {
                string id = StreamGraphics.drawText(key, pointerX, pointerY, 30, Color.WHITE);
                chars.Push(id);
                pointerX += 20;
            }
        }
        public void onKeyUp(string key)
        {
            // Do nothing
        }
    }
}