using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace GrepEngine.Engine.Objects
{
    public class Box : Transform
    {
        //2 opposite corners of the box
        const string FIRST_CORNER_ID = "v1";
        const string SECOND_CORNER_ID = "v2";

        public override void Render()
        {
            var v1 = Verticies.First();
            var v2 = Verticies.Last();

            new Quad { Verticies = { new Vector3(v1.X, v1.Y, v1.Z), new Vector3(v1.X, v1.Y, v2.Z),  new Vector3(v1.X, v2.Y, v2.Z), new Vector3(v1.X, v2.Y, v1.Z), } }.Render();
            new Quad { Verticies = { new Vector3(v1.X, v1.Y, v1.Z), new Vector3(v1.X, v1.Y, v2.Z),  new Vector3(v2.X, v1.Y, v2.Z), new Vector3(v2.X, v1.Y, v1.Z), } }.Render();
            new Quad { Verticies = { new Vector3(v2.X, v1.Y, v1.Z), new Vector3(v2.X, v1.Y, v2.Z),  new Vector3(v2.X, v2.Y, v2.Z), new Vector3(v2.X, v2.Y, v1.Z), } }.Render();
            new Quad { Verticies = { new Vector3(v1.X, v2.Y, v1.Z), new Vector3(v1.X, v2.Y, v2.Z),  new Vector3(v2.X, v2.Y, v2.Z), new Vector3(v2.X, v2.Y, v1.Z), } }.Render();
            new Quad { Verticies = { new Vector3(v2.X, v1.Y, v1.Z), new Vector3(v1.X, v1.Y, v1.Z),  new Vector3(v1.X, v2.Y, v1.Z), new Vector3(v2.X, v2.Y, v1.Z) } }.Render();
            new Quad { Verticies = { new Vector3(v1.X, v1.Y, v2.Z), new Vector3(v1.X, v2.Y, v2.Z),  new Vector3(v2.X, v2.Y, v2.Z), new Vector3(v2.X, v1.Y, v2.Z), } }.Render();
        }

        public Box()
        {

        }

        public Box(XElement element)
        {
            Verticies.Add(GetVertFromXmlAttribute(element, FIRST_CORNER_ID));
            Verticies.Add(GetVertFromXmlAttribute(element, SECOND_CORNER_ID));
        }

        
    }
}
