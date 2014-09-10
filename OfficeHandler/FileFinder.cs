using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeHandler
{
    public class FileFinder
    {
        public string _filename { get; set; }
        public string _basefolder { get; set; }
        public FileFinder(string filename)
        {
            _filename = filename;
            _basefolder = @"\\\\james\\Shared\\Research\\Fermentation\\";
        }

        public FileFinder()
        {
            _filename = string.Empty;
            _basefolder = @"\\\\james\\Shared\\Research\\Fermentation\\";
        }
        public IQueryable<string> QueryableFileList { get; set; }


        public IQueryable<string> FindOutlineFile(string runDate)
        {
            if (QueryableFileList == null)
                QueryableFileList = System.IO.Directory
                    .EnumerateFiles(_basefolder, "*.docx", SearchOption.AllDirectories)
                    .Where(x => x.Contains(runDate))
                    .Where(x => x.Contains(@"Outline"))
                    .AsQueryable<string>();
            //var list = QueryableFileList.ToList();
            return QueryableFileList;
        }

        public IQueryable<string> FindSOPFile(string runDate)
        {
            if (QueryableFileList == null)
                QueryableFileList = System.IO.Directory
                    .EnumerateFiles(_basefolder, "*.docx", SearchOption.AllDirectories)
                    .Where(x => x.Contains(runDate))
                    .Where(x => x.Contains(@"SOP"))
                    .AsQueryable<string>();

            //var list = QueryableFileList.ToList();
            return QueryableFileList;
        }

        public IQueryable<string> FindFileByName(string filename)
        {
            return QueryableFileList ?? (QueryableFileList = System.IO.Directory
                .EnumerateFiles(_basefolder, filename, SearchOption.AllDirectories)
                .AsQueryable<string>());
        }
        /// <summary>
        /// Finds a list of files in the top folder using a wildcard
        /// </summary>
        /// <param name="partial">string - the partial filename using wildcards</param>
        /// <returns>IQueryable<string> - A list of matching files (lazy)</returns>
        public IQueryable<string> FindByPartialFilename(string partial)
        {
            return QueryableFileList ?? (QueryableFileList = System.IO.Directory
                .EnumerateFiles(_basefolder, partial, SearchOption.TopDirectoryOnly)
                .AsQueryable<string>());
        }

        public FileInfo[] GetFileInfos()
        {
            DirectoryInfo dir = new DirectoryInfo(_basefolder);
            return dir.GetFiles();
        }

        public FileInfo GetFileInfo()
        {
            string filename = _basefolder;
            if (!filename.EndsWith("\\"))
                filename += "\\";
            filename += _filename;

            return new FileInfo(filename);
        }
    }
}
