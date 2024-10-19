using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace xxx
{
    public delegate void OnConnectionHandler();

    abstract class OnlineGame
    {
        protected BinaryReader reader;
        protected BinaryWriter writer;

        protected Thread thread;

        protected TcpClient client;

        protected int port;

        public Animal hostChar, joinChar;

        public event OnConnectionHandler OnConnection;

        /// <summary>
        /// 
        /// </summary>
        protected void RaiseOnConnectionEvent()
        {
            if (OnConnection != null)
            {
                OnConnection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            InitChars();
            StartCommunication();
        }

        protected abstract void InitChars();

        /// <summary>
        /// 
        /// </summary>
        public void StartCommunication()
        {
            thread = new Thread(new ThreadStart(SocketThread));
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        protected void ReadAndUpdateCharacter(Animal player)
        {
            Vector2 temp=new Vector2();
            SpriteEffects effect;
            States state;
            Console.WriteLine("starting reading!!!");
            Console.WriteLine("reading X position");
            temp.X = reader.ReadSingle();
            Console.WriteLine("X:" + temp.X);
            Console.WriteLine("reading Y position");
            Console.WriteLine("Y: "+ temp.Y);
            temp.Y = reader.ReadSingle();
            Console.WriteLine("reading Rot");
            player.Rot = reader.ReadSingle();
            Console.WriteLine("reading effect");
            effect = (SpriteEffects)reader.ReadInt32();
            Console.WriteLine("reading state");
            state = (States)reader.ReadInt32();

            player.Pos = temp;
            player.effects = effect;
            player.state = state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        protected void WriteCharacterData(Animal player)
        {
            Console.WriteLine("writing X pos");
            writer.Write(player.Pos.X);
            Console.WriteLine("writing Y pos");
            writer.Write(player.Pos.Y);
            Console.WriteLine("writing rot");
            writer.Write(player.Rot);
            Console.WriteLine("writing effects");
            writer.Write((int)player.effects);
            Console.WriteLine("writing state");
            writer.Write((int)player.state);
        }

        protected abstract void SocketThread();
    }
}