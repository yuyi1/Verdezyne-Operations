namespace FilterPipeline.Filters
{
    public class AcetateToAnionFilter : FilterBase<string>
    {
        protected override string Process(string input)
        {
            return ReplaceString(input);
        }

        public static string Filter(string input)
        {
            return ReplaceString(input);
        }

        private static string ReplaceString(string input)
        {
            string acetate = " Acetate";
            if (input.Contains(acetate))
            {
                return input.Replace(acetate, " Anion");
            }
            return input;
            
        }
    }
}