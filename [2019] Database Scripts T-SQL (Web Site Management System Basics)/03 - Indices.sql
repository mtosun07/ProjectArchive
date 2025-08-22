USE WebSiteBaseDB
GO



CREATE NONCLUSTERED INDEX [IX_IUDLogs_01] ON [IUDLogs]([OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_02] ON [IUDLogs]([OperationType_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_03] ON [IUDLogs]([IsSuccess], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_04] ON [IUDLogs]([TableName], [IsSuccess], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_05] ON [IUDLogs]([TableName], [PrimaryKeyValue_Checksum], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_06] ON [IUDLogs]([OperationType_EnumerationValue], [IsSuccess], [TableName], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_07] ON [IUDLogs]([TableName], [IsSuccess], [OperationType_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_08] ON [IUDLogs]([TableName], [OperationType_EnumerationValue], [IsSuccess], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_09] ON [IUDLogs]([IsSuccess], [TableName], [OperationType_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_10] ON [IUDLogs]([IsSuccess], [OperationType_EnumerationValue], [TableName], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_IUDLogs_11] ON [IUDLogs]([TableName], [PrimaryKeyValue_Checksum], [IsSuccess], [OperationType_EnumerationValue], [OperationDate])
GO

CREATE NONCLUSTERED INDEX [IX_IUDLogValues_1] ON [IUDLogValues]([IUDLogId], [ColumnName])
GO

CREATE NONCLUSTERED INDEX [IX_EventLogs_1] ON [EventLogs]([HappenedAt])
GO
CREATE NONCLUSTERED INDEX [IX_EventLogs_2] ON [EventLogs]([Title], [HappenedAt])
GO

CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_01] ON [ClientIpAddresses]([RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_02] ON [ClientIpAddresses]([Country], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_03] ON [ClientIpAddresses]([InternetServiceProviderName], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_04] ON [ClientIpAddresses]([OrganizationName], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_05] ON [ClientIpAddresses]([IsMobile], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_06] ON [ClientIpAddresses]([IsProxyOrVpnOrTorExitAddress], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_07] ON [ClientIpAddresses]([IpAddressV4], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_08] ON [ClientIpAddresses]([Status_IsSuccess], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_09] ON [ClientIpAddresses]([IpAddressV4], [Status_IsSuccess], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_10] ON [ClientIpAddresses]([Country], [RegionName], [City], [District], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_ClientIpAddresses_11] ON [ClientIpAddresses]([IpAddressV4], [Status_IsSuccess], [Continent], [Country], [RegionName], [City], [District], [InternetServiceProviderName], [IsMobile], [RecordDate])
GO

CREATE NONCLUSTERED INDEX [IX_Banishments_1] ON [Banishments]([IsEnabled], [BanishmentType_EnumerationValue], [BanishedValue_Checksum], [StartsAt], [EndsAt])
GO
CREATE NONCLUSTERED INDEX [IX_Banishments_2] ON [Banishments]([IsEnabled], [StartsAt], [EndsAt], [BanishmentType_EnumerationValue], [BanishedValue_Checksum])
GO

CREATE NONCLUSTERED INDEX [IX_Sessions_1] ON [Sessions]([ApplicationSessionId])
GO
CREATE NONCLUSTERED INDEX [IX_Sessions_2] ON [Sessions]([StartedAt])
GO
CREATE NONCLUSTERED INDEX [IX_Sessions_3] ON [Sessions]([InitialClientIpAddressId], [StartedAt])
GO

CREATE NONCLUSTERED INDEX [IX_AbandonedSessions_1] ON [AbandonedSessions]([HappenedAt])
GO
CREATE NONCLUSTERED INDEX [IX_AbandonedSessions_2] ON [AbandonedSessions]([Title], [HappenedAt])
GO

CREATE NONCLUSTERED INDEX [IX_PageRequests_1] ON [PageRequests]([RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_2] ON [PageRequests]([SessionId], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_3] ON [PageRequests]([ClientIpAddressId], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_4] ON [PageRequests]([UrlReferrer_Checksum], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_5] ON [PageRequests]([Path_Checksum], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_6] ON [PageRequests]([HttpMethod], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_7] ON [PageRequests]([FailureReason_EnumerationValue], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_8] ON [PageRequests]([SessionId], [ClientIpAddressId], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_PageRequests_9] ON [PageRequests]([SessionId], [FailureReason_EnumerationValue], [RequestDate])
GO

CREATE NONCLUSTERED INDEX [IX_FormResults_1] ON [FormResults]([PageRequestId])
GO
CREATE NONCLUSTERED INDEX [IX_FormResults_2] ON [FormResults]([RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_FormResults_3] ON [FormResults]([FormName], [RecordDate])
GO

CREATE NONCLUSTERED INDEX [IX_FormResultComponents_1] ON [FormResultComponents]([FormResultId], [Order])
GO

CREATE NONCLUSTERED INDEX [IX_SentEmails_01] ON [SentEmails]([RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_02] ON [SentEmails]([SentDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_03] ON [SentEmails]([FailureDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_04] ON [SentEmails]([ToAddress], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_05] ON [SentEmails]([FromAddress], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_06] ON [SentEmails]([FailureReason_EnumerationValue], [FailureDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_07] ON [SentEmails]([FromAddress], [SentDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_08] ON [SentEmails]([ToAddress], [SentDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_09] ON [SentEmails]([FromAddress], [ToAddress], [RecordDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_10] ON [SentEmails]([FailureReason_EnumerationValue], [FromAddress], [FailureDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_11] ON [SentEmails]([FailureReason_EnumerationValue], [ToAddress], [FailureDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_12] ON [SentEmails]([FromAddress], [FailureReason_EnumerationValue], [FailureDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_13] ON [SentEmails]([FromAddress], [ToAddress], [SentDate])
GO
CREATE NONCLUSTERED INDEX [IX_SentEmails_14] ON [SentEmails]([FailureReason_EnumerationValue], [FromAddress], [ToAddress], [FailureDate])
GO

CREATE NONCLUSTERED INDEX [IX_SentEmailAttachments_1] ON [SentEmailAttachments]([SentEmailId], [DisplayFileName])
GO

CREATE NONCLUSTERED INDEX [IX_ErrorLogs_1] ON [ErrorLogs]([PageRequestId])
GO
CREATE NONCLUSTERED INDEX [IX_ErrorLogs_2] ON [ErrorLogs]([OccuredAt])
GO
CREATE NONCLUSTERED INDEX [IX_ErrorLogs_3] ON [ErrorLogs]([ErrorType], [OccuredAt])
GO

CREATE NONCLUSTERED INDEX [IX_UserGroups_1] ON [UserGroups]([IsEnabled], [SupUserGroupId])
GO
CREATE NONCLUSTERED INDEX [IX_UserGroups_2] ON [UserGroups]([IsEnabled], [Name])
GO

CREATE NONCLUSTERED INDEX [IX_Users_1] ON [Users]([IsEnabled], [EmailAddress])
GO
CREATE NONCLUSTERED INDEX [IX_Users_2] ON [Users]([IsEnabled], [Username])
GO
CREATE NONCLUSTERED INDEX [IX_Users_3] ON [Users]([IsEnabled], [Is2FAActive])
GO
CREATE NONCLUSTERED INDEX [IX_Users_4] ON [Users]([IsEnabled], [UserGroupId])
GO
CREATE NONCLUSTERED INDEX [IX_Users_5] ON [Users]([IsEnabled], [LastName], [FirstName], [Username])
GO

CREATE NONCLUSTERED INDEX [IX_UserVerificationTokens_1] ON [UserVerificationTokens]([IsEnabled], [AdditionalData_Checksum])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationTokens_2] ON [UserVerificationTokens]([IsEnabled], [VerificationType_EnumerationValue], [UserId])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationTokens_3] ON [UserVerificationTokens]([IsEnabled], [UserId], [VerificationType_EnumerationValue])
GO

CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_1] ON [UserVerificationAttempts]([PageRequestId])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_2] ON [UserVerificationAttempts]([ResultUserVerificationTokenId])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_3] ON [UserVerificationAttempts]([AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_4] ON [UserVerificationAttempts]([AdditionalData_Checksum], [AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_5] ON [UserVerificationAttempts]([UserId], [AttemptDate], [ResultFailureReason_EnumerationValue])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_6] ON [UserVerificationAttempts]([VerificationType_EnumerationValue], [ResultFailureReason_EnumerationValue], [AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_7] ON [UserVerificationAttempts]([UserId], [VerificationType_EnumerationValue], [AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserVerificationAttempts_8] ON [UserVerificationAttempts]([UserId], [VerificationType_EnumerationValue], [ResultFailureReason_EnumerationValue], [AttemptDate])
GO

CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_1] ON [UserLoginAttempts]([PageRequestId])
GO
CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_2] ON [UserLoginAttempts]([ResultUserVerificationTokenId])
GO
CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_3] ON [UserLoginAttempts]([AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_4] ON [UserLoginAttempts]([ResultFailureReason_EnumerationValue], [AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_5] ON [UserLoginAttempts]([ResultUserId], [ResultFailureReason_EnumerationValue], [AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_6] ON [UserLoginAttempts]([Username_Checksum], [ResultFailureReason_EnumerationValue], [AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_7] ON [UserLoginAttempts]([ResultFailureReason_EnumerationValue], [Username_Checksum], [AttemptDate])
GO
CREATE NONCLUSTERED INDEX [IX_UserLoginAttempts_8] ON [UserLoginAttempts]([ResultFailureReason_EnumerationValue], [ResultUserId], [AttemptDate])
GO

CREATE NONCLUSTERED INDEX [IX_UserLogins_1] ON [UserLogins]([IsEnabled], [UserLoginAttemptId])
GO
CREATE NONCLUSTERED INDEX [IX_UserLogins_2] ON [UserLogins]([IsEnabled], [UserVerificationAttemptId])
GO

CREATE NONCLUSTERED INDEX [IX_Whitelist_1] ON [Whitelist]([IsEnabled], [UserId], [IpAddressV4])
GO
CREATE NONCLUSTERED INDEX [IX_Whitelist_2] ON [Whitelist]([IsEnabled], [StartsAt], [EndsAt], [IpAddressV4])
GO
CREATE NONCLUSTERED INDEX [IX_Whitelist_3] ON [Whitelist]([IsEnabled], [StartsAt], [EndsAt], [UserId])
GO
CREATE NONCLUSTERED INDEX [IX_Whitelist_4] ON [Whitelist]([IsEnabled], [IpAddressV4], [StartsAt], [EndsAt])
GO

CREATE NONCLUSTERED INDEX [IX_OperationLogs_01] ON [OperationLogs]([PageRequestId])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_02] ON [OperationLogs]([OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_03] ON [OperationLogs]([OperationType_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_04] ON [OperationLogs]([TargetContextName], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_05] ON [OperationLogs]([TargetContextName], [IUDLogId], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_06] ON [OperationLogs]([UserLoginId], [FailureReason_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_07] ON [OperationLogs]([UserLoginId], [OperationType_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_08] ON [OperationLogs]([TargetContextName], [TargetEntityName], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_09] ON [OperationLogs]([TargetContextName], [FailureReason_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_10] ON [OperationLogs]([OperationType_EnumerationValue], [FailureReason_EnumerationValue], [OperationDate])
GO
CREATE NONCLUSTERED INDEX [IX_OperationLogs_11] ON [OperationLogs]([TargetContextName], [TargetEntityName], [FailureReason_EnumerationValue], [OperationDate])
GO

CREATE NONCLUSTERED INDEX [IX_ApiConsumers_1] ON [ApiConsumers]([IsEnabled], [Grants_EnumerationValue])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumers_2] ON [ApiConsumers]([IsEnabled], [Name])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumers_3] ON [ApiConsumers]([IsEnabled], [PrivateKey])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumers_4] ON [ApiConsumers]([Name], [IsEnabled])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumers_5] ON [ApiConsumers]([PrivateKey], [IsEnabled])
GO

CREATE NONCLUSTERED INDEX [IX_ApiConsumerIpAddresses_1] ON [ApiConsumerIpAddresses]([IsEnabled], [ApiConsumerId])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumerIpAddresses_2] ON [ApiConsumerIpAddresses]([IsEnabled], [IpAddressV4])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumerIpAddresses_3] ON [ApiConsumerIpAddresses]([IpAddressV4], [IsEnabled])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumerIpAddresses_4] ON [ApiConsumerIpAddresses]([ApiConsumerId], [IsEnabled])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumerIpAddresses_5] ON [ApiConsumerIpAddresses]([ApiConsumerId], [IpAddressV4], [IsEnabled])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumerIpAddresses_6] ON [ApiConsumerIpAddresses]([IpAddressV4], [ApiConsumerId], [IsEnabled])
GO

CREATE NONCLUSTERED INDEX [IX_ApiConsumerMachines_1] ON [ApiConsumerMachines]([IsEnabled], [ApiConsumerId])
GO
CREATE NONCLUSTERED INDEX [IX_ApiConsumerMachines_2] ON [ApiConsumerMachines]([ApiConsumerId], [IsEnabled])
GO

CREATE NONCLUSTERED INDEX [IX_ApiConsumerMachineVariables_1] ON [ApiConsumerMachineVariables]([ApiConsumerMachineId], [Name])
GO

CREATE NONCLUSTERED INDEX [IX_ApiRequests_1] ON [ApiRequests]([PageRequestId])
GO
CREATE NONCLUSTERED INDEX [IX_ApiRequests_2] ON [ApiRequests]([RandomToken_Checksum])
GO
CREATE NONCLUSTERED INDEX [IX_ApiRequests_3] ON [ApiRequests]([ResultFailureReason_EnumerationValue], [ResultApiConsumerId], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_ApiRequests_4] ON [ApiRequests]([ResultApiConsumerId], [ResultFailureReason_EnumerationValue], [RequestDate])
GO
CREATE NONCLUSTERED INDEX [IX_ApiRequests_5] ON [ApiRequests]([RequestDate], [ResultApiConsumerId], [ResultFailureReason_EnumerationValue])
GO
CREATE NONCLUSTERED INDEX [IX_ApiRequests_6] ON [ApiRequests]([RequestDate], [ResultFailureReason_EnumerationValue], [ResultApiConsumerId])
GO