﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_RtspT
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //aaaa
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8554));
            CRtspRequest req = new CRtspRequest();
            req.Command = RtspCommands.OPTIONS;
            req.URL = new Uri("rtsp://127.0.0.1:8554/test");
            req.Version = "RTSP/1.0";
            req.CSeq = req.CSeq + 1;
            string send_str = "OPTIONS rtsp://example.com/media.mp4 RTSP/1.0" + "\r\n";
            send_str = send_str + "CSeq: 1" + "\r\n";
            send_str = send_str + "\r\n";
            send_str = req.ToString();
            System.Diagnostics.Trace.WriteLine(send_str);
            socket.Send(Encoding.UTF8.GetBytes(send_str));
            byte[] recv_buf = new byte[8192];
            int recv_len = socket.Receive(recv_buf);
            string recv_str = Encoding.UTF8.GetString(recv_buf);
            CRtspResponse resp = new CRtspResponse();
            resp.Parse(recv_str);
            System.Diagnostics.Trace.WriteLine(recv_str);

            req.Command = RtspCommands.DESCRIBE;
            req.CSeq = resp.CSeq + 1;
            send_str = req.ToString();
            System.Diagnostics.Trace.WriteLine(send_str);
            socket.Send(Encoding.UTF8.GetBytes(send_str));
            recv_buf = new byte[8192];
            recv_len = socket.Receive(recv_buf);
            recv_str = Encoding.UTF8.GetString(recv_buf);
            resp = new CRtspResponse();
            resp.Parse(recv_str);

            System.Diagnostics.Trace.WriteLine(recv_str);
            recv_len = socket.Receive(recv_buf);
            recv_str = Encoding.UTF8.GetString(recv_buf, 0, recv_len);
            System.Diagnostics.Trace.WriteLine(recv_str);
            Dictionary<string, string> hh = new Dictionary<string, string>();
            string[] sl = recv_str.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach(string oo in sl)
            {

            }
            CSDP sdp = new CSDP();
            



        }
    }



    public class CQRiffList : CQRiffChunk
    {

        //public long Pos { set; get; }
        //public int Offset { set; get; } = 12;
        //public string Tag { set; get; }
        //public int Size { set; get; }
        public string Name { set; get; }
        public ObservableCollection<CQRiffList> Datas { set; get; }
    }

    public class CQRiffChunk
    {
        public long Pos { set; get; }
        public int Offset { set; get; } = 12;
        public string Tag { set; get; }
        public int Size { set; get; }

    }
}
