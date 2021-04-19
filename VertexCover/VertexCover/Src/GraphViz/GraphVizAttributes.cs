using System;
using System.Collections.Generic;

namespace VertexCover.Src.GraphViz
{
    public class GraphVizAttributes
    {
        public string Name { get; }
        public string FontName { get; }
        public string Style { get; }
        public string Shape { get; }

        private readonly Dictionary<IGraphElement, List<Tuple<string, string>>> attributes;

        public GraphVizAttributes(string name, string fontName, string style, string shape)
        {
            Name = name;
            FontName = fontName;
            Style = style;
            Shape = shape;
            attributes = new Dictionary<IGraphElement, List<Tuple<string, string>>>();
        }

        /// <summary>
        /// Add an attribute for graph viz
        /// </summary>
        /// <param name="graphElement">The element you want to add attributes for</param>
        /// <param name="attribute">The attribute you want to add</param>
        public void AddAttribute(IGraphElement graphElement, Tuple<string, string> attribute)
        {
            if (!HasAttributes(graphElement))
            {
                var values = new List<Tuple<string, string>>
                {
                    attribute
                };
                attributes.Add(graphElement, values);
            }
            else
            {
                attributes[graphElement].Add(attribute);
            }
        }

        /// <summary>
        /// Get the list of attributes this element has
        /// </summary>
        /// <param name="graphElement">The element that you want attributes for</param>
        /// <returns></returns>
        public List<Tuple<string, string>> GetAttributes(IGraphElement graphElement)
        {
            return attributes[graphElement];
        }

        /// <summary>
        /// Check if this graph elements has attributes
        /// </summary>
        /// <param name="graphElement">The elements you want to add</param>
        /// <returns>True if the element has attributes</returns>
        public bool HasAttributes(IGraphElement graphElement)
        {
            return attributes.ContainsKey(graphElement);
        }
    }
}
