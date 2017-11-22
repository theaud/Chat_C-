using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using Chatroom_login;

namespace chatroomtry
{
    public partial class Server : Form
    {
        private IPEndPoint ServerInfo;//store the server's ip and port存放服务器的IP和端口信息
        private Socket ServerSocket;//socket of server 服务端运行的SOCKET
        private Thread ServerThread;//thread of server 服务端运行的线程
        private Socket[] ClientSocket;//sockets of client为客户端建立的SOCKET连接
        public int ClientNumb;//number of clients存放客户端数量
        private byte[] MsgBuffer;//message 存放消息数据
        private byte[] Msgsendback;
        public static string[,] tabUsers = new string[100,2];//to store the users and the room of each users
        public static string[] Roomtable = new string[100];//to store all the rooms existing in the database
        public static bool boolean = false;

        public Server()
        {
            InitializeComponent();
            ListenClient();
        }
        /// start listening to the client 
        private void ListenClient()
        {
            try
            {
                // Create a TCP/IP socket.
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                ServerInfo = new IPEndPoint(ip, Int32.Parse("3000"));
                //(lier) Bind the socket to the ServeurInfo 
                ServerSocket.Bind(ServerInfo);
                // listen for incoming connections.
                ServerSocket.Listen(10); //

                ClientSocket = new Socket[65535];
                MsgBuffer = new byte[65535];
                ClientNumb = 1;

                ServerThread = new Thread(new ThreadStart(RecieveAccept)); //Client start connection with the serveur
                ServerThread.Start();
            }
            catch (System.Exception ex)
            {

            }
        }

        // client connection 
        private void RecieveAccept()
        {
            while (true)
            {
                //wait for client connection
                ClientSocket[ClientNumb] = ServerSocket.Accept();

                //receive client message and beging ReceiveCallback method 
                ClientSocket[ClientNumb].BeginReceive(MsgBuffer, 0, MsgBuffer.Length, SocketFlags.None,
                new AsyncCallback(ReceiveCallback), ClientSocket[ClientNumb]);
                this.Invoke((MethodInvoker)delegate
                {
                    lock (this.listBox1)
                    this.listBox1.Items.Add("User：" + ClientNumb.ToString() + " Connect Successfully！" + "\r\n");
             
                });
                ClientNumb++;
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket rSocket = (Socket)ar.AsyncState;

                int rEnd = rSocket.EndReceive(ar);
                data.db_connection();
                //we store the message send by the user (username+µ+room_name+µ+msg)
                this.listBox1.Items.Add("callback");
                string Msgsend = Encoding.Unicode.GetString(MsgBuffer, 0, rEnd);
                //we divide Msgsend in parts : tab[0] with the type of Message,...
                var tab = Msgsend.Split(new[] { "µ" }, StringSplitOptions.None);

                if (tab[0] == "REGISTER")
                {
                    string typeMessage = tab[0];
                    string username = tab[1];
                    string password = tab[2];
                    if (data.user_register(username, password))
                    {
                        int nb = ClientNumb - 1;
                        data.fill_listbox();
                        do { } while (boolean == false);
                        boolean = false;
                        tabUsers[nb, 0] = username;
                        int size = Roomtable.GetLength(0);
                        string Msgsendback = "register success";
                        for (int i = 1; i < size; i++)
                        {
                            if (!String.IsNullOrEmpty(Roomtable[i]))
                            {
                                Msgsendback = Msgsendback + "µ" + Roomtable[i];
                            }

                        }
                        ClientSocket[ClientNumb - 1].Send(Encoding.Unicode.GetBytes(Msgsendback));
                        this.listBox1.Items.Add("FIN REGISTERRRR part" + tabUsers[nb, 0]);

                    }
                    else
                    {
                        ClientSocket[ClientNumb - 1].Send(Encoding.Unicode.GetBytes("register fail" + "µ" + "\n\r"));
                    }
                }
                else if (tab[0] == "CONNECTION")
                {
                    string typeMessage = tab[0];
                    string username = tab[1];
                    string password = tab[2];

                    if (data.validate_login(username,password))
                    {
                        int  nb = ClientNumb - 1;
                       
                        data.fill_listbox();
                        do { } while (boolean == false);
                        boolean = false;
                        tabUsers[nb, 0] = username;
                        int size = Roomtable.GetLength(0);
                        string Msgsendback="SUCCESS";
                        for (int i=1; i<size;i++)
                        {
                            if (!String.IsNullOrEmpty(Roomtable[i]))
                            {
                                Msgsendback = Msgsendback + "µ" + Roomtable[i];
                            }
                           
                        }
                        ClientSocket[ClientNumb - 1].Send(Encoding.Unicode.GetBytes(Msgsendback));
                        this.listBox1.Items.Add("FIN connection part " + username + "  " + (ClientNumb-1) + "   " + tabUsers[nb, 0]);
                    }
                    else
                    {
                        this.listBox1.Items.Add("Fail to login");
                        ClientSocket[ClientNumb - 1].Send(Encoding.Unicode.GetBytes("FAIL" +"µ"+username+"µ"+password +"µ"+"\n\r"));
                    }
                }
                else if (tab[0]=="ROOM")
                {
                    
                    string typeMessage = tab[0];
                    string username = tab[1];
                    string room_name = tab[2];
                    int nbLigns = tabUsers.GetLength(0);
                    int UserNumber=-1;
                    for (int i=1;i<nbLigns-1; i++)
                    {
                        if (tabUsers[i,0]==username)
                        {
                            UserNumber = i;
                            tabUsers[i,1] = room_name;
                        }
                    }
                    this.listBox1.Items.Add("FIN room part" + username + "   " + room_name + tabUsers[UserNumber, 0]);
                }
                else if (tab[0]=="CREATE_ROOM")
                {
                    string typeMessage = tab[0];
                    string username = tab[1];
                    string password = tab[2];
                    string room = tab[3];
                    int UserNumber=-1;
                    int nbLigns = tabUsers.GetLength(0);
                    for (int i = 1; i < nbLigns - 1; i++)
                    {
                        if(username==tabUsers[i,0])
                        {
                            UserNumber = i;
                        }
                    }
                    if (data.check_if_room_exist(room))
                    {
                        ClientSocket[ClientNumb - 1].Send(Encoding.Unicode.GetBytes("FAIL ROOM" + "µ" + room+ "µ" + "\n\r"));  
                    }
                    else
                    {
                        data.fill_listbox();
                        do { } while (boolean == false);
                        boolean = false;
                        int size = Roomtable.GetLength(0);
                        string Msgsendback = "SUCCESS ROOM";
                        for (int i = 1; i < size; i++)
                        {
                            if (!String.IsNullOrEmpty(Roomtable[i]))
                            {
                                Msgsendback = Msgsendback + "µ" + Roomtable[i];
                            }

                        }
                        ClientSocket[ClientNumb - 1].Send(Encoding.Unicode.GetBytes(Msgsendback));
                    }
                }
                else if(tab[0]=="CHANGE_ROOM")
                {
                    data.fill_listbox();
                    do { } while (boolean == false);
                    boolean = false;
                    string username = tab[1];
                    int size = Roomtable.GetLength(0);
                    for (int i = 1; i<size;i++)
                    {
                        if(tabUsers[i,0]==username)
                        {
                            tabUsers[i, 1] = "empty";
                        }
                    }
                    string Msgsendback = "CHANGED_ROOM";
                    for (int i = 1; i < size; i++)
                    {
                        if (!String.IsNullOrEmpty(Roomtable[i]))
                        {
                            Msgsendback = Msgsendback + "µ" + Roomtable[i];
                        }

                    }
                    ClientSocket[ClientNumb - 1].Send(Encoding.Unicode.GetBytes(Msgsendback)); 
                }
                else if (tab[0] == "MESSAGE")
                {
                    string username = tab[1];
                    string room_name = tab[2];
                    string message = tab[3];
                    int nbLigns = tabUsers.GetLength(0);
                    int UserNumber = -1;
                    Msgsendback = Encoding.Unicode.GetBytes("MESSAGE"+"µ"+username + "µ"+message + "\n\r"+ "µ" );
                    for (int i = 1; i < nbLigns - 1; i++)
                    {
                        if (tabUsers[i, 1] == room_name)
                        {
                            UserNumber = i;
                            if (ClientSocket[UserNumber].Connected)
                            {
                                ClientSocket[UserNumber].Send(Msgsendback);
                            }
                        }
                    }

            }
            rSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(ReceiveCallback), rSocket);
            }
            catch (System.Exception ex)
            {
                this.listBox1.Items.Add("problem");
                this.listBox1.Items.Add("error occurs" + ex.ToString()+ "\r\n");
            }
        }   
    }
}

