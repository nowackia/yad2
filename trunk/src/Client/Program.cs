using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Client.UI;
using Client.Log;
using System.Reflection;
using System.Threading;
using Yad.Net;
using Yad.Net.General.Messaging;
using System.Net.Sockets;
using System.IO;
using Client.Net.General.Messaging;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InfoLog.WriteStart();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			
           /* Connection.InitConnection("127.0.0.1", 15000);
            Connection.SendMessage(new TextMessage());
         
            Connection.CloseConnection();*/
            //Client();
			

			MiniForm miniForm = new MiniForm();
            miniForm.Hide();

            UIManager uiManager = new UIManager(miniForm);
            uiManager.Start();

            Application.Run(miniForm);

            InfoLog.WriteEnd();
            InfoLog.Close();
        }

        public static  void Client() {
            TcpClient client = new TcpClient();
            client.Connect("localhost", 1734);
            BinaryReader reader = new BinaryReader(client.GetStream());
            BinaryWriter writer = new BinaryWriter(client.GetStream());
            LoginMessage m = MessageFactory.Create(MessageType.Login) as LoginMessage;
            m.Login = "bla";
            m.Password = "ble";
            m.Serialize(writer);
            byte type;
            while (true) {

                type = reader.ReadByte();
                Yad.Net.General.Messaging.Message msg = MessageFactory.Create((MessageType)type);
                msg.Deserialize(reader);
            }
            
            
        }
    }
}
