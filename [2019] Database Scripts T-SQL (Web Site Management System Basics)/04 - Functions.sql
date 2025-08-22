USE WebSiteBaseDB
GO



CREATE FUNCTION IsPowerOfTwo (@value BIGINT)
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE WHEN @value & -@value = @value
				THEN 1
				ELSE 0
			END;
	END
GO

CREATE FUNCTION NullAwareComparison (@Value1 SQL_VARIANT, @Value2 SQL_VARIANT)
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE WHEN @Value1 = @Value2 OR (@Value1 IS NULL AND @Value2 IS NULL)
				THEN 1
				ELSE 0
			END;
	END
GO

CREATE FUNCTION NullAwareComparison_VarChar (@Value1 VARCHAR(MAX), @Value2 VARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE WHEN @Value1 = @Value2 OR (@Value1 IS NULL AND @Value2 IS NULL)
				THEN 1
				ELSE 0
			END;
	END
GO

CREATE FUNCTION NullAwareComparison_NVarChar (@Value1 NVARCHAR(MAX), @Value2 NVARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE WHEN @Value1 = @Value2 OR (@Value1 IS NULL AND @Value2 IS NULL)
				THEN 1
				ELSE 0
			END;
	END
GO

CREATE FUNCTION IsEmptyString (@Text VARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE WHEN @Text IS NULL
				THEN NULL
				ELSE
					CASE WHEN LEN(LTRIM(RTRIM(@Text))) = 0
						THEN 1
						ELSE 0
					END
			END;
	END
GO

CREATE FUNCTION IsEmptyString_N (@Text NVARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE WHEN @Text IS NULL
				THEN NULL
				ELSE
					CASE WHEN LEN(LTRIM(RTRIM(@Text))) = 0
						THEN 1
						ELSE 0
					END
			END;
	END
GO

CREATE FUNCTION IsNullOrEmpty_Guid (@guid UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		IF @guid IS NOT NULL
			BEGIN
				DECLARE @strGuid VARCHAR(MAX) = CONVERT(VARCHAR(MAX), @guid);
				IF @strGuid != '{}' AND [dbo].[IsEmptyString](@strGuid) = 0
					RETURN 0;
			END
		RETURN 1;
	END
GO

CREATE FUNCTION RandomIntegerToRange (@random INT, @min INT, @max INT)
RETURNS INT
AS
	BEGIN
		DECLARE
			@min_ BIGINT = ISNULL(@min, -2147483648),
			@max_ BIGINT = ISNULL(@max, 2147483647);
		IF @min_ = @max_
			RETURN @min_;
		IF @min_ > @max_
			BEGIN
				DECLARE @tmp INT = @min_;
				SET @min_ = @max_;
				SET @max_ = @tmp;
			END
		RETURN CONVERT(INT, ABS(CAST(ISNULL(@random, (SELECT [Value] FROM vwRandomIntegers)) AS BIGINT) % (@max_ - @min_ + 1)) + @min_);
	END
GO

CREATE FUNCTION ContainsEnumerationValue (@enumerationValue BIGINT, @enumerationResult BIGINT)
RETURNS BIT
AS
	BEGIN
		IF @enumerationValue IS NOT NULL AND @enumerationResult IS NOT NULL AND @enumerationValue > 0 AND @enumerationResult > 0 AND (@enumerationValue & @enumerationResult) = @enumerationValue
			RETURN 1;
		RETURN 0;
	END
GO

CREATE FUNCTION GetContainedEnumerationValues (@enumerationResult BIGINT)
RETURNS @enumerationValues TABLE ([Value] BIGINT)
AS
	BEGIN
		IF @enumerationResult IS NULL OR @enumerationResult <= 0
			RETURN;
		DECLARE @i INT = 0, @x BIGINT = 0, @base BIGINT = 2;
		WHILE @i < 63 AND @x <= @enumerationResult
			BEGIN
				SET @x = POWER(@base, @i);
				IF [dbo].[ContainsEnumerationValue](@x, @enumerationResult) = 1
					INSERT INTO @enumerationValues VALUES (@x);
				SET @i = @i + 1;
			END
		RETURN;
	END
GO

CREATE FUNCTION CapitalizeString(@string NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
	BEGIN
		IF @string IS NULL
			RETURN NULL;
		DECLARE @result NVARCHAR(MAX);
		SELECT @result = STRING_AGG(UPPER(LEFT(LTRIM(RTRIM([value])), 1)) + LOWER(SUBSTRING(LTRIM(RTRIM([value])), 2, LEN(LTRIM(RTRIM([value]))) - 1)), ' ') FROM STRING_SPLIT(@string, ' ') WHERE LEN(LTRIM(RTRIM([value]))) > 0;
		RETURN @result;
	END
GO

CREATE FUNCTION IsDateInRange(@date DATETIMEOFFSET, @minusSeconds INT)
RETURNS BIT
AS
	BEGIN
		DECLARE @now DATETIMEOFFSET = SYSDATETIMEOFFSET();
		DECLARE @minusMinute DATETIMEOFFSET = DATEADD(SECOND, -1 * @minusSeconds, @now);
		RETURN CASE WHEN @date <= @now AND @date >= @minusMinute
			THEN 1
			ELSE 0
		END;
	END
GO

CREATE FUNCTION GetRandomPassword(@minLength INT, @maxLength INT)
RETURNS NVARCHAR(MAX)
AS
	BEGIN
		DECLARE @alphaLower NVARCHAR(50) = 'abcdefghijklmnopqrstuvwxyz';	-- 45% (1-45)
		DECLARE @alphaUpper NVARCHAR(50) = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';	-- 25% (46-70)
		DECLARE @numeric NVARCHAR(50) = '0123456789';						-- 20% (71-90)
		DECLARE @special NVARCHAR(50) = '@?&$+-*/.,;:\!%|#';				-- 10% (91-100)
		DECLARE
			@password NVARCHAR(MAX) = '',
			@length INT = CAST([dbo].[RandomIntegerToRange](NULL, @minLength, @maxLength) AS INT),
			@i INT = 0,
			@ok1 BIT = 0,
			@ok2 BIT = 0,
			@ok3 BIT = 0,
			@ok4 BIT = 0,
			@rnd INT;
		WHILE (@i < @length)
			BEGIN
				SET @rnd = CAST([dbo].[RandomIntegerToRange](NULL, 1, 100) AS INT);
				IF @rnd < 46
					BEGIN
						SET @password = @password + SUBSTRING(@alphaLower, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@alphaLower)) AS INT), 1);
						SET @ok1 = 1;
					END
				ELSE IF @rnd < 71
					BEGIN
						SET @password = @password + SUBSTRING(@alphaUpper, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@alphaUpper)) AS INT), 1);
						SET @ok2 = 1;
					END
				ELSE IF @rnd < 91
					BEGIN
						SET @password = @password + SUBSTRING(@numeric, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@numeric)) AS INT), 1);
						SET @ok3 = 1;
					END
				ELSE
					BEGIN
						SET @password = @password + SUBSTRING(@special, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@special)) AS INT), 1);
						SET @ok4 = 1;
					END
				SET @i = @i + 1;
			END
		IF (@ok1 = 0)
			BEGIN
				SET @rnd = CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@password)) AS INT);
				SET @password =
					SUBSTRING(@password, 0, @rnd) +
					SUBSTRING(@alphaLower, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@alphaLower)) AS INT), 1) +
					SUBSTRING(@password, @rnd, LEN(@password) - @rnd);
			END
		IF (@ok2 = 0)
			BEGIN
				SET @rnd = CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@password)) AS INT);
				SET @password =
					SUBSTRING(@password, 0, @rnd) +
					SUBSTRING(@alphaUpper, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@alphaUpper)) AS INT), 1) +
					SUBSTRING(@password, @rnd, LEN(@password) - @rnd);
			END
		IF (@ok3 = 0)
			BEGIN
				SET @rnd = CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@password)) AS INT);
				SET @password =
					SUBSTRING(@password, 0, @rnd) +
					SUBSTRING(@numeric, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@numeric)) AS INT), 1) +
					SUBSTRING(@password, @rnd, LEN(@password) - @rnd);
			END
		IF (@ok4 = 0)
			BEGIN
				SET @rnd = CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@password)) AS INT);
				SET @password =
					SUBSTRING(@password, 0, @rnd) +
					SUBSTRING(@special, CAST([dbo].[RandomIntegerToRange](NULL, 0, LEN(@special)) AS INT), 1) +
					SUBSTRING(@password, @rnd, LEN(@password) - @rnd);
			END
		RETURN @password;
	END
GO

CREATE FUNCTION GetRandomVerificationToken()
RETURNS VARCHAR(MAX)
AS
	BEGIN
		RETURN CONVERT(VARCHAR(MAX), [dbo].[RandomIntegerToRange](NULL, 100000, 999999));
	END
GO

CREATE FUNCTION IsIpAddressV4(@ipAddress VARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE WHEN @ipAddress IS NULL
				THEN NULL
				ELSE
					CASE WHEN
							@ipAddress != '::1' AND (
								LEN(@ipAddress) > 15 OR
								@ipAddress NOT LIKE '%_.%_.%_.%_' OR
								@ipAddress LIKE '%.%.%.%.%' OR
								EXISTS(SELECT [Value] FROM STRING_SPLIT(@ipAddress, '.') WHERE TRY_CAST([Value] AS TINYINT) IS NULL))
						THEN 0
						ELSE 1
					END
			END;
	END
GO

CREATE FUNCTION IsEmailAddress(@email VARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		DECLARE @bitEmailVal AS BIT;
		DECLARE @emailText VARCHAR(MAX);
		SET @emailText = LTRIM(RTRIM(ISNULL(@email, '')));
		RETURN
			CASE
				WHEN @emailText = '' THEN 0
				WHEN @emailText LIKE '% %' THEN 0
				WHEN @emailText LIKE ('%["(),:;<>\]%') THEN 0
				WHEN SUBSTRING(@emailText, CHARINDEX('@', @emailText), LEN(@emailText)) LIKE ('%[!#$%&*+/=?^`_{|]%') THEN 0
				WHEN (LEFT(@emailText, 1) LIKE ('[-_.+]') OR RIGHT(@emailText, 1) LIKE ('[-_.+]')) THEN 0                                                                                    
				WHEN (@emailText LIKE '%[%' OR @emailText LIKE '%]%') THEN 0
				WHEN @emailText LIKE '%@%@%' THEN 0
				WHEN @emailText NOT LIKE '_%@_%._%' THEN 0
				WHEN LOWER(@emailText COLLATE SQL_Latin1_General_CP1253_CI_AI) NOT LIKE LOWER(@emailText) THEN 0
				ELSE 1 
			END;
	END 
GO

CREATE FUNCTION IsValidUsername(@username NVARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		DECLARE @trimmed NVARCHAR(MAX);
		SET @trimmed = LTRIM(RTRIM(ISNULL(@username, '')));
		RETURN
			CASE WHEN
				LEN(@trimmed) >= 3 AND LEN(@trimmed) <= 20 AND @trimmed LIKE '%[abcdefghijklmnopqrstuvwxyz0123456789_]%'
					THEN 1
					ELSE 0
			END;
	END
GO





CREATE FUNCTION CheckIfEnabled_UserGroups(@id UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		RETURN CASE
			WHEN EXISTS(SELECT 1 FROM [UserGroups] WHERE [Id] = @id AND [IsEnabled] = 1)
				THEN 1
				ELSE 0
			END;
	END
GO

CREATE FUNCTION CheckIfEnabled_UserLogins(@id UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		RETURN CASE
			WHEN EXISTS(SELECT 1 FROM [UserLogins] WHERE [Id] = @id AND [IsEnabled] = 1)
				THEN 1
				ELSE 0
			END;
	END
GO



CREATE FUNCTION IsNullOrEnabled_UserLoginId(@id UNIQUEIDENTIFIER, @userLoginId UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE
				WHEN @userLoginId IS NULL THEN 1
				WHEN @id = @userLoginId THEN 0
				WHEN EXISTS(SELECT 1 FROM [UserLogins] WHERE [Id] = @userLoginId AND [IsEnabled] = 1) THEN 1
				ELSE 0
			END;
	END
GO



CREATE FUNCTION Check_IUDLogValues(@id UNIQUEIDENTIFIER, @iUDLogId UNIQUEIDENTIFIER, @columnName VARCHAR(128), @columnDataType VARCHAR(128), @isNullable BIT, @oldValue_String NVARCHAR(MAX), @oldValue_Binary VARBINARY(MAX), @newValue_String NVARCHAR(MAX), @newValue_Binary VARBINARY(MAX))
RETURNS BIT
AS
	BEGIN
		IF [dbo].[IsEmptyString](@columnName) = 1
			RETURN 0;
		RETURN 1;
		DECLARE @columnName_ NVARCHAR(128) = UPPER(@columnName);
		IF EXISTS(SELECT 1 FROM [IUDLogValues] WHERE [Id] <> @id AND [IUDLogId] = @iUDLogId AND UPPER([ColumnName]) = @columnName_) OR [dbo].[IsEmptyString](@columnDataType) = 0 OR (@oldValue_String IS NOT NULL AND @oldValue_Binary IS NOT NULL) OR (@newValue_String IS NOT NULL AND @newValue_Binary IS NOT NULL) OR (@isNullable = 0 AND (@oldValue_String IS NULL OR @newValue_String IS NULL OR @oldValue_Binary IS NULL OR @newValue_Binary IS NULL))
			RETURN 0;
		RETURN 1;
	END
GO



CREATE FUNCTION Check_Banishments(@id UNIQUEIDENTIFIER, @startsAt DATETIMEOFFSET, @endsAt DATETIMEOFFSET)
RETURNS BIT
AS
	BEGIN
		DECLARE @max DATETIMEOFFSET = '9999-12-31T23:59:59.9999999 +14:00';
		RETURN CASE WHEN EXISTS(SELECT 1 FROM [Banishments] WHERE [StartsAt] <= @startsAt AND ISNULL([EndsAt], @max) >= ISNULL(@endsAt, @max))
			THEN 1
			ELSE 0
		END;
	END
GO



CREATE FUNCTION Check_FormResultComponents_Value(@valueTypes_EnumerationValue TINYINT, @value_String NVARCHAR(MAX), @value_Binary VARBINARY(MAX))
RETURNS BIT
AS
	BEGIN
		IF ((@value_String IS NULL AND @value_Binary IS NULL) OR (@value_String IS NOT NULL AND @value_Binary IS NOT NULL))
			RETURN 0;
		IF (@valueTypes_EnumerationValue <> 0 AND @valueTypes_EnumerationValue <> 4 AND @value_String IS NULL)
			RETURN 0;
		IF (@valueTypes_EnumerationValue = 4 AND @value_Binary IS NULL)
			RETURN 0;
		RETURN 1;
	END
GO



CREATE FUNCTION Check_UserGroups_Name(@id UNIQUEIDENTIFIER, @name NVARCHAR(100))
RETURNS BIT
AS
	BEGIN
		DECLARE @name_ NVARCHAR(100) = UPPER(@name);
		RETURN
			CASE
				WHEN
					[dbo].[IsEmptyString_N](@name) = 1 OR EXISTS(SELECT 1 FROM [UserGroups] WHERE [IsEnabled] = 1 AND UPPER([Name]) = @name_)
					THEN 0
				ELSE 1
			END;
	END
GO



CREATE FUNCTION Check_Users_Username(@id UNIQUEIDENTIFIER, @username VARCHAR(50))
RETURNS BIT
AS
	BEGIN
		DECLARE @username_ NVARCHAR(50) = LTRIM(RTRIM(ISNULL(LOWER(@username COLLATE SQL_Latin1_General_CP1253_CI_AI), '')));
		IF LTRIM(RTRIM(ISNULL(LOWER(@username), ''))) <> @username_
			RETURN 0;
		RETURN
			CASE
				WHEN [dbo].[IsValidUsername](@username_) = 0
					THEN 0
				WHEN EXISTS(SELECT 1 FROM [Users] WHERE [Id] <> @id AND [Username] <> @username_)
					THEN 0
				ELSE 1
			END;
		RETURN 1;
	END
GO

CREATE FUNCTION Check_Users_EmailAddress(@id UNIQUEIDENTIFIER, @emailAddress VARCHAR(300))
RETURNS BIT
AS
	BEGIN
		DECLARE @emailAddress_ VARCHAR(300) = LTRIM(RTRIM(ISNULL(LOWER(@emailAddress COLLATE SQL_Latin1_General_CP1253_CI_AI), '')));
		IF LTRIM(RTRIM(ISNULL(LOWER(@emailAddress), ''))) <> @emailAddress_
			RETURN 0;
		DECLARE @checksum INT = CHECKSUM(@emailAddress_);
		IF
			[dbo].[IsEmailAddress](@emailAddress_) = 0 OR
			EXISTS(
				SELECT
						1
					FROM [Users] t1
					LEFT JOIN [UserVerificationTokens] t2
						ON t1.[Id] = t2.[UserId]
					LEFT JOIN [UserVerificationAttempts] t3
						ON t2.[Id] = t3.[ResultUserVerificationTokenId]
					WHERE
						t1.[Id] <> @id AND
						t1.[IsEnabled] = 1 AND
						t1.[EmailAddress] = @emailAddress_ AND
						t2.[IsEnabled] = 1 AND
						t2.[VerificationType_EnumerationValue] = 1 AND
						t2.[AdditionalData_Checksum] = @checksum AND
						t2.[AdditionalData] = @emailAddress_ AND
						(t2.[VerificationTokenValidUntil] < SYSDATETIMEOFFSET() OR t3.[Id] IS NOT NULL))
			RETURN 0;
		RETURN 1;
	END
GO

CREATE FUNCTION Check_Users_Is2FAActive(@id UNIQUEIDENTIFIER, @emailAddress VARCHAR(300), @is2FAActive BIT)
RETURNS BIT
AS
	BEGIN
		IF @is2FAActive = 0
			RETURN 1;
		DECLARE @isVerified BIT;
		DECLARE @userId UNIQUEIDENTIFIER;
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
		RETURN
			CASE
				WHEN @userId = @id AND @isVerified = 1 THEN 1
				ELSE 0
			END;
	END
GO



CREATE FUNCTION Check_UserVerificationTokens_UserId(@userId UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE
				WHEN EXISTS(SELECT 1 FROM [Users] WHERE [Id] = @userId AND [IsEnabled] = 1)
					THEN 1
				ELSE 0
			END;
	END
GO



CREATE FUNCTION Check_UserVerificationAttempts(@id UNIQUEIDENTIFIER, @verificationType_EnumerationValue TINYINT, @userId UNIQUEIDENTIFIER, @verificationToken NVARCHAR(MAX), @additionalData NVARCHAR(MAX), @resultUserVerificationTokenId UNIQUEIDENTIFIER, @resultFailureReason_EnumerationValue INT)
RETURNS BIT
AS
	BEGIN
		IF @resultUserVerificationTokenId IS NULL
			RETURN 1;
		DECLARE @verificationType_EnumerationValue_ TINYINT;
		DECLARE @userId_ UNIQUEIDENTIFIER;
		DECLARE @verificationToken_ NVARCHAR(MAX);
		DECLARE @additionalData_ NVARCHAR(MAX);
		SELECT
				@userId_ = [UserId],
				@verificationType_EnumerationValue_ = [VerificationType_EnumerationValue],
				@verificationToken_ = [VerificationToken],
				@additionalData_ = [AdditionalData]
			FROM [UserVerificationTokens]
			WHERE
				[Id] = @resultUserVerificationTokenId
		IF
			@userId_ IS NOT NULL AND
			@userId_ = @userId AND
			@verificationType_EnumerationValue_ = @verificationType_EnumerationValue AND
			(
				@resultFailureReason_EnumerationValue IS NOT NULL OR
				(
					@verificationToken_ = @verificationToken AND
					(
						(@additionalData_ IS NULL AND @additionalData IS NULL) OR
						(@additionalData_ IS NOT NULL AND @additionalData IS NOT NULL AND @additionalData_ = @additionalData)
					)
				)
			)
			RETURN 1;
		RETURN 0;
	END
GO



CREATE FUNCTION Check_UserLoginAttempts_UserVerificationTokenId(@id UNIQUEIDENTIFIER, @resultUserVerificationTokenId UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE
				WHEN EXISTS(SELECT 1 FROM [UserLoginAttempts] WHERE [ResultUserVerificationTokenId] = @resultUserVerificationTokenId AND [Id] <> @id)
					THEN 0
				ELSE 1
			END
	END
GO



CREATE FUNCTION Check_UserLogins(@id UNIQUEIDENTIFIER, @userLoginAttemptId UNIQUEIDENTIFIER, @userVerificationAttemptId UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		IF
			@userVerificationAttemptId IS NOT NULL AND
			EXISTS(
				SELECT
						1
					FROM [UserLogins]
					WHERE
						[UserVerificationAttemptId] = @userVerificationAttemptId AND
						[Id] <> @id)
			RETURN 0;
		DECLARE @adminId UNIQUEIDENTIFIER;
		DECLARE @tokenId UNIQUEIDENTIFIER;
		SELECT
				@adminId = t1.[ResultUserId],
				@tokenId = t1.[ResultUserVerificationTokenId]
			FROM [UserLoginAttempts] t1
			INNER JOIN [Users] t2
			ON t2.[Id] = t1.[ResultUserId]
			WHERE
				t1.[Id] = @userLoginAttemptId AND
				t1.[ResultFailureReason_EnumerationValue] IS NULL AND
				t2.[IsEnabled] = 1;
		RETURN
			CASE
				WHEN @adminId IS NULL
					THEN 0
				WHEN @tokenId IS NULL
					THEN
						CASE
							WHEN @userVerificationAttemptId IS NULL
								THEN 1
							ELSE 0
						END
				WHEN @userVerificationAttemptId IS NULL
					THEN 0
				ELSE
					CASE
						WHEN EXISTS(
							SELECT
									1
								FROM [UserVerificationAttempts] t1
								INNER JOIN [UserLoginAttempts] t2
									 ON t2.[Id] = @userLoginAttemptId
								INNER JOIN [PageRequests] t3
									ON t3.[Id] = t1.[PageRequestId]
								INNER JOIN [PageRequests] t4
									ON t4.[Id] = t2.[PageRequestId]
								WHERE
									t1.[Id] = @userVerificationAttemptId AND
									t1.[VerificationType_EnumerationValue] = 2 AND
									t1.[ResultUserVerificationTokenId] = @tokenId AND
									t3.[SessionId] = t4.[SessionId])
							THEN 1
						ELSE 0
					END
			END;
	END
GO



CREATE FUNCTION Check_OperationLogs_TargetContextNameAndIUDLogId(@id UNIQUEIDENTIFIER, @targetContextName VARCHAR(50), @iudLogId UNIQUEIDENTIFIER)
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE
				WHEN EXISTS(SELECT 1 FROM [OperationLogs] WHERE [TargetContextName] = @targetContextName AND [IUDLogId] = @iudLogId AND [Id] <> @id)
					THEN 0
				ELSE 1
			END;
	END
GO



CREATE FUNCTION Check_Whitelist(@id UNIQUEIDENTIFIER, @userId UNIQUEIDENTIFIER, @ipAddressV4 VARCHAR(15), @startsAt DATETIMEOFFSET, @endsAt DATETIMEOFFSET)
RETURNS BIT
AS
	BEGIN
		DECLARE @max DATETIMEOFFSET = '9999-12-31T23:59:59.9999999 +14:00';
		IF
			NOT EXISTS(
				SELECT
						1
					FROM [Users]
					WHERE
						[Id] = @userId AND
						[IsEnabled] = 1) OR
			EXISTS(
				SELECT
						1
					FROM [Whitelist]
					WHERE
						[Id] <> @id AND
						[UserId] = @userId AND
						[IpAddressV4] = @ipAddressV4 AND
							((
								[StartsAt] = @startsAt AND
								[EndsAt] = @endsAt
							) OR
							(
								[IsEnabled] = 1 AND
								[StartsAt] <= @startsAt AND
								ISNULL([EndsAt], @max) >= ISNULL(@endsAt, @max)
							)))
			RETURN 0;
		RETURN 1;
	END
GO



CREATE FUNCTION Check_ApiConsumers_Name(@id UNIQUEIDENTIFIER, @name NVARCHAR(100))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE
				WHEN [dbo].[IsEmptyString_N](@name) = 0 AND NOT EXISTS(SELECT 1 FROM [ApiConsumers] WHERE [Name] = @name AND [IsEnabled] = 1 AND [Id] <> @id)
					THEN 1
				ELSE 0
			END;
	END
GO



CREATE FUNCTION Check_ApiConsumerIpAddresses_IpAddressV4(@id UNIQUEIDENTIFIER, @apiConsumerId UNIQUEIDENTIFIER, @ipAddressV4 VARCHAR(15))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE
				WHEN [dbo].[IsEmptyString_N](@ipAddressV4) = 0 AND NOT EXISTS(SELECT 1 FROM [ApiConsumerIpAddresses] WHERE [IpAddressV4] = @ipAddressV4 AND [ApiConsumerId] = @apiConsumerId AND [IsEnabled] = 1 AND [Id] <> @id)
					THEN 1
				ELSE 0
			END;
	END
GO



CREATE FUNCTION Check_ApiConsumerMachineVariables_ApiConsumerMachineIdAndName(@id UNIQUEIDENTIFIER, @apiConsumerMachineId UNIQUEIDENTIFIER, @name NVARCHAR(300))
RETURNS BIT
AS
	BEGIN
		RETURN
			CASE
				WHEN EXISTS(SELECT 1 FROM [ApiConsumerMachineVariables] WHERE [ApiConsumerMachineId] = @apiConsumerMachineId AND [Name] = @name AND [Id] <> @id)
					THEN 0
				ELSE 1
			END;
	END
GO



CREATE FUNCTION Check_ApiRequests_RandomToken(@id UNIQUEIDENTIFIER, @randomToken NVARCHAR(MAX))
RETURNS BIT
AS
	BEGIN
		DECLARE @checksum INT = CHECKSUM(@randomToken);
		RETURN
			CASE
				WHEN @randomToken IS NULL
					THEN 1
				WHEN [dbo].[IsEmptyString_N](@randomToken) = 1
					THEN 0
				WHEN EXISTS(SELECT 1 FROM [ApiRequests] WHERE [RandomToken_Checksum] = @checksum AND [RandomToken] = @randomToken AND [Id] <> @id)
					THEN 0
				ELSE 1
			END;
	END
GO