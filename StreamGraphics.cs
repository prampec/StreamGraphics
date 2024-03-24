/*
  StreamGraphics.cs -- StreamGraphics is a cross platform C# DotNet
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
    public sealed class StreamGraphics
    {
        private int bufferSize = 200;

        private static StreamGraphics? instance = null;
        private static object instLock = new object();

        Queue<object> commandQueue = new Queue<object>();

        List<GraphicsWorker> workers = new List<GraphicsWorker>();
        List<Thread> workerThreads = new List<Thread>();

        StreamGraphics()
        {

        }

        public static StreamGraphics Instance
        {
            get
            {
                lock (instLock)
                {
                    if (instance == null)
                    {
                        instance = new StreamGraphics();
                    }
                    return instance;
                }
            }
        }

        public static int width { get { return 800; } } // TODO: get this dynamically.
        public static int height { get { return 600; } } // TODO: get this dynamically.

        internal void reset()
        {
            lock (commandQueue)
            {
                commandQueue.Clear();
                // foreach (GraphicsWorker worker in workers)
                // {
                //     worker.reset();
                // }
                foreach (Thread workerThread in workerThreads)
                {
                    workerThread.Interrupt();
                }
                foreach (Thread workerThread in workerThreads)
                {
                    workerThread.Join();
                }
                workerThreads.Clear();
                clear();
                foreach (GraphicsWorker worker in workers)
                {
                    StreamGraphics.startWorker(worker);
                }
            }
        }

        internal void onPointerDown(int x, int y)
        {
            foreach (GraphicsWorker worker in workers)
            {
                InteractiveWorker? iw = worker as InteractiveWorker;
                if (iw != null)
                {
                    iw.onPointerDown(x, y);
                }
            }
        }

        internal void onKeyDown(string key)
        {
            foreach (GraphicsWorker worker in workers)
            {
                InteractiveWorker? iw = worker as InteractiveWorker;
                if (iw != null)
                {
                    iw.onKeyDown(key);
                }
            }
        }
        internal void onKeyUp(string key)
        {
            foreach (GraphicsWorker worker in workers)
            {
                InteractiveWorker? iw = worker as InteractiveWorker;
                if (iw != null)
                {
                    iw.onKeyUp(key);
                }
            }
        }

        private void addCommand(object command)
        {
            while (commandQueue.Count >= bufferSize)
            {
                Thread.Sleep(1);
            }
            lock (commandQueue)
            {
                commandQueue.Enqueue(command);
            }
        }

        public List<object> pullCommands(int count)
        {
            lock (commandQueue)
            {
                List<object> result = new List<object>();
                for (int i = 0; i < count; i++)
                {
                    if (commandQueue.Count <= 0)
                    {
                        break;
                    }
                    result.Add(commandQueue.Dequeue());
                }
                return result;
            }
        }

        public static void registerWorker(GraphicsWorker worker)
        {
            startWorker(worker);
            StreamGraphics.Instance.workers.Add(worker);
        }
        public static void shutdown()
        {
            foreach (Thread workerThread in StreamGraphics.Instance.workerThreads)
            {
                workerThread.Join(100);
            }
        }

        internal static void clear()
        {
            StreamGraphics.Instance.addCommand(new { type = "clear" });
        }

        public static string drawLine(int fromX, int fromY, int toX, int toY, Color color)
        {
            string id = generateId();
            StreamGraphics.Instance.addCommand(new
            {
                id = id,
                type = "line",
                fromX = fromX,
                fromY = fromY,
                toX = toX,
                toY = toY,
                color = color.toInt()
            });
            return id;
        }

        public static string drawCircle(int centerX, int centerY, int radius, Color color)
        {
            string id = generateId();
            StreamGraphics.Instance.addCommand(new
            {
                id = id,
                type = "circleCenter",
                centerX = centerX,
                centerY = centerY,
                radius = radius,
                color = color.toInt()
            });
            return id;
        }

        public static string drawRect(int x, int y, int width, int height, Color color)
        {
            string id = generateId();
            StreamGraphics.Instance.addCommand(new
            {
                id = id,
                type = "rect",
                x = x,
                y = y,
                width = width,
                height = height,
                color = color.toInt()
            });
            return id;
        }

        public static string drawSprite(
            string image, int x, int y)
        {
            string id = generateId();
            StreamGraphics.Instance.addCommand(new
            {
                id = id,
                type = "sprite",
                image = image,
                x = x,
                y = y
            });
            return id;
        }
        public static string drawText(
            string text, int x, int y, int size, Color color)
        {
            string id = generateId();
            StreamGraphics.Instance.addCommand(new
            {
                id = id,
                type = "text",
                text = text,
                x = x,
                y = y,
                size = size,
                color = color.toInt()
            });
            return id;
        }
        public static void delete(string id)
        {
            StreamGraphics.Instance.addCommand(new
            {
                type = "delete",
                id = id
            });
        }
        public static void moveTo(string id, int x, int y)
        {
            StreamGraphics.Instance.addCommand(new
            {
                type = "moveTo",
                x = x,
                y = y,
                id = id
            });
        }
        public static void rotateTo(string id, float angle)
        {
            StreamGraphics.Instance.addCommand(new
            {
                type = "rotateTo",
                angle = angle,
                id = id
            });
        }

        public static void setStepDelayMs(int delayMs)
        {
            StreamGraphics.Instance.addCommand(new
            {
                type = "stepDelayMs",
                value = delayMs
            });
        }

        public static void setBufferSize(int bufferSize)
        {
            StreamGraphics.Instance.bufferSize = bufferSize;
            StreamGraphics.Instance.addCommand(new
            {
                type = "bufferSize",
                value = bufferSize
            });
        }

        private static string generateId()
        {
            string guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return guid.Substring(0, 22);
        }

        private static void startWorker(GraphicsWorker worker)
        {
            Thread workerThread = new Thread((ThreadStart)delegate { runWorker(worker); });
            StreamGraphics.Instance.workerThreads.Add(workerThread);
            workerThread.Start();
        }

        private static void runWorker(GraphicsWorker worker)
        {
            try
            {
                worker.run();
            }
            catch (ThreadInterruptedException)
            {
                return;
            }
        }

        internal static void step()
        {
            StreamGraphics.Instance.addCommand(new
            {
                type = "step"
            });
        }
    }
}