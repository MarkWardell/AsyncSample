using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KattisUtilities
{
    public class ExcelKattisWriter : IKattisProblemWriter
    {
        public string FileName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Close()
        {
            throw new NotImplementedException();
        }

        public bool OpenForWrite()
        {
            throw new NotImplementedException();
        }

        public void WriteProblem(KattisFromHTMLProblem problem)
        {
            
        }
    }
}
