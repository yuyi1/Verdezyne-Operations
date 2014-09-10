namespace FilterPipeline.Filters
{
    public class NH4Filter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            if (input.Contains("NH4+ / NH3and YSI"))
                return "NH4+/NH3andYSI";
            if (input.Contains("NH"))
                return "NH4+/NH3";
            else
            {
                return input;
            }
        }
    }
}