USE WebSiteBaseDB
GO



CREATE PROC RaiseDeletingError
AS
	SET NOCOUNT ON;
	RAISERROR('Disallowed rows were getting deleted.', 16, 1);
GO

CREATE PROC RaiseUpdatingError
AS
	SET NOCOUNT ON;
	RAISERROR('Disallowed columns were getting updated.', 16, 1);
GO

CREATE PROC RaiseInsertingError
AS
	SET NOCOUNT ON;
	RAISERROR('Invalid values were getting inserted.', 16, 1);
GO

CREATE PROC RaiseSpecificError
(
	@message NVARCHAR(MAX)
)
AS
	SET NOCOUNT ON;
	DECLARE @msg NVARCHAR(MAX) = LTRIM(RTRIM(ISNULL(@message, N'An error occured.')));
	RAISERROR(@msg, 16, 1);
GO



CREATE PROCEDURE ExportLogsToXml
(
	@cleanupLogs BIT,
	@xml XML OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @operationTypes TABLE
	(
		[Value] TINYINT PRIMARY KEY,
		[Name] VARCHAR(20)
	);
	INSERT INTO @operationTypes VALUES
		(0, 'Other'),
		(1, 'Insert'),
		(2, 'Update'),
		(3, 'Enable'),
		(4, 'Disable'),
		(5, 'Delete');
	SET @xml =
	(
		SELECT
				t1.[Id] AS [@Id],
				t1.[OperationDate] AS [@OperationDate],
				t2.[Name] AS [OperationType],
				(CASE WHEN t1.[IsSuccess] = 0 THEN 'False' ELSE 'True' END) AS [IsSuccess],
				t1.[AdditionalData] AS [AdditionalData],
				t1.[TableName] AS [TableInfo/@Name],
				t1.[PrimaryKeyColumnName] AS [TableInfo/PrimaryKeyColumnName],
				t1.[PrimaryKeyDataType] AS [TableInfo/PrimaryKeyDataType],
				t1.[PrimaryKeyValue] AS [Values/@PrimaryKeyValue],
				(
					SELECT
							t3.[ColumnName] AS [@Name],
							t3.[ColumnDataType] AS [@DataType],
							t3.[IsNullable] AS [@IsNullable],
							t3.[OldValue_String] AS [@OldValue_String],
							CONCAT('<![CDATA[', CONVERT(NVARCHAR(MAX), t3.[OldValue_Binary]), ']]>') AS [@OldValue_Binary],
							t3.[NewValue_String] AS [@NewValue_String],
							CONCAT('<![CDATA[', CONVERT(NVARCHAR(MAX), t3.[NewValue_Binary]), ']]>') AS [@NewValue_Binary]
						FROM [IUDLogValues] t3
						WHERE t3.[IUDLogId] = t1.[Id]
						FOR XML
							PATH('Column'),
							ROOT('Values')
				) AS [Values]
			FROM [IUDLogs] t1
			LEFT JOIN @operationTypes t2
				ON t2.[Value] = t1.[OperationType_EnumerationValue]
			FOR XML
				PATH('Record'),
				ROOT('Logs')
	);
	DECLARE
		@values [dbo].[typeIUDLogValue],
		@logId UNIQUEIDENTIFIER;
	EXEC [dbo].[Insert_IUDLogs] 0, 1, NULL, NULL, NULL, NULL, 'Logs were exported as XML.', @values, @logId OUTPUT;
	IF @cleanupLogs = 1
		DELETE FROM IUDLogs;
GO



CREATE PROCEDURE EncryptUserPassword
(
	@username VARCHAR(50),
	@password NVARCHAR(MAX),
	@result NVARCHAR(MAX) OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @authenticator VARBINARY(MAX) = HASHBYTES('SHA1', CONVERT(VARBINARY(MAX), @username));
	OPEN SYMMETRIC KEY SyK_Users
		DECRYPTION BY CERTIFICATE Cert_Users;
	SET @result = CONVERT(NVARCHAR(MAX), ENCRYPTBYKEY(KEY_GUID('SyK_Users'), @password, 1, @authenticator));
	CLOSE SYMMETRIC KEY SyK_Users;
GO

CREATE PROCEDURE DecryptUserPassword
(
	@username VARCHAR(50),
	@password_encrypted NVARCHAR(MAX),
	@result NVARCHAR(MAX) OUTPUT
)
AS
	SET NOCOUNT ON;
	OPEN SYMMETRIC KEY SyK_Users
		DECRYPTION BY CERTIFICATE Cert_Users;
	SET @result = CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(@password_encrypted, 1, HASHBYTES('SHA1', CONVERT(VARBINARY(MAX), @username))));
	CLOSE SYMMETRIC KEY SyK_Users;
GO

CREATE PROCEDURE EncryptApiConsumerPrivateKey
(
	@authKey NVARCHAR(100),
	@privateKey NVARCHAR(MAX),
	@result NVARCHAR(MAX) OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @authenticator VARBINARY(MAX) = HASHBYTES('SHA1', CONVERT(VARBINARY(MAX), @authKey));
	OPEN SYMMETRIC KEY SyK_ApiConsumers
		DECRYPTION BY CERTIFICATE Cert_ApiConsumers;
	SET @result = CONVERT(NVARCHAR(MAX), ENCRYPTBYKEY(KEY_GUID('SyK_Users'), @privateKey, 1, @authenticator));
	CLOSE SYMMETRIC KEY SyK_ApiConsumers;
GO

CREATE PROCEDURE DecryptApiConsumerPrivateKey
(
	@authKey NVARCHAR(100),
	@privateKey_encrypted NVARCHAR(MAX),
	@result NVARCHAR(MAX) OUTPUT
)
AS
	SET NOCOUNT ON;
	OPEN SYMMETRIC KEY SyK_ApiConsumers
		DECRYPTION BY CERTIFICATE Cert_ApiConsumers;
	SET @result = CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(@privateKey_encrypted, 1, HASHBYTES('SHA1', CONVERT(VARBINARY(MAX), @authKey))));
	CLOSE SYMMETRIC KEY SyK_ApiConsumers;
GO

CREATE PROCEDURE IsVerifiedOrPendingEmailAddress
(
	@emailAddress VARCHAR(300),
	@isVerified BIT OUTPUT,
	@userId UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @emailAddress_ VARCHAR(300) = LTRIM(RTRIM(ISNULL(LOWER(@emailAddress COLLATE SQL_Latin1_General_CP1253_CI_AI), '')));
	IF LEN(@emailAddress_) > 0
		SELECT
				@userId = t1.[Id],
				@isVerified =
					CASE
						WHEN t3.[Id] IS NOT NULL
							THEN 1
						ELSE 0
					END
			FROM [Users] t1
			LEFT JOIN [UserVerificationTokens] t2 ON
				t2.[UserId] = t1.[Id]
			LEFT JOIN [UserVerificationAttempts] t3 ON
				t3.[ResultUserVerificationTokenId] = t2.[Id] AND
				t3.[ResultFailureReason_EnumerationValue] IS NULL
			WHERE
				t1.[EmailAddress] LIKE @emailAddress_ AND
				t2.[IsEnabled] = 1 AND
				t2.[VerificationType_EnumerationValue] = 1 AND
				t2.[AdditionalData] = @emailAddress_ AND
				(t2.[VerificationTokenValidUntil] < SYSDATETIMEOFFSET() OR t3.[Id] IS NOT NULL)
		IF @isVerified IS NULL
			SET @isVerified = 0;
GO

CREATE PROCEDURE IsUsernameAvailable
(
	@username VARCHAR(MAX),
	@isAvailable BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @un VARCHAR(MAX) = LTRIM(RTRIM(@username));
	IF @un IS NULL OR [dbo].[IsValidUsername](@un) = 0
		BEGIN
			SET @isAvailable = 0;
			RETURN;
		END
	SET @isAvailable =
		CASE
			WHEN EXISTS(SELECT 1 FROM [Users] WHERE [Username] = @un)
				THEN 0
			ELSE 1
		END;
	RETURN;
GO



CREATE PROCEDURE IsBanishedUser
(
	@userId UNIQUEIDENTIFIER,
	@isBanished BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @now DATETIMEOFFSET = SYSDATETIME();
	DECLARE @string VARCHAR(100) = CONVERT(VARCHAR(100), @userId);
	DECLARE @checksum INT = CHECKSUM(@string);
	IF EXISTS(
		SELECT
				1
			FROM [Banishments]
			WHERE
				[IsEnabled] = 1 AND
				[BanishmentType_EnumerationValue] = 7 AND
				[BanishedValue_Checksum] = @checksum AND
				[BanishedValue] = @string AND
				[StartsAt] <= @now AND
				([EndsAt] IS NULL OR [EndsAt] > @now))
		SET @isBanished = 1;
	ELSE
		SET @isBanished = 0;
	RETURN;
GO

CREATE PROCEDURE IsBanishedUserGroup
(
	@userGroupId UNIQUEIDENTIFIER,
	@isBanished BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @now DATETIMEOFFSET = SYSDATETIME();
	DECLARE @string VARCHAR(100) = CONVERT(VARCHAR(100), @userGroupId);
	DECLARE @checksum INT = CHECKSUM(@string);
	IF EXISTS(
		SELECT
				1
			FROM [Banishments]
			WHERE
				[IsEnabled] = 1 AND
				[BanishmentType_EnumerationValue] = 6 AND
				[BanishedValue_Checksum] = @checksum AND
				[BanishedValue] = @string AND
				[StartsAt] <= @now AND
				([EndsAt] IS NULL OR [EndsAt] > @now))
		SET @isBanished = 1;
	ELSE
		SET @isBanished = 0;
	RETURN;
GO

CREATE PROCEDURE IsBanishedIpAddressV4
(
	@ipAddressV4 VARCHAR(15),
	@isBanished BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @now DATETIMEOFFSET = SYSDATETIME();
	DECLARE @checksum INT = CHECKSUM(@ipAddressV4);
	IF EXISTS(
		SELECT
				1
			FROM [Banishments]
			WHERE
				[IsEnabled] = 1 AND
				[BanishmentType_EnumerationValue] = 2 AND
				[BanishedValue_Checksum] = @checksum AND
				[BanishedValue] = @ipAddressV4 AND
				[StartsAt] <= @now AND
				([EndsAt] IS NULL OR [EndsAt] > @now))
		SET @isBanished = 1;
	ELSE
		SET @isBanished = 0;
	RETURN;
GO

CREATE PROCEDURE GetWhitelistState
(
	@userId UNIQUEIDENTIFIER,
	@clientIpAddressV4 VARCHAR(15),
	@state TINYINT OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @now DATETIMEOFFSET = SYSDATETIMEOFFSET();
	DECLARE @checksumIp INT = CHECKSUM(@clientIpAddressV4);
	DECLARE @whitelist TABLE(
		[IpAddressV4] VARCHAR(15) NOT NULL
	);
	SET @state = 0;
	IF @userId IS NOT NULL
		INSERT INTO @whitelist
			SELECT
					[IpAddressV4]
				FROM [Whitelist]
				WHERE
					[IsEnabled] = 1 AND
					[UserId] = @userId AND
					[StartsAt] <= @now AND
					([EndsAt] IS NULL OR [EndsAt] > @now);
	IF
		EXISTS(SELECT 1 FROM @whitelist) AND
		NOT EXISTS(SELECT 1 FROM @whitelist WHERE [IpAddressV4] = @clientIpAddressV4)
		SET @state = 1;
	ELSE
		BEGIN
			DELETE FROM @whitelist;
			INSERT INTO @whitelist
				SELECT
						[IpAddressV4]
					FROM [Whitelist]
					WHERE
						[IsEnabled] = 1 AND
						[UserId] = NULL AND
						[StartsAt] <= @now AND
						([EndsAt] IS NULL OR [EndsAt] > @now);
			IF
				EXISTS(SELECT 1 FROM @whitelist) AND
				NOT EXISTS(SELECT 1 FROM @whitelist WHERE [IpAddressV4] = @clientIpAddressV4)
				SET @state = 2;
		END
	RETURN;
GO

CREATE PROCEDURE IsAuthorizedIpAddressV4
(
	@userId UNIQUEIDENTIFIER,
	@ipAddressV4 VARCHAR(15),
	@isAuthorized BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @isBanished BIT;
	EXEC [dbo].[IsBanishedIpAddressV4] @userId, @ipAddressV4, @isBanished OUTPUT;
	IF @isBanished = 0
		BEGIN
			DECLARE @state TINYINT;
			EXEC [dbo].[GetWhitelistState] @userId, @ipAddressV4, @state OUTPUT;
			SET @isAuthorized =
				CASE
					WHEN @state = 0
						THEN 1
					ELSE 0
				END;
		END
	ELSE
		SET @isAuthorized = 0;
	RETURN;
	RETURN;
GO

CREATE PROCEDURE GetSentEmailState
(
	@sentEmailId UNIQUEIDENTIFIER,
	@state TINYINT OUTPUT
	-- 0: Not Found
	-- 1: Sent
	-- 2: Pending
	-- 3: Failed
)
AS
	SET NOCOUNT ON;
	SET @state = 0;
	SELECT TOP 1
			@state =
				CASE
					WHEN [FailureReason_EnumerationValue] IS NOT NULL
						THEN 3
					WHEN [SentDate] IS NOT NULL
						THEN 1
					ELSE 2
				END
		FROM [SentEmails]
		WHERE
			[Id] = @sentEmailId;
	RETURN;
GO

CREATE PROC GetVerificationTokenValidUntil
(
	@date DATETIMEOFFSET OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @date = DATEADD(MINUTE, 10, SYSDATETIMEOFFSET());
GO

CREATE PROCEDURE IsVeryFirstPassword
(
	@userId UNIQUEIDENTIFIER,
	@result BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @result =
		CASE
			WHEN EXISTS(
				SELECT
						1
				FROM [IUDLogs] t1
				INNER JOIN [IUDLogValues] t2
				ON
					t1.[Id] = t2.[IUDLogId] AND
					t2.[ColumnName] = 'Password'
				WHERE
					t1.[TableName] = 'Users' AND
					t1.[IsSuccess] = 1 AND
					t1.[OperationType_EnumerationValue] = 2 AND
					CONVERT(UNIQUEIDENTIFIER, t1.[PrimaryKeyValue]) = @userId AND
					t2.[OldValue_String] <> t2.[NewValue_String])
				THEN 0
			ELSE 1
		END;
GO

CREATE PROC IsSamePasswordWithRecents
(
	@count INT,
	@userId UNIQUEIDENTIFIER,
	@newPassword NVARCHAR(MAX),
	@found BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @found = 0;
	DECLARE @currentUserName VARCHAR(50);
	DECLARE @currentPassword NVARCHAR(MAX);
	SELECT
			@currentUserName = [Username],
			@currentPassword = [Password]
		FROM [Users]
		WHERE
			[Id] = @userId;
	IF @currentUserName IS NULL
		RETURN;
	DECLARE @passwords TABLE
	(
		[Id] UNIQUEIDENTIFIER,
		[Date] DATETIMEOFFSET,
		[Password] NVARCHAR(MAX)
	);
	INSERT INTO @passwords ([Id], [Date], [Password])
		SELECT TOP (@count)
				t1.[Id],
				t1.[OperationDate],
					CASE
						WHEN t2.[Id] IS NULL
							THEN @currentPassword
						ELSE t2.[NewValue_String]
					END
			FROM [IUDLogs] t1
			LEFT JOIN [IUDLogValues] t2 ON
				t2.[IUDLogId] = t1.[Id] AND
				t2.[ColumnName] = 'Password'
			WHERE
				(t1.[OperationType_EnumerationValue] = 1 OR t1.[OperationType_EnumerationValue] = 2) AND
				t1.[IsSuccess] = 1 AND
				t1.[TableName] = 'Users' AND
				CONVERT(UNIQUEIDENTIFIER, t1.[PrimaryKeyValue]) = @userId
			ORDER BY
				t1.[OperationDate] DESC;
	DECLARE @id UNIQUEIDENTIFIER;
	DECLARE @date DATETIMEOFFSET;
	DECLARE @password NVARCHAR(MAX);
	DECLARE _cursorPasswords CURSOR FOR
		SELECT
				[Id],
				[Date],
				[Password]
			FROM @passwords;
	OPEN _cursorPasswords;
	FETCH NEXT FROM _cursorPasswords INTO @id, @date, @password;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @username VARCHAR(50);
			SELECT TOP 1
					@username =
						CASE
							WHEN t2.[Id] IS NULL
								THEN @currentUserName
							ELSE t2.[NewValue_String]
						END
				FROM [IUDLogs] t1
				LEFT JOIN [IUDLogValues] t2 ON
					t2.[IUDLogId] = t1.[Id] AND
					t2.[ColumnName] = 'Username'
				WHERE
					(t1.[OperationType_EnumerationValue] = 1 OR t1.[OperationType_EnumerationValue] = 2) AND
					t1.[IsSuccess] = 1 AND
					t1.[TableName] = 'Users' AND
					CONVERT(UNIQUEIDENTIFIER, t1.[PrimaryKeyValue]) = @userId AND
					t1.[OperationDate] <= @date
				ORDER BY
					t1.[OperationDate] DESC;
			DECLARE @decrypted NVARCHAR(MAX);
			EXEC [dbo].[DecryptUserPassword] @username, @password, @decrypted OUTPUT;
			IF @decrypted = @newPassword
				BEGIN
					SET @found = 1;
					BREAK;
				END
			FETCH NEXT FROM _cursorPasswords INTO @id, @date, @password;
		END
	CLOSE _cursorPasswords;
	DEALLOCATE _cursorPasswords;
GO

CREATE PROCEDURE GetApiConsumerId
(
	@privateKey NVARCHAR(300),
	@apiConsumerId UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	IF @privateKey IS NOT NULL
		BEGIN
			DECLARE @id UNIQUEIDENTIFIER;
			DECLARE @name_ NVARCHAR(100);
			DECLARE @privateKey_ NVARCHAR(300);
			DECLARE @decrypted NVARCHAR(300);
			DECLARE _cursor_stp_getapiconsumerid CURSOR FOR
				SELECT
						[Id],
						[Name],
						[PrivateKey]
					FROM [ApiConsumers];
			OPEN _cursor_stp_getapiconsumerid;
			FETCH NEXT FROM _cursor_stp_getapiconsumerid INTO @id, @name_, @privateKey_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					EXEC [dbo].[DecryptApiConsumerPrivateKey] @id, @name_, @privateKey_, @decrypted OUTPUT;
					IF @privateKey = @decrypted
						BEGIN
							SET @apiConsumerId = @id;
							BREAK;
						END
					FETCH NEXT FROM _cursor_stp_getapiconsumerid INTO @name_, @privateKey_;
				END
			CLOSE _cursor_stp_getapiconsumerid;
			DEALLOCATE _cursor_stp_getapiconsumerid;
		END
	RETURN;
GO



CREATE PROCEDURE Insert_IUDLogs
(
	@operationType_EnumerationValue TINYINT,
	@isSuccess BIT,
	@tableName VARCHAR(128),
	@primaryKeyColumnName VARCHAR(128),
	@primaryKeyDataType VARCHAR(128),
	@primaryKeyValue NVARCHAR(MAX),
	@additionalData NVARCHAR(MAX),
	@values [dbo].[typeIUDLogValue] READONLY,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [IUDLogs] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [IUDLogs] ([Id], [OperationType_EnumerationValue], [IsSuccess], [TableName], [PrimaryKeyColumnName], [PrimaryKeyDataType], [PrimaryKeyValue], [AdditionalData])
		VALUES (@id, @operationType_EnumerationValue, @isSuccess, @tableName, @primaryKeyColumnName, @primaryKeyDataType, @primaryKeyValue, @additionalData);
	INSERT INTO [IUDLogValues] ([IUDLogId], [ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [OldValue_Binary], [NewValue_String], [NewValue_Binary])
		SELECT @id, t1.[ColumnName], t1.[ColumnDataType], t1.[IsNullable], t1.[OldValue_String], t1.[OldValue_Binary], t1.[NewValue_String], t1.[NewValue_Binary] FROM @values t1;
	RETURN;
GO

CREATE PROCEDURE Delete_IUDLogs
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [IUDLogValues] WHERE [IUDLogId] = @id;
	DELETE [IUDLogs] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_EventLogs
(
	@title NVARCHAR(200),
    @additionalData NVARCHAR(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [EventLogs] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [EventLogs] ([Id], [Title], [AdditionalData])
		VALUES (@id, @title, @additionalData);
	RETURN;
GO

CREATE PROCEDURE Delete_EventLogs
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [EventLogs] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_ClientIpAddresses
(
	@ipAddressV4 VARCHAR(15),
	@status_IsSuccess BIT,
	@message VARCHAR(200),
	@continent VARCHAR(20),
	@continentCode VARCHAR(5),
	@country VARCHAR(50),
	@countryCode VARCHAR(5),
	@region VARCHAR(100),
	@regionName VARCHAR(200),
	@city VARCHAR(200),
	@district VARCHAR(200),
	@zipCode VARCHAR(20),
	@latitude FLOAT,
	@longitude FLOAT,
	@timeZone VARCHAR(100),
	@timeZoneUtcDstOffsetInSeconds INT,
	@currency VARCHAR(10),
	@internetServiceProviderName VARCHAR(200),
	@organizationName VARCHAR(200),
	@autonomousService VARCHAR(200),
	@autonomousServiceName VARCHAR(200),
	@reverseDnsOfIpAddress VARCHAR(255),
	@isMobile BIT,
	@isProxyOrVpnOrTorExitAddress BIT,
	@isHostingOrColocatedOrDataCenter BIT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @existingId UNIQUEIDENTIFIER;
	SELECT TOP 1 @existingId = Id
		FROM [ClientIpAddresses]
		WHERE
			[Status_IsSuccess] = @status_IsSuccess AND
			[dbo].[NullAwareComparison]([Latitude], @latitude) = 1 AND
			[dbo].[NullAwareComparison]([Longitude], @longitude) = 1 AND
			[dbo].[NullAwareComparison]([TimeZoneUtcDstOffsetInSeconds], @timeZoneUtcDstOffsetInSeconds) = 1 AND
			[dbo].[NullAwareComparison]([IsMobile], @isMobile) = 1 AND
			[dbo].[NullAwareComparison]([IsProxyOrVpnOrTorExitAddress], @isProxyOrVpnOrTorExitAddress) = 1 AND
			[dbo].[NullAwareComparison]([IsHostingOrColocatedOrDataCenter], @isHostingOrColocatedOrDataCenter) = 1 AND
			[IpAddressV4] = @ipAddressV4 AND
			[dbo].[NullAwareComparison_VarChar]([Message], @message) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([Continent], @continent) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([Country], @country) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([RegionName], @regionName) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([City], @city) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([District], @district) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([InternetServiceProviderName], @internetServiceProviderName) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([OrganizationName], @organizationName) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([AutonomousService], @autonomousService) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([AutonomousServiceName], @autonomousServiceName) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([ReverseDnsOfIpAddress], @reverseDnsOfIpAddress) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([Currency], @currency) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([ContinentCode], @continentCode) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([CountryCode], @countryCode) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([Region], @region) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([ZipCode], @zipCode) = 1 AND
			[dbo].[NullAwareComparison_VarChar]([TimeZone], @timeZone) = 1
		ORDER BY [RecordDate] DESC;
	IF [dbo].[IsNullOrEmpty_Guid](@existingId) = 0
		BEGIN
			SET @id = @existingId;
			RETURN;
		END
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [ClientIpAddresses] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [ClientIpAddresses] ([Id], [IpAddressV4], [Status_IsSuccess], [Message], [Continent], [ContinentCode], [Country], [CountryCode], [Region], [RegionName], [City], [District], [ZipCode], [Latitude], [Longitude], [TimeZone], [TimeZoneUtcDstOffsetInSeconds], [Currency], [InternetServiceProviderName], [OrganizationName], [AutonomousService], [AutonomousServiceName], [ReverseDnsOfIpAddress], [IsMobile], [IsProxyOrVpnOrTorExitAddress], [IsHostingOrColocatedOrDataCenter])
		VALUES (@id, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter);
	RETURN;
GO

CREATE PROCEDURE Delete_ClientIpAddresses
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [ClientIpAddresses] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_Banishments
(
	@banismentType_EnumerationValue TINYINT,
	@banishedValue NVARCHAR(MAX),
	@startsAt DATETIMEOFFSET,
	@endsAt  DATETIMEOFFSET,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [Banishments] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [Banishments] ([Id], [BanishmentType_EnumerationValue], [BanishedValue], [StartsAt], [EndsAt])
		VALUES (@id, @banismentType_EnumerationValue, @banishedValue, @startsAt, @endsAt);
	RETURN;
GO

CREATE PROCEDURE Update_Banishments
(
	@id UNIQUEIDENTIFIER,
	@startsAt DATETIMEOFFSET,
	@endsAt  DATETIMEOFFSET
)
AS
	SET NOCOUNT ON;
	UPDATE [Banishments]
		SET
			[StartsAt] = @startsAt,
			[EndsAt] = @endsAt
		WHERE [Id] = @id;
	RETURN;
GO

CREATE PROCEDURE Enable_Banishments
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [Banishments]
		SET
			[IsEnabled] = 1
		WHERE
			[Id] = @id AND
			[IsEnabled] = 0;
	RETURN;
GO

CREATE PROCEDURE Disable_Banishments
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [Banishments]
		SET
			[IsEnabled] = 0
		WHERE
			[Id] = @id AND
			[IsEnabled] = 1;
	RETURN;
GO

CREATE PROCEDURE Delete_Banishments
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [Banishments] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_Sessions
(
    @applicationSessionId VARCHAR(100),
    @initialClientIpAddressId UNIQUEIDENTIFIER,
	@sessionVariables NVARCHAR(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [Sessions] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [Sessions] ([Id], [ApplicationSessionId], [InitialClientIpAddressId], [SessionVariables])
		VALUES (@id, @applicationSessionId, @initialClientIpAddressId, @sessionVariables);
	RETURN;
GO

CREATE PROCEDURE Delete_Sessions
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [Sessions] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_AbandonedSessions
(
    @sessionId UNIQUEIDENTIFIER,
	@title NVARCHAR(200),
    @additionalData NVARCHAR(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [AbandonedSessions] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [AbandonedSessions] ([Id], [SessionId], [Title], [AdditionalData])
		VALUES (@id, @sessionId, @title, @additionalData);
	RETURN;
GO

CREATE PROCEDURE Delete_AbandonedSessions
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [AbandonedSessions] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_PageRequests
(
	@failsWhenDifferentIpAddress BIT,
    @sessionId UNIQUEIDENTIFIER,
    @clientIpAddressId UNIQUEIDENTIFIER,
	@url NVARCHAR(MAX),
	@path NVARCHAR(MAX),
	@queryString NVARCHAR(MAX),
	@urlReferrer NVARCHAR(MAX),
	@userHostAddress NVARCHAR(50),
	@userHostName NVARCHAR(MAX),
	@userAgent NVARCHAR(MAX),
	@userLanguages NVARCHAR(MAX),
	@isSecureConnection BIT,
	@httpMethod VARCHAR(50),
	@contentEncoding VARCHAR(100),
	@filesCount INT,
	@totalBytes BIGINT,
	@requestVariables NVARCHAR(MAX),
	@failureReason_EnumerationValue INT,
	@additionalData NVARCHAR(MAX),
	@resultFailureReason_EnumerationValue INT OUTPUT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @now DATETIMEOFFSET = SYSDATETIMEOFFSET();
	DECLARE @ipAddressV4 VARCHAR(15);
	SELECT @ipAddressV4 = [IpAddressV4] FROM [ClientIpAddresses] WHERE [Id] = @clientIpAddressId;
	DECLARE @userAgent_cheksum INT = CHECKSUM(@userAgent);
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [PageRequests] WHERE [Id] = @id)
		SET @id = NEWID();
	SET @resultFailureReason_EnumerationValue = @failureReason_EnumerationValue;
	IF EXISTS(SELECT 1 FROM [AbandonedSessions] WHERE [SessionId] = @sessionId)
		SET @resultFailureReason_EnumerationValue = @resultFailureReason_EnumerationValue | 1;
	IF @failsWhenDifferentIpAddress = 1
		BEGIN
			DECLARE @initialIpAddressV4 VARCHAR(15);
			SELECT
					@initialIpAddressV4 = t2.[IpAddressV4]
				FROM [Sessions] t1
				INNER JOIN [ClientIpAddresses] t2 ON
					t2.[Id] = t1.[InitialClientIpAddressId]
				WHERE
					t1.[Id] = @sessionId;
			IF @initialIpAddressV4 <> @ipAddressV4
				SET @resultFailureReason_EnumerationValue = @resultFailureReason_EnumerationValue | 2;
		END
	IF EXISTS(
		SELECT
				1
			FROM [Banishments]
			WHERE
				[IsEnabled] = 1 AND
				[StartsAt] <= @now AND
				([EndsAt] IS NULL OR [EndsAt] > @now) AND
				[BanishmentType_EnumerationValue] = 1 AND
				[BanishedValue_Checksum] = @userAgent_cheksum AND
				[BanishedValue] = @userAgent)
		SET @resultFailureReason_EnumerationValue = @resultFailureReason_EnumerationValue | 512;
	DECLARE @ipAddressV4_cheksum INT = CHECKSUM(@ipAddressV4);
	IF EXISTS(
		SELECT
				1
			FROM [Banishments]
			WHERE
				[IsEnabled] = 1 AND
				[StartsAt] <= @now AND
				([EndsAt] IS NULL OR [EndsAt] > @now) AND
				[BanishmentType_EnumerationValue] = 2 AND
				[BanishedValue_Checksum] = @ipAddressV4_cheksum AND
				[BanishedValue] = @ipAddressV4)
		SET @resultFailureReason_EnumerationValue = @resultFailureReason_EnumerationValue | 1024;
	INSERT INTO [PageRequests] ([Id], [SessionId], [ClientIpAddressId], [Url], [Path], [QueryString], [UrlReferrer], [UserHostAddress], [UserHostName], [UserAgent],[UserLanguages], [IsSecureConnection], [HttpMethod], [ContentEncoding], [FilesCount], [TotalBytes], [RequestVariables], [FailureReason_EnumerationValue], [AdditionalData])
		VALUES (@id, @sessionId, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData);
	RETURN;
GO

CREATE PROCEDURE Delete_PageRequests
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [PageRequests] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_FormResults
(
	@pageRequestId UNIQUEIDENTIFIER,
	@formName NVARCHAR(200),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT * FROM [FormResults] WHERE Id = @id)
		SET @id = NEWID();
	INSERT INTO [FormResults] ([Id], [PageRequestId], [FormName])
		VALUES (@id, @pageRequestId, @formName);
	RETURN;
GO

CREATE PROCEDURE Delete_FormResults
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [FormResultComponents] WHERE [FormResultId] = @id;
	DELETE [FormResults] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_FormResultComponents
(
	@formResultId UNIQUEIDENTIFIER,
    @order INT,
	@displayName NVARCHAR(200),
	@valueTypes_EnumerationValue TINYINT,
	@valueTypeName VARCHAR(300),
    @value_String NVARCHAR(MAX),
    @value_Binary VARBINARY(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT * FROM [FormResultComponents] WHERE Id = @id)
		SET @id = NEWID();
	INSERT INTO [FormResultComponents] ([Id], [FormResultId], [Order], [DisplayName], [ValueTypes_EnumerationValue], [ValueTypeName], [Value_String], [Value_Binary])
		VALUES (@id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary);
	RETURN;
GO

CREATE PROCEDURE Delete_FormResultComponents
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [FormResultComponents] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_SentEmails
(
	@toAddress VARCHAR(300),
	@fromDisplayName VARCHAR(100),
	@toDisplayName VARCHAR(100),
	@subject NVARCHAR(MAX),
	@message NVARCHAR(MAX),
	@encoding VARCHAR(10),
	@isMessageHtml BIT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT * FROM [SentEmails] WHERE Id = @id)
		SET @id = NEWID();
	INSERT INTO [SentEmails] ([Id], [ToAddress], [FromDisplayName], [ToDisplayName], [Subject], [Message], [Encoding], [IsMessageHtml])
		VALUES (@id, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml);
	RETURN;
GO

CREATE PROCEDURE Update_SentEmails_Sent
(
	@id UNIQUEIDENTIFIER,
	@fromAddress VARCHAR(300),
	@sentDate DATETIMEOFFSET
)
AS
	SET NOCOUNT ON;
	UPDATE [SentEmails]
		SET
			[FromAddress] = @fromAddress,
			[SentDate] = @sentDate
		WHERE
			[Id] = @id
GO

CREATE PROCEDURE Update_SentEmails_Failure
(
	@id UNIQUEIDENTIFIER,
	@failureDate DATETIMEOFFSET,
	@failureReasons_EnumerationValue INT,
	@additionalData NVARCHAR(MAX)
)
AS
	SET NOCOUNT ON;
	UPDATE [SentEmails]
		SET
			[FailureDate] = @failureDate,
			[FailureReason_EnumerationValue] = @failureReasons_EnumerationValue,
			[AdditionalData] = @additionalData
		WHERE
			[Id] = @id
GO

CREATE PROCEDURE Update_SentEmails_RemoveFailure
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [SentEmails]
		SET
			[SentDate] = NULL,
			[FromAddress] = NULL,
			[FailureDate] = NULL,
			[FailureReason_EnumerationValue] = NULL,
			[AdditionalData] = NULL
		WHERE
			[Id] = @id AND
			[FailureReason_EnumerationValue] IS NOT NULL
GO

CREATE PROCEDURE Delete_SentEmails
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [SentEmailAttachments] WHERE [SentEmailId] = @id;
	DELETE [SentEmails] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_SentEmailAttachments
(
	@sentEmailId UNIQUEIDENTIFIER,
	@filePath NVARCHAR(MAX),
	@displayFileName NVARCHAR(200),
	@contentType NVARCHAR(100),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT * FROM [SentEmailAttachments] WHERE Id = @id)
		SET @id = NEWID();
	INSERT INTO [SentEmailAttachments] ([Id], [SentEmailId], [FilePath], [DisplayFileName], [ContentType])
		VALUES (@id, @sentEmailId, @filePath, @displayFileName, @contentType);
	RETURN;
GO

CREATE PROCEDURE Delete_SentEmailAttachments
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [SentEmailAttachments] WHERE [Id] = @id;
	RETURN;
GO


CREATE PROCEDURE Insert_ErrorLogs
(
	@pageRequestId UNIQUEIDENTIFIER,
	@errorType NVARCHAR(200),
    @errorMessage NVARCHAR(MAX),
    @additionalData NVARCHAR(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT * FROM [ErrorLogs] WHERE Id = @id)
		SET @id = NEWID();
	INSERT INTO [ErrorLogs] ([Id], [PageRequestId], [ErrorType], [ErrorMessage], [AdditionalData])
		VALUES (@id, @pageRequestId, @errorType, @errorMessage, @additionalData);
	RETURN;
GO

CREATE PROCEDURE Delete_ErrorLogs
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [ErrorLogs] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Instert_UserGroups
(
	@supUserGroupId UNIQUEIDENTIFIER,
	@name NVARCHAR(100),
	@grants VARBINARY(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [UserGroups] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [UserGroups] ([Id], [SupUserGroupId], [Name], [Grants])
		VALUES (@id, @supUserGroupId, @name, @grants);
	RETURN;
GO

CREATE PROCEDURE Update_UserGroups
(
	@id UNIQUEIDENTIFIER,
	@supUserGroupId UNIQUEIDENTIFIER,
	@name NVARCHAR(100),
	@grants VARBINARY(MAX)
)
AS
	SET NOCOUNT ON;
	UPDATE [UserGroups]
		SET
			[SupUserGroupId] = @supUserGroupId,
			[Name] = @name,
			[Grants] = @grants
		WHERE [Id] = @id;
	RETURN;
GO

CREATE PROCEDURE Enable_UserGroups
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [UserGroups] SET [IsEnabled] = 1 WHERE [Id] = @id AND [IsEnabled] = 0;
	RETURN;
GO

CREATE PROCEDURE Disable_UserGroups
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [UserGroups] SET [IsEnabled] = 0 WHERE [Id] = @id AND [IsEnabled] = 1;
	RETURN;
GO

CREATE PROCEDURE Delete_UserGroups_DeleteRelativesToo
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	WITH RecursiveCTE AS (
		SELECT [Id], [SupUserGroupId] FROM [UserGroups] WHERE [Id] = @id
		UNION ALL
		SELECT ug.[Id], ug.[SupUserGroupId] FROM [UserGroups] ug
		INNER JOIN RecursiveCTE cte ON ug.[SupUserGroupId] = cte.[Id])
	UPDATE [Users] SET [UserGroupId] = NULL WHERE [UserGroupId] IN (SELECT [Id] FROM RecursiveCTE);
	DELETE FROM [UserGroups] WHERE [Id] IN (SELECT [Id] FROM RecursiveCTE);
	RETURN;
GO

CREATE PROCEDURE Delete_UserGroups_DestroyRelativity
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	WITH RecursiveCTE AS (
		SELECT [Id], [SupUserGroupId] FROM [UserGroups] WHERE [Id] = @id
		UNION ALL
		SELECT ug.[Id], ug.[SupUserGroupId] FROM [UserGroups] ug
		INNER JOIN RecursiveCTE cte ON ug.[SupUserGroupId] = cte.[Id])
	UPDATE [Users] SET [UserGroupId] = NULL WHERE [UserGroupId] IN (SELECT [Id] FROM RecursiveCTE);
	UPDATE [UserGroups] SET [SupUserGroupId] = NULL WHERE [Id] IN (SELECT [Id] FROM RecursiveCTE);
	RETURN;
GO



CREATE PROCEDURE Insert_Users
(
	@userGroupId UNIQUEIDENTIFIER,
    @emailAddress VARCHAR(300),
    @username VARCHAR(50),
    @firstName NVARCHAR(200),
    @lastName NVARCHAR(200),
	@grants VARBINARY(MAX),
	@firstPassword NVARCHAR(MAX) OUTPUT,
	@emailAddressVerificationTokenValidUntil DATETIMEOFFSET OUTPUT,
	@emailAddressVerificationToken VARCHAR(50) OUTPUT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [Users] WHERE [Id] = @id)
		SET @id = NEWID();
	DECLARE @emailAddress_ VARCHAR(300) = LOWER(@emailAddress COLLATE SQL_Latin1_General_CP1253_CI_AI);
	SET @firstPassword = [dbo].[GetRandomPassword](8, 12);
	INSERT INTO [Users] ([Id], [UserGroupId], [EmailAddress], [Username], [Password], [FirstName], [LastName], [Grants])
		VALUES (@id, @userGroupId, @emailAddress, @username, @firstPassword, @firstName, @lastName, @grants);
	SELECT TOP 1
			@emailAddressVerificationToken = t1.[VerificationToken],
			@emailAddressVerificationTokenValidUntil = t1.[VerificationTokenValidUntil]
		FROM [UserVerificationTokens] t1
		LEFT JOIN [UserVerificationAttempts] t2
			ON t2.[ResultUserVerificationTokenId] = t1.[Id]
		WHERE
			t1.[UserId] = @id AND
			t1.[IsEnabled] = 1 AND
			t1.[VerificationType_EnumerationValue] = 1 AND
			t1.[AdditionalData] = @emailAddress_ AND
			t2.[Id] IS NULL
		ORDER BY
			t1.[VerificationTokenValidUntil] DESC
	RETURN;
GO

CREATE PROCEDURE Update_Users
(
	@id UNIQUEIDENTIFIER,
	@userGroupId UNIQUEIDENTIFIER,
	@username VARCHAR(50),
    @firstName NVARCHAR(200),
    @lastName NVARCHAR(200),
	@is2FAActive BIT
)
AS
	SET NOCOUNT ON;
	UPDATE
			[Users]
		SET
			[UserGroupId] = @userGroupId,
			[Username] = @username,
			[FirstName] = @firstName,
			[LastName] = @lastName,
			[Is2FAActive] = @is2FAActive
		WHERE
			[Id] = @id;
	RETURN;
GO

CREATE PROCEDURE Enable_Users
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [Users] SET [IsEnabled] = 1 WHERE [Id] = @id;
	RETURN;
GO

CREATE PROCEDURE Disable_Users
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [Users] SET [IsEnabled] = 0 WHERE [Id] = @id;
	RETURN;
GO

CREATE PROCEDURE Update_Users_Grants
(
	@id UNIQUEIDENTIFIER,
	@grants VARBINARY(MAX)
)
AS
	SET NOCOUNT ON;
	UPDATE [Users] SET [Grants] = @grants WHERE [Id] = @id;
GO

CREATE PROCEDURE Update_Users_EmailAddress
(
	@id UNIQUEIDENTIFIER,
    @emailAddress VARCHAR(300),
	@verificationToken VARCHAR(50) OUTPUT,
	@verificationTokenValidUntil DATETIMEOFFSET OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @emailAddress_ VARCHAR(300) = LOWER(@emailAddress COLLATE SQL_Latin1_General_CP1253_CI_AI);
	UPDATE [Users] SET [EmailAddress] = @emailAddress WHERE [Id] = @id;
	SELECT TOP 1
			@verificationToken = t1.[VerificationToken],
			@verificationTokenValidUntil = t1.[VerificationTokenValidUntil]
		FROM [UserVerificationTokens] t1
		LEFT JOIN [UserVerificationAttempts] t2
			ON t2.[ResultUserVerificationTokenId] = t1.[Id]
		WHERE
			t1.[UserId] = @id AND
			t1.[IsEnabled] = 1 AND
			t1.[VerificationType_EnumerationValue] = 1 AND
			t1.[AdditionalData] = @emailAddress_ AND
			t2.[Id] IS NULL
		ORDER BY
			t1.[VerificationTokenValidUntil] DESC
	RETURN;
GO

CREATE PROCEDURE Update_Users_Password
(
	@id UNIQUEIDENTIFIER,
    @currentPassword NVARCHAR(MAX),
    @newPassword NVARCHAR(MAX),
	@isSuccess BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @isSuccess = 0;
	DECLARE @username NVARCHAR(50);
	DECLARE @result BIT;
	DECLARE @pwd NVARCHAR(MAX);
	SELECT
			@username = [Username],
			@pwd = [Password]
		FROM [Users]
		WHERE
			[Id] = @id AND
			[IsEnabled] = 1;
	EXEC [dbo].[DecryptUserPassword] @username, @pwd, @result OUTPUT;
	IF @result = @currentPassword
		BEGIN
			UPDATE [Users] SET [Password] = @newPassword WHERE [Id] = @id;
			SET @isSuccess = 1;
		END
	RETURN;
GO

CREATE PROCEDURE Update_Users_ForgottenPassword_InitializeVerificationToken
(
    @username NVARCHAR(MAX),
	@verificationToken VARCHAR(50) OUTPUT,
	@verificationTokenValidUntil DATETIMEOFFSET OUTPUT,
	@userEmailAddress VARCHAR(300) OUTPUT
)
AS
	SET NOCOUNT ON;
	DECLARE @userId UNIQUEIDENTIFIER;
	SELECT
			@userId = [Id],
			@userEmailAddress = [EmailAddress]
		FROM [Users]
		WHERE
			[Username] = @username AND
			[IsEnabled] = 1;
	EXEC [dbo].[GetVerificationTokenValidUntil] @verificationTokenValidUntil OUTPUT;
	IF @userId IS NOT NULL
		BEGIN
			DECLARE @uid UNIQUEIDENTIFIER;
			DECLARE @isVerified BIT;
			EXEC [dbo].[IsVerifiedOrPendingEmailAddress] @userEmailAddress, @isVerified OUTPUT, @uid OUTPUT;
			IF @uid = @userId AND @isVerified = 1
				BEGIN
					DECLARE @vtid UNIQUEIDENTIFIER;
					EXEC [dbo].[Insert_UserVerificationTokens] @userId, NULL, 2, @verificationToken OUTPUT, @verificationTokenValidUntil OUTPUT, @vtid OUTPUT;
				END
			ELSE
				SET @userEmailAddress = NULL;
		END
	RETURN;
GO

CREATE PROCEDURE Update_Users_ForgottenPassword
(
    @userVerificationAttemptId UNIQUEIDENTIFIER,
    @newPassword NVARCHAR(MAX),
	@isSuccess BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @isSuccess = 0;
	DECLARE @tokenId UNIQUEIDENTIFIER;
	DECLARE @userId UNIQUEIDENTIFIER;
	SELECT
			@userId = [UserId],
			@tokenId = [ResultUserVerificationTokenId]
		FROM [UserVerificationAttempts]
		WHERE
			[Id] = @userVerificationAttemptId AND
			[VerificationType_EnumerationValue] = 2 AND
			[ResultFailureReason_EnumerationValue] = NULL;
	IF
		EXISTS(
			SELECT
					1
				FROM [UserVerificationTokens] t1
				INNER JOIN [Users] t2
				ON t2.[Id] = t1.[UserId]
				WHERE
					t1.[Id] = @tokenId AND
					t1.[VerificationType_EnumerationValue] = 2 AND
					t1.[IsEnabled] = 1 AND
					t2.[Id] = @userId AND
					t2.[IsEnabled] = 1)
		BEGIN
			DECLARE @lastTokenId UNIQUEIDENTIFIER;
			SELECT TOP 1
					@lastTokenId = [Id]
				FROM [UserVerificationTokens]
				WHERE
					[VerificationType_EnumerationValue] = 2 AND
					[UserId] = @userId
				ORDER BY
					[VerificationTokenValidUntil] DESC;
			IF @lastTokenId = @tokenId
				BEGIN
					UPDATE [Users] SET [Password] = @newPassword WHERE [Id] = @userId;
					SET @isSuccess = 1;
				END
		END	
	RETURN;
GO

CREATE PROCEDURE Update_Users_VeryFirstPassword
(
    @userLoginAttemptId UNIQUEIDENTIFIER,
    @newPassword NVARCHAR(MAX),
	@isSuccess BIT OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @isSuccess = 0;
	
	DECLARE @userId UNIQUEIDENTIFIER;
	SELECT
			@userId = t1.[ResultUserId]
		FROM [UserLoginAttempts] t1
		LEFT JOIN [Users] t2 ON
			t2.[Id] = t1.[ResultUserId]
		WHERE
			t1.[Id] = @userLoginAttemptId AND
			t1.[ResultUserId] IS NOT NULL AND
			(t1.[ResultFailureReason_EnumerationValue] & 1024) > 0 AND
			t2.[IsEnabled] = 1;
	IF @userId IS NOT NULL
		BEGIN
			DECLARE @result BIT;
			EXEC [dbo].[IsVeryFirstPassword] @userId, @result OUTPUT;
			IF @result = 1
				BEGIN
					UPDATE [Users] SET [Password] = @newPassword WHERE [Id] = @userId;
					SET @isSuccess = 1;
				END
		END
	RETURN;
GO

CREATE PROCEDURE Delete_Users
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [Users] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_UserVerificationTokens
(
	@userId UNIQUEIDENTIFIER,
	@additionalData NVARCHAR(MAX),
	@verificationType_EnumerationValue TINYINT,
	@verificationToken VARCHAR(50) OUTPUT,
	@verificationTokenValidUntil DATETIMEOFFSET OUTPUT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [UserVerificationTokens] WHERE [Id] = @id)
		SET @id = NEWID();
	DECLARE @isBanished BIT = 0;
	DECLARE @userGroupId UNIQUEIDENTIFIER;
	DECLARE @exists BIT;
	SELECT
			@exists = 1,
			@userGroupId = [UserGroupId]
		FROM [Users]
		WHERE
			[Id] = @userId AND
			[IsEnabled] = 1;
	IF @exists = 1
		BEGIN
			EXEC [dbo].[IsBanishedUser] @userId, @isBanished OUTPUT;
			IF @isBanished = 0 AND  @userGroupId IS NOT NULL
				EXEC [dbo].[IsBanishedUserGroup] @userId, @isBanished OUTPUT;
		END
	EXEC [dbo].[GetVerificationTokenValidUntil] @verificationTokenValidUntil OUTPUT;
	IF @isBanished = 0
		BEGIN
			SET @verificationToken = [dbo].[GetRandomVerificationToken]();
			INSERT INTO [UserVerificationTokens] ([Id], [UserId], [AdditionalData], [VerificationType_EnumerationValue], [VerificationToken], [VerificationTokenValidUntil])
				VALUES (@id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil);
		END
	RETURN;
GO

CREATE PROCEDURE Disable_UserVerificationTokens
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [UserVerificationTokens] SET [IsEnabled] = 0 WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_UserVerificationAttempts
(
	@pageRequestId UNIQUEIDENTIFIER,
	@verificationType_EnumerationValue TINYINT,
	@userId UNIQUEIDENTIFIER,
	@additionalData NVARCHAR(MAX),
	@verificationToken VARCHAR(50),
	@resultFailureReason_EnumerationValue INT OUTPUT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [UserVerificationAttempts] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [UserVerificationAttempts] ([Id], [pageRequestId], [VerificationType_EnumerationValue], [UserId], [VerificationToken], [AdditionalData])
		VALUES (@id, @pageRequestId, @verificationType_EnumerationValue, @userId, @verificationToken, @additionalData);
	SELECT
			@resultFailureReason_EnumerationValue = [ResultFailureReason_EnumerationValue]
		FROM [UserVerificationAttempts]
		WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_UserLoginAttempts
 (
	@pageRequestId UNIQUEIDENTIFIER,
	@username NVARCHAR(MAX),
	@password NVARCHAR(MAX),
	@resultFailureReason_EnumerationValue INT OUTPUT,
	@TwoFAVerificationToken VARCHAR(50) OUTPUT,
	@TwoFAVerificationTokenValidUntil DATETIMEOFFSET OUTPUT,
	@userEmailAddress VARCHAR(300) OUTPUT,
 	@id UNIQUEIDENTIFIER OUTPUT
 )
 AS
 	SET NOCOUNT ON;
 	SET @id = NEWID();
 	WHILE EXISTS(SELECT 1 FROM UserLoginAttempts WHERE [Id] = @id)
 		SET @id = NEWID();
 	INSERT INTO [UserLoginAttempts] ([Id], [pageRequestId], [Username], [Password])
 		VALUES (@id, @pageRequestId, @username, @password);
	DECLARE @userId UNIQUEIDENTIFIER;
	DECLARE @tokenId UNIQUEIDENTIFIER;
	SELECT
			@resultFailureReason_EnumerationValue = [ResultFailureReason_EnumerationValue],
			@userId = [ResultUserId],
			@tokenId = [ResultUserVerificationTokenId]
	FROM [UserLoginAttempts]
	WHERE
		[Id] = @id;
	IF @resultFailureReason_EnumerationValue IS NULL AND @userId IS NOT NULL
		BEGIN
			SELECT
					@userEmailAddress = [EmailAddress]
				FROM [Users]
				WHERE
					[Id] = @userId AND
					[IsEnabled] = 1;
			IF @tokenId IS NOT NULL
				SELECT
						@TwoFAVerificationToken = [VerificationToken],
						@TwoFAVerificationTokenValidUntil = [VerificationTokenValidUntil]
					FROM [UserVerificationTokens]
						WHERE
							[Id] = @tokenId AND
							[IsEnabled] = 1;
		END
 	RETURN;
GO



CREATE PROCEDURE Insert_UserLogins
 (
	@userLoginAttemptId UNIQUEIDENTIFIER,
	@userVerificationAttemptId UNIQUEIDENTIFIER,
 	@id UNIQUEIDENTIFIER OUTPUT
 )
 AS
 	SET NOCOUNT ON;
 	SET @id = NEWID();
 	WHILE EXISTS(SELECT 1 FROM [UserLogins] WHERE [Id] = @id)
 		SET @id = NEWID();
 	INSERT INTO [UserLogins] ([Id], [UserLoginAttemptId], [UserVerificationAttemptId])
 		VALUES (@id, @userLoginAttemptId, @userVerificationAttemptId);
 	RETURN;
GO

CREATE PROCEDURE Disable_UserLogins
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [UserLogins] SET [IsEnabled] = 0 WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_Whitelist
(
	@userId UNIQUEIDENTIFIER,
	@ipAddressV4 NVARCHAR(15),
	@startsAt DATETIMEOFFSET,
	@endsAt  DATETIMEOFFSET,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [Whitelist] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [Whitelist] ([Id], [UserId], [IpAddressV4], [StartsAt], [EndsAt])
		VALUES (@id, @userId, @ipAddressV4, @startsAt, @endsAt);
	RETURN;
GO

CREATE PROCEDURE Update_Whitelist
(
	@id UNIQUEIDENTIFIER,
	@startsAt DATETIMEOFFSET,
	@endsAt  DATETIMEOFFSET
)
AS
	SET NOCOUNT ON;
	UPDATE [Whitelist]
		SET
			[StartsAt] = @startsAt,
			[EndsAt] = @endsAt
		WHERE [Id] = @id;
	RETURN;
GO

CREATE PROCEDURE Enable_Whitelist
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [Whitelist]
		SET
			[IsEnabled] = 1
		WHERE
			[Id] = @id AND
			[IsEnabled] = 0;
	RETURN;
GO

CREATE PROCEDURE Disable_Whitelist
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [Whitelist]
		SET
			[IsEnabled] = 0
		WHERE
			[Id] = @id AND
			[IsEnabled] = 1;
	RETURN;
GO

CREATE PROCEDURE Delete_Whitelist
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [Whitelist] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_OperationLogs
(
	@operationType_EnumerationValue TINYINT,
	@pageRequestId UNIQUEIDENTIFIER,
	@userLoginId UNIQUEIDENTIFIER,
	@targetContextName VARCHAR(50),
	@targetEntityName VARCHAR(100),
	@iudLogId UNIQUEIDENTIFIER,
	@failureReason_EnumerationValue INT,
	@additionalData NVARCHAR(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [OperationLogs] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [OperationLogs] ([Id], [OperationType_EnumerationValue], [PageRequestId], [UserLoginId], [TargetContextName], [TargetEntityName], [IUDLogId], [FailureReason_EnumerationValue], [AdditionalData])
		VALUES (@id, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iudLogId, @failureReason_EnumerationValue, @additionalData);
	RETURN;
GO

CREATE PROCEDURE Delete_OperationLogs
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [OperationLogs] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_ApiConsumers
(
	@name NVARCHAR(100),
	@apiTypes_EnumerationValue INT,
	@privateKey NVARCHAR(100) OUTPUT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [ApiConsumers] WHERE [Id] = @id)
		SET @id = NEWID();
	DECLARE @id_ UNIQUEIDENTIFIER;
	SET @privateKey = [dbo].[GetRandomPassword](20, 100);
	EXEC [dbo].[GetApiConsumerId] @privateKey, @id_ OUTPUT;
	WHILE @id_ IS NOT NULL
		BEGIN
			SET @privateKey = [dbo].[GetRandomPassword](20, 100);
			EXEC [dbo].[GetApiConsumerId] @privateKey, @id_ OUTPUT;
		END
	INSERT INTO [ApiConsumers] ([Id], [Name], [PrivateKey], [Grants_EnumerationValue])
		VALUES (@id, @name, @privateKey, @apiTypes_EnumerationValue);
	RETURN;
GO

CREATE PROCEDURE Enable_ApiConsumers
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [ApiConsumers]
		SET
			[IsEnabled] = 1
		WHERE
			[Id] = @id AND
			[IsEnabled] = 0;
	RETURN;
GO

CREATE PROCEDURE Disable_ApiConsumers
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [ApiConsumers]
		SET
			[IsEnabled] = 0
		WHERE
			[Id] = @id AND
			[IsEnabled] = 1;
	RETURN;
GO

CREATE PROCEDURE Delete_ApiConsumers
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DECLARE @machineId UNIQUEIDENTIFIER;
	DECLARE _cursor_deleteapiconsumers_machines CURSOR FOR
		SELECT
				[Id]
			FROM [ApiConsumerMachines]
			WHERE
				[ApiConsumerId] = @id;
	OPEN _cursor_deleteapiconsumers_machines;
	FETCH NEXT FROM _cursor_deleteapiconsumers_machines INTO @machineId;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC [dbo].[Delete_ApiConsumerMachines] @machineId;
			FETCH NEXT FROM _cursor_deleteapiconsumers_machines INTO @machineId;
		END
	CLOSE _cursor_deleteapiconsumers_machines;
	DEALLOCATE _cursor_deleteapiconsumers_machines;	
	DELETE [ApiConsumerIpAddresses] WHERE [ApiConsumerId] = @id;
	DELETE [ApiRequests] WHERE [ResultApiConsumerId] = @id;
	DELETE [ApiConsumers] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_ApiConsumerIpAddresses
(
	@apiConsumerId UNIQUEIDENTIFIER,
	@ipAddressV4 VARCHAR(15),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [ApiConsumerIpAddresses] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [ApiConsumerIpAddresses] ([Id], [ApiConsumerId], [IpAddressV4])
		VALUES (@id, @apiConsumerId, @ipAddressV4);
	RETURN;
GO

CREATE PROCEDURE Enable_ApiConsumerIpAddresses
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [ApiConsumerIpAddresses]
		SET
			[IsEnabled] = 1
		WHERE
			[Id] = @id AND
			[IsEnabled] = 0;
	RETURN;
GO

CREATE PROCEDURE Disable_ApiConsumerIpAddresses
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [ApiConsumerIpAddresses]
		SET
			[IsEnabled] = 0
		WHERE
			[Id] = @id AND
			[IsEnabled] = 1;
	RETURN;
GO

CREATE PROCEDURE Delete_ApiConsumerIpAddresses
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [ApiConsumerIpAddresses] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_ApiConsumerMachines
(
	@apiConsumerId UNIQUEIDENTIFIER,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [ApiConsumerMachines] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [ApiConsumerMachines] ([Id], [ApiConsumerId])
		VALUES (@id, @apiConsumerId);
	RETURN;
GO

CREATE PROCEDURE Enable_ApiConsumerMachines
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [ApiConsumerMachines]
		SET
			[IsEnabled] = 1
		WHERE
			[Id] = @id AND
			[IsEnabled] = 0;
	RETURN;
GO

CREATE PROCEDURE Disable_ApiConsumerMachines
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	UPDATE [ApiConsumerMachines]
		SET
			[IsEnabled] = 0
		WHERE
			[Id] = @id AND
			[IsEnabled] = 1;
	RETURN;
GO

CREATE PROCEDURE Delete_ApiConsumerMachines
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [ApiConsumerMachineVariables] WHERE [ApiConsumerMachineId] = @id;
	DELETE [ApiConsumerMachines] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_ApiConsumerMachineVariables
(
	@apiConsumerMachineId UNIQUEIDENTIFIER,
	@name NVARCHAR(300),
	@value NVARCHAR(MAX),
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [ApiConsumerMachineVariables] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [ApiConsumerMachineVariables] ([Id], [ApiConsumerMachineId], [Name], [Value])
		VALUES (@id, @apiConsumerMachineId, @name, @value);
	RETURN;
GO

CREATE PROCEDURE Delete_ApiConsumerMachineVariables
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [ApiConsumerMachineVariables] WHERE [Id] = @id;
	RETURN;
GO



CREATE PROCEDURE Insert_ApiRequests
(
	@pageRequestId UNIQUEIDENTIFIER,
	@randomToken NVARCHAR(MAX),
	@privateKey NVARCHAR(MAX),
	@machineVariablesJson NVARCHAR(MAX),
	@resultApiConsumerId UNIQUEIDENTIFIER OUTPUT,
	@resultFailureReason_EnumerationValue INT OUTPUT,
	@id UNIQUEIDENTIFIER OUTPUT
)
AS
	SET NOCOUNT ON;
	SET @id = NEWID();
	WHILE EXISTS(SELECT 1 FROM [ApiRequests] WHERE [Id] = @id)
		SET @id = NEWID();
	INSERT INTO [ApiRequests] ([Id], [PageRequestId], [RandomToken], [PrivateKey], [MachineVariablesJson])
		VALUES (@id, @pageRequestId, @randomToken, @privateKey, @machineVariablesJson);
	SELECT
			@resultApiConsumerId = [ResultApiConsumerId],
			@resultFailureReason_EnumerationValue = [ResultFailureReason_EnumerationValue]
		FROM [ApiRequests]
		WHERE
			[Id] = @id;
	RETURN;
GO

CREATE PROCEDURE Delete_ApiRequests
(
	@id UNIQUEIDENTIFIER
)
AS
	SET NOCOUNT ON;
	DELETE [ApiRequests] WHERE [Id] = @id;
	RETURN;
GO