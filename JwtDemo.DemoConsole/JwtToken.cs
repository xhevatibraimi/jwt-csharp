namespace JwtDemo.ConsoleDemo
{
    public class JwtToken
    {
        public object Header { get; set; }
        public object Payload { get; set; }
        public string Signature { get; set; }
    }
}
