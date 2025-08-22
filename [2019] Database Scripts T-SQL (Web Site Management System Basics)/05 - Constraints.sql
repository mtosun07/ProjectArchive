USE WebSiteBaseDB
GO


ALTER TABLE [IUDLogs] ADD CONSTRAINT [PK_IUDLogs.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [IUDLogs] ADD CONSTRAINT [DF_IUDLogs.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [IUDLogs] ADD CONSTRAINT [DF_IUDLogs.OperationDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [OperationDate]
GO
ALTER TABLE [IUDLogs] ADD CONSTRAINT [CH_IUDLogs.TableName] CHECK ([TableName] IS NULL OR [dbo].[IsEmptyString]([TableName]) = 0)
GO
ALTER TABLE [IUDLogs] ADD CONSTRAINT [CH_IUDLogs.PrimaryKeyColumnName] CHECK ([PrimaryKeyColumnName] IS NULL OR [dbo].[IsEmptyString]([PrimaryKeyColumnName]) = 0)
GO
ALTER TABLE [IUDLogs] ADD CONSTRAINT [CH_IUDLogs.PrimaryKeyDataType] CHECK ([PrimaryKeyDataType] IS NULL OR [dbo].[IsEmptyString]([PrimaryKeyDataType]) = 0)
GO
ALTER TABLE [IUDLogs] ADD CONSTRAINT [CH_IUDLogs.IsSuccess_AdditionalData] CHECK ((([IsSuccess] = 0 AND [AdditionalData] IS NOT NULL) OR [IsSuccess] = 1) AND ([AdditionalData] IS NULL OR [dbo].[IsEmptyString]([AdditionalData]) = 0))
GO
ALTER TABLE [IUDLogs] ADD CONSTRAINT [CH_IUDLogs.OperationDate] CHECK ([dbo].[IsDateInRange]([OperationDate], 300) = 1)
GO


ALTER TABLE [IUDLogValues] ADD CONSTRAINT [PK_IUDLogValues.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [IUDLogValues] ADD CONSTRAINT [FK_IUDLogValues.IUDLogId] FOREIGN KEY ([IUDLogId]) REFERENCES [IUDLogs]([Id])
GO
ALTER TABLE [IUDLogValues] ADD CONSTRAINT [DF_IUDLogValues.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [IUDLogValues] ADD CONSTRAINT [CH_IUDLogValues] CHECK ([dbo].[Check_IUDLogValues]([Id], [IUDLogId], [ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [OldValue_Binary], [NewValue_String], [NewValue_Binary]) = 1)
GO


ALTER TABLE [EventLogs] ADD CONSTRAINT [PK_EventLogs.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [EventLogs] ADD CONSTRAINT [DF_EventLogs.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [EventLogs] ADD CONSTRAINT [DF_EventLogs.HappenedAt] DEFAULT (SYSDATETIMEOFFSET()) FOR [HappenedAt]
GO
ALTER TABLE [EventLogs] ADD CONSTRAINT [CH_EventLogs.Title] CHECK ([dbo].[IsEmptyString_N]([Title]) = 0)
GO
ALTER TABLE [EventLogs] ADD CONSTRAINT [CH_EventLogs.HappenedAt] CHECK ([dbo].[IsDateInRange]([HappenedAt], 300) = 1)
GO


ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [PK_ClientIpAddresses.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [DF_ClientIpAddresses.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [DF_ClientIpAddresses.RecordDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [RecordDate]
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.IpAddressV4] CHECK ([dbo].[IsEmptyString]([IpAddressV4]) = 0 AND [dbo].[IsIpAddressV4]([IpAddressV4]) = 1)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.Message] CHECK (([Status_IsSuccess] = 1 AND [Message] IS NULL) OR ([Status_IsSuccess] = 0 AND [dbo].[IsEmptyString]([Message]) = 0))
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.Continent] CHECK ([Continent] IS NULL OR [dbo].[IsEmptyString]([Continent]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.ContinentCode] CHECK ([ContinentCode] IS NULL OR [dbo].[IsEmptyString]([ContinentCode]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.Country] CHECK ([Country] IS NULL OR [dbo].[IsEmptyString]([Country]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.CountryCode] CHECK ([CountryCode] IS NULL OR [dbo].[IsEmptyString]([CountryCode]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.Region] CHECK ([Region] IS NULL OR [dbo].[IsEmptyString]([Region]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.RegionName] CHECK ([RegionName] IS NULL OR [dbo].[IsEmptyString]([RegionName]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.City] CHECK ([City] IS NULL OR [dbo].[IsEmptyString]([City]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.District] CHECK ([District] IS NULL OR [dbo].[IsEmptyString]([District]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.ZipCode] CHECK ([ZipCode] IS NULL OR [dbo].[IsEmptyString]([ZipCode]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.TimeZone] CHECK ([TimeZone] IS NULL OR [dbo].[IsEmptyString]([TimeZone]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.Currency] CHECK ([Currency] IS NULL OR [dbo].[IsEmptyString]([Currency]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.InternetServiceProviderName] CHECK ([InternetServiceProviderName] IS NULL OR [dbo].[IsEmptyString]([InternetServiceProviderName]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.OrganizationName] CHECK ([OrganizationName] IS NULL OR [dbo].[IsEmptyString]([OrganizationName]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.AutonomousService] CHECK ([AutonomousService] IS NULL OR [dbo].[IsEmptyString]([AutonomousService]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.AutonomousServiceName] CHECK ([AutonomousServiceName] IS NULL OR [dbo].[IsEmptyString]([AutonomousServiceName]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.ReverseDnsOfIpAddress] CHECK ([ReverseDnsOfIpAddress] IS NULL OR [dbo].[IsEmptyString]([ReverseDnsOfIpAddress]) = 0)
GO
ALTER TABLE [ClientIpAddresses] ADD CONSTRAINT [CH_ClientIpAddresses.RecordDate] CHECK ([dbo].[IsDateInRange]([RecordDate], 300) = 1)
GO


ALTER TABLE [Banishments] ADD CONSTRAINT [PK_Banishments.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [Banishments] ADD CONSTRAINT [DF_Banishments.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [Banishments] ADD CONSTRAINT [DF_Banishments.StartsAt] DEFAULT (SYSDATETIMEOFFSET()) FOR [StartsAt]
GO
ALTER TABLE [Banishments] ADD CONSTRAINT [DF_Banishments.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [Banishments] ADD CONSTRAINT [CH_Banishments.BanishmentType_EnumerationValue] CHECK ([BanishmentType_EnumerationValue] > 0)
GO
ALTER TABLE [Banishments] ADD CONSTRAINT [CH_Banishments.BanishedValue] CHECK ([BanishedValue] IS NULL OR [dbo].[IsEmptyString_N]([BanishedValue]) = 0)
GO
ALTER TABLE [Banishments] ADD CONSTRAINT [CH_Banishments.EndsAt] CHECK ([EndsAt] IS NULL OR [EndsAt] > [StartsAt])
GO
ALTER TABLE [Banishments] ADD CONSTRAINT [CH_Banishments] CHECK ([dbo].[Check_Banishments]([Id], [StartsAt], [EndsAt]) = 1)
GO


ALTER TABLE [Sessions] ADD CONSTRAINT [PK_Sessions.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [Sessions] ADD CONSTRAINT [DF_Sessions.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [Sessions] ADD CONSTRAINT [DF_Sessions.StartedAt] DEFAULT (SYSDATETIMEOFFSET()) FOR [StartedAt]
GO
ALTER TABLE [Sessions] ADD CONSTRAINT [CH_Sessions.ApplicationSessionId] CHECK ([dbo].[IsEmptyString]([ApplicationSessionId]) = 0)
GO
ALTER TABLE [Sessions] ADD CONSTRAINT [CH_Sessions.SessionsVariables] CHECK ([SessionVariables] IS NULL OR [dbo].[IsEmptyString_N]([SessionVariables]) = 0)
GO
ALTER TABLE [Sessions] ADD CONSTRAINT [CH_Sessions.StartedAt] CHECK ([dbo].[IsDateInRange]([StartedAt], 60) = 1)
GO


ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [PK_AbandonedSessions.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [UQ_AbandonedSessions.SessionId] UNIQUE NONCLUSTERED ([SessionId])
GO
ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [FK_AbandonedSessions.SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Sessions]([Id])
GO
ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [DF_AbandonedSessions.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [DF_AbandonedSessions.HappenedAt] DEFAULT (SYSDATETIMEOFFSET()) FOR [HappenedAt]
GO
ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [CH_AbandonedSessions.Title] CHECK ([dbo].[IsEmptyString_N]([Title]) = 0)
GO
ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [CH_AbandonedSessions.AdditionalData] CHECK ([AdditionalData] IS NULL OR [dbo].[IsEmptyString_N]([AdditionalData]) = 0)
GO
ALTER TABLE [AbandonedSessions] ADD CONSTRAINT [CH_AbandonedSessions.HappenedAt] CHECK ([dbo].[IsDateInRange]([HappenedAt], 60) = 1)
GO


ALTER TABLE [PageRequests] ADD CONSTRAINT [PK_PageRequests.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [FK_PageRequests.SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Sessions]([Id])
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [FK_PageRequests.ClientIpAddressId] FOREIGN KEY ([ClientIpAddressId]) REFERENCES [ClientIpAddresses]([Id])
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [DF_PageRequests.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [DF_PageRequests.RequestDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [RequestDate]
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.Url] CHECK ([dbo].[IsEmptyString_N]([Url]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.Path] CHECK ([dbo].[IsEmptyString_N]([Path]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.QueryString] CHECK ([QueryString] IS NULL OR [dbo].[IsEmptyString_N]([QueryString]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.UrlReferrer] CHECK ([UrlReferrer] IS NULL OR [dbo].[IsEmptyString_N]([UrlReferrer]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.UserHostAddress] CHECK ([dbo].[IsEmptyString]([UserHostAddress]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.UserHostName] CHECK ([UserHostName] IS NULL OR [dbo].[IsEmptyString_N]([UserHostName]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.UserAgent] CHECK ([UserAgent] IS NULL OR [dbo].[IsEmptyString_N]([UserAgent]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.UserLanguages] CHECK ([UserLanguages] IS NULL OR [dbo].[IsEmptyString_N]([UserLanguages]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.HttpMethod] CHECK ([dbo].[IsEmptyString]([HttpMethod]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.ContentEncoding] CHECK ([ContentEncoding] IS NULL OR [dbo].[IsEmptyString]([ContentEncoding]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.FilesCount] CHECK ([FilesCount] >= 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.TotalBytes] CHECK ([TotalBytes] >= 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.RequestVariables] CHECK ([RequestVariables] IS NULL OR [dbo].[IsEmptyString_N]([RequestVariables]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.FailureReason_EnumerationValue] CHECK ([FailureReason_EnumerationValue] IS NULL OR [FailureReason_EnumerationValue] >= 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.AdditionalData] CHECK ([AdditionalData] IS NULL OR [dbo].[IsEmptyString_N]([AdditionalData]) = 0)
GO
ALTER TABLE [PageRequests] ADD CONSTRAINT [CH_PageRequests.RequestDate] CHECK ([dbo].[IsDateInRange]([RequestDate], 60) = 1)
GO


ALTER TABLE [FormResults] ADD CONSTRAINT [PK_FormResults.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [FormResults] ADD CONSTRAINT [FK_FormResults.PageRequestId] FOREIGN KEY ([PageRequestId]) REFERENCES [PageRequests]([Id])
GO
ALTER TABLE [FormResults] ADD CONSTRAINT [DF_FormResults.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [FormResults] ADD CONSTRAINT [DF_FormResults.RecordDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [RecordDate]
GO
ALTER TABLE [FormResults] ADD CONSTRAINT [CH_FormResults.FormName] CHECK ([dbo].[IsEmptyString_N]([FormName]) = 0)
GO
ALTER TABLE [FormResults] ADD CONSTRAINT [CH_FormResults.RecordDate] CHECK ([dbo].[IsDateInRange]([RecordDate], 300) = 1)
GO


ALTER TABLE [FormResultComponents] ADD CONSTRAINT [PK_FormResultComponents.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [UQ_FormResultComponents.FormResultIdAndOrder] UNIQUE NONCLUSTERED ([FormResultId], [Order])
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [FK_FormResultComponents.FormResultId] FOREIGN KEY ([FormResultId]) REFERENCES [FormResults]([Id])
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [DF_FormResultComponents.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [CH_FormResultComponents.Order] CHECK ([Order] > 0)
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [CH_FormResultComponents.DisplayName] CHECK ([dbo].[IsEmptyString_N]([DisplayName]) = 0)
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [CH_FormResultComponents.ValueTypeName] CHECK ([ValueTypeName] IS NULL OR [dbo].[IsEmptyString]([ValueTypeName]) = 0)
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [CH_FormResultComponents.ValueTypeNameAndValueTypesEnumerationValue] CHECK ([ValueTypeName] IS NOT NULL OR [ValueTypes_EnumerationValue] > 0)
GO
ALTER TABLE [FormResultComponents] ADD CONSTRAINT [CH_FormResultComponents.Value] CHECK ([dbo].[Check_FormResultComponents_Value]([ValueTypes_EnumerationValue], [Value_String], [Value_Binary]) = 1)
GO


ALTER TABLE [SentEmails] ADD CONSTRAINT [PK_SentEmails.Id] PRIMARY KEY CLUSTERED (Id ASC)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [DF_SentEmails.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [DF_SentEmails.RecordDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [RecordDate]
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.ToAddress] CHECK ([dbo].[IsEmptyString]([ToAddress]) = 0)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.Subject] CHECK ([Subject] IS NULL OR [dbo].[IsEmptyString_N]([Subject]) = 0)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.Message] CHECK ([Message] IS NULL OR [dbo].[IsEmptyString_N]([Message]) = 0)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.Encoding] CHECK ([Encoding] IS NULL OR [dbo].[IsEmptyString_N]([Encoding]) = 0)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.FromDisplayName] CHECK ([FromDisplayName] IS NULL OR [dbo].[IsEmptyString_N]([FromDisplayName]) = 0)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.ToDisplayName] CHECK ([ToDisplayName] IS NULL OR [dbo].[IsEmptyString_N]([ToDisplayName]) = 0)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.FromAddress] CHECK ([FromAddress] IS NULL OR [dbo].[IsEmptyString]([FromAddress]) = 0)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.RecordDate] CHECK ([dbo].[IsDateInRange]([RecordDate], 300) = 1)
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.FailureDate] CHECK ([FailureDate] IS NULL OR ([SentDate] IS NOT NULL AND [FailureDate] >= [SentDate]))
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.SentDate] CHECK (([SentDate] IS NOT NULL AND [FromAddress] IS NOT NULL AND [SentDate] >= [RecordDate]) OR ([SentDate] IS NULL AND [FromAddress] IS NULL))
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.FailureReason_EnumerationValue] CHECK ([FailureReason_EnumerationValue] IS NULL OR ([FailureReason_EnumerationValue] > 0 AND [FailureDate] IS NOT NULL))
GO
ALTER TABLE [SentEmails] ADD CONSTRAINT [CH_SentEmails.AdditionalData] CHECK ([AdditionalData] IS NULL OR [dbo].[IsEmptyString_N]([AdditionalData]) = 0)
GO


ALTER TABLE [SentEmailAttachments] ADD CONSTRAINT [PK_SentEmailAttachments.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [SentEmailAttachments] ADD CONSTRAINT [FK_SentEmailAttachments.SentEmailId] FOREIGN KEY ([SentEmailId]) REFERENCES [SentEmails]([Id])
GO
ALTER TABLE [SentEmailAttachments] ADD CONSTRAINT [DF_SentEmailAttachments.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [SentEmailAttachments] ADD CONSTRAINT [CH_SentEmailAttachments.ContentType] CHECK ([dbo].[IsEmptyString_N]([ContentType]) = 0)
GO
ALTER TABLE [SentEmailAttachments] ADD CONSTRAINT [CH_SentEmailAttachments.FilePath] CHECK ([dbo].[IsEmptyString_N]([FilePath]) = 0)
GO
ALTER TABLE [SentEmailAttachments] ADD CONSTRAINT [CH_SentEmailAttachments.DisplayFileName] CHECK (dbo.[IsEmptyString_N]([DisplayFileName]) = 0)
GO


ALTER TABLE [ErrorLogs] ADD CONSTRAINT [PK_ErrorLogs.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [ErrorLogs] ADD CONSTRAINT [FK_ErrorLogs.PageRequestId] FOREIGN KEY ([PageRequestId]) REFERENCES [PageRequests]([Id])
GO
ALTER TABLE [ErrorLogs] ADD CONSTRAINT [DF_ErrorLogs.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [ErrorLogs] ADD CONSTRAINT [DF_ErrorLogs.OccuredDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [OccuredAt]
GO
ALTER TABLE [ErrorLogs] ADD CONSTRAINT [CH_ErrorLogs.ErrorType] CHECK ([dbo].[IsEmptyString_N]([ErrorType]) = 0)
GO
ALTER TABLE [ErrorLogs] ADD CONSTRAINT [CH_ErrorLogs.ErrorMessage] CHECK ([ErrorMessage] IS NULL OR [dbo].[IsEmptyString_N]([ErrorMessage]) = 0)
GO
ALTER TABLE [ErrorLogs] ADD CONSTRAINT [CH_ErrorLogs.AdditionalData] CHECK ([AdditionalData] IS NULL OR [dbo].[IsEmptyString_N]([AdditionalData]) = 0)
GO
ALTER TABLE [ErrorLogs] ADD CONSTRAINT [CH_ErrorLogs.OccuredAt] CHECK ([dbo].[IsDateInRange]([OccuredAt], 60) = 1)
GO


ALTER TABLE [UserGroups] ADD CONSTRAINT [PK_UserGroups.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [UserGroups] ADD CONSTRAINT [FK_UserGroups.SupUserGroupId] FOREIGN KEY ([SupUserGroupId]) REFERENCES [UserGroups]([Id])
GO
ALTER TABLE [UserGroups] ADD CONSTRAINT [DF_UserGroups.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [UserGroups] ADD CONSTRAINT [DF_UserGroups.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [UserGroups] ADD CONSTRAINT [CH_UserGroups.Name] CHECK ([dbo].[Check_UserGroups_Name]([Id], [Name]) = 1)
GO
ALTER TABLE [UserGroups] ADD CONSTRAINT [CH_UserGroups.SupUserGroupId] CHECK ([SupUserGroupId] IS NULL OR [dbo].[CheckIfEnabled_UserGroups]([SupUserGroupId]) = 1)
GO


ALTER TABLE [Users] ADD CONSTRAINT [PK_Users.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [Users] ADD CONSTRAINT [FK_Users.UserGroupId] FOREIGN KEY ([UserGroupId]) REFERENCES [UserGroups]([Id])
GO
ALTER TABLE [Users] ADD CONSTRAINT [DF_Users.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [Users] ADD CONSTRAINT [DF_Users.Is2FAActive] DEFAULT (0) FOR [Is2FAActive]
GO
ALTER TABLE [Users] ADD CONSTRAINT [DF_Users.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [Users] ADD CONSTRAINT [CH_Users.EmailAddress] CHECK ([dbo].[Check_Users_EmailAddress]([Id], [EmailAddress]) = 1)
GO
ALTER TABLE [Users] ADD CONSTRAINT [CH_Users.Username] CHECK ([dbo].[Check_Users_Username]([Id], [Username]) = 1)
GO
ALTER TABLE [Users] ADD CONSTRAINT [CH_Users.Password] CHECK ([dbo].[IsEmptyString_N]([Password]) = 0)
GO
ALTER TABLE [Users] ADD CONSTRAINT [CH_Users.FirstName] CHECK ([dbo].[IsEmptyString_N]([FirstName]) = 0)
GO
ALTER TABLE [Users] ADD CONSTRAINT [CH_Users.LastName] CHECK ([dbo].[IsEmptyString_N]([LastName]) = 0)
GO
ALTER TABLE [Users] ADD CONSTRAINT [CH_Users.Is2FAActive] CHECK ([dbo].[Check_Users_Is2FAActive]([Id], [EmailAddress], [Is2FAActive]) = 1)
GO
ALTER TABLE [Users] ADD CONSTRAINT [CH_Users.UserGroupId] CHECK ([UserGroupId] IS NULL OR [dbo].[CheckIfEnabled_UserGroups]([UserGroupId]) = 1)
GO


ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [PK_UserVerificationTokens.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [FK_UserVerificationTokens.UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
GO
ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [DF_UserVerificationTokens.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [DF_UserVerificationTokens.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [DF_UserVerificationTokens.VerificationToken] DEFAULT ([dbo].[GetRandomVerificationToken]()) FOR [VerificationToken]
GO
ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [DF_UserVerificationTokens.VerificationTokenValidUntil] DEFAULT (DATEADD(MINUTE, 5, SYSDATETIMEOFFSET())) FOR [VerificationTokenValidUntil]
GO
ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [CH_UserVerificationTokens.AdditionalData] CHECK ([AdditionalData] IS NULL OR [dbo].[IsEmptyString_N]([AdditionalData]) = 0)
GO
--ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [CH_UserVerificationTokens.IsEnabled] CHECK ([dbo].[Check_UserVerificationTokens_IsEnabled]([Id], [IsEnabled]) = 1)
--GO
ALTER TABLE [UserVerificationTokens] ADD CONSTRAINT [CH_UserVerificationTokens.UserId] CHECK ([dbo].[Check_UserVerificationTokens_UserId]([UserId]) = 1)
GO


ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [PK_UserVerificationAttempts.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [FK_UserVerificationAttempts.PageRequestId] FOREIGN KEY ([PageRequestId]) REFERENCES [PageRequests]([Id])
GO
ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [FK_UserVerificationAttempts.ResultUserVerificationTokenId] FOREIGN KEY ([ResultUserVerificationTokenId]) REFERENCES [UserVerificationTokens]([Id])
GO
ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [DF_UserVerificationAttempts.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [DF_UserVerificationAttempts.AttemptDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [AttemptDate]
GO
ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [CH_UserVerificationAttempts] CHECK ([dbo].[Check_UserVerificationAttempts]([Id], [VerificationType_EnumerationValue], [UserId], [VerificationToken], [AdditionalData], [ResultUserVerificationTokenId], [ResultFailureReason_EnumerationValue]) = 1)
GO
ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [CH_UserVerificationAttempts.AttemptDate] CHECK ([dbo].[IsDateInRange]([AttemptDate], 60) = 1)
GO
ALTER TABLE [UserVerificationAttempts] ADD CONSTRAINT [CH_UserVerificationAttempts.ResultFailureReason_EnumerationValue] CHECK ([ResultFailureReason_EnumerationValue] IS NULL OR ResultFailureReason_EnumerationValue >= 0)
GO


ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [PK_UserLoginAttempts.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [FK_UserLoginAttempts.ResultUserId] FOREIGN KEY ([ResultUserId]) REFERENCES [Users]([Id])
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [FK_UserLoginAttempts.ResultUserVerificationTokenId] FOREIGN KEY ([ResultUserVerificationTokenId]) REFERENCES [UserVerificationTokens]([Id])
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [FK_UserLoginAttempts.PageRequestId] FOREIGN KEY ([PageRequestId]) REFERENCES [PageRequests]([Id])
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [DF_UserLoginAttempts.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [DF_UserLoginAttempts.AttemptDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [AttemptDate]
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [CH_UserLoginAttempts] CHECK ([dbo].[Check_UserLoginAttempts_UserVerificationTokenId]([Id], [ResultUserVerificationTokenId]) = 1)
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [CH_UserLoginAttempts.AttemptDate] CHECK ([dbo].[IsDateInRange]([AttemptDate], 60) = 1)
GO
ALTER TABLE [UserLoginAttempts] ADD CONSTRAINT [CH_UserLoginAttempts.ResultFailureReason_EnumerationValue] CHECK ([ResultFailureReason_EnumerationValue] IS NULL OR ResultFailureReason_EnumerationValue >= 0)
GO


ALTER TABLE [UserLogins] ADD CONSTRAINT [PK_UserLogins.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [UserLogins] ADD CONSTRAINT [FK_UserLogins.UserLoginAttemptId] FOREIGN KEY ([UserLoginAttemptId]) REFERENCES [UserLoginAttempts]([Id])
GO
ALTER TABLE [UserLogins] ADD CONSTRAINT [FK_UserLogins.UserVerificationAttemptId] FOREIGN KEY ([UserVerificationAttemptId]) REFERENCES [UserVerificationAttempts]([Id])
GO
ALTER TABLE [UserLogins] ADD CONSTRAINT [UQ_UserLogins.UserLoginAttemptId] UNIQUE ([UserLoginAttemptId])
GO
ALTER TABLE [UserLogins] ADD CONSTRAINT [DF_UserLogins.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [UserLogins] ADD CONSTRAINT [DF_UserLogins.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [UserLogins] ADD CONSTRAINT [CH_UserLogins] CHECK ([dbo].[Check_UserLogins]([Id], [UserLoginAttemptId], [UserVerificationAttemptId]) = 1)
GO


ALTER TABLE [Whitelist] ADD CONSTRAINT [PK_Whitelist.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [Whitelist] ADD CONSTRAINT [FK_Whitelist.UserId] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
GO
ALTER TABLE [Whitelist] ADD CONSTRAINT [DF_Whitelist.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [Whitelist] ADD CONSTRAINT [DF_Whitelist.StartsAt] DEFAULT (SYSDATETIMEOFFSET()) FOR [StartsAt]
GO
ALTER TABLE [Whitelist] ADD CONSTRAINT [DF_Whitelist.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [Whitelist] ADD CONSTRAINT [CH_Whitelist.IpAddressV4] CHECK ([dbo].[IsEmptyString]([IpAddressV4]) = 0 AND [dbo].[IsIpAddressV4]([IpAddressV4]) = 1)
GO
ALTER TABLE [Whitelist] ADD CONSTRAINT [CH_Whitelist.EndsAt] CHECK ([EndsAt] IS NULL OR [EndsAt] > [StartsAt])
GO
ALTER TABLE [Whitelist] ADD CONSTRAINT [CH_Whitelist] CHECK ([dbo].[Check_Whitelist]([Id], [UserId], [IpAddressV4], [StartsAt], [EndsAt]) = 1)
GO


ALTER TABLE [OperationLogs] ADD CONSTRAINT [PK_OperationLogs.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [FK_OperationLogs.PageRequestId] FOREIGN KEY ([PageRequestId]) REFERENCES [PageRequests]([Id])
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [FK_OperationLogs.UserLoginId] FOREIGN KEY ([UserLoginId]) REFERENCES [UserLogins]([Id])
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [DF_OperationLogs.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [DF_OperationLogs.OperationDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [OperationDate]
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.TargetContextName] CHECK ([TargetContextName] IS NULL OR [dbo].[IsEmptyString]([TargetContextName]) = 0)
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.OperationType_EnumerationValue] CHECK ([OperationType_EnumerationValue] > 0)
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.TargetEntityName] CHECK ([TargetEntityName] IS NULL OR ([TargetContextName] IS NOT NULL AND [TargetEntityName] IS NOT NULL AND [dbo].[IsEmptyString]([TargetEntityName]) = 0))
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.IUDLogId] CHECK ([IUDLogId] IS NULL OR ([TargetEntityName] IS NOT NULL AND [IUDLogId] IS NOT NULL))
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.AdditionalData] CHECK ([AdditionalData] IS NULL OR [dbo].[IsEmptyString_N]([AdditionalData]) = 0)
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.OperationDate] CHECK ([dbo].[IsDateInRange]([OperationDate], 60) = 1)
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.FailureReason_EnumerationValue] CHECK ([FailureReason_EnumerationValue] IS NULL OR [FailureReason_EnumerationValue] >= 0)
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.TargetContextNameAndIUDLogId] CHECK ([dbo].[Check_OperationLogs_TargetContextNameAndIUDLogId]([Id], [TargetContextName], [IUDLogId]) = 1)
GO
ALTER TABLE [OperationLogs] ADD CONSTRAINT [CH_OperationLogs.UserLoginId] CHECK ([dbo].[CheckIfEnabled_UserLogins]([UserLoginId]) = 1)
GO


ALTER TABLE [ApiConsumers] ADD CONSTRAINT [PK_ApiConsumers.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [ApiConsumers] ADD CONSTRAINT [DF_ApiConsumers.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [ApiConsumers] ADD CONSTRAINT [DF_ApiConsumers.PrivateKey] DEFAULT ([dbo].[GetRandomPassword](20, 100)) FOR [PrivateKey]
GO
ALTER TABLE [ApiConsumers] ADD CONSTRAINT [DF_ApiConsumers.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [ApiConsumers] ADD CONSTRAINT [CH_ApiConsumers.Grants_EnumerationValue] CHECK ([Grants_EnumerationValue] >= 0)
GO
ALTER TABLE [ApiConsumers] ADD CONSTRAINT [CH_ApiConsumers.Name] CHECK ([dbo].[Check_ApiConsumers_Name]([Id], [Name]) = 1)
GO
ALTER TABLE [ApiConsumers] ADD CONSTRAINT [CH_ApiConsumers.PrivateKey] CHECK ([dbo].[IsEmptyString_N]([PrivateKey]) = 0)
GO


ALTER TABLE [ApiConsumerIpAddresses] ADD CONSTRAINT [PK_ApiConsumerIpAddresses.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [ApiConsumerIpAddresses] ADD CONSTRAINT [FK_ApiConsumerIpAddresses.ApiConsumerId] FOREIGN KEY ([ApiConsumerId]) REFERENCES [ApiConsumers]([Id])
GO
ALTER TABLE [ApiConsumerIpAddresses] ADD CONSTRAINT [DF_ApiConsumerIpAddresses.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [ApiConsumerIpAddresses] ADD CONSTRAINT [DF_ApiConsumerIpAddresses.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO
ALTER TABLE [ApiConsumerIpAddresses] ADD CONSTRAINT [CH_ApiConsumerIpAddresses.IpAddressV4] CHECK ([dbo].[Check_ApiConsumerIpAddresses_IpAddressV4]([Id], [ApiConsumerId], [IpAddressV4]) = 1)
GO


ALTER TABLE [ApiConsumerMachines] ADD CONSTRAINT [PK_ApiConsumerMachines.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [ApiConsumerMachines] ADD CONSTRAINT [FK_ApiConsumerMachines.ApiConsumerId] FOREIGN KEY ([ApiConsumerId]) REFERENCES [ApiConsumers]([Id])
GO
ALTER TABLE [ApiConsumerMachines] ADD CONSTRAINT [DF_ApiConsumerMachines.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [ApiConsumerMachines] ADD CONSTRAINT [DF_ApiConsumerMachines.IsEnabled] DEFAULT (1) FOR [IsEnabled]
GO


ALTER TABLE [ApiConsumerMachineVariables] ADD CONSTRAINT [PK_ApiConsumerMachineVariables.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [ApiConsumerMachineVariables] ADD CONSTRAINT [FK_ApiConsumerMachineVariables.ApiConsumerMachineId] FOREIGN KEY ([ApiConsumerMachineId]) REFERENCES [ApiConsumerMachines]([Id])
GO
ALTER TABLE [ApiConsumerMachineVariables] ADD CONSTRAINT [DF_ApiConsumerMachineVariables.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [ApiConsumerMachineVariables] ADD CONSTRAINT [CH_ApiConsumerMachineVariables.ApiConsumerMachineIdAndName] CHECK ([dbo].[Check_ApiConsumerMachineVariables_ApiConsumerMachineIdAndName]([Id], [ApiConsumerMachineId], [Name]) = 1)
GO


ALTER TABLE [ApiRequests] ADD CONSTRAINT [PK_ApiRequests.Id] PRIMARY KEY CLUSTERED ([Id] ASC)
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [FK_ApiRequests.PageRequestId] FOREIGN KEY ([PageRequestId]) REFERENCES [PageRequests]([Id])
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [FK_ApiRequests.ResultApiConsumerId] FOREIGN KEY ([ResultApiConsumerId]) REFERENCES [ApiConsumers]([Id])
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [DF_ApiRequests.Id] DEFAULT (NEWID()) FOR [Id]
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [DF_ApiRequests.RequestDate] DEFAULT (SYSDATETIMEOFFSET()) FOR [RequestDate]
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [CH_ApiRequests.RandomToken] CHECK ([dbo].[Check_ApiRequests_RandomToken]([Id], [RandomToken]) = 1)
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [CH_ApiRequests.PrivateKey] CHECK ([PrivateKey] IS NULL OR [dbo].[IsEmptyString_N]([PrivateKey]) = 0)
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [CH_ApiRequests.MachineVariablesJson] CHECK ([MachineVariablesJson] IS NULL OR ([dbo].[IsEmptyString_N]([MachineVariablesJson]) = 0 AND ISJSON([MachineVariablesJson]) > 0))
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [CH_ApiRequests.ResultFailureReason_EnumerationValue] CHECK ([ResultFailureReason_EnumerationValue] IS NULL OR [ResultFailureReason_EnumerationValue] >= 0)
GO
ALTER TABLE [ApiRequests] ADD CONSTRAINT [CH_ApiRequests.RequestDate] CHECK ([dbo].[IsDateInRange]([RequestDate], 60) = 1)
GO