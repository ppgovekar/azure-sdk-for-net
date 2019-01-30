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
        public static bool ValidateNamespaceParams(SBNamespace setNamespaceResponse, SBNamespace getNamespaceResponse)
        {
            Assert.NotNull(setNamespaceResponse);
            Assert.NotNull(getNamespaceResponse);
            if (setNamespaceResponse.Name != null) { Assert.Equal(setNamespaceResponse.Name, getNamespaceResponse.Name); }
            if (setNamespaceResponse.Location != null) { Assert.Equal(setNamespaceResponse.Location, getNamespaceResponse.Location, StringComparer.CurrentCultureIgnoreCase); }
            if (setNamespaceResponse.Tags != null)
            {
                Assert.Equal(setNamespaceResponse.Tags.Count, getNamespaceResponse.Tags.Count);
                foreach (var tag in getNamespaceResponse.Tags)
                {
                    Assert.Contains(setNamespaceResponse.Tags, t => t.Key.Equals(tag.Key));
                    Assert.Contains(setNamespaceResponse.Tags, t => t.Value.Equals(tag.Value));
                }
            }
            if (setNamespaceResponse.Sku != null)
            {
                if (setNamespaceResponse.Sku.Tier.ToString() == "Basic" || setNamespaceResponse.Sku.Tier.ToString() == "Standard")
                {
                    Assert.Equal(setNamespaceResponse.Sku.Name, getNamespaceResponse.Sku.Name);
                    Assert.Equal(setNamespaceResponse.Sku.Tier, getNamespaceResponse.Sku.Tier);
                }
                else
                {
                    Assert.Equal(setNamespaceResponse.Sku.Name, getNamespaceResponse.Sku.Name);
                    Assert.Equal(setNamespaceResponse.Sku.Tier, getNamespaceResponse.Sku.Tier);
                    Assert.Equal(setNamespaceResponse.Sku.Capacity, getNamespaceResponse.Sku.Capacity);
                }
            }

            return true;
        }

        public static bool ValidateQueueParams(SBQueue setQueueResponse, SBQueue getQueueResponse)
        {
            Assert.NotNull(setQueueResponse);
            Assert.NotNull(getQueueResponse);
            if (setQueueResponse.Name != null) { Assert.Equal(setQueueResponse.Name, getQueueResponse.Name); }
            if (setQueueResponse.Status != null) { Assert.Equal(setQueueResponse.Status, getQueueResponse.Status); }
            if (setQueueResponse.EnableExpress != null) { Assert.Equal(setQueueResponse.EnableExpress, getQueueResponse.EnableExpress); }
            if (setQueueResponse.EnableBatchedOperations != null) { Assert.Equal(setQueueResponse.EnableBatchedOperations, getQueueResponse.EnableBatchedOperations); }
            if (setQueueResponse.EnablePartitioning != null) { Assert.Equal(setQueueResponse.EnablePartitioning, getQueueResponse.EnablePartitioning); }
            if (setQueueResponse.DeadLetteringOnMessageExpiration != null) { Assert.Equal(setQueueResponse.DeadLetteringOnMessageExpiration, getQueueResponse.DeadLetteringOnMessageExpiration); }
            if (setQueueResponse.RequiresSession != null) { Assert.Equal(setQueueResponse.RequiresSession, getQueueResponse.RequiresSession); }
            if (setQueueResponse.RequiresDuplicateDetection != null)
            {
                Assert.Equal(setQueueResponse.RequiresDuplicateDetection, getQueueResponse.RequiresDuplicateDetection);
                Assert.Equal(setQueueResponse.DuplicateDetectionHistoryTimeWindow, getQueueResponse.DuplicateDetectionHistoryTimeWindow);
            }
            if (setQueueResponse.MaxDeliveryCount != null) { Assert.Equal(setQueueResponse.MaxDeliveryCount, getQueueResponse.MaxDeliveryCount); }
            if (setQueueResponse.MaxSizeInMegabytes != null) { Assert.Equal(setQueueResponse.MaxSizeInMegabytes, getQueueResponse.MaxSizeInMegabytes); }
            if (setQueueResponse.ForwardTo != null) { Assert.Equal(setQueueResponse.ForwardTo, getQueueResponse.ForwardTo); }
            if (setQueueResponse.ForwardDeadLetteredMessagesTo != null) { Assert.Equal(setQueueResponse.ForwardDeadLetteredMessagesTo, getQueueResponse.ForwardDeadLetteredMessagesTo); }
            if (setQueueResponse.AutoDeleteOnIdle != null) { Assert.Equal(setQueueResponse.AutoDeleteOnIdle, getQueueResponse.AutoDeleteOnIdle); }
            if (setQueueResponse.DefaultMessageTimeToLive != null) { Assert.Equal(setQueueResponse.DefaultMessageTimeToLive, getQueueResponse.DefaultMessageTimeToLive); }
            if (setQueueResponse.LockDuration != null) { Assert.Equal(setQueueResponse.LockDuration, getQueueResponse.LockDuration); }

            return true;
        }

        public static bool ValidateTopicParams(SBTopic setTopicResponse, SBTopic getTopicResponse)
        {
            Assert.NotNull(setTopicResponse);
            Assert.NotNull(getTopicResponse);
            if (setTopicResponse.Name != null) { Assert.Equal(setTopicResponse.Name, getTopicResponse.Name); }
            if (setTopicResponse.Status != null) { Assert.Equal(setTopicResponse.Status, getTopicResponse.Status); }
            if (setTopicResponse.EnableExpress != null) { Assert.Equal(setTopicResponse.EnableExpress, getTopicResponse.EnableExpress); }
            if (setTopicResponse.EnableBatchedOperations != null) { Assert.Equal(setTopicResponse.EnableBatchedOperations, getTopicResponse.EnableBatchedOperations); }
            if (setTopicResponse.EnablePartitioning != null) { Assert.Equal(setTopicResponse.EnablePartitioning, getTopicResponse.EnablePartitioning); }
            if (setTopicResponse.SupportOrdering != null) { Assert.Equal(setTopicResponse.SupportOrdering, getTopicResponse.SupportOrdering); }
            if (setTopicResponse.RequiresDuplicateDetection != null)
            {
                Assert.Equal(setTopicResponse.RequiresDuplicateDetection, getTopicResponse.RequiresDuplicateDetection);
                Assert.Equal(setTopicResponse.DuplicateDetectionHistoryTimeWindow, getTopicResponse.DuplicateDetectionHistoryTimeWindow);
            }
            if (setTopicResponse.MaxSizeInMegabytes != null) { Assert.Equal(setTopicResponse.MaxSizeInMegabytes, getTopicResponse.MaxSizeInMegabytes); }
            if (setTopicResponse.AutoDeleteOnIdle != null) { Assert.Equal(setTopicResponse.AutoDeleteOnIdle, getTopicResponse.AutoDeleteOnIdle); }
            if (setTopicResponse.DefaultMessageTimeToLive != null) { Assert.Equal(setTopicResponse.DefaultMessageTimeToLive, getTopicResponse.DefaultMessageTimeToLive); }

            return true;
        }

        public static bool ValidateSubParams(SBSubscription setSubResponse, SBSubscription getSubResponse)
        {
            Assert.NotNull(setSubResponse);
            Assert.NotNull(getSubResponse);
            if (setSubResponse.Name != null) { Assert.Equal(setSubResponse.Name, getSubResponse.Name); }
            if (setSubResponse.Status != null) { Assert.Equal(setSubResponse.Status, getSubResponse.Status); }
            if (setSubResponse.EnableBatchedOperations != null) { Assert.Equal(setSubResponse.EnableBatchedOperations, getSubResponse.EnableBatchedOperations); }
            if (setSubResponse.DeadLetteringOnMessageExpiration != null) { Assert.Equal(setSubResponse.DeadLetteringOnMessageExpiration, getSubResponse.DeadLetteringOnMessageExpiration); }
            if (setSubResponse.RequiresSession != null) { Assert.Equal(setSubResponse.RequiresSession, getSubResponse.RequiresSession); }
            if (setSubResponse.DuplicateDetectionHistoryTimeWindow != null) { Assert.Equal(setSubResponse.DuplicateDetectionHistoryTimeWindow, getSubResponse.DuplicateDetectionHistoryTimeWindow); }
            if (setSubResponse.MaxDeliveryCount != null) { Assert.Equal(setSubResponse.MaxDeliveryCount, getSubResponse.MaxDeliveryCount); }
            if (setSubResponse.ForwardTo != null) { Assert.Equal(setSubResponse.ForwardTo, getSubResponse.ForwardTo); }
            if (setSubResponse.ForwardDeadLetteredMessagesTo != null) { Assert.Equal(setSubResponse.ForwardDeadLetteredMessagesTo, getSubResponse.ForwardDeadLetteredMessagesTo); }
            if (setSubResponse.AutoDeleteOnIdle != null) { Assert.Equal(setSubResponse.AutoDeleteOnIdle, getSubResponse.AutoDeleteOnIdle); }
            if (setSubResponse.DefaultMessageTimeToLive != null) { Assert.Equal(setSubResponse.DefaultMessageTimeToLive, getSubResponse.DefaultMessageTimeToLive); }
            if (setSubResponse.LockDuration != null) { Assert.Equal(setSubResponse.LockDuration, getSubResponse.LockDuration); }

            return true;
        }
    }
}
