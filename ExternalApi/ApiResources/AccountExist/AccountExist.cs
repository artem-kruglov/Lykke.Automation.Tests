﻿using LykkeAutomation.Api.ApiModels.AccountExistModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lykke.Client.AutorestClient.Models;
using TestsCore.TestsCore;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomation.Api.ApiResources.AccountExist
{
    public class AccountExist : LykkeExternalApi
    {
        private string resource = "/AccountExist";

        public IResponse<AccountExistModel> GetAccountExistResponse(string email)
        {
            return Request.Get(resource).AddQueryParameter("email", email).Build().Execute<AccountExistModel>();
        }
    }
}
