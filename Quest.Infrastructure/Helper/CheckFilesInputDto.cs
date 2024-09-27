using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quest.Infrastructure.Helper;
public class CheckFilesInputDto
{
    public List<string> Folders { get; set; } = new List<string>();
    public int MaximumConcurentProcessingJobs { get;set; }
}
