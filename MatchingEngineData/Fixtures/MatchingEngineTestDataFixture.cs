﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs.RabbitMQ;
using Autofac;
using XUnitTestCommon.Utils;
using MatchingEngineData.DependencyInjection;
using XUnitTestData.Services;
using XUnitTestData.Domains;
using XUnitTestData.Repositories;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MatchingEngineData.DTOs.RabbitMQ;
using XUnitTestData.Repositories.MatchingEngine;
using XUnitTestData.Domains.MatchingEngine;

namespace AFTMatchingEngine.Fixtures
{
    public class MatchingEngineTestDataFixture : IDisposable
    {
        public MatchingEngineConsumer Consumer;

        //public IDictionaryManager<IAccount> AccountManager;
        public AccountRepository AccountRepository;
        public CashSwapRepository CashSwapRepository;

        public string TestAccountId1;
        public string TestAccountId2;
        public string TestAsset1;
        public string TestAsset2;

        private Dictionary<Type, List<IRabbitMQOperation>> RabbitMqMessages;

        private RabbitMQConsumer<CashOperation> CashInOutSubscription;
        private RabbitMQConsumer<CashTransferOperation> CashTransferSubscription;
        private RabbitMQConsumer<CashSwapOperation> CashSwapSubscription;

        private ConfigBuilder _configBuilder;

        private List<string> _createdQueues;
        private IContainer container;

        public MatchingEngineTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("MatchingEngine");
            prepareConsumer();
            prepareRabbitQueues();
            prepareRabbitMQConnections();
            prepareDependencyContainer();

            prepareTestData();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MatchingEngineTestModule(_configBuilder));
            this.container = builder.Build();

            //this.AccountManager = RepositoryUtils.PrepareRepositoryManager<IAccount>(this.container);
            this.AccountRepository = (AccountRepository)this.container.Resolve<IDictionaryRepository<IAccount>>();
            this.CashSwapRepository = (CashSwapRepository)this.container.Resolve<IDictionaryRepository<ICashSwap>>();
        }

        private void prepareRabbitMQConnections()
        {
            RabbitMqMessages = new Dictionary<Type, List<IRabbitMQOperation>>();

            CashInOutSubscription = new RabbitMQConsumer<CashOperation>(
                _configBuilder, "cashinout", "automation_functional_tests");

            CashInOutSubscription.SubscribeMessageHandler(handleOperationMessages);
            CashInOutSubscription.Start();

            CashTransferSubscription = new RabbitMQConsumer<CashTransferOperation>(
                _configBuilder, "transfers", "automation_functional_tests");
            CashTransferSubscription.SubscribeMessageHandler(handleOperationMessages);
            CashTransferSubscription.Start();

            CashSwapSubscription = new RabbitMQConsumer<CashSwapOperation>(
                _configBuilder, "cashswap", "automation_functional_tests");
            CashSwapSubscription.SubscribeMessageHandler(handleOperationMessages);
            CashSwapSubscription.Start();
        }

        private void prepareConsumer()
        {
            if (Int32.TryParse(_configBuilder.Config["Port"], out int port))
            {
                Consumer = new MatchingEngineConsumer(_configBuilder.Config["BaseUrl"], port);
            }
            else
            {
                throw new FormatException();
            }
        }

        private void prepareRabbitQueues()
        {
            _createdQueues = new List<string>();
            RabbitMQHttpApiConsumer.Setup(_configBuilder);

            List<Task<bool>> createQueueTasks = new List<Task<bool>>();
            createQueueTasks.Add(createQueue("lykke.cashinout", "lykke.cashinout.automation_functional_tests"));
            createQueueTasks.Add(createQueue("lykke.transfers", "lykke.transfers.automation_functional_tests"));
            createQueueTasks.Add(createQueue("lykke.cashswap", "lykke.cashswap.automation_functional_tests"));

            Task.WhenAll(createQueueTasks).Wait();
        }

        private async Task<bool> createQueue(string exchangeName, string queueName)
        {
            RabbitMQHttpApiQueueResultDTO queueModel = Task.Run(async () =>
            {
                return await RabbitMQHttpApiConsumer.GetQueueByNameAsync(queueName);
            }).Result;

            if (queueModel != null)
            {
                await RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName);
            }

            bool IsBinded = false;

            bool IsCreated = await RabbitMQHttpApiConsumer.CreateQueueAsync(queueName);
            if (IsCreated)
            {
                IsBinded = await RabbitMQHttpApiConsumer.BindQueueAsync(exchangeName, queueName);
            }

            if (IsBinded)
            {
                _createdQueues.Add(queueName);
            }

            return IsBinded;
        }

        private void prepareTestData()
        {
            TestAccountId1 = "AFTest_Client1";
            TestAccountId2 = "AFTest_Client2";
            TestAsset1 = "LKK";
            TestAsset2 = "USD";
        }

        public Task<IRabbitMQOperation> WaitForRabbitMQ<T>(string transactionId)
        {
            Stopwatch stopWatch = new Stopwatch();
            IRabbitMQOperation message = default(IRabbitMQOperation);

            stopWatch.Start();
            while (message == default(IRabbitMQOperation) && stopWatch.Elapsed.TotalMilliseconds < 10000)
            {
                if (RabbitMqMessages.ContainsKey(typeof(T)))
                {
                    message = RabbitMqMessages[typeof(T)].Where(m => m.id == transactionId).FirstOrDefault();
                }
                if (message == default(IRabbitMQOperation))
                {
                    Thread.Sleep(100);
                }
            }
            stopWatch.Stop();

            return Task.FromResult(message);
        }

        private Task handleOperationMessages(IRabbitMQOperation msg)
        {
            if (!RabbitMqMessages.ContainsKey(msg.GetType()))
            {
                RabbitMqMessages[msg.GetType()] = new List<IRabbitMQOperation>();
            }

            RabbitMqMessages[msg.GetType()].Add(msg);

            return Task.FromResult(msg);
        }

        public void Dispose()
        {
            CashInOutSubscription.Stop();

            List<Task<bool>> deleteTasks = new List<Task<bool>>();

            foreach (string queueName in _createdQueues)
            {
                deleteTasks.Add(RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName));
            }

            Task.WhenAll(deleteTasks).Wait();
            
        }
    }
}
