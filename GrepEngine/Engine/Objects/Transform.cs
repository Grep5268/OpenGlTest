using OpenTK;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GrepEngine.Engine.Objects
{
    public abstract class Transform
    {
        public List<Vector3> Verticies { get; set; } = new List<Vector3>();
        public int TextureId { get; set; }

        public abstract void Render();
    }
}
