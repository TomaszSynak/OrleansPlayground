namespace Grains
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [Serializable]
    public class HelloWorldState
    {
        [SuppressMessage("Usage", "CA2235:Mark all non-serializable fields", Justification = "False negative - it's serializable property")]
        public string Greetings { get; set; }
    }
}
