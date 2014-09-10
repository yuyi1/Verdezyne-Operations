namespace FilterPipeline.Filters
{
    public class SharedDriveToUri : FilterBase<string>
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
            string ret =  input.Replace("S:\\", "\\\\james\\Shared\\");
            if (!ret.EndsWith("\\"))
                ret += "\\";
            return ret;

        }
    }
}