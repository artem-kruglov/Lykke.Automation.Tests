﻿using System;
using System.Collections.Generic;
using Autofac;
using System.Text;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Services;
using XUnitTestData.Repositories;
using XUnitTestCommon.Utils;
using AzureStorage.Tables;
using XUnitTestData.Repositories.MatchingEngine;
using XUnitTestData.Domains.MatchingEngine;

namespace MatchingEngineData.DependencyInjection
{
    class MatchingEngineTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public MatchingEngineTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AccountRepository(
                    new AzureTableStorage<AccountEntity>(
                        _configBuilder.Config["BalancesInfoConnectionString"], "Accounts", null)))
                .As<IDictionaryRepository<IAccount>>();

            builder.Register(c => new CashSwapRepository(
                    new AzureTableStorage<CashSwapEntity>(
                        _configBuilder.Config["BalancesInfoConnectionString"], "SwapOperationsCash", null)))
                .As<IDictionaryRepository<ICashSwap>>();

            //RepositoryUtils.RegisterDictionaryManager<IAccount>(builder);

        }
    }
}
