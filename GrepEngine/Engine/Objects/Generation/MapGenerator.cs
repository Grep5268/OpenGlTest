using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GrepEngine.Engine.Objects.Generation
{
    public class MapGenerator
    {
        XElement MapData;
        List<Transform> Transforms = new List<Transform>();

        public MapGenerator(string xmlFileLocation)
        {
            MapData = XElement.Load(xmlFileLocation);
        }

        public void LoadMap()
        {
            //parse xml, generate transforms, render, etc
            LoadElement(MapData);
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (var transform in Transforms)
            {
                transform.Render();
            }
        }

        private void LoadElement(XElement element)
        {
            foreach (var child in element.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case NodeType.BODY:
                        LoadElement(child);
                        break;
                    case NodeType.QUAD:
                        Transforms.Add(new Quad(child));
                        break;
                }
            }
        }
    }
}
