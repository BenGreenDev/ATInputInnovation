using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace IRCChatApi
{
    public class IRCChat
    {
        TcpClient client;
        StreamReader readerBuffer;
        StreamWriter writeBuffer;
        Thread ircChatThread;
        bool isThreadActive = true;
        bool isConnected = false;
        Queue<string> messageQueue = new Queue<string>();

        string username = "razznecrobot";
        string oAuth = "oauth:34yxbzlc5dqfpl80ev5szdo6zuzccp";
        string channelName = "razznecro";
        
        public bool InitaliseIRC(string _username, string _oAuth, string _ircChannelName)
        {
            try 
            {
                username = _username;
                oAuth = _oAuth;
                channelName = _ircChannelName;

                bool successful = Connect();

                return successful;
            }
            catch
            {
                return false;
            }
        }

        public bool IsConnected()
        {
            return isConnected;
        }
        private bool Connect()
        {
            client = new TcpClient("irc.twitch.tv", 6667);

            //Get stream from tcpclient
            readerBuffer = new StreamReader(client.GetStream());
            writeBuffer = new StreamWriter(client.GetStream());

            writeBuffer.WriteLine("PASS " + oAuth + Environment.NewLine +
                                    "NICK " + username + Environment.NewLine);
            writeBuffer.WriteLine("JOIN " + "#" + channelName);
            writeBuffer.Flush();


            isThreadActive = true;
            ircChatThread = new Thread(ProcessIRCChat);
            ircChatThread.Start();

            if (client.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                client.Close();
                isThreadActive = false;
                isConnected = false;
                messageQueue.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckIfMessageIsInQueue()
        {
            if (messageQueue != null)
            {
                if (messageQueue.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string GetMessageFromQueue()
        {
            if (messageQueue != null)
            {
                return messageQueue.Dequeue();
            }
            else
            {
                return "No String Available";
            }
        }

        private bool CheckIfMessageIsValid(string message)
        {
            if (message == "Improperly formatted auth")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Need to add a check to see if the channel you are connecting to exists.

        private void ProcessIRCChat()
        {
            while (isThreadActive)
            {
                if (client != null)
                {
                    if (!client.Connected)
                    {
                        //Failed to connect
                        isConnected = false;
                        Connect();
                    }
                    else
                    {
                        isConnected = true;

                        //If tcp connection is active, and 
                        if (client.Available > 0 || readerBuffer.Peek() >= 0)
                        {
                            string message = readerBuffer.ReadLine();

                            //Response from server always has points of interest separated by a :
                            var messageIndex = message.IndexOf(":", 1);

                            bool isMessageValid = CheckIfMessageIsValid(message);

                            if (isMessageValid)
                            {
                                messageQueue.Enqueue(message.Substring(messageIndex + 1));
                                Debug.WriteLine(message.Substring(messageIndex + 1));
                            }
                            else
                            {
                                Disconnect();
                                Debug.WriteLine(message.Substring(messageIndex + 1));
                            }
                        }
                    }
                }

                Thread.Sleep(20);
            }
        }
    }
}
