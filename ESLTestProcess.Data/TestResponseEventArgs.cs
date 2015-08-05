using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESLTestProcess.Data
{
    public class TestResponseEventArgs : EventArgs
    {
        public string Parameter;
        public byte[] RawValue;
        public string Value;
        public TestStatus Status;
    }
}   
