using System.Collections.Generic;

namespace Todo.Interfaces
{
    public interface IIocMultipleExampleService
    {
        int type { get; }
        IEnumerable<string> MultipleIocExample();


    }
}
