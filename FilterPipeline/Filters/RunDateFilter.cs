namespace FilterPipeline.Filters
{
    public class RunDateFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            if (input.Contains("Run") && input.Contains("Date"))
            {
                return string.Empty;
            }
            return input;
        }
    }
}