using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace Integrator_PF6000
{
    public partial class Form1 : Form
    {
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = default(NetworkStream);
        string dataread;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "172.20.64.215"; //your controller ip address
            textBox2.Text = "4545";
        }

        private void connect()
        {
            timer1.Start();
            Thread ctThread = new Thread(getMessage);
            clientSocket.Connect(textBox1.Text, Int32.Parse(textBox2.Text));
            ctThread.Start();
        }

        private void getMessage()
        {
            string returndata;
            while (true)
            {
                serverStream = clientSocket.GetStream();
                var buffsize = clientSocket.ReceiveBufferSize;
                byte[] instream = new byte[buffsize];
                serverStream.Read(instream, 0, buffsize);
                returndata = System.Text.Encoding.Default.GetString(instream);
                dataread = returndata;
                msg();
            }
        }
        private void msg()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg));
            }
            else
            {
                textBox3.Text = dataread;

                //parse received data
                if (dataread.Substring(4, 4) == "0002")
                {
                    label4.Text = dataread.Substring(32, 25); //get controller name
                }
                else if (dataread.Substring(4, 4) == "0061")
                {
                    if (dataread.Substring(107, 1) == "0")
                    {
                        label6.Text = "NG";
                    }
                    else { label6.Text = "OK"; }
                    label5.Text = dataread.Substring(140, 6);//torque
                    label7.Text = dataread.Substring(59, 15);//vin
                    label11.Text = dataread.Substring(221, 10);//tightening id
                    label13.Text = dataread.Substring(176, 19);//timestamp
                    MID0062();
                }
            }
        }
        private void MID0001()
        {
            string MID = "00200001            \0";
            byte[] outstream = Encoding.Default.GetBytes(MID);

            serverStream.Write(outstream, 0, outstream.Length);
            serverStream.Flush();
        }
        private void MID9999()
        {
            string MID = "00209999            \0";
            byte[] outstream = Encoding.Default.GetBytes(MID);

            serverStream.Write(outstream, 0, outstream.Length);
            serverStream.Flush();
        }
        private void MID0060()
        {
            string MID = "00200060            \0";
            byte[] outstream = Encoding.Default.GetBytes(MID);

            serverStream.Write(outstream, 0, outstream.Length);
            serverStream.Flush();
        }
        private void MID0062()
        {
            string MID = "00200062            \0";
            byte[] outstream = Encoding.Default.GetBytes(MID);

            serverStream.Write(outstream, 0, outstream.Length);
            serverStream.Flush();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MID0001();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MID0060();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MID9999();
        }

    }
}
