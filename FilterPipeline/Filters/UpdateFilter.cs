namespace FilterPipeline.Filters
{
    public class UpdateFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            // Handles both LastUpdate and UpdateBy
            if (input.Contains("LastUpdate"))
            {
                return string.Empty;
            }
            return input;
        }
    }
}