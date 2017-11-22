using System;
using System.Windows.Forms;

namespace chatroom_client
{
    static class Program
    {
        /// 应用程序的主入口点。
     
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new client_login());
        }
    }
}
