// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Sql
{
    using Microsoft.Rest;
    using Microsoft.Rest.Azure;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for ManagedDatabaseSecurityAlertPoliciesOperations.
    /// </summary>
    public static partial class ManagedDatabaseSecurityAlertPoliciesOperationsExtensions
    {
            /// <summary>
            /// Gets a managed database's security alert policy.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='databaseName'>
            /// The name of the managed database for which the security alert policy is
            /// defined.
            /// </param>
            public static ManagedDatabaseSecurityAlertPolicy Get(this IManagedDatabaseSecurityAlertPoliciesOperations operations, string resourceGroupName, string managedInstanceName, string databaseName)
            {
                return operations.GetAsync(resourceGroupName, managedInstanceName, databaseName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets a managed database's security alert policy.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='databaseName'>
            /// The name of the managed database for which the security alert policy is
            /// defined.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ManagedDatabaseSecurityAlertPolicy> GetAsync(this IManagedDatabaseSecurityAlertPoliciesOperations operations, string resourceGroupName, string managedInstanceName, string databaseName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetWithHttpMessagesAsync(resourceGroupName, managedInstanceName, databaseName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Creates or updates a database's security alert policy.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='databaseName'>
            /// The name of the managed database for which the security alert policy is
            /// defined.
            /// </param>
            /// <param name='parameters'>
            /// The database security alert policy.
            /// </param>
            public static ManagedDatabaseSecurityAlertPolicy CreateOrUpdate(this IManagedDatabaseSecurityAlertPoliciesOperations operations, string resourceGroupName, string managedInstanceName, string databaseName, ManagedDatabaseSecurityAlertPolicy parameters)
            {
                return operations.CreateOrUpdateAsync(resourceGroupName, managedInstanceName, databaseName, parameters).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates or updates a database's security alert policy.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The name of the resource group that contains the resource. You can obtain
            /// this value from the Azure Resource Manager API or the portal.
            /// </param>
            /// <param name='managedInstanceName'>
            /// The name of the managed instance.
            /// </param>
            /// <param name='databaseName'>
            /// The name of the managed database for which the security alert policy is
            /// defined.
            /// </param>
            /// <param name='parameters'>
            /// The database security alert policy.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ManagedDatabaseSecurityAlertPolicy> CreateOrUpdateAsync(this IManagedDatabaseSecurityAlertPoliciesOperations operations, string resourceGroupName, string managedInstanceName, string databaseName, ManagedDatabaseSecurityAlertPolicy parameters, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateOrUpdateWithHttpMessagesAsync(resourceGroupName, managedInstanceName, databaseName, parameters, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
