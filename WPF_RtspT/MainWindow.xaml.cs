using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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
            Socket m_SocketAccept = null;
            
        }



        void Parse()
        {
            List<CQRiffList> riffs_tree = new List<CQRiffList>();
            List<CQRiffList> riffs = new List<CQRiffList>();
            var progressHandler = new Progress<Tuple<long, long>>(value =>
            {
                long vv = value.Item1 * 100 / value.Item2;
                //this.textblock_progress.Text = string.Format("{0}%", vv);
            });
            var progress = progressHandler as IProgress<Tuple<long, long>>;
            Task.Run(new Action(() =>
            {
                while (true)
                {
                    progress.Report(new Tuple<long, long>(this.m_File.Position, this.m_File.Length));
                    byte[] header = new byte[4];

                    int readlen = this.m_File.Read(header, 0, header.Length);

                    var bb = header.Except(this.m_Riff);
                    var cc = header.Except(this.m_List);
                    if (cc.Count() == 0 || bb.Count() == 0)
                    {
                        CQRiffList riff = new CQRiffList();
                        riff.Pos = this.m_File.Position - 4;
                        riff.Tag = Encoding.UTF8.GetString(header);
                        readlen = this.m_File.Read(header, 0, header.Length);
                        riff.Size = BitConverter.ToInt32(header, 0);
                        readlen = this.m_File.Read(header, 0, header.Length);
                        riff.Name = Encoding.UTF8.GetString(header);
                        riff.Datas = new ObservableCollection<CQRiffList>();
                        if (riff.Tag == "RIFF")
                        {
                            riffs_tree.Add(riff);

                            //this.Riffs.Add(riff);
                        }
                        else
                        {
                            riffs.Last().Datas.Add(riff);
                        }
                        riffs.Add(riff);

                    }
                    else
                    {
                        CQRiffList riff = new CQRiffList();
                        riff.Pos = this.m_File.Position - 4;
                        riff.Tag = Encoding.UTF8.GetString(header);
                        readlen = this.m_File.Read(header, 0, header.Length);
                        riff.Size = BitConverter.ToInt32(header, 0);

                        int ss = riff.Size % 2;

                        long pos = this.m_File.Position + riff.Size + ss;
                        if (riffs.Count > 0)
                        {
                            riffs.Last().Datas.Add(riff);
                        }

                        if (riff.Size < 0)
                        {
                            break;
                        }
                        else if (riffs.Count > 0)
                        {
                            while (riffs.Count > 0)
                            {
                                if (pos >= riffs.Last().Pos + 8 + riffs.Last().Size)
                                {

                                    if (riffs.Count > 0)
                                    {
                                        riffs.Remove(riffs.Last());
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                        this.m_File.Position = pos;
                    }
                    if ((this.m_File.Length - this.m_File.Position) <= 4)
                    {
                        break;
                    }
                }

            })).ContinueWith(aa =>
            {
                foreach (var oo in riffs_tree)
                {
                    this.Riffs.Add(oo);
                }
                //this.textblock_progress.Text = "";
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        Stream m_File = null;
        public ObservableCollection<CQRiffList> Riffs { set; get; } = new ObservableCollection<CQRiffList>();
        byte[] m_Riff = new byte[] { 0x52, 0x49, 0x46, 0x46 };
        byte[] m_List = new byte[] { 0x4c, 0x49, 0x53, 0x54 };

        async Task<List<CQRiffList>> FindRiffRoot(Stream stream, byte[] find_header, long begin, long end)
        {
            stream.Position = begin;
            List<CQRiffList> riffs = new List<CQRiffList>();
            byte[] header = new byte[4];
            while (stream.Position <= end)
            {
                int readlen = await stream.ReadAsync(header, 0, header.Length);
                var bb = header.Except(this.m_Riff);
                var cc = header.Except(this.m_List);
                if (bb.Count() == 0 || cc.Count() == 0)
                {
                    CQRiffList riff = new CQRiffList();
                    riff.Pos = stream.Position - 4;
                    riff.Tag = Encoding.UTF8.GetString(header);
                    readlen = await stream.ReadAsync(header, 0, header.Length);
                    riff.Size = BitConverter.ToInt32(header, 0);
                    readlen = await stream.ReadAsync(header, 0, header.Length);
                    riff.Name = Encoding.UTF8.GetString(header);
                    riff.Datas = new ObservableCollection<CQRiffList>();
                    riffs.Add(riff);
                    stream.Position = stream.Position + riff.Size - 4;
                }
                else
                {
                    CQRiffList riff = new CQRiffList();
                    riff.Tag = Encoding.UTF8.GetString(header);
                    readlen = await stream.ReadAsync(header, 0, header.Length);
                    riff.Size = BitConverter.ToInt32(header, 0);
                    if (riff.Size < 0)
                    {
                        break;
                    }
                    stream.Position = stream.Position + riff.Size;
                    if (stream.Position < stream.Length)
                    {
                        riffs.Add(riff);
                    }
                }
            }

            return riffs;
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
