﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AlgoStoreData.Fixtures;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using AlgoStoreData.DTOs;
using XUnitTestData.Entities.AlgoStore;
using System.IO;
using AlgoStoreData.HelpersAlgoStore;

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStore")]
    public partial class AlgoStoreTests : AlgoStoreTestDataFixture
    {

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task UploadMetadataWithEmptyDescription()
        {

            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = Helpers.RandomString(8),
                Description = ""
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status == HttpStatusCode.OK);
            MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);

            DataManager.addSingleMetadata(responceMetaData);

            Assert.AreEqual(metadata.Name, responceMetaData.Name);
            Assert.AreEqual(metadata.Description, responceMetaData.Description);
            Assert.NotNull(responceMetaData.Date);
            Assert.NotNull(responceMetaData.Id);
            Assert.Null(responceMetaData.Status);

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == responceMetaData.Id) as MetaDataEntity;

            Assert.NotNull(metaDataEntity);
            Assert.AreEqual(metaDataEntity.Id, responceMetaData.Id);
            Assert.AreEqual(metaDataEntity.Name, responceMetaData.Name);
            Assert.AreEqual(metaDataEntity.Description, responceMetaData.Description);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task DeleteMetadataOnlyWithIdParam()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = "This Name Is Invalid"
            };

            string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responceCascadeDelete.Status == HttpStatusCode.NoContent);
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task DeleteMetadataWithUpoadAlgoString()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = temporaryResponseDTO.Name
            };

            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            string AlgoId = editMetaData.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId },
                {"Data" , Helpers.RandomString(300) }
            };

            var responceUploadString = await this.Consumer.ExecuteRequest(url, quaryParam, null, Method.POST);
            Assert.That(responceUploadString.Status == HttpStatusCode.NoContent);

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadString.Status == HttpStatusCode.OK);

            UploadStringDTO uploadedStringContent = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadString.ResponseJson);

            Assert.That(quaryParam["Data"].Equals(uploadedStringContent.Data));

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responceCascadeDelete.Status == HttpStatusCode.NoContent);
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task DeleteMetadataWithUpoadAlgoBinary()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = temporaryResponseDTO.Name
            };

            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = editMetaData.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responceCascadeDelete.Status == HttpStatusCode.NoContent);
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task DeleteMetadataWithDeployedAlgo()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = temporaryResponseDTO.Name
            };

            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = editMetaData.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoId
            };

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == AlgoId) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responceCascadeDelete.Status == HttpStatusCode.NoContent);
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task DeleteMetadataWithStoppedAlgo()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = temporaryResponseDTO.Name
            };

            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = editMetaData.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoId
            };

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == AlgoId) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_STOP;

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.That(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.That(stopResponse.Status.Equals("STOPPED"));

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responceCascadeDelete.Status == HttpStatusCode.NoContent);
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task DeleteMetadataWithStartedAlgo()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = temporaryResponseDTO.Name
            };

            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = editMetaData.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoId
            };

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == AlgoId) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responceCascadeDelete.Status == HttpStatusCode.NoContent);
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task UploadBinaryAlgoWithIdThatHasAlreadyUploadedAlgo()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = DataManager.getMetaDataForBinaryUpload().Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            var responceAllClientMetadataSecondTme = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExistsSecondTime = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExistsSecondTime);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task UploadBinaryAlgoWithWrongId()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", "non-existing-id-234-555-666" }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NotFound);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(quaryParam["AlgoId"], BinaryAlgoFileType.JAR);
            Assert.False(blobExists);
        }

        [Test]
        [Category("Functional")]
        [Category("FullRegression")]
        [Category("AlgoStore")]
        public async Task DeployBianryAlgoWithWrongId()
        {
            string AlgoID = "non-existing-id-234-555-666";

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.NotFound);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == AlgoID) as RuntimeDataEntity;
            Assert.Null(runtimeDataEntity);

        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task RestartBinary()
        {

            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetaDataForBinaryUpload();

            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = temporaryResponseDTO.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoId
            };

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == AlgoId) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_STOP;

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.That(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.That(stopResponse.Status.Equals("STOPPED"));

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponceSecond = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponseSecond = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task DoubleStopBinary()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetaDataForBinaryUpload();

            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = temporaryResponseDTO.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoId
            };

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == AlgoId) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_STOP;

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.That(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.That(stopResponse.Status.Equals("STOPPED"));

            var stopBinaryResponseSecondTime = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.That(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponseSecondTime = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.That(stopResponse.Status.Equals("STOPPED"));
        }

        [Test]
        [Category("Functional")]
        [Category("FullRegression")]
        [Category("AlgoStore")]
        public async Task DoubleStartBinary()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetaDataForBinaryUpload();

            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = temporaryResponseDTO.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists);

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoId
            };

            url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == AlgoId) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));

            var startBinaryresponceSecondTime = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponseSecondTime = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task GetLogOnStoppedAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));

            url = ApiPaths.ALGO_STORE_ALGO_STOP;

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.That(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.That(stopResponse.Status.Equals("STOPPED"));

            url = ApiPaths.ALGO_STORE_ALGO_LOG;

            Dictionary<string, string> algoIDLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID }
            };

            var algoIDLogResponse = await this.Consumer.ExecuteRequest(url, algoIDLog, null, Method.GET);
            Assert.That(algoIDLogResponse.Status == HttpStatusCode.OK);

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task GetLogOnDeployedOnlyAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);        

            url = ApiPaths.ALGO_STORE_ALGO_LOG;

            Dictionary<string, string> algoIDLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID }
            };

            var algoIDLogResponse = await this.Consumer.ExecuteRequest(url, algoIDLog, null, Method.GET);
            Assert.That(algoIDLogResponse.Status == HttpStatusCode.OK);

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task GetTailLogOnStoppedAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status.Equals("STARTED"));

            url = ApiPaths.ALGO_STORE_ALGO_STOP;

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.That(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.That(stopResponse.Status.Equals("STOPPED"));

            url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                {"Tail" , "60" }
            };

            var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
            Assert.That(algoIDTailLogResponse.Status == HttpStatusCode.OK);

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task GetTailLogOnDeployedOnlyAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.That(uploadBinaryresponce.Status == HttpStatusCode.OK);

            url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                {"Tail" , "60" }
            };

            var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
            Assert.That(algoIDTailLogResponse.Status == HttpStatusCode.OK);

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task UploadStringLarge()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            MetaDataResponseDTO metadataWithUploadedString = DataManager.getMetaDataForBinaryUpload();

            string AlgoId = metadataWithUploadedString.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId },
                {"Data" , Helpers.RandomString(1000) }
            };

            var responceUploadString = await this.Consumer.ExecuteRequest(url, quaryParam, null, Method.POST);
            Assert.That(responceUploadString.Status == HttpStatusCode.NoContent);

            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.STRING);
            Assert.That(blobExists);
        }

        [Test]
        [Category("FullRegression")]
        [Category("Functional")]
        [Category("AlgoStore")]
        public async Task GetStringWrongId()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", "non-existing-id-234-555-666" }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadString.Status == HttpStatusCode.NotFound);
        }
    }
}
