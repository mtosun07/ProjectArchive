USE WebSiteBaseDB
GO



CREATE TABLE [IUDLogs]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[OperationType_EnumerationValue] TINYINT NOT NULL,
	-- 0: Other
	-- 1: Insert
	-- 2: Update
	-- 3: Enable
	-- 4: Disable
	-- 5: Delete
	[IsSuccess] BIT NOT NULL,
	[TableName] VARCHAR(128) NULL,
	[PrimaryKeyColumnName] VARCHAR(128) NULL,
	[PrimaryKeyDataType] VARCHAR(128) NULL,
	[PrimaryKeyValue] NVARCHAR(MAX) NULL,
	[AdditionalData] NVARCHAR(MAX) NULL,
	[OperationDate] DATETIMEOFFSET NOT NULL,
	[PrimaryKeyValue_Checksum] AS CHECKSUM(UPPER([PrimaryKeyValue]))
)
GO

CREATE TABLE [IUDLogValues]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[IUDLogId] UNIQUEIDENTIFIER NOT NULL,
	[ColumnName] VARCHAR(128) NOT NULL,
	[ColumnDataType] VARCHAR(128) NOT NULL,
	[IsNullable] BIT NOT NULL,
	[OldValue_String] NVARCHAR(MAX) NULL,
	[OldValue_Binary] VARBINARY(MAX) NULL,
	[NewValue_String] NVARCHAR(MAX) NULL,
	[NewValue_Binary] VARBINARY(MAX) NULL
)
GO

CREATE TABLE [EventLogs]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Title] NVARCHAR(200) NOT NULL,
    [AdditionalData] NVARCHAR(MAX) NULL,
	[HappenedAt] DATETIMEOFFSET NOT NULL
)
GO

CREATE TABLE [ClientIpAddresses]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[RecordDate] DATETIMEOFFSET NOT NULL,
    [IpAddressV4] VARCHAR(15) NOT NULL,
	[Status_IsSuccess] BIT NOT NULL,
	[Message] VARCHAR(200) NULL,
	[Continent] VARCHAR(20) NULL,
	[ContinentCode] VARCHAR(5) NULL,
	[Country] VARCHAR(50) NULL,
	[CountryCode] VARCHAR(5) NULL,
	[Region] VARCHAR(100) NULL,
	[RegionName] VARCHAR(200) NULL,
	[City] VARCHAR(200) NULL,
	[District] VARCHAR(200) NULL,
	[ZipCode] VARCHAR(20) NULL,
	[Latitude] FLOAT NULL,
	[Longitude] FLOAT NULL,
	[TimeZone] VARCHAR(100) NULL,
	[TimeZoneUtcDstOffsetInSeconds] INT NULL,
	[Currency] VARCHAR(10) NULL,
	[InternetServiceProviderName] VARCHAR(200) NULL,
	[OrganizationName] VARCHAR(200) NULL,
	[AutonomousService] VARCHAR(200) NULL,
	[AutonomousServiceName] VARCHAR(200) NULL,
	[ReverseDnsOfIpAddress] VARCHAR(255) NULL,
	[IsMobile] BIT NULL,
	[IsProxyOrVpnOrTorExitAddress] BIT NULL,
	[IsHostingOrColocatedOrDataCenter] BIT NULL
)
GO

CREATE TABLE [Banishments]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[BanishmentType_EnumerationValue] TINYINT NOT NULL,
	--  0: NONE
	--  1: Banished User Agent
	--  2: Banished Ip Address
	--  3: Banished Language
	--  4: Banished Time Zone
	--  5: Banished Location
	--  6: Banished User Group
	--  7: Banished User
	[BanishedValue] NVARCHAR(MAX) NOT NULL,
	[StartsAt] DATETIMEOFFSET NOT NULL,
	[EndsAt]  DATETIMEOFFSET NULL,
	[IsEnabled] BIT NOT NULL,
	[BanishedValue_Checksum] AS CHECKSUM(UPPER([BanishedValue]))
)
GO

CREATE TABLE [Sessions]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [StartedAt] DATETIMEOFFSET NOT NULL,
    [ApplicationSessionId] VARCHAR(100) NOT NULL,
	[InitialClientIpAddressId] UNIQUEIDENTIFIER NOT NULL,
	[SessionVariables] NVARCHAR(MAX) NULL
)
GO

CREATE TABLE [AbandonedSessions]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [SessionId] UNIQUEIDENTIFIER NOT NULL, -- unique
	[HappenedAt] DATETIMEOFFSET NOT NULL,
	[Title] NVARCHAR(200) NOT NULL,
    [AdditionalData] NVARCHAR(MAX) NULL
)
GO

CREATE TABLE [PageRequests]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [SessionId] UNIQUEIDENTIFIER NOT NULL,
	[RequestDate] DATETIMEOFFSET NOT NULL,
    [ClientIpAddressId] UNIQUEIDENTIFIER NOT NULL,
	[Url] NVARCHAR(MAX) NOT NULL,
	[Path] NVARCHAR(MAX) NOT NULL,
	[QueryString] NVARCHAR(MAX) NULL,
	[UrlReferrer] NVARCHAR(MAX) NULL,
	[UserHostAddress] NVARCHAR(50) NOT NULL,
	[UserHostName] NVARCHAR(MAX) NULL,
	[UserAgent] NVARCHAR(MAX) NULL,
	[UserLanguages] NVARCHAR(MAX) NULL,
	[IsSecureConnection] BIT NOT NULL,
	[HttpMethod] VARCHAR(50) NOT NULL,
	[ContentEncoding] VARCHAR(100) NULL,
	[FilesCount] INT NOT NULL,
	[TotalBytes] BIGINT NOT NULL,
	[RequestVariables] NVARCHAR(MAX) NULL,
	[FailureReason_EnumerationValue] INT NULL, -- check on stp not on trg | no check constraint
	--        0: Other
	--        1: Abandoned Session
	--        2: Different Ip Address
	--        4: Unauthorized
	--        8: Incorrect Path
	--       16: Incorrect Query String
	--       32: Incorrect Files
	--       64: Incorrect HTTP Method
	--      128: Bot Detected
	--      256: VPN Detected
	--      512: Banished User Agent
	--     1024: Banished IP Address
	--     2048: Banished Language
	--     4096: Banished Time Zone
	--     8192: Banished Location
	--    16384: IP Address not in Whitelist
    [AdditionalData] NVARCHAR(MAX) NULL,
	[UrlReferrer_Checksum] AS CHECKSUM(UPPER([UrlReferrer])),
	[Url_Checksum] AS CHECKSUM(UPPER([Url])),
	[Path_Checksum] AS CHECKSUM(UPPER([Path]))
)
GO

CREATE TABLE [FormResults]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[PageRequestId] UNIQUEIDENTIFIER NOT NULL,	
	[RecordDate] DATETIMEOFFSET NOT NULL,
	[FormName] NVARCHAR(200) NOT NULL
)
GO

CREATE TABLE [FormResultComponents]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[FormResultId] UNIQUEIDENTIFIER NOT NULL,
    [Order] INT NOT NULL,
	[DisplayName] NVARCHAR(200) NOT NULL,
	[ValueTypes_EnumerationValue] TINYINT NOT NULL,
		--  0: Other
		--  1: Html
		--  2: Xml
		--  3: Json
		--  4: Binary
		--  5: FilePath
		--  6: Url
		--  7: String
		--  8: Char
		--  9: Boolean
		-- 10: Guid
		-- 11: Byte
		-- 12: Sbyte
		-- 13: Short
		-- 14: Ushort
		-- 15: Int
		-- 16: Uint
		-- 17: Long
		-- 18: Ulong
		-- 19: Float
		-- 20: Double
		-- 21: Decimal
		-- 22: DateTime
		-- 23: DateTimeUTC
		-- 24: TimeSpan
	[ValueTypeName] VARCHAR(300) NULL,
    [Value_String] NVARCHAR(MAX) NULL,
    [Value_Binary] VARBINARY(MAX) NULL
)
GO

CREATE TABLE [SentEmails]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[RecordDate] DATETIMEOFFSET NOT NULL,
	[ToAddress] VARCHAR(300) NOT NULL,
	[FromDisplayName] VARCHAR(100) NULL,
	[ToDisplayName] VARCHAR(100) NULL,
	[Subject] NVARCHAR(MAX) NULL,
	[Message] NVARCHAR(MAX) NULL,
	[Encoding] VARCHAR(10) NULL,
	[IsMessageHtml] BIT NOT NULL,
	[SentDate] DATETIMEOFFSET NULL,
	[FromAddress] VARCHAR(300) NULL,
	[FailureDate] DATETIMEOFFSET NULL,
	[FailureReason_EnumerationValue] INT NULL,
    [AdditionalData] NVARCHAR(MAX) NULL
)
GO

CREATE TABLE [SentEmailAttachments]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[SentEmailId] UNIQUEIDENTIFIER NOT NULL,
	[FilePath] NVARCHAR(MAX) NOT NULL,
	[DisplayFileName] NVARCHAR(200) NOT NULL,
	[ContentType] NVARCHAR(100) NULL
)
GO

CREATE TABLE [ErrorLogs]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[PageRequestId] UNIQUEIDENTIFIER NULL,
	[ErrorType] NVARCHAR(200) NOT NULL,
    [ErrorMessage] NVARCHAR(MAX) NULL,
    [AdditionalData] NVARCHAR(MAX) NULL,
	[OccuredAt] DATETIMEOFFSET NOT NULL
)
GO

CREATE TABLE [UserGroups]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[SupUserGroupId] UNIQUEIDENTIFIER NULL,
	[Name] NVARCHAR(100) NOT NULL,
	[Grants] VARBINARY(MAX) NOT NULL,
	[IsEnabled] BIT NOT NULL
)
GO

CREATE TABLE [Users]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[UserGroupId] UNIQUEIDENTIFIER NULL,
    [EmailAddress] VARCHAR(300) NOT NULL, -- unique | must be lower case
	[Username] VARCHAR(50) NOT NULL, -- unique | must be lower case (min 3 max 20 chars)
    [Password] NVARCHAR(MAX) NOT NULL, -- encrypted
    [FirstName] NVARCHAR(200) NOT NULL, -- capitalized
    [LastName] NVARCHAR(200) NOT NULL, -- capitalized
	[Is2FAActive] BIT NOT NULL,
	[Grants] VARBINARY(MAX) NOT NULL, -- derived from grants
	[IsEnabled] BIT NOT NULL
)
GO

CREATE TABLE [UserVerificationTokens]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[AdditionalData] NVARCHAR(MAX) NULL, -- e.g. email address
	[VerificationType_EnumerationValue] TINYINT NOT NULL,
		-- 0: NONE
		-- 1: Verify E-mail Address
		-- 2: Change Password
		-- 3: 2FA Log in
	[VerificationToken] VARCHAR(50) NOT NULL,
	[VerificationTokenValidUntil] DATETIMEOFFSET NOT NULL,
    [IsEnabled] BIT NOT NULL, -- turns 0 when re-sent
	[AdditionalData_Checksum] AS CHECKSUM(UPPER([AdditionalData]))
)
GO

CREATE TABLE [UserVerificationAttempts]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [PageRequestId] UNIQUEIDENTIFIER NOT NULL,
	[AttemptDate] DATETIMEOFFSET NOT NULL,
	[VerificationType_EnumerationValue] TINYINT NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
	[VerificationToken] NVARCHAR(MAX) NULL,
	[AdditionalData] NVARCHAR(MAX) NULL,
	[ResultUserVerificationTokenId] UNIQUEIDENTIFIER NULL, -- unique when not null
	[ResultFailureReason_EnumerationValue] INT NULL, -- check on trg not on stp | no check constraint
	--		0: Other
	--		1: Page Request Failure
	--		2: Not in Whitelist
	--		4: Incorrect Query
	--		8: Incorrect Token
	--     16: Out of date
	--	   32: Incorrect Additional Data
	--	   64: Disabled Verification Token
	--	  128: Disabled User
	--    256: Banished User Group
	--    512: Banished User
	--	 1024: Not in Whitelist (User)
	[AdditionalData_Checksum] AS CHECKSUM(UPPER([AdditionalData]))
)
GO

CREATE TABLE [UserLoginAttempts]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [PageRequestId] UNIQUEIDENTIFIER NOT NULL,
    [AttemptDate] DATETIMEOFFSET NOT NULL,
    [Username] NVARCHAR(MAX) NOT NULL, -- must be lower case
    [Password] NVARCHAR(MAX) NOT NULL, -- encrypted
	[ResultUserId] UNIQUEIDENTIFIER NULL,
	[ResultUserVerificationTokenId] UNIQUEIDENTIFIER NULL, -- if valued, that means 2FA activated -- unique when not null
	[ResultFailureReason_EnumerationValue] INT NULL, -- check on trg not on stp | no check constraint
	--		0: Other
	--		1: Page Request Failure
	--		2: Not in Whitelist
	--		4: Incorrect Username
	--		8: Incorrect Password
	--	   16: No Verified Email Address
	--	   32: Email Address Verified for Another User
	--	   64: Disabled User
	--    128: Banished User Group
	--    256: Banished User
	--	  512: Not in Whitelist (User)
    [Username_Checksum] AS CHECKSUM([Username])
)
GO

CREATE TABLE [UserLogins]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[UserLoginAttemptId] UNIQUEIDENTIFIER NOT NULL, -- unique
	[UserVerificationAttemptId] UNIQUEIDENTIFIER NULL, -- 2FA -- unique when not null
	[IsEnabled] BIT NOT NULL
)
GO

CREATE TABLE [Whitelist] -- if any record where UserId is null, then no logins except this table
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NULL,
    [IpAddressV4] VARCHAR(15) NOT NULL, -- unique with userid
	[StartsAt] DATETIMEOFFSET NOT NULL,
	[EndsAt]  DATETIMEOFFSET NULL,
	[IsEnabled] BIT NOT NULL
)
GO

CREATE TABLE [OperationLogs]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[OperationDate] DATETIMEOFFSET NOT NULL,
	[OperationType_EnumerationValue] BIGINT NOT NULL,
	[PageRequestId] UNIQUEIDENTIFIER NOT NULL,
	[UserLoginId] UNIQUEIDENTIFIER NOT NULL,
	[TargetContextName] VARCHAR(50) NULL,
	[TargetEntityName] VARCHAR(100) NULL,
	[IUDLogId] UNIQUEIDENTIFIER NULL,
	[FailureReason_EnumerationValue] INT NULL, -- check programatically
	--		0: Other
	--		1: Disabled Login
	--		2: Disabled User
	--		4: Not Granted
	--		8: Banished User Group
	--	   16: Banished User
	--	   32: Different Session
	[AdditionalData] NVARCHAR(MAX) NULL
)
GO


CREATE TABLE [ApiConsumers]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(100) NOT NULL, -- unique
	[PrivateKey] NVARCHAR(300) NOT NULL, --encrypted by name
	[Grants_EnumerationValue] INT NOT NULL,
	--    0: NONE
	--    1: Emails
	[IsEnabled] BIT NOT NULL
)
GO


CREATE TABLE [ApiConsumerIpAddresses]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[ApiConsumerId] UNIQUEIDENTIFIER NOT NULL,
	[IpAddressV4] VARCHAR(15) NOT NULL, -- unique with api consumer id
	[IsEnabled] BIT NOT NULL
)
GO


CREATE TABLE [ApiConsumerMachines]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[ApiConsumerId] UNIQUEIDENTIFIER NOT NULL,
	[IsEnabled] BIT NOT NULL
)
GO


CREATE TABLE [ApiConsumerMachineVariables]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[ApiConsumerMachineId] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(300) NOT NULL, -- unique with machine id
	[Value] NVARCHAR(MAX) NULL
)
GO


CREATE TABLE [ApiRequests]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[PageRequestId] UNIQUEIDENTIFIER NOT NULL,
	[RequestDate] DATETIMEOFFSET NOT NULL,
	[RandomToken] NVARCHAR(MAX) NULL, -- to check unique
	[PrivateKey] NVARCHAR(MAX) NULL, -- encrypted by PageRequestId
	[MachineVariablesJson] NVARCHAR(MAX) NULL,
	[ResultApiConsumerId] UNIQUEIDENTIFIER NULL,
	[ResultFailureReason_EnumerationValue] INT NULL,
	--   0: Other
	--   1: Page Request Failure
	--   2: No Random Token
	--   4: Unexpected Random Token
	--   8: No Private Key
	--  16: Incorrect Private Key
	--  32: Unexpected IP Address
	--  64: Unexpected Machine Variables
	-- 128: Disabled API Consumer
	-- 256: Used Page Request
	[RandomToken_Checksum] AS CHECKSUM([RandomToken])
)
GO