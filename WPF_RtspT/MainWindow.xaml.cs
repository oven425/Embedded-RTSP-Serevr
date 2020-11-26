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
            //aaaa
            
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
