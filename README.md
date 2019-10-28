# StreamGraphics

StreamGraphics is a cross platform C# DotNet web application framework for visualizing basic graphics in a web browser.

The idea is that
- all codes written in C#, 
- graphics commands are pushed to the web interface,
- visualization is done in the web interface,
- graphics about to be displayed is buffered,
- displaying graphics is done with strict timing, thus animations can be drawn.

![Screenshot](https://sharedinventions.com/wp-content/uploads/2019/10/Screen-Shot-2019-10-17-at-21.26.40.png)

The visualization is done with PixiJS.

Code is written for .Net Core v2.2

# Getting started

You can start the code "dotnet run".

When started up, the program will execute "DemoWorker" code as default. You can change which worker to run in `Program.cs` using the `StreamGraphics.registerWorker()` method.

Please try SimpleWorker and `StepWorker` for more demonstrations.

# Graphics workers

As mentioned before, you can register workers in `Program.cs` using the `StreamGraphics.registerWorker()` method.
A Graphics Worker is a set of code responsible of generating what to be visualize. So this is the part where one
shoud put user code. In a graphics worker you can use StreamGraphics static methods to draw on the canvas, like
`drawLine`.

IMPORTANT: StreamGraphics is aimed to be an animated presenter. Each small change is done with one single graphics command. Commands are batched to form a page-change in one step. So each batch of changes you make must be terminated with the `StreamGraphics.step()` command.

If you implement `InteractiveWorker`, than your code will receive some GUI events, like keyboard presses and mouse click. Note however, that these events lacks from delays, so it is not ment for real-time interactions.

(Theoretically it is possible to register multiply workers on a signle application, but currently there is no good use-case for this feature.)

# TODO

- Be more responsive.
- Start worker threads per HTTP session.
- Provide more graphics primitives.
