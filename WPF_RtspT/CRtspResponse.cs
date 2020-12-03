using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_RtspT
{
    public class CRtspResponse
    {
        public string Version { set; get; }
        public int StatusCode { set; get; }
        public string ReasonPhrase { set; get; }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public int CSeq { set; get; }
        public int ContentLength { set; get; }
        public string Server { set; get; }
        public CTransport TransPort { set; get; }
        public bool Parse(string data)
        {
            this.Headers.Clear();
            string[] sl = data.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            if(sl.Length>0)
            {
                string[] headers = sl[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if(headers.Length >=3)
                {
                    this.Version = headers[0];
                    this.StatusCode = int.Parse(headers[1]);
                    this.ReasonPhrase = "";
                    for(int i=2; i<headers.Length; i++)
                    {
                        if(string.IsNullOrEmpty(this.ReasonPhrase) == false)
                        {
                            this.ReasonPhrase = this.ReasonPhrase + " ";
                        }
                        this.ReasonPhrase = headers[2];
                    }
                }
            }
            for(int i=1; i<sl.Length; i++)
            {
                int findindex = sl[i].IndexOf(":");
                if(findindex >=0)
                {
                    string key_str = sl[i].Substring(0, findindex).TrimEnd();
                    string value_str = sl[i].Substring(findindex + 1, sl[i].Length - (findindex + 1)).TrimStart();
                    this.Headers[key_str] = value_str;
                    //Transport: RTP/AVP/UDP;unicast;client_port=64445-64446;server_port=64447-64448;ssrc=0CCB61AB;mode=play
                    switch (key_str.ToUpperInvariant())
                    {
                        case "CSEQ":
                            {
                                this.CSeq = int.Parse(value_str);
                            }
                            break;
                        case "Content-Length":
                            {
                                this.ContentLength = int.Parse(value_str);
                            }
                            break;
                        case "SERVER":
                            {
                                this.Server = value_str;
                            }
                            break;
                        case "TRANSPORT":
                            {
                                this.TransPort = this.ParseTransPort(value_str);
                                
                                

                            }
                            break;
                    }
                }
                
            }
            
            return true;
        }

        CTransport ParseTransPort(String data)
        {
            CTransport transport = new CTransport();
            string[] sl = data.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            this.TransPort.Type = sl[0].Trim();
            for (int j = 2; j < sl.Length; j++)
            {
                string[]keyvalues =  sl[j].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if(keyvalues.Length == 2)
                {

                }
            }

            return transport;
        }
        
    }

}
