# StreamGraphics

StreamGraphics is a cross platform C# DotNet web application framework for visualizing basic graphics in a web browser.

The idea is that
- all codes writen in C#, 
- graphics commands are pushed to the web interface,
- visualization is done in the web interface.

![Screenshot](https://sharedinventions.com/wp-content/uploads/2019/10/Screen-Shot-2019-10-17-at-21.26.40.png)

The visualization is done with PixiJS.

Code is written for .Net Core v2.2

# Getting started

You can start the code "dotnet run".

The program will run the DemoWorker codes.

Workers are registered to the application in Program.cs using the
StreamGraphics.registerWorker() method.

# TODO

- Be more responsive
- Start worker threads per HTTP session.
