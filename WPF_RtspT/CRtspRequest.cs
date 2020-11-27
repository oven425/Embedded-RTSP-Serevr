using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_RtspT
{
    public class CRtspRequest
    {
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public RtspCommands Command { set; get; }
    }

    public enum RtspCommands
    {
        Option,
        Describe,
        Setup,
        Play,
        Pause,
        TearDown
    }
}
