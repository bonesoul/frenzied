using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Frenzied.GamePlay.GameModes
{
    public interface IContainer
    {
        void Attach(Shape shape);
        void Detach(Shape shape);
    }
}
