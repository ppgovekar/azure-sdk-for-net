using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Azure.Management.ServiceBus;
using Microsoft.Azure.Management.ServiceBus.Models;

namespace ServiceBus.Tests.TestHelper
{
    class ServiceBusTestValidationHelper
    {
        public static bool ValidateNamespaceParams(SBNamespace createNamespaceResponse, SBNamespace getNamespaceResponse)
        {
            Assert.NotNull(createNamespaceResponse);
            Assert.NotNull(getNamespaceResponse);
            Assert.Equal(createNamespaceResponse.Name, getNamespaceResponse.Name);
            Assert.Equal(createNamespaceResponse.Location, getNamespaceResponse.Location, StringComparer.CurrentCultureIgnoreCase);
            if (getNamespaceResponse.Tags.Count > 0)
            {
                Assert.Equal(createNamespaceResponse.Tags.Count, getNamespaceResponse.Tags.Count);
                foreach (var tag in getNamespaceResponse.Tags)
                {
                    Assert.Contains(createNamespaceResponse.Tags, t => t.Key.Equals(tag.Key));
                    Assert.Contains(createNamespaceResponse.Tags, t => t.Value.Equals(tag.Value));
                }
            }
            if (getNamespaceResponse.Sku.Tier.ToString() == "Basic" || getNamespaceResponse.Sku.Tier.ToString() == "Standard")
            {
                Assert.Equal(createNamespaceResponse.Sku.Name, getNamespaceResponse.Sku.Name);
                Assert.Equal(createNamespaceResponse.Sku.Tier, getNamespaceResponse.Sku.Tier);
            }
            else
            {
                Assert.Equal(createNamespaceResponse.Sku.Name, getNamespaceResponse.Sku.Name);
                Assert.Equal(createNamespaceResponse.Sku.Tier, getNamespaceResponse.Sku.Tier);
                Assert.Equal(createNamespaceResponse.Sku.Capacity, getNamespaceResponse.Sku.Capacity);
            }

            return true;
        }
    }
}
