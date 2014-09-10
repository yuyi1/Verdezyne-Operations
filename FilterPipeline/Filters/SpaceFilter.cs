namespace FilterPipeline.Filters
{
    public class SpaceFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            return input.Replace(" ", string.Empty);
        }
    }
}