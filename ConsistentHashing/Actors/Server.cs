namespace ConsistentHashing.Actors
{
    internal class Server
    {
        private string _name;
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        public Server(string name)
        {
            _name = name;
        }

        public string GetValue(string key)
        {
            return _values[key];
        }

        public void SetValue(string key, string value)
        {
            _values[key] = value;
        }

        public void RemoveValue(string key)
        {
            _values.Remove(key);
        }
    }
}
