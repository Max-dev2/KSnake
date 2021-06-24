using System.Collections.Generic;

namespace KSnake
{
    interface IDrawList<T>
    {
        public List<T> DrawList(SnakeField snakeField, List<T> entity);

    }
}
