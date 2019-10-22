/*
  Color.cs -- StreamGraphics is a cross platform C# DotNet
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

namespace StreamGraphics
{
    public class Color
    {
        private int red;
        private int green;
        private int blue;

        public Color(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public int Red
        {
            get { return red; }
            set { this.red = value; }
        }

        public int Green
        {
            get { return green; }
            set { this.green = value; }
        }

        public int Blue
        {
            get { return blue; }
            set { this.blue = value; }
        }

        public int toInt()
        {
            return red * 256 * 256 + green * 256 + blue;
        }

        public static Color randomLight(Random rnd)
        {
            while(true)
            {
                int r = rnd.Next(255);
                int g = rnd.Next(255);
                int b = rnd.Next(255);

                if ((r + b + g) > 150)
                {
                    return new Color(r, g, b);
                }
            }
        }

        public static readonly Color WHITE = new Color(255, 255, 255);
        public static readonly Color BLACK = new Color(0, 0, 0);
        public static readonly Color RED = new Color(255, 0, 0);
        public static readonly Color GREEN = new Color(0, 255, 0);
        public static readonly Color BLUE = new Color(0, 0, 255);
        public static readonly Color YELLOW = new Color(255, 255, 0);
        public static readonly Color MAGENTA = new Color(255, 0, 255);
        public static readonly Color CYAN = new Color(0, 255, 255);
        public static readonly Color GRAY = new Color(128, 128, 128);
        public static readonly Color LIGHT_GRAY = new Color(211, 211, 211);
    }
}