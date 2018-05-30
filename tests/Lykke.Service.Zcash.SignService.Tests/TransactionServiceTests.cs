using System.Linq;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Zcash.SignService.Core.Domain.Transactions;
using Lykke.Service.Zcash.SignService.Services;
using Moq;
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
        public ILog _log = new LogToMemory();
        public Mock<IBlockchainReader> _blockchainReader = new Mock<IBlockchainReader>();
        public string _tx = "030000807082c40301ac3e4e9435a8369e049b47906ddaa09601cd7e7cfe2f229e0bd305202a066f8e0100000000ffffffff0280969800000000001976a91415b6246e9b88867cdc3e14b9a5085813ca6d8b4888acc9180202000000001976a9144faeeb51bcd0b49f238b323e5f1c6c8bf11ae02a88ac000000005782030000";
        public string[] _privateKeys = new[] { "cTD2Ew71UHXkn2XTJLyfu6Rbo1os5zCF9sKZm4oiXshcYo6YPcKY" };
        public Utxo[] _outputs = new[]
        {
            new Utxo { TxId = "8e6f062a2005d30b9e222ffe7c7ecd0196a0da6d90479b049e36a835944e3eac", Vout = 1, Amount = 0.43695697m, ScriptPubKey = "76a9144faeeb51bcd0b49f238b323e5f1c6c8bf11ae02a88ac" }
        };
        public string _txSigned = "030000807082c40301ac3e4e9435a8369e049b47906ddaa09601cd7e7cfe2f229e0bd305202a066f8e010000006b483045022100ee41236b2550aa334948e1b750b8f9dd12f30ea15b879fe0bf7f9a86989bef230220336f17006636afc4165bef0a5fc1e79a1204f21ec1f1ad71c3c326e4091bc7fa012103f9e72f0713a4d4a980309a14a2ba563e0b1125ad067818e77553a1eefbfc5be7ffffffff0280969800000000001976a91415b6246e9b88867cdc3e14b9a5085813ca6d8b4888acc9180202000000001976a9144faeeb51bcd0b49f238b323e5f1c6c8bf11ae02a88ac000000005782030000";

        static TransactionServiceTests()
        {
            ZcashNetworks.Instance.EnsureRegistered();
        }

        public TransactionServiceTests()
        {
            _blockchainReader.Setup(m => m.SignRawTransaction(It.IsAny<string>(), It.IsAny<Utxo[]>(), It.IsAny<string[]>()))
                .Returns((string tx, Utxo[] outputs, string[] keys) => Task.FromResult(new SignResult { Complete = true, Hex = string.Empty }));
        }

        [Fact]
        public async Task ShouldSignLocally()
        {
            // Arrange
            var transactionService = new TransactionService(_log);

            // Act
            var signed = await transactionService.SignAsync(_tx, _outputs, _privateKeys);

            // Assert
            Assert.Equal(_txSigned, signed);
        }

        [Fact]
        public async Task ShouldSignRemotely()
        {
            // Arrange
            var transactionService = new TransactionService(_log, _blockchainReader.Object);

            // Act
            var signed = await transactionService.SignAsync(_tx, _outputs, _privateKeys);

            // Assert
            _blockchainReader.Verify(m => m.SignRawTransaction(It.IsAny<string>(), It.IsAny<Utxo[]>(), It.IsAny<string[]>()), Times.Once);
        }

        [Fact]
        public async Task ShouldValidateLocally()
        {
            // Arrange
            var transactionService = new TransactionService(_log);

            // Act
            var isValid = await transactionService.ValidateNotSignedTransactionAsync(_tx);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public async Task ShouldValidateRemotely()
        {
            // Arrange
            var transactionService = new TransactionService(_log, _blockchainReader.Object);

            // Act
            var signed = await transactionService.ValidateNotSignedTransactionAsync(_tx);

            // Assert
            _blockchainReader.Verify(m => m.DecodeRawTransaction(It.IsAny<string>()), Times.Once);
        }
    }
}
