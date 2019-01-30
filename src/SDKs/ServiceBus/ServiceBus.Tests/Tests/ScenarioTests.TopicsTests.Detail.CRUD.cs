// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


namespace ServiceBus.Tests.ScenarioTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using Microsoft.Azure.Management.ServiceBus;
    using Microsoft.Azure.Management.ServiceBus.Models;
    using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
    using TestHelper;
    using Xunit;
    public partial class ScenarioTests 
    {
        [Fact]
        public void TopicsCreateGetUpdateDeleteDetail()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                InitializeClients(context);

                var location = this.ResourceManagementClient.GetLocationFromProvider();

                var resourceGroup = "v-ajnavtest";
                //var resourceGroup = this.ResourceManagementClient.TryGetResourceGroup(location);
                //if (string.IsNullOrWhiteSpace(resourceGroup))
                //{
                //    resourceGroup = TestUtilities.GenerateName(ServiceBusManagementHelper.ResourceGroupPrefix);
                //    this.ResourceManagementClient.TryRegisterResourceGroup(location, resourceGroup);
                //}

                var namespaceName = TestUtilities.GenerateName(ServiceBusManagementHelper.NamespacePrefix);

                var createNamespaceResponse = this.ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceName,
                    new SBNamespace()
                    {
                        Location = location,
                        Sku = new SBSku
                        {
                            Name = SkuName.Standard,
                            Tier = SkuTier.Standard
                        }
                    });

                Assert.NotNull(createNamespaceResponse);
                Assert.Equal(createNamespaceResponse.Name, namespaceName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                // Create Invalid TopicName
                var topicNameGeneration = TestUtilities.GenerateName(ServiceBusManagementHelper.TopicPrefix);
                var topicInvalidName = topicNameGeneration + "$";

                //Invalid operation TopicName
                try
                {
                    var createinvalidTopicResponse = this.ServiceBusManagementClient.Topics.CreateOrUpdate(resourceGroup, namespaceName, topicInvalidName,
                                    new SBTopic() { EnableExpress = true });
                }
                catch (Exception e)
                {
                    Assert.Equal("Operation returned an invalid status code 'BadRequest'", e.Message);
                }

                // Create a Topic
                var topicName = TestUtilities.GenerateName(ServiceBusManagementHelper.TopicPrefix);
                var createTopicResponse = this.ServiceBusManagementClient.Topics.CreateOrUpdate(resourceGroup, namespaceName, topicName,
                new SBTopic() { EnableExpress = true });

                Assert.NotNull(createTopicResponse);
                Assert.Equal(createTopicResponse.Name, topicName);

                // Get the created topic
                var topicGetResponse = ServiceBusManagementClient.Topics.Get(resourceGroup, namespaceName, topicName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateTopicParams(createTopicResponse, topicGetResponse));

                // Get all Topics   
                var getTopicsListAllResponse = ServiceBusManagementClient.Topics.ListByNamespace(resourceGroup, namespaceName);
                Assert.NotNull(getTopicsListAllResponse);
                Assert.True(getTopicsListAllResponse.Count() >= 1);                
                Assert.True(getTopicsListAllResponse.All(ns => ns.Id.Contains(resourceGroup)));

                // Update Topic
                var updateTopicsParameter = new SBTopic() {EnableExpress = true};

                var updateTopicsResponse = ServiceBusManagementClient.Topics.CreateOrUpdate(resourceGroup, namespaceName, topicName, updateTopicsParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.NotEqual(updateTopicsResponse.UpdatedAt, topicGetResponse.UpdatedAt);
                Assert.True(ServiceBusTestValidationHelper.ValidateTopicParams(updateTopicsParameter, updateTopicsResponse));

                // Get the created topic to check the Updated values. 
                var getTopicResponse = ServiceBusManagementClient.Topics.Get(resourceGroup, namespaceName, topicName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.NotEqual(getTopicResponse.UpdatedAt, topicGetResponse.UpdatedAt);
                Assert.True(ServiceBusTestValidationHelper.ValidateTopicParams(updateTopicsResponse, getTopicResponse));

                // Update Topic AutoDelete
                var updateTopicsAutoDeleteParameter = new SBTopic() { EnableExpress = true, AutoDeleteOnIdle = TimeSpan.MaxValue };

                var updateTopicsAutoDeleteResponse = ServiceBusManagementClient.Topics.CreateOrUpdate(resourceGroup, namespaceName, topicName, updateTopicsAutoDeleteParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.NotEqual(updateTopicsAutoDeleteResponse.UpdatedAt, topicGetResponse.UpdatedAt);
                Assert.True(ServiceBusTestValidationHelper.ValidateTopicParams(updateTopicsAutoDeleteParameter, updateTopicsAutoDeleteResponse));

                // Get the created topic to check the Updated values. 
                getTopicResponse = ServiceBusManagementClient.Topics.Get(resourceGroup, namespaceName, topicName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.NotEqual(getTopicResponse.UpdatedAt, topicGetResponse.UpdatedAt);
                Assert.True(ServiceBusTestValidationHelper.ValidateTopicParams(updateTopicsAutoDeleteResponse, getTopicResponse));

                // Update Topic Ttl
                var updateTopicsTtlParameter = new SBTopic()
                {
                    EnableExpress = true,
                    AutoDeleteOnIdle = TimeSpan.MaxValue,
                    DefaultMessageTimeToLive = TimeSpan.MaxValue,
                    MaxSizeInMegabytes = 2048
                };

                var updateTopicsTtlResponse = ServiceBusManagementClient.Topics.CreateOrUpdate(resourceGroup, namespaceName, topicName, updateTopicsTtlParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.NotEqual(updateTopicsTtlResponse.UpdatedAt, topicGetResponse.UpdatedAt);
                Assert.True(ServiceBusTestValidationHelper.ValidateTopicParams(updateTopicsTtlParameter, updateTopicsTtlResponse));

                // Get the created topic to check the Updated values. 
                getTopicResponse = ServiceBusManagementClient.Topics.Get(resourceGroup, namespaceName, topicName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.NotEqual(getTopicResponse.UpdatedAt, topicGetResponse.UpdatedAt);
                Assert.True(ServiceBusTestValidationHelper.ValidateTopicParams(updateTopicsTtlResponse, getTopicResponse));

                // Delete Created Topics 
                ServiceBusManagementClient.Topics.Delete(resourceGroup, namespaceName, topicName);                

                // Delete namespace                                   
                ServiceBusManagementClient.Namespaces.DeleteWithHttpMessagesAsync(resourceGroup, namespaceName, null, new CancellationToken()).ConfigureAwait(false);
            }
        }
    }
}
