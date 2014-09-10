namespace FilterPipeline.Filters
{
    public class IdFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            if (input.Contains("Id"))
            {
                return string.Empty;
            }
            return input;
        }
    }
}