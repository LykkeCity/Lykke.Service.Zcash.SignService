using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Lykke.Service.Zcash.SignService.Models.Sign;
using NBitcoin;
using NBitcoin.JsonConverters;
using Newtonsoft.Json;
using Xunit;

namespace Lykke.Service.Zcash.SignService.Tests
{
    public class SignTransactionRequestTests
    {
        public string SerializeContext(Transaction tx, ICoin[] coins)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(Serializer.ToString((tx, coins))));
        }

        [Fact]
        public void ShouldSerializeDeserializeData()
        {
            // Arrange
            var txSvcTests = new TransactionServiceTests();
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { txSvcTests.fromPrivateKey },
                TransactionContext = SerializeContext(txSvcTests.tx, txSvcTests.spentCoins)
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request));

            // Assert;
            Assert.Empty(validationResult);
        }

        [Fact]
        public void ShouldNotValidate_IfTxIsNull()
        {
            // Arrange
            var txSvcTests = new TransactionServiceTests();
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { txSvcTests.fromPrivateKey },
                TransactionContext = SerializeContext((Transaction)null, txSvcTests.spentCoins)
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.TransactionContext), validationResult.First().MemberNames);
        }

        [Fact]
        public void ShouldNotValidate_IfCoinsArrayIsNull()
        {
            // Arrange
            var txSvcTests = new TransactionServiceTests();
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { txSvcTests.fromPrivateKey },
                TransactionHex = SerializeContext(txSvcTests.tx, (ICoin[])null)
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.TransactionContext), validationResult.First().MemberNames);
        }

        [Fact]
        public void ShouldNotValidate_IfKeyIsInvalid()
        {
            // Arrange
            var txSvcTests = new TransactionServiceTests();
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { "invalid" },
                TransactionContext = SerializeContext(txSvcTests.tx, txSvcTests.spentCoins)
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.PrivateKeys), validationResult.First().MemberNames);
        }
    }
}
