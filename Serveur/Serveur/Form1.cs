using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace Serveur
{
    public partial class Form1 : Form
    {
        // Changer le chemin d'accès à la Base de Donnée pour faire fonctionner le programme
        private const string CHEMIN_BASE = "C:\\Users\\Camille\\Desktop\\Chat\\Serveur\\Utilisateurs.mdb";
        private OleDbConnection ConnexionBase = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CHEMIN_BASE);

        Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        string LocalIP;

        private string GetLocalIP()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        public Form1()
        {
            InitializeComponent();
            LocalIP = GetLocalIP();

            Ecouter();
        }

        public void PgmClient(ParametresThread param)
        {
            TcpListener serv = new TcpListener(IPAddress.Parse(LocalIP), 11000);

            byte[] data = new byte[1464];

            data = (byte[])aResult.AsyncState;

            //Décryptage et affichage du message.
            ASCIIEncoding eEncoding = new ASCIIEncoding();
            string message = eEncoding.GetString(data);

            string m = message.Substring(0, 2);


            switch (m)
            {
                case "#c":
                    {
                        message = message.Substring(2, message.Length - 3);

                        string pseudo = message.Split(' ')[0];
                        string mdp = message.Split(' ')[1];
                        ConnexionBase.Open();
                        OleDbCommand Commande = new OleDbCommand();
                        Commande.Connection = ConnexionBase;

                        Commande.CommandText = "SELECT * FROM Utilisateurs WHERE Pseudo <> @PSEUDO AND MotDePasse <> @MOTDEPASSE";
                        Commande.Parameters.Add("PSEUDO", pseudo);
                        Commande.Parameters.Add("MOTDEPASSE", mdp);

                        Commande.ExecuteNonQuery();
                        OleDbDataReader reader = Commande.ExecuteReader();
                        break;
                    }

                case "#d":
                    {
                        message = message.Substring(2, message.Length - 3);

                        string pseudo = message.Split(' ')[0];
                        string mdp = message.Split(' ')[1];
                        ConnexionBase.Open();
                        OleDbCommand Commande = new OleDbCommand();
                        Commande.Connection = ConnexionBase;

                        Commande.CommandText = "DELETE FROM Utilisateurs WHERE Pseudo <> @PSEUDO AND MotDePasse <> @MOTDEPASSE";
                        Commande.Parameters.Add("PSEUDO", pseudo);
                        Commande.Parameters.Add("MOTDEPASSE", mdp);

                        Commande.ExecuteNonQuery();
                        OleDbDataReader reader = Commande.ExecuteReader();
                        break;
                    }

                case "#m":
                    {
                        message = message.Substring(2, message.Length - 3);

                        Discussion.Items.Add(message);

                        break;
                    }
            }
        }

        private void Ecouter()
        {
            IPEndPoint IPServeur = new IPEndPoint(IPAddress.Parse(LocalIP), 11000);
            sck.Bind(IPServeur);
            sck.Listen(100);

            while (true)
            {
                Socket SckClient = sck.Accept();
                ParametresThread param = new ParametresThread(SckClient, IPServeur);

                ThreadStart starter = delegate { PgmClient(param); };
                new Thread(starter).Start();
            }
            sck.Close();

        }
    }

    public class ParametresThread
    {
        Socket sck;
        IPEndPoint IP;

         public void parametresThread(Socket sck, IPEndPoint IP)
        {
            this.sck = sck;
            this.IP = IP;
        }
    }
}

