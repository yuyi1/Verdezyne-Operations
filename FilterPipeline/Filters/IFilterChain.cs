// Pasted from <http://rantdriven.com/post/Simple-Pipe-and-Filters-Implementation-in-C-with-Fluent-Interface-Behavior.aspx>
// Search OneNote for Filter

namespace FilterPipeline.Filters
{
    /// <summary>
    /// Generic Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFilterChain<T>
    {
        T Execute(T input);
        IFilterChain<T> Register(IFilter<T> filter);
    }
}
