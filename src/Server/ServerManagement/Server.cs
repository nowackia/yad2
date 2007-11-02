using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Server.ServerManagement {
    class Server {

        private int _portNumber;
        private TcpListener _listener;
        private IPAddress _ipaddress;
        private List<Player> _players;
        
        public Server(string ipString, int PortNumber) {
            _portNumber = PortNumber;
            _ipaddress = IPAddress.Parse(ipString);
            _players = new List<Player>();
        }

        public void AcceptConnections() {
            TcpClient client = _listener.AcceptTcpClient();
            _players.Add(new Player(client));
        }

        
        


    }
}
