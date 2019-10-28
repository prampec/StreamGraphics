/*
  stream-graphics.js -- StreamGraphics is a cross platform C# DotNet
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

var StreamGraphics = {
    app: new PIXI.Application({ antialias: true }),
    // graphics: new PIXI.Graphics(),
    commandQueue: [],
    running: false,
    loadTimer: 0,
    queueTimer: 0,
    bufferSize: 2000,
    stepDelayMs: 10,
    objects: {},
    textStyle: new PIXI.TextStyle({
        fontFamily: 'Lucida Console',
        // fontSize: 36,
        // fontStyle: 'italic',
        // fontWeight: 'bold',
        // fill: ['#ffffff', '#00ff99'], // gradient
        // stroke: '#4a1850',
        // strokeThickness: 5,
        // dropShadow: true,
        // dropShadowColor: '#000000',
        // dropShadowBlur: 4,
        // dropShadowAngle: Math.PI / 6,
        // dropShadowDistance: 6,
        // wordWrap: true,
        // wordWrapWidth: 440,
    }),
    init: function() {
        document.body.appendChild(this.app.view);

        // this.graphics.lineStyle(2, 0xFFFFFF, 1);
        // this.graphics.position.x = 0;
        // this.graphics.position.y = 0;
        // this.app.stage.addChild(this.graphics);

        // const mouseposition = app.renderer.plugins.interaction.mouse.global;
        this.app.renderer.plugins.interaction.on('pointerdown', (evetData) => {
            var e = evetData.data.global;
            this.sendAction("pointerdown&x=" + e.x + "&y=" + e.y);
        });

        this.initKeyboardEvents();

        return this;
    },
    load: function() {
        var sg = StreamGraphics;
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
                var commands = JSON.parse(this.responseText);
                commands.forEach(command => {
                    sg.commandQueue.push(command);
                });
            }
        };
        var queueSpace = sg.bufferSize - sg.commandQueue.length;
        xmlhttp.open("GET", "api/commands?count=" + queueSpace, true);
        xmlhttp.send();
    },
    processQueue: function() {
        var sg = StreamGraphics;
        while (sg.commandQueue.length > 0)
        {
            var command = sg.commandQueue.shift();
            if (command.type == "clear")
            {
                sg.app.stage.removeChildren();
                sg.objects = {};
            }
            else if (command.type == "delete")
            {
                var g = sg.objects[command.id];
                sg.app.stage.removeChild(g);
            }
            else if (command.type == "moveTo")
            {
                var g = sg.objects[command.id];
                g.x = command.x;
                g.y = command.y;
            }
            else if (command.type == "rotateTo")
            {
                var g = sg.objects[command.id];
                g.angle = command.angle;
            }
            else if (command.type == "line")
            {
                var g = new PIXI.Graphics();
                g.lineStyle(2, command.color, 1);
                g.moveTo(command.fromX, command.fromY);
                g.lineTo(command.toX, command.toY);
                sg.app.stage.addChild(g);
                sg.objects[command.id] = g;
            }
            else if (command.type == "circleCenter")
            {
                var g = new PIXI.Graphics();
                g.lineStyle(0);
                g.beginFill(command.color, 1);
                g.drawCircle(
                    0,
                    0,
                    command.radius);
                g.endFill();
                g.x = command.centerX;
                g.y = command.centerY;
                sg.app.stage.addChild(g);
                sg.objects[command.id] = g;
            }
            else if (command.type == "rect")
            {
                var g = new PIXI.Graphics();
                g.lineStyle(0);
                g.beginFill(command.color, 1);
                g.drawRect(
                    0, 0,
                    command.width, command.height);
                g.endFill();
                g.x = command.x;
                g.y = command.y;
                sg.app.stage.addChild(g);
                sg.objects[command.id] = g;
            }
            else if (command.type == "sprite")
            {
                var g = PIXI.Sprite.from(command.image);
                g.x = command.x;
                g.y = command.y;
                sg.app.stage.addChild(g);
                sg.objects[command.id] = g;
            }
            else if (command.type == "text")
            {
                var style = Object.assign({}, sg.textStyle);
                style.fill = command.color;
                style.fontFamily = 'Monaco';
                style.fontSize = command.size;
                var g = new PIXI.Text(command.text, style);
                g.x = command.x;
                g.y = command.y;
                sg.app.stage.addChild(g);
                sg.objects[command.id] = g;
            }
            else if (command.type == "stepDelayMs")
            {
                sg.stepDelayMs = command.value;
                sg.pause();
                sg.run();
            }
            else if (command.type == "bufferSize")
            {
                sg.bufferSize = command.value;
            }
            if (command.type == "step")
            {
                break;
            }
        }
    },
    pause: function() {
        if (this.running) {
            clearTimeout(this.loadTimer);
            clearTimeout(this.queueTimer);
            // this.app.ticker.add(this.processQueue);
            this.running = false;
        }
    },
    run: function()
    {
        if (!this.running)
        {
            this.loadTimer = setInterval(this.load, 500);
            this.queueTimer = setInterval(this.processQueue, this.stepDelayMs);
            // this.app.ticker.add(this.processQueue);
            this.running = true;
        }
    },
    reset: function()
    {
        this.sendAction("reset");
    },
    sendAction: function(action)
    {
        var sg = StreamGraphics;
        var xmlhttp = new XMLHttpRequest();
        xmlhttp.onreadystatechange = function() {
            if (this.readyState == 4 && this.status == 200) {
                if (action == "reset")
                {
                    sg.commandQueue = [];
                }
            }
        };
        xmlhttp.open("POST", "api/commands?action=" + action, true);
        xmlhttp.send();
    },
    initKeyboardEvents: function () {
        var keysDown = new Set();

        //The `downHandler`
        var downHandler = event => {
            if (!keysDown.has(event.key))
            {
                keysDown.add(event.key);
                this.keyPress(event.key);
            }
            event.preventDefault();
        };
      
        //The `upHandler`
        var upHandler = event => {
            if (keysDown.has(event.key))
            {
                keysDown.delete(event.key);
                this.keyRelease(event.key);
            }
            event.preventDefault();
        };
      
        //Attach event listeners
        const downListener = downHandler.bind();
        const upListener = upHandler.bind();
        
        window.addEventListener(
          "keydown", downListener, false
        );
        window.addEventListener(
          "keyup", upListener, false
        );
    },
    keyPress: function(key)
    {
        this.sendAction("keydown&key=" + encodeURIComponent(key));
    },
    keyRelease: function(key)
    {
        this.sendAction("keyup&key=" + encodeURIComponent(key));
    },
}