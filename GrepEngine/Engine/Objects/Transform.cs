using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GrepEngine.Engine.Objects
{
    public abstract class Transform
    {
        public List<Vector3> Verticies { get; set; } = new List<Vector3>();
        public int TextureId { get; set; }

        public abstract void Render();

        protected Vector3 GetVertFromXmlAttribute(XElement element, string attr)
        {
            var v = element.Attribute(attr).Value;
            var vert = v.Split(',').Select(float.Parse).ToArray();
            return new Vector3(vert[0], vert[1], vert[2]);
        }
    }
}
