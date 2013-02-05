namespace Frenzied.GamePlay.Modes
{
    public interface IContainer
    {
        void Attach(Shape shape);
        void Detach(Shape shape);
    }
}
