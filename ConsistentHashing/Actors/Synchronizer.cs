using System.Collections.Generic;

namespace ConsistentHashing.Actors
{
    internal class Interval
    {
        public int Start { get; set; }

        public int End { get; set; }

        public Server Server { get; set; }

        public Interval Split(int startingFrom, Server newServer)
        {
            var newInterval = new Interval
            {
                Start = startingFrom,
                End = End,
                Server = newServer
            };

            End = newInterval.Start;
            return newInterval;
        }

        public void Expand(int newEnd)
        {
            End = newEnd;
        }
    }

    internal class Synchronizer
    {
        private readonly int _virtualNodeMultiplier;
        private readonly int _precision = 3600;

        private readonly List<Interval> _map = new List<Interval>();

        //private readonly ServerManager _serverManager = new ServerManager();

        public Synchronizer(int virtualNodeMultiplier)
        {
            _virtualNodeMultiplier = virtualNodeMultiplier;
        }

        public void Store(string key, string value)
        {
            var server = GetServerForKey(key);
            server.SetValue(key, value);
        }

        public string Get(string key) 
        {
            var server = GetServerForKey(key);
            return server.GetValue(key);
        }

        public void AddServer(string name)
        {
            var firstKey = GetPosition(name);
            var interval = GetIntervalForPosition(firstKey);

            _map.Add(interval.Split(firstKey, new Server(name)));
        }

        public void RemoveServer(string name)
        {
            var firstKey = GetPosition(name);
            var interval = GetIntervalForPosition(firstKey);
        }

        private Server GetServerForKey(string key)
        {
            var interval = GetIntervalForKey(key);
            return interval.Server;
        }

        private Interval GetIntervalForKey(string key)
        {
            return GetIntervalForPosition(GetPosition(key));
        }

        private Interval GetIntervalForPosition(int position)
        {
            return _map.First(x => x.Start <= position && x.End >= position);
        }

        private int GetPosition(string key)
        {
            return CreateMD5(key) % _precision;
        }

        private static int CreateMD5(string input)
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);

            return BitConverter.ToInt32(hashBytes, 0);
        }
    }

}
