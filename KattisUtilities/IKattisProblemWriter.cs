using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KattisUtilities
{
    interface IKattisProblemWriter
    {
       string FileName { get; set; }
       bool OpenForWrite();
       void WriteProblem(KattisFromHTMLProblem problem);
       bool Close();
    }
}
