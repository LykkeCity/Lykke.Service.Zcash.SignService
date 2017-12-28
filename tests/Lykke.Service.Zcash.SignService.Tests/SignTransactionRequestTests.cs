﻿using System;
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
        [Fact]
        public void ShouldSerializeDeserializeData()
        {
            // Arrange
            var txSvcTests = new TransactionServiceTests();
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { txSvcTests.fromPrivateKey },
                TransactionHex = Serializer.ToString((txSvcTests.tx, txSvcTests.spentCoins))
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
                TransactionHex = Serializer.ToString(((Transaction)null, txSvcTests.spentCoins))
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.TransactionHex), validationResult.First().MemberNames);
        }

        [Fact]
        public void ShouldNotValidate_IfCoinsArrayIsNull()
        {
            // Arrange
            var txSvcTests = new TransactionServiceTests();
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { txSvcTests.fromPrivateKey },
                TransactionHex = Serializer.ToString((txSvcTests.tx, (ICoin[])null))
            });

            // Act
            var request = JsonConvert.DeserializeObject<SignTransactionRequest>(body);
            var validationResult = request.Validate(new ValidationContext(request));

            // Assert
            Assert.NotEmpty(validationResult);
            Assert.Contains(nameof(SignTransactionRequest.TransactionHex), validationResult.First().MemberNames);
        }

        [Fact]
        public void ShouldNotValidate_IfKeyIsInvalid()
        {
            // Arrange
            var txSvcTests = new TransactionServiceTests();
            var body = JsonConvert.SerializeObject(new
            {
                PrivateKeys = new[] { "invalid" },
                TransactionHex = Serializer.ToString((txSvcTests.tx, txSvcTests.spentCoins))
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
