namespace Lykke.Service.Zcash.SignService.Core.Domain.Transactions
{
    public class RawTransaction
    {
        public string TxId { get; set; }
        public Input[] Vin { get; set; }
        public Output[] Vout { get; set; }

        public class Input
        {
            public string TxId { get; set; }
            public uint Vout { get; set; }
            public ScriptSig ScriptSig { get; set; }
        }

        public class Output
        {
            public decimal Value { get; set; }
            public uint N { get; set; }
            public ScriptPubKey ScriptPubKey { get; set; }
        }

        public class ScriptPubKey
        {
            public string[] Addresses { get; set; }
        }

        public class ScriptSig
        {
            public string Asm { get; set; }
            public string Hex { get; set; }
        }
    }
}
