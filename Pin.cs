using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TestLogicAPP
{
        /// <summary>
    /// Элемент соединяющий ноды
    /// </summary>
    public class Pin : IDrawable, IDragable
    {
        public Node Parent { get; set; }
        public Point RelativeLocation { get;set;}
        public GraphicsPath Path { get; set; }
        public Color BorderColor { get; set; }
        public Color FillColor { get; set; }
        private Point drag;
 
        public Point Location
        {
            get { return Parent.Location.Add(RelativeLocation); }
        }
 
        public Pin(Node parent)
        {
            BorderColor = Color.Navy;
            Parent = parent;
            Path = new GraphicsPath();
            Path.AddEllipse(new Rectangle(-5, -5, 10, 10));
        }
 
        public void Paint(Graphics gr)
        {
            if (drag != Point.Empty)
                gr.DrawLink(Location, Location.Add(drag));
 
            var state = gr.Save();
            gr.TranslateTransform(Location.X, Location.Y);
            gr.FillPath(FillColor.Brush(), Path);
            gr.DrawPath(BorderColor.Pen(), Path);
            if (drag != Point.Empty)
            {
                gr.DrawEllipse(BorderColor.Pen(), drag.X - 5, drag.Y - 5, 10, 10);
                gr.FillEllipse(FillColor.Brush(), drag.X - 5, drag.Y - 5, 10, 10);
            }
            gr.Restore(state);
        }
 
        public bool Hit(Point point)
        {
            return Path.IsVisible(point.Sub(Location));
        }
 
        public void Drag(Point offset)
        {
            drag = drag.Add(offset);
        }
 
        public IDragable StartDrag(Point p)
        {
            return this;
        }
 
        public void EndDrag()
        {
            var p = Location.Add(drag);
            //ищем целевой объект
            foreach (var node in Parent.Model.OfType<Node>())
            if(node != Parent && node.AcceptPin)
            {
                if (node.Hit(p))
                    node.Linked.Add(Parent);
            }
            drag = Point.Empty;
        }

        public Node Набор_соединений
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }
    }
}
