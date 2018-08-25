using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GrepEngine.Engine.Objects
{
    public class Quad : Transform
    {
        const string FIRST_VERTEX_ID = "v1";
        const string SECOND_VERTEX_ID = "v2";
        const string THIRD_VERTEX_ID = "v3";
        const string FOURTH_VERTEX_ID = "v4";

        public override void Render()
        {
            GL.Begin(PrimitiveType.Quads);

            var normal = Vector3.Cross(Vector3.Subtract(this.Verticies.ElementAt(0), this.Verticies.ElementAt(1)), Vector3.Subtract(this.Verticies.ElementAt(0),this.Verticies.ElementAt(2)));
            GL.Normal3(Vector3.Normalize(normal));

            for (int i = 0; i < 4; i++)
            {
                var vert = this.Verticies.ElementAt(i);
                var textCoord1 = i == 0 || i == 3 ? 1 : 0;
                var textCoord2 = i == 0 || i == 1 ? 1 : 0;

                GL.TexCoord2(textCoord1, textCoord2);
                GL.Vertex3(vert.X, vert.Y, vert.Z);
            }
        }

        public Quad()
        {

        }

        public Quad(XElement element)
        {
            Verticies.Add(GetVertFromXmlAttribute(element, FIRST_VERTEX_ID));
            Verticies.Add(GetVertFromXmlAttribute(element, SECOND_VERTEX_ID));
            Verticies.Add(GetVertFromXmlAttribute(element, THIRD_VERTEX_ID));
            Verticies.Add(GetVertFromXmlAttribute(element, FOURTH_VERTEX_ID));
        }
    }
}
