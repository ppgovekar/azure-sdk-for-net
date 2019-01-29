﻿// Copyright (c) Microsoft Corporation. All rights reserved.
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
        public void NamespaceCreateGetUpdateDeleteDeatil()
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
                Assert.NotNull(getNamespaceResponse);
                Assert.Equal("Succeeded", getNamespaceResponse.ProvisioningState, StringComparer.CurrentCultureIgnoreCase);
                Assert.Equal(location, getNamespaceResponse.Location, StringComparer.CurrentCultureIgnoreCase);

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
                var updateNamespaceParameter = new SBNamespaceUpdateParameters()
                {
                    Location = location,
                    Tags = new Dictionary<string, string>()
                        {
                            {"tag3", "value3"},
                            {"tag4", "value4"}
                        }
                };

                var updateNamespaceResponse = ServiceBusManagementClient.Namespaces.Update(resourceGroup, namespaceName, updateNamespaceParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(5));

                // Get the updated namespace
                getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);
                Assert.NotNull(getNamespaceResponse);
                Assert.Equal(location, getNamespaceResponse.Location, StringComparer.CurrentCultureIgnoreCase);
                Assert.Equal(namespaceName, getNamespaceResponse.Name);
                Assert.Equal(2, getNamespaceResponse.Tags.Count);
                foreach (var tag in updateNamespaceParameter.Tags)
                {
                    Assert.Contains(getNamespaceResponse.Tags, t => t.Key.Equals(tag.Key));
                    Assert.Contains(getNamespaceResponse.Tags, t => t.Value.Equals(tag.Value));
                }

                //Update namespace sku capacity and tier to Premuium
                var updateNamespaceCapacityParameter = new SBNamespaceUpdateParameters()
                {
                    Location = location,
                    Sku = new SBSku
                    {
                        Capacity = 2,
                        Name = SkuName.Premium,
                        Tier = SkuTier.Premium
                    },
                    Tags = new Dictionary<string, string>()
                        {
                            {"tag3", "value3"},
                            {"tag4", "value4"}
                        }
                };

                var updateNamespaceCapacityResponse = ServiceBusManagementClient.Namespaces.Update(resourceGroup, namespaceName, updateNamespaceCapacityParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(5));

                // Get the updated namespace
                getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);
                Assert.NotNull(getNamespaceResponse);
                Assert.Equal(location, getNamespaceResponse.Location, StringComparer.CurrentCultureIgnoreCase);
                Assert.Equal(namespaceName, getNamespaceResponse.Name);
                Assert.Equal(2, getNamespaceResponse.Tags.Count);
                Assert.Equal(updateNamespaceCapacityParameter.Sku.Name, getNamespaceResponse.Sku.Name);
                Assert.Equal(updateNamespaceCapacityParameter.Sku.Tier, getNamespaceResponse.Sku.Tier);
                Assert.Equal(updateNamespaceCapacityParameter.Sku.Capacity, getNamespaceResponse.Sku.Capacity);

                //Update namespace sku tier to basic
                var updateNamespaceSkuParameter = new SBNamespaceUpdateParameters()
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
                            {"tag4", "value4"}
                        }
                };

                var updateNamespaceSkuResponse = ServiceBusManagementClient.Namespaces.Update(resourceGroup, namespaceName, updateNamespaceSkuParameter);

                TestUtilities.Wait(TimeSpan.FromSeconds(5));

                // Get the updated namespace
                getNamespaceResponse = ServiceBusManagementClient.Namespaces.Get(resourceGroup, namespaceName);
                Assert.NotNull(getNamespaceResponse);
                Assert.Equal(location, getNamespaceResponse.Location, StringComparer.CurrentCultureIgnoreCase);
                Assert.Equal(namespaceName, getNamespaceResponse.Name);
                Assert.Equal(2, getNamespaceResponse.Tags.Count);
                Assert.Equal(updateNamespaceSkuParameter.Sku.Name, getNamespaceResponse.Sku.Name);
                Assert.Equal(updateNamespaceSkuParameter.Sku.Tier, getNamespaceResponse.Sku.Tier);

                // Delete namespace
                ServiceBusManagementClient.Namespaces.Delete(resourceGroup, namespaceName);
            }
        }
    }
}