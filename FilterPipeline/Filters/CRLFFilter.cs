namespace FilterPipeline.Filters
{
    public class CrlfFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            var ret = input;
            if (ret.Contains("\n"))
                return ret.Replace("\n", string.Empty);
            return input;
        }
    }
}