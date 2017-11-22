using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using chatroomtry;
namespace Chatroom_login
{
    class data
    {
        private static string constring;
        private static MySqlConnection conDatabase;

        public static void db_connection()
        {
            try
            {
                constring = "Server=localhost;Database=chatroom;Uid=root;Pwd=;";
                conDatabase = new MySqlConnection(constring);
                conDatabase.Open();
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static bool user_register(string user, string pass)
        {
            if (user == "" || pass == "")
            {
                MessageBox.Show("Please enter your username or password");
                return false;
            }
            else if (check_if_user_exist(user))
            {
                MessageBox.Show("This username is already exist!");
                return false;
            }

            string Query = "INSERT INTO chatroom.users(user_name,user_password)VALUES('"+ user +"','"+ pass +"');";
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;

            try
            {
                myReader = cmdDatabase.ExecuteReader();
                MessageBox.Show("SAVED");
                while (myReader.Read())
                {
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public static bool validate_login(string user, string pass)
        {
            string Query = "SELECT * FROM chatroom.users WHERE user_name = '"+ user +"' AND user_password ='"+ pass +"'";
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            myReader = cmdDatabase.ExecuteReader();
            if (myReader.Read())
            {
                myReader.Close();
                return true;
            }
            
            else
            {
                myReader.Close();
                return false;
            }

        }
        
        private static bool check_if_user_exist(string user)
        {
            
            string Query = "SELECT * FROM chatroom.users WHERE user_name = '" + user + "' ";
            MySqlCommand cmdDatabase = new MySqlCommand(Query, conDatabase);
            MySqlDataReader myReader;
            myReader = cmdDatabase.ExecuteReader();
            if (myReader.Read())
            {
                myReader.Close();
                return true;
            }
            else
            {
                myReader.Close();
                return false;
            }
        }


        public static bool check_if_room_exist(string room) //check if the room name already existed
        {
            string constring = "Server=localhost;Database=chatroom;Uid=root;Pwd=;";
            string query = "SELECT * FROM chatroom.rooms WHERE room_name = '"+ room +"' ";
            MySqlConnection conDataBase = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand(query, conDataBase);
            MySqlDataReader myReader;
            conDataBase.Open();
            myReader = cmdDataBase.ExecuteReader();
            if (myReader.Read())
            {
                myReader.Close();
                return true;
            }
            else
            {
                save_room(room);
                myReader.Close();
                return false;
            }  
          }

        public static void save_room(string room)
            {
                //we save the room in the database
                string query = "INSERT INTO chatroom.rooms(room_name)VALUES('"+ room +"');";
                string constring = "Server=localhost;Database=chatroom;Uid=root;Pwd=;";
                MySqlConnection conDataBase = new MySqlConnection(constring);
                MySqlCommand cmdDataBase = new MySqlCommand(query, conDataBase);
                MySqlDataReader myReader;
                try
                {
                    conDataBase.Open();
                    myReader = cmdDataBase.ExecuteReader();
                    while (myReader.Read())
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        public static void fill_listbox()
        {
           string constring = "Server=localhost;Database=chatroom;Uid=root;Pwd=;";
           string query = "select*from chatroom.rooms;"; // order by room_number
            MySqlConnection conDataBase = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand(query, conDataBase);
            try
            {
                conDataBase.Open();
                MySqlDataReader myReader = cmdDataBase.ExecuteReader();
                int size=Server.Roomtable.GetLength(0);
                int i = 1;
                    while (myReader.Read())
                    {
                        string rname = myReader.GetString("room_name");
                        Server.Roomtable[i] = rname;
                        i++;   
                    }
                    Server.boolean = true;   
            }
            catch (MySqlException e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
