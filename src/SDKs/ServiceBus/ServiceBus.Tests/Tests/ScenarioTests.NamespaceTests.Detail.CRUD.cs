// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace ServiceBus.Tests.ScenarioTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Azure.Management.ServiceBus;
    using Microsoft.Azure.Management.ServiceBus.Models;
    using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
    using TestHelper;
    using Xunit;
    public partial class ScenarioTests
    {
        [Fact]
        public void NamespaceCreateGetUpdateDeleteDetail()
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

                // Create Invalid Namespace
                var invalidNamespaceName = TestUtilities.GenerateName(null);

                //Check Invalid namespace name available
                var checkInvalidNamespaceavailable = ServiceBusManagementClient.Namespaces.CheckNameAvailabilityMethod(new CheckNameAvailability() { Name = invalidNamespaceName });

                Assert.Equal("InvalidName", checkInvalidNamespaceavailable.Reason.ToString());

                // Create Namespace
                var namespaceName = TestUtilities.GenerateName(ServiceBusManagementHelper.NamespacePrefix);

                //Check namespace name available
                var checknamespaceavailable = ServiceBusManagementClient.Namespaces.CheckNameAvailabilityMethod(new CheckNameAvailability() { Name = namespaceName });

                var createNamespaceResponse = this.ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceName,
                    new SBNamespace()
                    {
                        Location = location,
                        Sku = new SBSku
                        {
                            Name = SkuName.Standard,
                            Tier = SkuTier.Standard
                        },
                        Tags = new Dictionary<string, string>()
                        {
                            {"tag1", "value1"},
                            {"tag2", "value2"}
                        }
                    });

                Assert.NotNull(createNamespaceResponse);
                Assert.Equal(createNamespaceResponse.Name, namespaceName);

                TestUtilities.Wait(TimeSpan.FromSeconds(5));

                // Get the created namespace
                var getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);                
                if (string.Compare(getNamespaceResponse.ProvisioningState, "Succeeded", true) != 0)
                    TestUtilities.Wait(TimeSpan.FromSeconds(5));

                getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);
                Assert.Equal("Succeeded", getNamespaceResponse.ProvisioningState, StringComparer.CurrentCultureIgnoreCase);
                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(createNamespaceResponse, getNamespaceResponse));                

                // Get all namespaces created within a resourceGroup
                var getAllNamespacesResponse = ServiceBusManagementClient.Namespaces.ListByResourceGroupAsync(resourceGroup).Result;
                Assert.NotNull(getAllNamespacesResponse);
                Assert.True(getAllNamespacesResponse.Count() >= 1);
                Assert.Contains(getAllNamespacesResponse, ns => ns.Name == namespaceName);
                Assert.True(getAllNamespacesResponse.All(ns => ns.Id.Contains(resourceGroup)));

                // Get all namespaces created within the subscription irrespective of the resourceGroup
                getAllNamespacesResponse = ServiceBusManagementClient.Namespaces.List();
                Assert.NotNull(getAllNamespacesResponse);
                Assert.True(getAllNamespacesResponse.Count() >= 1);
                Assert.Contains(getAllNamespacesResponse, ns => ns.Name == namespaceName);

                //Update namespace tags
                var updateNamespaceParameter = new SBNamespace()
                {
                    Location = location,
                    Tags = new Dictionary<string, string>()
                        {
                            {"tag3", "value3"},
                            {"tag4", "value4"}
                        }
                };

                var updateNamespaceResponse = ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceName, updateNamespaceParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceParameter, updateNamespaceResponse));

                // Get the updated namespace
                getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceResponse, getNamespaceResponse));

                //Update namespace sku tier to basic
                var updateNamespaceSkuParameter = new SBNamespace()
                {
                    Location = location,
                    Sku = new SBSku
                    {
                        Name = SkuName.Basic,
                        Tier = SkuTier.Basic
                    },
                    Tags = new Dictionary<string, string>()
                        {
                            {"tag3", "value3"},
                            {"tag4", "value4"},
                            {"tag5", "value5"}
                        }
                };

                var updateNamespaceSkuResponse = ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceName, updateNamespaceSkuParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceSkuParameter, updateNamespaceSkuResponse));

                // Get the updated namespace
                getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceSkuResponse, getNamespaceResponse));

                //Update namespace sku tier to Standard
                var updateNamespaceSkuStdParameter = new SBNamespace()
                {
                    Location = location,
                    Sku = new SBSku
                    {
                        Name = SkuName.Standard,
                        Tier = SkuTier.Standard
                    }
                };

                var updateNamespaceSkuStdResponse = ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceName, updateNamespaceSkuStdParameter);
                            
                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceSkuStdParameter, updateNamespaceSkuStdResponse));

                // Get the updated namespace
                getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceSkuStdResponse, getNamespaceResponse));

                //Create topic to test Standard to basic conversion
                var topicName = TestUtilities.GenerateName(ServiceBusManagementHelper.TopicPrefix);
                var createTopicResponse = this.ServiceBusManagementClient.Topics.CreateOrUpdate(resourceGroup, namespaceName, topicName,
                new SBTopic() { EnableExpress = true });
                Assert.NotNull(createTopicResponse);
                Assert.Equal(createTopicResponse.Name, topicName);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                //Update namespace sku tier to Basic
                var updateNamespaceSkuBasicParameter = new SBNamespace()
                {
                    Location = location,
                    Sku = new SBSku
                    {
                        Name = SkuName.Basic,
                        Tier = SkuTier.Basic
                    }
                };

                try
                {
                    var updateNamespaceSkuBasicResponse = ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceName, updateNamespaceSkuBasicParameter);
                }
                catch (Exception e)
                {
                    Assert.Equal("Operation returned an invalid status code 'Conflict'", e.Message);
                }

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                // Delete Created Topics 
                ServiceBusManagementClient.Topics.Delete(resourceGroup, namespaceName, topicName);

                // Delete namespace
                ServiceBusManagementClient.Namespaces.Delete(resourceGroup, namespaceName);

                // Check that namespace is deleted
                getAllNamespacesResponse = ServiceBusManagementClient.Namespaces.ListByResourceGroupAsync(resourceGroup).Result;
                Assert.DoesNotContain(getAllNamespacesResponse, ns => ns.Name == namespaceName);

                TestUtilities.Wait(TimeSpan.FromSeconds(5));


                // Create Premium Namespace
                var namespaceNamePrem = TestUtilities.GenerateName(ServiceBusManagementHelper.NamespacePrefix);

                //Check namespace name available
                var checkpremnamespaceavailable = ServiceBusManagementClient.Namespaces.CheckNameAvailabilityMethod(new CheckNameAvailability() { Name = namespaceNamePrem });

                var createPremNamespaceResponse = this.ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceNamePrem,
                    new SBNamespace()
                    {
                        Location = location,
                        Sku = new SBSku
                        {
                            Name = SkuName.Premium,
                            Tier = SkuTier.Premium,
                            Capacity = 1
                        },
                        Tags = new Dictionary<string, string>()
                        {
                            {"tag1", "value1"},
                            {"tag2", "value2"}
                        }
                    });

                Assert.NotNull(createPremNamespaceResponse);
                Assert.Equal(createPremNamespaceResponse.Name, namespaceNamePrem);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                // Get the created namespace
                var getPremNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceNamePrem);
                if (string.Compare(getPremNamespaceResponse.ProvisioningState, "Succeeded", true) != 0)
                    TestUtilities.Wait(TimeSpan.FromSeconds(5));

                getPremNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceNamePrem);
                Assert.Equal("Succeeded", getPremNamespaceResponse.ProvisioningState, StringComparer.CurrentCultureIgnoreCase);
                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(createPremNamespaceResponse, getPremNamespaceResponse));

                // Get all namespaces created within a resourceGroup
                var getAllNamespacesResponseNew = ServiceBusManagementClient.Namespaces.ListByResourceGroupAsync(resourceGroup).Result;
                Assert.NotNull(getAllNamespacesResponseNew);
                Assert.True(getAllNamespacesResponseNew.Count() >= 1);
                Assert.Contains(getAllNamespacesResponseNew, ns => ns.Name == namespaceNamePrem);
                Assert.True(getAllNamespacesResponseNew.All(ns => ns.Id.Contains(resourceGroup)));

                // Get all namespaces created within the subscription irrespective of the resourceGroup
                getAllNamespacesResponseNew = ServiceBusManagementClient.Namespaces.List();
                Assert.NotNull(getAllNamespacesResponseNew);
                Assert.True(getAllNamespacesResponseNew.Count() >= 1);
                Assert.Contains(getAllNamespacesResponseNew, ns => ns.Name == namespaceNamePrem);

                //Update namespace sku tier to Premium
                var updateNamespaceSkuPremParameter = new SBNamespace()
                {
                    Location = location,
                    Sku = new SBSku
                    {
                        Name = SkuName.Premium,
                        Tier = SkuTier.Premium,
                        Capacity = 4
                    }
                };

                var updateNamespaceSkuPremResponse = ServiceBusManagementClient.Namespaces.CreateOrUpdate(resourceGroup, namespaceNamePrem, updateNamespaceSkuPremParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceSkuPremParameter, updateNamespaceSkuPremResponse));

                // Get the updated namespace
                getPremNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceNamePrem);

                TestUtilities.Wait(TimeSpan.FromSeconds(2));

                Assert.True(ServiceBusTestValidationHelper.ValidateNamespaceParams(updateNamespaceSkuPremResponse, getPremNamespaceResponse));

                // Delete namespace
                ServiceBusManagementClient.Namespaces.Delete(resourceGroup, namespaceNamePrem);

                // Check that namespace is deleted
                getAllNamespacesResponseNew = ServiceBusManagementClient.Namespaces.ListByResourceGroupAsync(resourceGroup).Result;
                Assert.DoesNotContain(getAllNamespacesResponseNew, ns => ns.Name == namespaceNamePrem);
            }
        }
    }
}