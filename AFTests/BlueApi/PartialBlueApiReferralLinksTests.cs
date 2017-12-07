﻿using System;
using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using BlueApiData.DTOs.ReferralLinks;
using XUnitTestData.Entities.BlueApi;
using XUnitTestCommon.Consumers;
using BlueApiData;
using System.Web;
using Lykke.Service.Balances.AutorestClient.Models;
using Lykke.Service.Balances.Client.ResponseModels;
using System.Collections.Generic;
using System.Linq;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests
    {
        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("RequestInvitationLink")]
        public async Task RequestInvitationLink()
        {
            await this.PrepareRequestInvitationLink();

            var url = ApiPaths.REFERRAL_LINKS_REQUEST_INVITATION_LINK_PATH;
            var response = await this.InvitationLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.Created);

            var parsedResponse = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(response.ResponseJson);
            var refLinkId = Guid.Empty;

            Assert.True(!String.IsNullOrEmpty(parsedResponse.RefLinkId));
            Assert.True(Guid.TryParse(parsedResponse.RefLinkId, out refLinkId));
            Assert.True(refLinkId != Guid.Empty);

            ReferralLinkEntity entity = await this.ReferralLinkRepository.TryGetAsync(parsedResponse.RefLinkId) as ReferralLinkEntity;
            Assert.True(entity.Type == "Invitation");
            Assert.True(entity.State == "Created");
            Assert.True(entity.Asset == Constants.TREE_COIN_NAME);
            Assert.True(entity.Url == parsedResponse.RefLinkUrl);

            //Attempt to request second invitation link should fail
            response = await this.InvitationLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status != HttpStatusCode.Created);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ClaimInvitationLink")]
        public async Task ClaimInvitationLink()
        {
            await this.PrepareClainInvitationLink();

            var url = ApiPaths.REFERRAL_LINKS_CLAIM_INVITATION_LINK_PATH;

            //send request without data 
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.POST);
            Assert.True(response.Status == HttpStatusCode.UnsupportedMediaType);

            var body = new InvitationLinkClaimDTO()
            {
                ReferalLinkId = GlobalConstants.AutoTest,
                ReferalLinkUrl = GlobalConstants.AutoTest,
                IsNewClient = false
            };

            //send request with wrong data 
            response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);



            //Create link to be claimed
            var createLinkUrl = ApiPaths.REFERRAL_LINKS_REQUEST_INVITATION_LINK_PATH;
            ApiConsumer createLinkConsumer = this.InvitationLinkClaimersConsumers[0];
            this.InvitationLinkClaimersConsumers.RemoveAt(0);

            var createLinkResponse = await createLinkConsumer.ExecuteRequest(createLinkUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(createLinkResponse.Status == HttpStatusCode.Created);
            var createdLink = JsonUtils.DeserializeJson<RequestInvitationLinkResponseDto>(createLinkResponse.ResponseJson);

            body = new InvitationLinkClaimDTO()
            {
                ReferalLinkId = createdLink.RefLinkId,
                ReferalLinkUrl = createdLink.RefLinkUrl,
                IsNewClient = true
            };
            string claimParam = JsonUtils.SerializeObject(body);

            for (int i = 0; i < this.InvitationLinkClaimersConsumers.Count; i++)
            {
                ApiConsumer claimConsumer = this.InvitationLinkClaimersConsumers[i];
                var claimResponse = await claimConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, claimParam, Method.POST);
                InvitationLinkClaimResponseDTO parsedClaimResponse = JsonUtils.DeserializeJson<InvitationLinkClaimResponseDTO>(claimResponse.ResponseJson);

                List<ClientBalanceResponseModel> senderBalances = (await this.BalancesClient.GetClientBalances(createLinkConsumer.ClientInfo.Account.Id)).ToList();
                ClientBalanceResponseModel senderBalance = senderBalances.Where(b => b.AssetId == Constants.TREE_COIN_ID).FirstOrDefault();

                List<ClientBalanceResponseModel> recieverBalances = (await this.BalancesClient.GetClientBalances(claimConsumer.ClientInfo.Account.Id)).ToList();
                ClientBalanceResponseModel recieverBalance = recieverBalances.Where(b => b.AssetId == Constants.TREE_COIN_ID).FirstOrDefault();

                //assert first five claimers should claim successfully and recieve reward
                if (i < 5)
                {
                    Assert.True(Guid.TryParse(parsedClaimResponse.TransactionRewardSender, out Guid temp1));
                    Assert.True(Guid.TryParse(parsedClaimResponse.TransactionRewardRecipient, out Guid temp2));

                    Assert.True(senderBalance.Balance == (i + 1) * Constants.TREE_COIN_INVIRATION_AWARD);
                    Assert.True(recieverBalance.Balance == Constants.TREE_COIN_INVIRATION_AWARD);
                }
                else
                {
                    Assert.Null(parsedClaimResponse.TransactionRewardSender);
                    Assert.Null(parsedClaimResponse.TransactionRewardRecipient);

                    Assert.True(senderBalance.Balance == 5 * Constants.TREE_COIN_INVIRATION_AWARD);
                    Assert.Null(recieverBalance);
                }

                //attempt to claim again with single user should result in error
                if (i == 0)
                {
                    var secondClaimResponse = await claimConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, claimParam, Method.POST);
                    Assert.True(secondClaimResponse.Status != HttpStatusCode.OK);
                }
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksGet")]
        public async Task GetRefLinkDataById()
        {
            string url = ApiPaths.REFERRAL_LINKS_BASE_PATH + "/id/" + this.TestInvitationLink.RefLinkId;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            RefferalLinkDataDTO parsedResponse = JsonUtils.DeserializeJson<RefferalLinkDataDTO>(response.ResponseJson);

            Assert.True(parsedResponse.Id == this.TestInvitationLink.RefLinkId);
            Assert.True(parsedResponse.Url == this.TestInvitationLink.RefLinkUrl);
            Assert.True(parsedResponse.SenderClientId == this.GlobalConsumer.ClientInfo.Account.Id);
            Assert.True(parsedResponse.Asset == Constants.TREE_COIN_NAME);
            Assert.True(parsedResponse.State == "Created");
            Assert.True(parsedResponse.Amount == 1.0);
            Assert.True(parsedResponse.Type == "Invitation");
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksGet")]
        public async Task GetRefLinkDataByUrl()
        {
            string url = ApiPaths.REFERRAL_LINKS_BASE_PATH + "/url/" + HttpUtility.UrlEncode(this.TestInvitationLink.RefLinkUrl);
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            RefferalLinkDataDTO parsedResponse = JsonUtils.DeserializeJson<RefferalLinkDataDTO>(response.ResponseJson);

            Assert.True(parsedResponse.Id == this.TestInvitationLink.RefLinkId);
            Assert.True(parsedResponse.Url == this.TestInvitationLink.RefLinkUrl);
            Assert.True(parsedResponse.SenderClientId == this.GlobalConsumer.ClientInfo.Account.Id);
            Assert.True(parsedResponse.Asset == Constants.TREE_COIN_NAME);
            Assert.True(parsedResponse.State == "Created");
            Assert.True(parsedResponse.Amount == 1.0);
            Assert.True(parsedResponse.Type == "Invitation");
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksGet")]
        public async Task GetRefLinkStatistics()
        {
            string url = ApiPaths.REFERRAL_LINKS_BASE_PATH + "/statistics";
            var response = await this.GlobalConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            RefLinksStatisticsDTO parsedResponse = JsonUtils.DeserializeJson<RefLinksStatisticsDTO>(response.ResponseJson);

            Assert.True(parsedResponse.NumberOfInvitationLinksSent == 1);
            Assert.True(parsedResponse.NumberOfInvitationLinksAccepted == 2);
        }

        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksPost")]
        public async Task RequestGiftCoinLink()
        {
            await PrepareRequestGiftCoinLink();

            string url = ApiPaths.REFERRAL_LINKS_REQUEST_GIFTCOINS_LINK_PATH;

            //Attempt to create giftcoin link with negative ammount
            RequestGiftCoinsLinkRequestDto requestParam = new RequestGiftCoinsLinkRequestDto()
            {
                Asset = Constants.GIFT_COIN_ASSET_NAME,
                Amount = -0.1

            };
            var response = await GiftCoinLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(requestParam), Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);

            //Attempt to create giftcoin link with more money than the user has
            requestParam = new RequestGiftCoinsLinkRequestDto()
            {
                Asset = Constants.GIFT_COIN_ASSET_NAME,
                Amount = Constants.GIFT_COIN_REQUEST_INITIAL_BALANCE + 1.0

            };
            response = await GiftCoinLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(requestParam), Method.POST);
            Assert.True(response.Status == HttpStatusCode.BadRequest);

            //Attempt a proper request
            requestParam = new RequestGiftCoinsLinkRequestDto()
            {
                Asset = Constants.GIFT_COIN_ASSET_NAME,
                Amount = 1.0

            };
            response = await GiftCoinLinkRequestConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(requestParam), Method.POST);
            Assert.True(response.Status == HttpStatusCode.Created);

            RequestGiftCoinsLinkResponseDto parsedResponse = JsonUtils.DeserializeJson<RequestGiftCoinsLinkResponseDto>(response.ResponseJson);

            Assert.True(Guid.TryParse(parsedResponse.RefLinkId, out var temp));
            Assert.True(parsedResponse.RefLinkUrl.StartsWith("http"));
        }

        [Ignore("test will fail")]
        [Test]
        [Category("Smoke")]
        [Category("ReferralLinks")]
        [Category("ReferralLinksPost")]
        public async Task ClaimGiftCoinLink()
        {
            await PrepareClaimGiftCoinLink();
            string url = ApiPaths.REFERRAL_LINKS_CLAIM_GIFTCOINS_LINK_PATH;

            ClaimGiftCoinLinkRequest body = new ClaimGiftCoinLinkRequest()
            {
                ReferalLinkId = this.TestGiftCoinLink.RefLinkId,
                ReferalLinkUrl = this.TestGiftCoinLink.RefLinkUrl
            };

            var response = await this.GiftCoinLinkClaimConsumers[1].ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);

            //TODO check parsedResponse

            List<ClientBalanceResponseModel> claimedClientBalances =  (await this.BalancesClient.GetClientBalances(this.GiftCoinLinkClaimConsumers[1].ClientInfo.Account.Id)).ToList();
            ClientBalanceResponseModel claimedBalance = claimedClientBalances.Where(b => b.AssetId == Constants.GIFT_COIN_ASSET_ID).FirstOrDefault();
            Assert.NotNull(claimedBalance);

            Assert.True(claimedBalance.Balance == Constants.GIFT_COIN_AWARD);

            //Attempt another claim for same link should fail
            response = await this.GiftCoinLinkClaimConsumers[2].ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(body), Method.POST);
            Assert.True(response.Status != HttpStatusCode.OK);

            //check sender balance GiftCoinLinkClaimConsumers[0]
            List<ClientBalanceResponseModel> senderBalances = (await this.BalancesClient.GetClientBalances(GiftCoinLinkClaimConsumers[0].ClientInfo.Account.Id)).ToList();
            ClientBalanceResponseModel senderBalance = senderBalances.Where(b => b.AssetId == Constants.GIFT_COIN_ASSET_ID).FirstOrDefault();

            Assert.NotNull(senderBalance);
            Assert.True(senderBalance.Balance == Constants.GIFT_COIN_REQUEST_INITIAL_BALANCE - Constants.GIFT_COIN_AWARD);
        }

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForNonExistingClient")]
        //public async Task RequestGiftCoinsLinkForNonExistingClient()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { SenderClientId = "123" });

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForInvalidAsset")]
        //public async Task RequestGiftCoinsLinkForInvalidAsset()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123" });

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForInvalidAmount")]
        //public async Task RequestGiftCoinsLinkForInvalidAmount()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Amount = 123 });

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}

        //[Test]
        //[Category("Smoke")]
        //[Category("ReferralLinks")]
        //[Category("RequestGiftCoinsLinkForInvalidAllParams")]
        //public async Task RequestGiftCoinsLinkForInvalidAllParams()
        //{
        //    var url = ApiPaths.REFERRAL_LINKS_GIFTCOINS_LINK_PATH;
        //    var body = JsonUtils.SerializeObject(new RequestGiftCoinsLinkRequestDto { Asset = "123", Amount = 123, SenderClientId = "123"});

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, body, Method.POST);

        //    Assert.True(response.Status == HttpStatusCode.BadRequest);
        //}
    }
}