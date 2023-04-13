using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace Graphic_editor
{
    public interface ICommand
    {
        void Execute();
        void UnExecute();
    }
    public class DrawCommand : ICommand
    {
        private UIElement element;
        private Canvas canvas;
        public DrawCommand(UIElement shape, Canvas canvas)
        {
            this.element = shape;
            this.canvas = canvas;
        }
        public void Execute()
        {
            canvas.Children.Add(element);
        }
        public void UnExecute()
        {
            canvas.Children.Remove(element);
        }
    }
}
