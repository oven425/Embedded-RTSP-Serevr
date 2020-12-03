using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_RtspT
{
    public class CTransport
    {
        public static CTransport Parse(string data)
        {
            CTransport cc = null;

            return cc;
        }
        public string Type { set; get; }
        public int ClientPort_RTP { set; get; }
        public int ClientPort_RTCP { set; get; }
        public int ServerPort_RTP { set; get; }
        public int ServerPort_RTCP { set; get; }
        public override string ToString()
        {
            string str = $"Transport: RTP/AVP;unicast;client_port={this.ClientPort_RTP}-{this.ClientPort_RTCP}";
            return str;
        }

        
    }

    
}
