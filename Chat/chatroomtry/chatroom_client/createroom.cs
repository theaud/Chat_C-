using System;
using System.Text;
using System.Windows.Forms;

namespace chatroom_client
{
    public partial class createroom : Form
    {
        public int room_number;
        public string user_name;
        public string room_name;
        public static string validateRoom="null";
        client c2;

        public createroom(client c)
        {
            c2 = c;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = c2.username;
            string password = c2.password;

            if (textBox2.Text == null || textBox2.Text == String.Empty)
            {
                MessageBox.Show("Room name can't be empty!");
            }
            else
            {
                c2.ClientSocket.Send(Encoding.Unicode.GetBytes("CREATE_ROOM" + "µ" + username + "µ" + password + "µ" + textBox2.Text + "µ" + "\r\n"));
                System.Threading.Thread.SpinWait(10000);
                do
                {

                } while (validateRoom == "null");
                if (validateRoom == "success")
                {
                    validateRoom ="null";
                    choose_room c3 = new choose_room(c2);
                    c3.username = c2.username;
                    this.Hide();
                    c3.ShowDialog();
                }
            }
        }
    }
}
