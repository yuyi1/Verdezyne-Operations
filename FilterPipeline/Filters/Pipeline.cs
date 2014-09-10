// Pasted from <http://rantdriven.com/post/Simple-Pipe-and-Filters-Implementation-in-C-with-Fluent-Interface-Behavior.aspx>
// Search OneNote for Filter

namespace FilterPipeline.Filters
{
    public class Pipeline<T> : IFilterChain<T>
    {
        private IFilter<T> _root;

        #region Members

        public T Execute(T input)
        {
            return _root.Execute(input);
        }

        public IFilterChain<T> Register(IFilter<T> filter)
        {
            if (_root == null)
            {
                _root = filter;
            }
            else
            {
                _root.Register(filter);
               
            }
            return this;

        }
        #endregion 
    }
}