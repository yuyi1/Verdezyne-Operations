namespace FilterPipeline.Filters
{
    public class NameFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            if (input.Contains("Name"))
            {
                if (input.Contains("Phase"))
                {
                    return input;
                }
                return string.Empty;
            }
            return input;
        }
    }
}