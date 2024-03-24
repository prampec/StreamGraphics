/*
  Program.cs -- StreamGraphics is a cross platform C# DotNet
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

namespace StreamGraphics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StreamGraphics.registerWorker(new DemoWorker());

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // TODO: let dynamic configuration take effect without this dummy route.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            StreamGraphics.shutdown();
        }
    }
}