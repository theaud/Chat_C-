using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
namespace chatroom_client
{
  
    public partial class client : Form
    {
        public IPEndPoint ServerInfo;
        public Socket ClientSocket;
        public object obj;
        public string username;
        public string room_name;
        public string password;
        // save the message received信息接收缓存
        public Byte[] MsgBuffer;
        //save the message send信息发送存储
        public Byte[] MsgSend;
        public string MsgReceived = "";

        public string[] RoomList = new string[100];//to store all the available rooms
        public string check_roomchange;

        public client()
        {
            InitializeComponent();
            ConnectServer();
            this.button1.Click += new EventHandler(button1_Click);
        }
        
        /// connect server 打开客户端，即连接服务器
       private void ConnectServer()
        {
            try
            {
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                MsgBuffer = new byte[65535];
                MsgSend = new byte[65535];
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                ServerInfo = new IPEndPoint(ip, Int32.Parse("3000"));
                ClientSocket.Connect(ServerInfo); //The client connect to the server

                ClientSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, SocketFlags.None, 
                   new AsyncCallback(ReceiveCallback), null);//Commence à la réception asynchrone de données à partir du Server.
                     //Délégué AsyncCallback qui fait référence à la méthode à appeler quand l'opération est terminée.
            }

            catch (System.Exception ex)
            {
                MessageBox.Show("blabla");
            }

        }
        /// receive message回调时调用
        public void ReceiveCallback(IAsyncResult ar)
        {
            textBox3.Text = username;
            textBox1.Text = "Room"+" : "+room_name;
            int rEnd = ClientSocket.EndReceive(ar);
           
            MsgReceived = Encoding.Unicode.GetString(MsgBuffer, 0, rEnd);
            var tab = MsgReceived.Split(new[] { "µ" }, StringSplitOptions.None);
            int size = tab.GetLength(0);
            if (tab[0]=="MESSAGE")
            {
                string UserIdMessage = tab[1];
                string message = tab[2];
                this.listBox1.Items.Add( UserIdMessage+" : "+message+ "\r\n");
                
            }
            if (tab[0] == "FAIL ROOM")
            {
                MessageBox.Show("the room exists already ! ");
                createroom.validateRoom = "wrong";
            }
            if (tab[0] == "SUCCESS ROOM")
            {
                for (int i = 1; i < size; i++)
                {
                    RoomList[i] = tab[i];

                }
                createroom.validateRoom = "success";
            }
            if (tab[0] == "SUCCESS")
            {
                for (int i =1; i<size; i++)
                {
                    RoomList[i] = tab[i];

                }
                client_login.checkConnection = "success";
                MessageBox.Show("success login");
            }
            if (tab[0] == "FAIL")
            {
                MessageBox.Show("The username or the password are wrong");
                client_login.checkConnection = "wrong";
            }
            if (tab[0] == "register fail")
            {
                MessageBox.Show("Choose another username");
                client_login.checkConnection = "wrong";
            }
            if (tab[0] == "register success")
            {
                for (int i = 1; i < size; i++)
                {
                    RoomList[i] = tab[i];

                }
                MessageBox.Show("success register");
                client_login.checkConnection = "success";
            }
            if (tab[0] == "CHANGED_ROOM")
            {
                for (int i = 1; i < size; i++)
                {
                    RoomList[i] = tab[i];

                }
                MessageBox.Show("success change room");
                check_roomchange="checked";
            }
            ClientSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
        }

        
        /// send message 发送信息
        private void button1_Click(object sender, EventArgs e)
        {
           MsgSend = Encoding.Unicode.GetBytes("MESSAGE" + "µ" + username +"µ" +room_name+ "µ" + this.textBox2.Text + "\n\r");
            if (ClientSocket.Connected)
            {
                ClientSocket.Send(MsgSend);
                textBox2.Text = "";
            }

        }

        private void button2_Click(object sender, EventArgs e) //change the room
        {
            MsgSend = Encoding.Unicode.GetBytes("CHANGE_ROOM" +"µ"+username+"µ"+ "\n\r");
           
            ClientSocket.Send(MsgSend);
            MessageBox.Show("room menu");
            do
            {

            } while (check_roomchange != "checked");

            check_roomchange = "wrong";
            room_name = "empty";
            this.textBox1.Text = "";
            this.listBox1.Items.Clear();
            choose_room c4 = new choose_room(this);
            c4.username = username;
            this.Hide();
            c4.ShowDialog();  
        }
    }
}
