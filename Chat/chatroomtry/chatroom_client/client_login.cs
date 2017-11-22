using System;
using System.Text;
using System.Windows.Forms;


namespace chatroom_client
{
    public partial class client_login : Form
    {
        public static string checkConnection;
        public client c5;

        public client_login()
        {
            InitializeComponent();
            c5 = new client();
            checkConnection = "null";
        }

        private void button1_Click(object sender, EventArgs e)//register
        {
            c5.ClientSocket.Send(Encoding.Unicode.GetBytes("REGISTER"+"µ" +username.Text.ToString() + "µ" + password.Text.ToString() + "µ" + "\r\n"));
            c5.username = username.Text.ToString();
            c5.password= password.Text.ToString();
            c5.room_name = "empty";
            //we wait until the login and the password are checked by the server.
            do
            {

            } while (checkConnection == "null");
            //the login and password are good
            if (checkConnection == "success")
            {
                checkConnection = "null";
                choose_room c4 = new choose_room(c5);
                c4.username = username.Text.ToString();
                this.Hide();
                c4.ShowDialog();
            }


        }

        private void button2_Click(object sender, EventArgs e)//log in part
        {
            c5.username = username.Text.ToString();
            c5.password = password.Text.ToString();
            c5.room_name = "empty";
            c5.ClientSocket.Send(Encoding.Unicode.GetBytes("CONNECTION" + "µ" + username.Text.ToString() + "µ" +password.Text.ToString() 
                + "µ"+ "\r\n"));
            do
            {

            } while (checkConnection == "null");
            if (checkConnection == "success")
            {
                checkConnection = "null";
                choose_room c4 = new choose_room(c5);
                c4.username = username.Text.ToString();
                this.Hide();
                c4.ShowDialog();
            } 
        }   
    }
}
