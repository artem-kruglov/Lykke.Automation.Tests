﻿using ApiV2Data.Fixtures;
using RestSharp;
using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTestCommon;
using ApiV2Data.DTOs;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.ApiV2;
using FluentAssertions;
using System.Linq;
using XUnitTestData.Repositories;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetAllWallets()
        {
            string url = _fixture.ApiEndpointNames["Wallets"];
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletDTO>>(response.ResponseJson);

            foreach (WalletEntity entity in _fixture.AllWalletsFromDb)
            {
                entity.ShouldBeEquivalentTo(parsedResponse.Where(w => w.Id == entity.Id).FirstOrDefault(), o => o
                .ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetSingleWallets()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/" + _fixture.TestWallet.Id;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WalletDTO parsedResponse = JsonUtils.DeserializeJson<WalletDTO>(response.ResponseJson);

            _fixture.TestWallet.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetAllWalletBalances()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/balances";
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletBalanceDTO>>(response.ResponseJson);

            foreach (WalletBalanceDTO wbDTO in parsedResponse)
            {
                WalletEntity walletEntity = _fixture.AllWalletsFromDb.Where(w => w.Id == wbDTO.Id).FirstOrDefault();
                if (walletEntity != null)
                {
                    Assert.True(walletEntity.Name == wbDTO.Name);
                    Assert.True(walletEntity.Type == wbDTO.Type);
                    Assert.True(walletEntity.Description == wbDTO.Description);
                }


                AccountEntity accountEntity = await _fixture.AccountManager.TryGetAsync(wbDTO.Id) as AccountEntity;

                if (accountEntity != null)
                {
                    foreach (BalanceDTO balanceDTO in accountEntity.BalancesParsed)
                    {
                        var parsedBalance = wbDTO.Balances.Where(b => b.AssetId == balanceDTO.Asset).FirstOrDefault();
                        Assert.NotNull(parsedBalance);
                        Assert.True(balanceDTO.Balance == parsedBalance.Balance);
                    }
                }
                else
                {
                    Assert.True(wbDTO.Balances.Count == 0);
                }
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetBalancesByWalletId()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/" + _fixture.TestWallet.Id + "/balances";
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WBalanceDTO>>(response.ResponseJson);

            foreach (BalanceDTO balanceDTO in _fixture.TestWalletAccount.BalancesParsed)
            {
                var parsedBalance = parsedResponse.Where(b => b.AssetId == balanceDTO.Asset).FirstOrDefault();
                Assert.NotNull(parsedBalance);
                Assert.True(balanceDTO.Balance == parsedBalance.Balance);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetWalletBalancesByAssetId()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/balances/" + _fixture.TestWalletAssetId;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WalletSingleBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WalletSingleBalanceDTO>>(response.ResponseJson);

            foreach (WalletSingleBalanceDTO wbDTO in parsedResponse)
            {
                WalletEntity walletEntity = _fixture.AllWalletsFromDb.Where(w => w.Id == wbDTO.Id).FirstOrDefault();
                if (walletEntity != null)
                {
                    Assert.True(walletEntity.Name == wbDTO.Name);
                    Assert.True(walletEntity.Type == wbDTO.Type);
                    Assert.True(walletEntity.Description == wbDTO.Description);
                }


                AccountEntity accountEntity = await _fixture.AccountManager.TryGetAsync(wbDTO.Id) as AccountEntity;

                if (accountEntity != null)
                {
                    BalanceDTO balanceDTO = accountEntity.BalancesParsed.Where(b => b.Asset == _fixture.TestWalletAssetId).FirstOrDefault();
                    Assert.True(balanceDTO.Balance == wbDTO.Balances.Balance);
                }
                else
                {
                    Assert.Null(wbDTO.Balances);
                }
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetWalletBalanceByWalletAndAssetId()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/" + _fixture.TestWallet.Id + "/balances/" + _fixture.TestWalletAssetId;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WBalanceDTO parsedResponse = JsonUtils.DeserializeJson<WBalanceDTO>(response.ResponseJson);

            BalanceDTO accountBalance = _fixture.TestWalletAccount.BalancesParsed.Where(b => b.Asset == _fixture.TestWalletAssetId).FirstOrDefault();

            Assert.NotNull(accountBalance);
            Assert.True(parsedResponse.Balance == accountBalance.Balance);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetWalletTradeBalances()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/trading/balances";
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            List<WBalanceDTO> parsedResponse = JsonUtils.DeserializeJson<List<WBalanceDTO>>(response.ResponseJson);

            AccountEntity entity = await _fixture.AccountManager.TryGetAsync(_fixture.TestClientId) as AccountEntity;
            Assert.NotNull(entity);

            foreach (BalanceDTO entityBalance in entity.BalancesParsed)
            {
                WBalanceDTO responseBalance = parsedResponse.Where(b => b.AssetId == entityBalance.Asset).FirstOrDefault();
                Assert.NotNull(responseBalance);
                Assert.True(entityBalance.Balance == responseBalance.Balance);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsGet")]
        public async void GetWalletTradeBalanceByAssetId()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/trading/balances/" + _fixture.TestWalletAssetId;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            WBalanceDTO parsedResponse = JsonUtils.DeserializeJson<WBalanceDTO>(response.ResponseJson);

            AccountEntity entity = await _fixture.AccountManager.TryGetAsync(_fixture.TestClientId) as AccountEntity;
            Assert.NotNull(entity);
            BalanceDTO entityBalance = entity.BalancesParsed.Where(b => b.Asset == _fixture.TestWalletAssetId).FirstOrDefault();
            Assert.NotNull(entityBalance);

            Assert.True(entityBalance.Balance == parsedResponse.Balance);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsPost")]
        public async void CreateWallet()
        {
            WalletDTO createdWallet = await _fixture.CreateTestWallet();
            Assert.NotNull(createdWallet);

            WalletEntity entity = await _fixture.WalletRepository.TryGetAsync(createdWallet.Id) as WalletEntity;
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(createdWallet, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsPost")]
        public async void CreateHFTWallet()
        {
            WalletDTO createdWallet = await _fixture.CreateTestWallet(true);
            Assert.NotNull(createdWallet);

            WalletEntity entity = await _fixture.WalletRepository.TryGetAsync(createdWallet.Id) as WalletEntity;
            Assert.NotNull(entity);

            entity.ShouldBeEquivalentTo(createdWallet, o => o
            .ExcludingMissingMembers()
            .Excluding(w => w.Type));
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Wallets")]
        [Trait("Category", "WalletsDelete")]
        public async void DeleteWallet()
        {
            string url = _fixture.ApiEndpointNames["Wallets"] + "/" + _fixture.TestWalletDelete.Id;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);

            Assert.True(response.Status == HttpStatusCode.OK);

            WalletEntity entity = await _fixture.WalletRepository.TryGetAsync(_fixture.TestWalletDelete.Id) as WalletEntity;
            Assert.True(entity.State == "deleted");
        }
    }
}
