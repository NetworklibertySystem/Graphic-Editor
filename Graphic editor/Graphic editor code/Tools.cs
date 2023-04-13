using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
namespace Graphic_editor
{
    namespace editor
    {
        public abstract class Tool
        {
            public ToolType type { get; protected set; }
            public abstract void OnPressed(Editor window, Point startPos);
            public abstract void OnHold(Point currentPos);
            public abstract ICommand OnReleased(Canvas cnvPaint);
        }

        public class Eraser : Tool
        {
            private PathFigure figure;
            private Path path;
            public Eraser() { type = ToolType.Eraser; }
            public override void OnPressed(Editor window, Point startPos)
            {
                figure = new PathFigure() { StartPoint = startPos };
                Path path = new Path()
                {
                    Stroke = Brushes.White,
                    StrokeThickness = window.currentBrushThickness,
                    Data = new PathGeometry() { Figures = { figure } }
                };

                window.cnvPaint.Children.Add(path);
                this.path = path;
            }
            public override void OnHold(Point currentPos)
            {
                figure.Segments.Add(new LineSegment(currentPos, true));
                path.Data = new PathGeometry() { Figures = { figure } };
            }
            public override ICommand OnReleased(Canvas cnvPaint)
            {
                figure = null;
                var p = path;
                path = null;
                return new DrawCommand(p, cnvPaint);
            }
        }
        public class _Brush : Tool
        {
            private PathFigure figure;
            private Path path;
            public _Brush() { type = ToolType.Brush; }
            public override void OnPressed(Editor window, Point startPos)
            {
                figure = new PathFigure() { StartPoint = startPos };
                Path path = new Path()
                {
                    Stroke = window.currentBrush,
                    StrokeThickness = window.currentBrushThickness,
                    Data = new PathGeometry() { Figures = { figure } }
                };
                window.cnvPaint.Children.Add(path);
                this.path = path;
            }
            public override void OnHold(Point currentPos)
            {
                figure.Segments.Add(new LineSegment(currentPos, true));
                path.Data = new PathGeometry() { Figures = { figure } };
            }
            public override ICommand OnReleased(Canvas cnvPaint)
            {
                figure = null;
                var p = path;
                path = null;
                return new DrawCommand(p, cnvPaint);
            }
        }
        public class Pencil : Tool
        {
            private PathFigure figure;
            private Path path;
            public Pencil() { type = ToolType.Pencil; }
            public override void OnPressed(Editor window, Point startPos)
            {
                figure = new PathFigure() { StartPoint = startPos };
                Path path = new Path()
                {
                    Stroke = window.currentBrush,
                    StrokeThickness = 0.5,
                    Data = new PathGeometry() { Figures = { figure } }
                };

                window.cnvPaint.Children.Add(path);
                this.path = path;
            }
            public override void OnHold(Point currentPos)
            {
                figure.Segments.Add(new LineSegment(currentPos, true));
                path.Data = new PathGeometry() { Figures = { figure } };
            }
            public override ICommand OnReleased(Canvas cnvPaint)
            {
                figure = null;
                var p = path;
                path = null;
                return new DrawCommand(p, cnvPaint);
            }

            public PathSegmentCollection CreateCircleShape(Editor window, MouseEventArgs e)
            {
                PathSegmentCollection collection = new PathSegmentCollection();
                for(double i = 0; i < e.GetPosition(window.cnvPaint).X - figure.StartPoint.X; i += 0.5)
                {
                    collection.Add(new LineSegment(new Point(figure.StartPoint.X + i, figure.StartPoint.Y + Math.Sin(i * 5 / (e.GetPosition(window.cnvPaint).Y - figure.StartPoint.Y)) * (e.GetPosition(window.cnvPaint).Y - figure.StartPoint.Y)), true));
                }
                return collection;
            }
        }
        public class Square : Tool
        {
            public Square() { type = ToolType.Square; }
            private PathFigure figure;
            private Path path;
            public override void OnPressed(Editor window, Point startPos)
            {
                figure = new PathFigure() { StartPoint = startPos };
                Path path = new Path()
                {
                    Stroke = window.currentBrush,
                    StrokeThickness = window.currentBrushThickness,
                    Data = new PathGeometry() { Figures = { figure } }
                };

                window.cnvPaint.Children.Add(path);
                this.path = path;
            }

            public override void OnHold(Point currentPos)
            {
                double l = Math.Max(Math.Abs(currentPos.X - figure.StartPoint.X), Math.Abs(currentPos.Y - figure.StartPoint.Y));
                Point L = new Point(l * (currentPos.X - figure.StartPoint.X > 0 ? 1 : -1), l * (currentPos.Y - figure.StartPoint.Y > 0 ? 1 : -1));

                PathSegmentCollection segments = new PathSegmentCollection()
                {
                    new LineSegment(new Point(figure.StartPoint.X + L.X, figure.StartPoint.Y), true),
                    new LineSegment(new Point(figure.StartPoint.X + L.X, figure.StartPoint.Y + L.Y), true),
                    new LineSegment(new Point(figure.StartPoint.X, figure.StartPoint.Y + L.Y), true),
                    new LineSegment(figure.StartPoint, true),
                };
                figure.Segments = segments;
                path.Data = new PathGeometry() { Figures = { figure } };
            }
            public override ICommand OnReleased(Canvas cnvPaint)
            {
                figure = null;
                var p = path;
                path = null;
                return new DrawCommand(p, cnvPaint);
            }
        }

        public class Rectangle : Tool
        {
            public Rectangle() { type = ToolType.Rectangle; }
            private PathFigure figure;
            private Path path;
            public override void OnPressed(Editor window, Point startPos)
            {
                figure = new PathFigure() { StartPoint = startPos };
                Path path = new Path()
                {
                    Stroke = window.currentBrush,
                    StrokeThickness = window.currentBrushThickness,
                    Data = new PathGeometry() { Figures = { figure } }
                };

                window.cnvPaint.Children.Add(path);
                this.path = path;
            }
            public override void OnHold(Point currentPos)
            {
                PathSegmentCollection segments = new PathSegmentCollection()
                {
                    new LineSegment(new Point(currentPos.X, figure.StartPoint.Y), true),
                    new LineSegment(currentPos, true),
                    new LineSegment(new Point(figure.StartPoint.X, currentPos.Y), true),
                    new LineSegment(figure.StartPoint, true),
                };
                figure.Segments = segments;
                path.Data = new PathGeometry() { Figures = { figure } };
            }
            public override ICommand OnReleased(Canvas cnvPaint)
            {
                figure = null;
                var p = path;
                path = null;
                return new DrawCommand(p, cnvPaint);
            }
        }

        public class Rhombus : Tool
        {
            public Rhombus() { type = ToolType.Rhombus; }
            private PathFigure figure;
            private Path path;
            public override void OnPressed(Editor window, Point startPos)
            {
                figure = new PathFigure() { StartPoint = startPos };
                Path path = new Path()
                {
                    Stroke = window.currentBrush,
                    StrokeThickness = window.currentBrushThickness,
                    Data = new PathGeometry() { Figures = { figure } }
                };

                window.cnvPaint.Children.Add(path);
                this.path = path;
            }
            public override void OnHold(Point currentPos)
            {
                Point L = new Point(currentPos.X - figure.StartPoint.X, currentPos.Y - figure.StartPoint.Y);

                PathSegmentCollection segments = new PathSegmentCollection()
                {
                    new LineSegment(new Point(figure.StartPoint.X + L.X / 2, figure.StartPoint.Y), false),
                    new LineSegment(new Point(figure.StartPoint.X + L.X, figure.StartPoint.Y + L.Y / 2), true),
                    new LineSegment(new Point(figure.StartPoint.X + L.X / 2, figure.StartPoint.Y + L.Y), true),
                    new LineSegment(new Point(figure.StartPoint.X, figure.StartPoint.Y + L.Y / 2), true),
                    new LineSegment(new Point(figure.StartPoint.X + L.X / 2, figure.StartPoint.Y), true),
                };
                figure.Segments = segments;
                path.Data = new PathGeometry() { Figures = { figure } };
            }

            public override ICommand OnReleased(Canvas cnvPaint)
            {
                figure = null;
                var p = path;
                path = null;
                return new DrawCommand(p, cnvPaint);
            }
        }

        /*
        public class FlatBrush : Tool
        {
            public FlatBrush() { type = ToolType.FlatBrush; }
            private PathFigure figure;
            private Path path;
            public override void OnPressed(Editor window, MouseEventArgs e)
            {
                figure = new PathFigure() { StartPoint = e.GetPosition(window.cnvPaint) };
                Path path = new Path()
                {
                    Stroke = window.currentBrush,
                    StrokeThickness = 0.5,
                    Data = new PathGeometry() { Figures = { figure } }
                };

                window.cnvPaint.Children.Add(path);
                this.path = path;
            }
            public override void OnHold(Editor window, MouseEventArgs e)
            {
                PathSegmentCollection points = new PathSegmentCollection()
                {
                    new LineSegment(e.GetPosition(window.cnvPaint), true),
                };
                figure.Segments = points;
                path.Data = new PathGeometry() { Figures = { figure } };
            }

            public override ICommand OnReleased(Editor window, MouseEventArgs e)
            {
                figure = null;
                var p = path;
                path = null;
                return new DrawCommand(p, window.cnvPaint);
            }
        }*/

        //        public class FlatBrush : Tool
        //        {
        //            public FlatBrush() { type = ToolType.FlatBrush; }
        //            public override void OnHold(Editor window, MouseEventArgs e)
        //            { }
        //        }

        //        public class FunBrush : Tool
        //        {
        //            public FunBrush() { type = ToolType.FunBrush; }
        //            public override void OnHold(Editor window, MouseEventArgs e)
        //            { window.cnvPaint.Children.Add(window.currentPath); window.AddDraw(e); }
        //        }
        public class Ellipse : Tool
        {
            Image anImage;
            EllipseGeometry ellipseGeometry;
            Point start;
            bool isFilled;
            public Ellipse(bool isFilled) { type = ToolType.Ellipse; this.isFilled = isFilled; }
            public override void OnPressed(Editor window, Point startPos)
            {
                start = startPos;
                GeometryGroup ellipses = new GeometryGroup();
                ellipseGeometry = new EllipseGeometry(start, 0, 0);
                ellipses.Children.Add(ellipseGeometry);
                ellipses.Children.Add(new EllipseGeometry(new Point(0, 0), 0, 0));

                GeometryDrawing aGeometryDrawing = new GeometryDrawing
                {
                    Geometry = ellipses,

                    // Paint the drawing with a gradient.
                    Brush = isFilled ? window.currentBrushFilling : null,

                    // Outline the drawing with a solid color.
                    Pen = new Pen(window.currentBrush, window.currentBrushThickness)
                };

                anImage = new Image {
                    Source = new DrawingImage(aGeometryDrawing),
                    Stretch = Stretch.None
                };

                window.cnvPaint.Children.Add(anImage);
            }

            public override void OnHold(Point currentPos)
            {
                ellipseGeometry.RadiusX = (currentPos - start).X / 2;
                ellipseGeometry.RadiusY = (currentPos - start).Y / 2;
                ellipseGeometry.Center = new Point(start.X + (currentPos - start).X / 2, start.Y + (currentPos - start).Y / 2);
            }

            public override ICommand OnReleased(Canvas cnvPaint)
            {
                anImage.Source.Freeze();
                Image im = anImage;
                anImage = null;
                ellipseGeometry = null;
                return new DrawCommand(im, cnvPaint);
            }
        }
//        public class Triangle : Tool
//        {
//            public Triangle() { type = ToolType.Triangle; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class _LineSegment : Tool
//        {
//            public _LineSegment() { type = ToolType._LineSegment; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Rectangle : Tool
//        {
//            public Rectangle() { type = ToolType.Rectangle; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Rhombus : Tool
//        {
//            public Rhombus() { type = ToolType.Rhombus; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Trapezoid : Tool
//        {
//            public Trapezoid() { type = ToolType.Trapezoid; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Pentagon : Tool
//        {
//            public Pentagon() { type = ToolType.Pentagon; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Hexagon : Tool
//        {
//            public Hexagon() { type = ToolType.Hexagon; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Parallelogram : Tool
//        {
//            public Parallelogram() { type = ToolType.Parallelogram; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Octagon : Tool
//        {
//            public Octagon() { type = ToolType.Octagon; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Decagon : Tool
//        {
//            public Decagon() { type = ToolType.Decagon; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Duodecagon : Tool
//        {
//            public Duodecagon() { type = ToolType.Duodecagon; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Star : Tool
//        {
//            public Star() { type = ToolType.Star; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class EquilateralTriangle : Tool
//        {
//            public EquilateralTriangle() { type = ToolType.EquilateralTriangle; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        CurvedTriangle
//        public class IsoscelesTriangle : Tool
//        {
//            public IsoscelesTriangle() { type = ToolType.IsoscelesTriangle; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class CurvedTriangle : Tool
//        {
//            public CurvedTriangle() { type = ToolType.CurvedTriangle; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Heart : Tool
//        {
//            public Heart() { type = ToolType.Heart; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//        public class Arrow : Tool
//        {
//            public Arrow() { type = ToolType.Arrow; }
//            public override void OnHold(Editor window, MouseEventArgs e)
//            { }
//        }
//    }
//}

    }
}