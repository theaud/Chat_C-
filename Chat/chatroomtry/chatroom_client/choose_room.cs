using System;
using System.Text;
using System.Windows.Forms;
namespace chatroom_client
{
    public partial class choose_room : Form
    {
        public string username;
        client c2;
 
        public choose_room(client c)
        {
            c2 = c; // we affect
            InitializeComponent();
            fill_listbox();
        }
        void fill_listbox()
        {
            for (int i = 1; i < 100; i++)
            {
                string uname = c2.RoomList[i];
                if (!String.IsNullOrEmpty(uname)){ listBox1.Items.Add(uname); }
            }
        }

       
        private void button1_Click(object sender, EventArgs e)//enter the room
        {
            if(listBox1.SelectedItem==null)
                MessageBox.Show("You must choose a room first!");
            else
            {
                string room= listBox1.Text;
                string user = username;

                c2.username = username;
                c2.room_name = room;

                c2.ClientSocket.Send(Encoding.Unicode.GetBytes("ROOM" + "µ" + username + "µ" + room + "µ" + "\r\n"));
                this.Hide();
                c2.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)//create a room
        {
            createroom c1 = new createroom(c2);
            c1.user_name = username;
            this.Hide();
            c1.ShowDialog(); 
        } 
    }
}
