﻿/*
 *   Coinium project - crypto currency pool software - https://github.com/raistlinthewiz/coinium
 *   Copyright (C) 2013 Hüseyin Uslu, Int6 Studios - http://www.coinium.org
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Serilog;
using coinium.Core.Mining.Service;

namespace coinium.Net.RPC.Server
{
    public class RPCServer:Net.Server
    {
        private static object[] _services =
        {
            new MiningService()
        };

        public RPCServer()
        {
            this.OnConnect += RPCServerNew_OnConnect;
            this.OnDisconnect += RPCServerNew_OnDisconnect;
            this.DataReceived += RPCServerNew_DataReceived;
        }

        void RPCServerNew_OnConnect(object sender, ConnectionEventArgs e)
        {
            Log.Verbose("RPC-client connected: {0}", e.Connection.ToString());
            
            var client = new RPCClient(e.Connection);
            e.Connection.Client = client;
        }

        void RPCServerNew_OnDisconnect(object sender, ConnectionEventArgs e)
        {
            Log.Verbose("RPC-client disconnected: {0}", e.Connection.ToString());
        }

        void RPCServerNew_DataReceived(object sender, ConnectionDataEventArgs e)
        {
            var connection = (Connection)e.Connection;
            ((RPCClient)connection.Client).Parse(e);
        }
    }
}
