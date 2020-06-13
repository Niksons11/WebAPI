using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FileData
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string Url { get; set; }
        public string ModifiedOn { get; set; }

        public string Description { get; set; }

        public string Publisher { get; set; }

        public string PublisherFileName { get; set; }
    }
}
