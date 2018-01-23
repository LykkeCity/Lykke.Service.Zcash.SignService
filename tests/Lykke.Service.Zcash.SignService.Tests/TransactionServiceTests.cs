using System.Linq;
using Lykke.Service.Zcash.SignService.Services;
using NBitcoin;
using NBitcoin.JsonConverters;
using NBitcoin.Policy;
using NBitcoin.Zcash;
using Newtonsoft.Json;
using Xunit;

namespace Lykke.Service.Zcash.SignService.Tests
{
    public class TransactionServiceTests
    {
        public Network network = ZcashNetworks.Testnet;
        public string from = "tmBh9ifoTp5keLnCcVnpLzkuMiTLMoPaYdR";
        public string fromPrivateKey = "cQV87Ee69NavGbV7ZG1KNAQ745X7nrPCnVPg8mECnvQmuJwZfq3d";
        public BitcoinAddress fromAddress;
        public Key fromKey;
        public string to = "tmA4rvdJU3HZ4ZUzZSjEUg7wbf1unbDBvGb";
        public BitcoinAddress toAddress;
        public Transaction prevTx = Transaction.Parse("010000000149d203b476351c318c1ed2862f48f8534e62177ebbf4f73908e1bd52f584b2e9010000006b48304502210087c1e03e8343d15be894943aa4046f1d5b23fdda30475929d6a99b653505ae7f0220295a65254f3e5abc23df905de0314c9f202f9a24b4d4fafc444f83f7eeb04fb30121033395bb0aefcbaf06ac52fbb98b8fb087ddf5d8a6b78f7f29c2b25b990ff1eaaaffffffff0200e1f505000000001976a91415b6246e9b88867cdc3e14b9a5085813ca6d8b4888acf09aeb0b000000001976a9147176f5d11e11c59a1248ef0bf0d6dadb2be1686188ac00000000");
        public TransactionBuilder txBuilder = new TransactionBuilder();
        public Transaction tx;
        public ICoin[] spentCoins;
        public TransactionService service;

        static TransactionServiceTests()
        {
            ZcashNetworks.Register();
        }

        public TransactionServiceTests()
        {
            fromAddress = new BitcoinPubKeyAddress(from);
            fromKey = Key.Parse(fromPrivateKey);
            toAddress = new BitcoinPubKeyAddress(to);
            tx = txBuilder
                .AddCoins(prevTx.Outputs.AsCoins().Where(c => c.ScriptPubKey.GetDestinationAddress(network).ToString() == from).ToArray())
                .Send(toAddress, Money.Coins(1))
                .SetChange(fromAddress)
                .SubtractFees()
                .SendFees(txBuilder.EstimateFees(new FeeRate(Money.Satoshis(1024))))
                .BuildTransaction(false);
            spentCoins = txBuilder.FindSpentCoins(tx);
            service = new TransactionService();
        }

        [Fact]
        public void ShouldSignTransaction()
        {
            // Arrange
            
            // Act
            var signedTransactionHex = service.Sign(tx, spentCoins, new[] { fromKey });
            var signedTx = Transaction.Parse(signedTransactionHex);

            // Assert
            Assert.True(new TransactionBuilder()
                .AddCoins(spentCoins)
                .SetTransactionPolicy(new StandardTransactionPolicy { CheckFee = false })
                .Verify(signedTx, out var errors));
        }
    }
}
