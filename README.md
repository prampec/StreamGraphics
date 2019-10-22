# StreamGraphics

StreamGraphics is a cross platform C# DotNet web application framework for visualizing basic graphics in a web browser.

The idea is that
- all codes written in C#, 
- graphics commands are pushed to the web interface,
- visualization is done in the web interface.

![Screenshot](https://sharedinventions.com/wp-content/uploads/2019/10/Screen-Shot-2019-10-17-at-21.26.40.png)

The visualization is done with PixiJS.

Code is written for .Net Core v2.2

# Getting started

You can start the code "dotnet run".

When started up, the program will execute "DemoWorker" code as default. You can change which worker to run in Program.cs using the StreamGraphics.registerWorker() method.

Please try SimpleWorker and StepWorker for more demonstrations.

# Graphics workers

As mentioned before, you can register workers in Program.cs using the StreamGraphics.registerWorker() method.
A Graphics Worker is a set of code responsible of generating what to be visualize. So this is the part where one
shoud put user code. In a graphics worker you can use StreamGraphics static methods to draw on the canvas, like
drawLine.

If you implement InteractiveWorker, than your code will receive some GUI events, like keyboard presses and mouse click. Note however, that these events lacks from delays, so it is not ment for real-time interactions.

(Theoretically it is possible to register multiply workers on a signle application, but currently there is no good use-case for this feature.)

# TODO

- Be more responsive.
- Start worker threads per HTTP session.
- Provide more graphics primitives.
