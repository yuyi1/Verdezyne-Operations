namespace FilterPipeline.Filters
{
    public abstract class FilterBase<T> : IFilter<T>
    {
        private IFilter<T> _next;

        protected abstract T Process(T input);

        #region "IFilter<T> Members"

        public T Execute(T input)
        {
            T val = Process(input);
            if (_next != null) val = _next.Execute(val);
            return val;
        }

        public void Register(IFilter<T> filter)
        {
            if (_next == null)
            {
                _next = filter;
            }
            else
            {
                _next.Register(filter);
            }
        }
        #endregion
    }
}