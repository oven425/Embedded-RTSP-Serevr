using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_RtspT
{
    public enum OverTypes
    {
        UDP,
        TCP,
        Http
    }

    
    public class CRtspRequest
    {
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public RtspCommands Command { set; get; }
        public string Version { set; get; }
        public string URL { set; get; }
        public int CSeq { set; get; }
        public CTransport Transport;
        public override string ToString()
        {
            StringBuilder strb = new StringBuilder();
            strb.AppendLine($"{this.Command} {this.URL} {this.Version}");
            strb.AppendLine($"CSeq: {this.CSeq}");
            switch(this.Command)
            {
                case RtspCommands.SETUP:
                    {
                        strb.AppendLine(this.Transport.ToString());
                    }
                    break;
            }
            strb.AppendLine();
            return strb.ToString();
        }
    }

    public enum RtspCommands
    {
        OPTIONS,
        DESCRIBE,
        SETUP,
        PLAY,
        PAUSE,
        TearDown
    }
}
