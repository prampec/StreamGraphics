/*
  CommandsController.cs -- StreamGraphics is a cross platform C# DotNet
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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StreamGraphics.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get([FromQuery] int count = 0)
        {
            return Ok(StreamGraphics.Insance.pullCommands(count));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromQuery] string action,
            [FromQuery] int x = 0, [FromQuery] int y = 0,
            [FromQuery] string key = null)
        {
            if (action == "reset")
            {
                StreamGraphics.Insance.reset();
            }
            else if (action == "pointerdown")
            {
                StreamGraphics.Insance.onPointerDown(x, y);
            }
            else if (action == "keydown")
            {
                if (key == null)
                {
                    key = " ";
                }
                StreamGraphics.Insance.onKeyDown(key);
            }
            else if (action == "keyup")
            {
                if (key == null)
                {
                    key = " ";
                }
                StreamGraphics.Insance.onKeyUp(key);
            }
        }
    }
}
