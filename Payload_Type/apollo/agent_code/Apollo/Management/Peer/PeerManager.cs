﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.Peers.SMB;
using ApolloInterop.Interfaces;
using ApolloInterop.Structs.MythicStructs;
using AI = ApolloInterop;
namespace Apollo.Management.Peer
{
    public class PeerManager : AI.Classes.PeerManager
    {
        public PeerManager(IAgent agent) : base(agent)
        {

        }

        public override IPeer AddPeer(PeerInformation connectionInfo)
        {
            switch(connectionInfo.C2Profile.Name.ToUpper())
            {
                case "SMB":
                    SMBPeer peer = new SMBPeer(_agent, connectionInfo.C2Profile);
                    peer.Start();
                    while(!_peers.TryAdd(peer.GetUUID(), peer))
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                    return peer;
                default:
                    throw new Exception("Not implemented.");
            }
        }

        public override bool Route(DelegateMessage msg)
        {
            if (_peers.ContainsKey(msg.UUID))
            {
                // ???
                _peers[msg.UUID].ProcessMessage(msg);
                return true;
            }
            return false;
        }
    }
}
