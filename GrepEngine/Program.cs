using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using GrepEngine.Game;

namespace GrepEngine
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var window = new GameWindow(512, 512);
            var game = new GameMain(window);
            
        }
    }
}
