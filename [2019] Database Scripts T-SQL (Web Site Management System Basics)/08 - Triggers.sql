USE WebSiteBaseDB
GO



CREATE TRIGGER trgEventLogs_InsteadOfDelete
ON EventLogs
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgEventLogs_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@title NVARCHAR(200),
		@additionalData NVARCHAR(MAX),
		@happenedAt DATETIMEOFFSET;
	DECLARE _cursor_trgEventLogs_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[Title],
				[AdditionalData],
				[HappenedAt]
			FROM deleted;
	OPEN _cursor_trgEventLogs_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgEventLogs_InsteadOfDelete INTO @id, @title, @additionalData, @happenedAt;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM EventLogs WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'EventLogs', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'EventLogs', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('Title', 'NVARCHAR(200)', 0, @title, NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL), 
						('HappenedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @happenedAt), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE EventLogs WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'EventLogs', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'EventLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgEventLogs_InsteadOfDelete INTO @id, @title, @additionalData, @happenedAt;
		END
	CLOSE _cursor_trgEventLogs_InsteadOfDelete;
	DEALLOCATE _cursor_trgEventLogs_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgEventLogs_InsteadOfUpdate
ON EventLogs
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgEventLogs_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@title NVARCHAR(200),
		@additionalData NVARCHAR(MAX),
		@happenedAt DATETIMEOFFSET,
		@id_ UNIQUEIDENTIFIER,
		@title_ NVARCHAR(200),
		@additionalData_ NVARCHAR(MAX),
		@happenedAt_ DATETIMEOFFSET;
	DECLARE _cursor_trgEventLogs_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[Title],
				D.[AdditionalData],
				D.[HappenedAt],
				I.[Id],
				I.[Title],
				I.[AdditionalData],
				I.[HappenedAt]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgEventLogs_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgEventLogs_InsteadOfUpdate INTO @id, @title, @additionalData, @happenedAt, @id_, @title_, @additionalData_, @happenedAt_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('Title', 'NVARCHAR(200)', 0, @title, @title_), 
				('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_), 
				('HappenedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @happenedAt), CONVERT(NVARCHAR(MAX), @happenedAt_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'EventLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgEventLogs_InsteadOfUpdate INTO @id, @title, @additionalData, @happenedAt, @id_, @title_, @additionalData_, @happenedAt_;
		END
	CLOSE _cursor_trgEventLogs_InsteadOfUpdate;
	DEALLOCATE _cursor_trgEventLogs_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgEventLogs_InsteadOfInsert
ON EventLogs
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgEventLogs_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@title NVARCHAR(200),
		@additionalData NVARCHAR(MAX),
		@happenedAt DATETIMEOFFSET;
	DECLARE _cursor_trgEventLogs_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[Title],
				[AdditionalData],
				[HappenedAt]
			FROM inserted;
	OPEN _cursor_trgEventLogs_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgEventLogs_InsteadOfInsert INTO @id, @title, @additionalData, @happenedAt;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO EventLogs ([Id], [Title], [AdditionalData], [HappenedAt])
					VALUES (@id, @title, @additionalData, @happenedAt);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('Title', 'NVARCHAR(200)', 0, @title), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData), 
					('HappenedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @happenedAt));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'EventLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgEventLogs_InsteadOfInsert INTO @id, @title, @additionalData, @happenedAt;
		END
	CLOSE _cursor_trgEventLogs_InsteadOfInsert;
	DEALLOCATE _cursor_trgEventLogs_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgClientIpAddresses_InsteadOfDelete
ON ClientIpAddresses
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgClientIpAddresses_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
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
		@isHostingOrColocatedOrDataCenter BIT;
	DECLARE _cursor_trgClientIpAddresses_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[RecordDate],
				[IpAddressV4],
				[Status_IsSuccess],
				[Message],
				[Continent],
				[ContinentCode],
				[Country],
				[CountryCode],
				[Region],
				[RegionName],
				[City],
				[District],
				[ZipCode],
				[Latitude],
				[Longitude],
				[TimeZone],
				[TimeZoneUtcDstOffsetInSeconds],
				[Currency],
				[InternetServiceProviderName],
				[OrganizationName],
				[AutonomousService],
				[AutonomousServiceName],
				[ReverseDnsOfIpAddress],
				[IsMobile],
				[IsProxyOrVpnOrTorExitAddress],
				[IsHostingOrColocatedOrDataCenter]
			FROM deleted;
	OPEN _cursor_trgClientIpAddresses_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgClientIpAddresses_InsteadOfDelete INTO @id, @recordDate, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM ClientIpAddresses WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ClientIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ClientIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate), NULL), 
						('IpAddressV4', 'VARCHAR(15)', 0, @ipAddressV4, NULL), 
						('Status_IsSuccess', 'BIT', 0, CONVERT(NVARCHAR(MAX), @status_IsSuccess), NULL), 
						('Message', 'VARCHAR(200)', 1, @message, NULL), 
						('Continent', 'VARCHAR(20)', 1, @continent, NULL), 
						('ContinentCode', 'VARCHAR(5)', 1, @continentCode, NULL), 
						('Country', 'VARCHAR(50)', 1, @country, NULL), 
						('CountryCode', 'VARCHAR(5)', 1, @countryCode, NULL), 
						('Region', 'VARCHAR(100)', 1, @region, NULL), 
						('RegionName', 'VARCHAR(200)', 1, @regionName, NULL), 
						('City', 'VARCHAR(200)', 1, @city, NULL), 
						('District', 'VARCHAR(200)', 1, @district, NULL), 
						('ZipCode', 'VARCHAR(20)', 1, @zipCode, NULL), 
						('Latitude', 'FLOAT', 1, CONVERT(NVARCHAR(MAX), @latitude), NULL), 
						('Longitude', 'FLOAT', 1, CONVERT(NVARCHAR(MAX), @longitude), NULL), 
						('TimeZone', 'VARCHAR(100)', 1, @timeZone, NULL), 
						('TimeZoneUtcDstOffsetInSeconds', 'INT', 1, CONVERT(NVARCHAR(MAX), @timeZoneUtcDstOffsetInSeconds), NULL), 
						('Currency', 'VARCHAR(10)', 1, @currency, NULL), 
						('InternetServiceProviderName', 'VARCHAR(200)', 1, @internetServiceProviderName, NULL), 
						('OrganizationName', 'VARCHAR(200)', 1, @organizationName, NULL), 
						('AutonomousService', 'VARCHAR(200)', 1, @autonomousService, NULL), 
						('AutonomousServiceName', 'VARCHAR(200)', 1, @autonomousServiceName, NULL), 
						('ReverseDnsOfIpAddress', 'VARCHAR(255)', 1, @reverseDnsOfIpAddress, NULL), 
						('IsMobile', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isMobile), NULL), 
						('IsProxyOrVpnOrTorExitAddress', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isProxyOrVpnOrTorExitAddress), NULL), 
						('IsHostingOrColocatedOrDataCenter', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isHostingOrColocatedOrDataCenter), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE ClientIpAddresses WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'ClientIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ClientIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgClientIpAddresses_InsteadOfDelete INTO @id, @recordDate, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter;
		END
	CLOSE _cursor_trgClientIpAddresses_InsteadOfDelete;
	DEALLOCATE _cursor_trgClientIpAddresses_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgClientIpAddresses_InsteadOfUpdate
ON ClientIpAddresses
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgClientIpAddresses_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
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
		@id_ UNIQUEIDENTIFIER,
		@recordDate_ DATETIMEOFFSET,
		@ipAddressV4_ VARCHAR(15),
		@status_IsSuccess_ BIT,
		@message_ VARCHAR(200),
		@continent_ VARCHAR(20),
		@continentCode_ VARCHAR(5),
		@country_ VARCHAR(50),
		@countryCode_ VARCHAR(5),
		@region_ VARCHAR(100),
		@regionName_ VARCHAR(200),
		@city_ VARCHAR(200),
		@district_ VARCHAR(200),
		@zipCode_ VARCHAR(20),
		@latitude_ FLOAT,
		@longitude_ FLOAT,
		@timeZone_ VARCHAR(100),
		@timeZoneUtcDstOffsetInSeconds_ INT,
		@currency_ VARCHAR(10),
		@internetServiceProviderName_ VARCHAR(200),
		@organizationName_ VARCHAR(200),
		@autonomousService_ VARCHAR(200),
		@autonomousServiceName_ VARCHAR(200),
		@reverseDnsOfIpAddress_ VARCHAR(255),
		@isMobile_ BIT,
		@isProxyOrVpnOrTorExitAddress_ BIT,
		@isHostingOrColocatedOrDataCenter_ BIT;
	DECLARE _cursor_trgClientIpAddresses_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[RecordDate],
				D.[IpAddressV4],
				D.[Status_IsSuccess],
				D.[Message],
				D.[Continent],
				D.[ContinentCode],
				D.[Country],
				D.[CountryCode],
				D.[Region],
				D.[RegionName],
				D.[City],
				D.[District],
				D.[ZipCode],
				D.[Latitude],
				D.[Longitude],
				D.[TimeZone],
				D.[TimeZoneUtcDstOffsetInSeconds],
				D.[Currency],
				D.[InternetServiceProviderName],
				D.[OrganizationName],
				D.[AutonomousService],
				D.[AutonomousServiceName],
				D.[ReverseDnsOfIpAddress],
				D.[IsMobile],
				D.[IsProxyOrVpnOrTorExitAddress],
				D.[IsHostingOrColocatedOrDataCenter],
				I.[Id],
				I.[RecordDate],
				I.[IpAddressV4],
				I.[Status_IsSuccess],
				I.[Message],
				I.[Continent],
				I.[ContinentCode],
				I.[Country],
				I.[CountryCode],
				I.[Region],
				I.[RegionName],
				I.[City],
				I.[District],
				I.[ZipCode],
				I.[Latitude],
				I.[Longitude],
				I.[TimeZone],
				I.[TimeZoneUtcDstOffsetInSeconds],
				I.[Currency],
				I.[InternetServiceProviderName],
				I.[OrganizationName],
				I.[AutonomousService],
				I.[AutonomousServiceName],
				I.[ReverseDnsOfIpAddress],
				I.[IsMobile],
				I.[IsProxyOrVpnOrTorExitAddress],
				I.[IsHostingOrColocatedOrDataCenter]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgClientIpAddresses_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgClientIpAddresses_InsteadOfUpdate INTO @id, @recordDate, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter, @id_, @recordDate_, @ipAddressV4_, @status_IsSuccess_, @message_, @continent_, @continentCode_, @country_, @countryCode_, @region_, @regionName_, @city_, @district_, @zipCode_, @latitude_, @longitude_, @timeZone_, @timeZoneUtcDstOffsetInSeconds_, @currency_, @internetServiceProviderName_, @organizationName_, @autonomousService_, @autonomousServiceName_, @reverseDnsOfIpAddress_, @isMobile_, @isProxyOrVpnOrTorExitAddress_, @isHostingOrColocatedOrDataCenter_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate), CONVERT(NVARCHAR(MAX), @recordDate_)), 
				('IpAddressV4', 'VARCHAR(15)', 0, @ipAddressV4, @ipAddressV4_), 
				('Status_IsSuccess', 'BIT', 0, CONVERT(NVARCHAR(MAX), @status_IsSuccess), CONVERT(NVARCHAR(MAX), @status_IsSuccess_)), 
				('Message', 'VARCHAR(200)', 1, @message, @message_), 
				('Continent', 'VARCHAR(20)', 1, @continent, @continent_), 
				('ContinentCode', 'VARCHAR(5)', 1, @continentCode, @continentCode_), 
				('Country', 'VARCHAR(50)', 1, @country, @country_), 
				('CountryCode', 'VARCHAR(5)', 1, @countryCode, @countryCode_), 
				('Region', 'VARCHAR(100)', 1, @region, @region_), 
				('RegionName', 'VARCHAR(200)', 1, @regionName, @regionName_), 
				('City', 'VARCHAR(200)', 1, @city, @city_), 
				('District', 'VARCHAR(200)', 1, @district, @district_), 
				('ZipCode', 'VARCHAR(20)', 1, @zipCode, @zipCode_), 
				('Latitude', 'FLOAT', 1, CONVERT(NVARCHAR(MAX), @latitude), CONVERT(NVARCHAR(MAX), @latitude_)), 
				('Longitude', 'FLOAT', 1, CONVERT(NVARCHAR(MAX), @longitude), CONVERT(NVARCHAR(MAX), @longitude_)), 
				('TimeZone', 'VARCHAR(100)', 1, @timeZone, @timeZone_), 
				('TimeZoneUtcDstOffsetInSeconds', 'INT', 1, CONVERT(NVARCHAR(MAX), @timeZoneUtcDstOffsetInSeconds), CONVERT(NVARCHAR(MAX), @timeZoneUtcDstOffsetInSeconds_)), 
				('Currency', 'VARCHAR(10)', 1, @currency, @currency_), 
				('InternetServiceProviderName', 'VARCHAR(200)', 1, @internetServiceProviderName, @internetServiceProviderName_), 
				('OrganizationName', 'VARCHAR(200)', 1, @organizationName, @organizationName_), 
				('AutonomousService', 'VARCHAR(200)', 1, @autonomousService, @autonomousService_), 
				('AutonomousServiceName', 'VARCHAR(200)', 1, @autonomousServiceName, @autonomousServiceName_), 
				('ReverseDnsOfIpAddress', 'VARCHAR(255)', 1, @reverseDnsOfIpAddress, @reverseDnsOfIpAddress_), 
				('IsMobile', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isMobile), CONVERT(NVARCHAR(MAX), @isMobile_)), 
				('IsProxyOrVpnOrTorExitAddress', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isProxyOrVpnOrTorExitAddress), CONVERT(NVARCHAR(MAX), @isProxyOrVpnOrTorExitAddress_)), 
				('IsHostingOrColocatedOrDataCenter', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isHostingOrColocatedOrDataCenter), CONVERT(NVARCHAR(MAX), @isHostingOrColocatedOrDataCenter_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'ClientIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgClientIpAddresses_InsteadOfUpdate INTO @id, @recordDate, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter, @id_, @recordDate_, @ipAddressV4_, @status_IsSuccess_, @message_, @continent_, @continentCode_, @country_, @countryCode_, @region_, @regionName_, @city_, @district_, @zipCode_, @latitude_, @longitude_, @timeZone_, @timeZoneUtcDstOffsetInSeconds_, @currency_, @internetServiceProviderName_, @organizationName_, @autonomousService_, @autonomousServiceName_, @reverseDnsOfIpAddress_, @isMobile_, @isProxyOrVpnOrTorExitAddress_, @isHostingOrColocatedOrDataCenter_;
		END
	CLOSE _cursor_trgClientIpAddresses_InsteadOfUpdate;
	DEALLOCATE _cursor_trgClientIpAddresses_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgClientIpAddresses_InsteadOfInsert
ON ClientIpAddresses
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgClientIpAddresses_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
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
		@isHostingOrColocatedOrDataCenter BIT;
	DECLARE _cursor_trgClientIpAddresses_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[RecordDate],
				[IpAddressV4],
				[Status_IsSuccess],
				[Message],
				[Continent],
				[ContinentCode],
				[Country],
				[CountryCode],
				[Region],
				[RegionName],
				[City],
				[District],
				[ZipCode],
				[Latitude],
				[Longitude],
				[TimeZone],
				[TimeZoneUtcDstOffsetInSeconds],
				[Currency],
				[InternetServiceProviderName],
				[OrganizationName],
				[AutonomousService],
				[AutonomousServiceName],
				[ReverseDnsOfIpAddress],
				[IsMobile],
				[IsProxyOrVpnOrTorExitAddress],
				[IsHostingOrColocatedOrDataCenter]
			FROM inserted;
	OPEN _cursor_trgClientIpAddresses_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgClientIpAddresses_InsteadOfInsert INTO @id, @recordDate, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO ClientIpAddresses ([Id], [RecordDate], [IpAddressV4], [Status_IsSuccess], [Message], [Continent], [ContinentCode], [Country], [CountryCode], [Region], [RegionName], [City], [District], [ZipCode], [Latitude], [Longitude], [TimeZone], [TimeZoneUtcDstOffsetInSeconds], [Currency], [InternetServiceProviderName], [OrganizationName], [AutonomousService], [AutonomousServiceName], [ReverseDnsOfIpAddress], [IsMobile], [IsProxyOrVpnOrTorExitAddress], [IsHostingOrColocatedOrDataCenter])
					VALUES (@id, @recordDate, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate)), 
					('IpAddressV4', 'VARCHAR(15)', 0, @ipAddressV4), 
					('Status_IsSuccess', 'BIT', 0, CONVERT(NVARCHAR(MAX), @status_IsSuccess)), 
					('Message', 'VARCHAR(200)', 1, @message), 
					('Continent', 'VARCHAR(20)', 1, @continent), 
					('ContinentCode', 'VARCHAR(5)', 1, @continentCode), 
					('Country', 'VARCHAR(50)', 1, @country), 
					('CountryCode', 'VARCHAR(5)', 1, @countryCode), 
					('Region', 'VARCHAR(100)', 1, @region), 
					('RegionName', 'VARCHAR(200)', 1, @regionName), 
					('City', 'VARCHAR(200)', 1, @city), 
					('District', 'VARCHAR(200)', 1, @district), 
					('ZipCode', 'VARCHAR(20)', 1, @zipCode), 
					('Latitude', 'FLOAT', 1, CONVERT(NVARCHAR(MAX), @latitude)), 
					('Longitude', 'FLOAT', 1, CONVERT(NVARCHAR(MAX), @longitude)), 
					('TimeZone', 'VARCHAR(100)', 1, @timeZone), 
					('TimeZoneUtcDstOffsetInSeconds', 'INT', 1, CONVERT(NVARCHAR(MAX), @timeZoneUtcDstOffsetInSeconds)), 
					('Currency', 'VARCHAR(10)', 1, @currency), 
					('InternetServiceProviderName', 'VARCHAR(200)', 1, @internetServiceProviderName), 
					('OrganizationName', 'VARCHAR(200)', 1, @organizationName), 
					('AutonomousService', 'VARCHAR(200)', 1, @autonomousService), 
					('AutonomousServiceName', 'VARCHAR(200)', 1, @autonomousServiceName), 
					('ReverseDnsOfIpAddress', 'VARCHAR(255)', 1, @reverseDnsOfIpAddress), 
					('IsMobile', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isMobile)), 
					('IsProxyOrVpnOrTorExitAddress', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isProxyOrVpnOrTorExitAddress)), 
					('IsHostingOrColocatedOrDataCenter', 'BIT', 1, CONVERT(NVARCHAR(MAX), @isHostingOrColocatedOrDataCenter));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'ClientIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgClientIpAddresses_InsteadOfInsert INTO @id, @recordDate, @ipAddressV4, @status_IsSuccess, @message, @continent, @continentCode, @country, @countryCode, @region, @regionName, @city, @district, @zipCode, @latitude, @longitude, @timeZone, @timeZoneUtcDstOffsetInSeconds, @currency, @internetServiceProviderName, @organizationName, @autonomousService, @autonomousServiceName, @reverseDnsOfIpAddress, @isMobile, @isProxyOrVpnOrTorExitAddress, @isHostingOrColocatedOrDataCenter;
		END
	CLOSE _cursor_trgClientIpAddresses_InsteadOfInsert;
	DEALLOCATE _cursor_trgClientIpAddresses_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgBanishments_InsteadOfDelete
ON Banishments
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgBanishments_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@banishmentType_EnumerationValue TINYINT,
		@banishedValue NVARCHAR(MAX),
		@startsAt DATETIMEOFFSET,
		@endsAt DATETIMEOFFSET,
		@isEnabled BIT;
	DECLARE _cursor_trgBanishments_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[BanishmentType_EnumerationValue],
				[BanishedValue],
				[StartsAt],
				[EndsAt],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgBanishments_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgBanishments_InsteadOfDelete INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Banishments WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('BanishmentType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @banishmentType_EnumerationValue), NULL), 
						('BanishedValue', 'NVARCHAR(MAX)', 0, @banishedValue, NULL), 
						('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt), NULL), 
						('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt), NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE Banishments WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_Banishments] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgBanishments_InsteadOfDelete INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled;
		END
	CLOSE _cursor_trgBanishments_InsteadOfDelete;
	DEALLOCATE _cursor_trgBanishments_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgBanishments_InsteadOfUpdate
ON Banishments
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgBanishments_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@banishmentType_EnumerationValue TINYINT,
		@banishedValue NVARCHAR(MAX),
		@startsAt DATETIMEOFFSET,
		@endsAt DATETIMEOFFSET,
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@banishmentType_EnumerationValue_ TINYINT,
		@banishedValue_ NVARCHAR(MAX),
		@startsAt_ DATETIMEOFFSET,
		@endsAt_ DATETIMEOFFSET,
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND D.[BanishmentType_EnumerationValue] = I.[BanishmentType_EnumerationValue] AND D.[BanishedValue] = I.[BanishedValue] AND D.[StartsAt] = I.[StartsAt] AND ((D.[EndsAt] IS NULL AND I.[EndsAt] IS NULL) OR D.[EndsAt] = I.[EndsAt])
		WHERE
			D.[IsEnabled] <> I.[IsEnabled];
	DECLARE _cursor_trgBanishments_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[BanishmentType_EnumerationValue],
				D.[BanishedValue],
				D.[StartsAt],
				D.[EndsAt],
				D.[IsEnabled],
				I.[Id],
				I.[BanishmentType_EnumerationValue],
				I.[BanishedValue],
				I.[StartsAt],
				I.[EndsAt],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgBanishments_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgBanishments_InsteadOfUpdate INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled, @id_, @banishmentType_EnumerationValue_, @banishedValue_, @startsAt_, @endsAt_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('BanishmentType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @banishmentType_EnumerationValue), CONVERT(NVARCHAR(MAX), @banishmentType_EnumerationValue_)), 
						('BanishedValue', 'NVARCHAR(MAX)', 0, @banishedValue, @banishedValue_), 
						('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt), CONVERT(NVARCHAR(MAX), @startsAt_)), 
						('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt), CONVERT(NVARCHAR(MAX), @endsAt_)), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_));
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgBanishments_InsteadOfUpdate INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled, @id_, @banishmentType_EnumerationValue_, @banishedValue_, @startsAt_, @endsAt_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgBanishments_InsteadOfUpdate INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled, @id_, @banishmentType_EnumerationValue_, @banishedValue_, @startsAt_, @endsAt_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM Banishments WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE Banishments
									SET
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgBanishments_InsteadOfUpdate INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled, @id_, @banishmentType_EnumerationValue_, @banishedValue_, @startsAt_, @endsAt_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgBanishments_InsteadOfUpdate;
	DEALLOCATE _cursor_trgBanishments_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgBanishments_InsteadOfInsert
ON Banishments
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgBanishments_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@banishmentType_EnumerationValue TINYINT,
		@banishedValue NVARCHAR(MAX),
		@startsAt DATETIMEOFFSET,
		@endsAt DATETIMEOFFSET,
		@isEnabled BIT;
	DECLARE _cursor_trgBanishments_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[BanishmentType_EnumerationValue],
				[BanishedValue],
				[StartsAt],
				[EndsAt],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgBanishments_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgBanishments_InsteadOfInsert INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						INSERT INTO Banishments ([Id], [BanishmentType_EnumerationValue], [BanishedValue], [StartsAt], [EndsAt], [IsEnabled])
							VALUES (@id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 1, 1, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('BanishmentType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @banishmentType_EnumerationValue)), 
					('BanishedValue', 'NVARCHAR(MAX)', 0, @banishedValue), 
					('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt)), 
					('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt)), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'Banishments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgBanishments_InsteadOfInsert INTO @id, @banishmentType_EnumerationValue, @banishedValue, @startsAt, @endsAt, @isEnabled;
		END
	CLOSE _cursor_trgBanishments_InsteadOfInsert;
	DEALLOCATE _cursor_trgBanishments_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgSessions_InsteadOfDelete
ON Sessions
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSessions_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@startedAt DATETIMEOFFSET,
		@applicationSessionId VARCHAR(100),
		@initialClientIpAddressId UNIQUEIDENTIFIER,
		@sessionVariables NVARCHAR(MAX);
	DECLARE _cursor_trgSessions_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[StartedAt],
				[ApplicationSessionId],
				[InitialClientIpAddressId],
				[SessionVariables]
			FROM deleted;
	OPEN _cursor_trgSessions_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgSessions_InsteadOfDelete INTO @id, @startedAt, @applicationSessionId, @initialClientIpAddressId, @sessionVariables;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Sessions WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Sessions', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Sessions', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('StartedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startedAt), NULL), 
						('ApplicationSessionId', 'VARCHAR(100)', 0, @applicationSessionId, NULL), 
						('InitialClientIpAddressId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @initialClientIpAddressId), NULL), 
						('SessionVariables', 'NVARCHAR(MAX)', 1, @sessionVariables, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE Sessions WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'Sessions', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Sessions', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgSessions_InsteadOfDelete INTO @id, @startedAt, @applicationSessionId, @initialClientIpAddressId, @sessionVariables;
		END
	CLOSE _cursor_trgSessions_InsteadOfDelete;
	DEALLOCATE _cursor_trgSessions_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgSessions_InsteadOfUpdate
ON Sessions
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSessions_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@startedAt DATETIMEOFFSET,
		@applicationSessionId VARCHAR(100),
		@initialClientIpAddressId UNIQUEIDENTIFIER,
		@sessionVariables NVARCHAR(MAX),
		@id_ UNIQUEIDENTIFIER,
		@startedAt_ DATETIMEOFFSET,
		@applicationSessionId_ VARCHAR(100),
		@initialClientIpAddressId_ UNIQUEIDENTIFIER,
		@sessionVariables_ NVARCHAR(MAX);
	DECLARE _cursor_trgSessions_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[StartedAt],
				D.[ApplicationSessionId],
				D.[InitialClientIpAddressId],
				D.[SessionVariables],
				I.[Id],
				I.[StartedAt],
				I.[ApplicationSessionId],
				I.[InitialClientIpAddressId],
				I.[SessionVariables]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgSessions_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgSessions_InsteadOfUpdate INTO @id, @startedAt, @applicationSessionId, @initialClientIpAddressId, @sessionVariables, @id_, @startedAt_, @applicationSessionId_, @initialClientIpAddressId_, @sessionVariables_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('StartedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startedAt), CONVERT(NVARCHAR(MAX), @startedAt_)), 
				('ApplicationSessionId', 'VARCHAR(100)', 0, @applicationSessionId, @applicationSessionId_), 
				('InitialClientIpAddressId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @initialClientIpAddressId), CONVERT(NVARCHAR(MAX), @initialClientIpAddressId_)), 
				('SessionVariables', 'NVARCHAR(MAX)', 1, @sessionVariables, @sessionVariables_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Sessions', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgSessions_InsteadOfUpdate INTO @id, @startedAt, @applicationSessionId, @initialClientIpAddressId, @sessionVariables, @id_, @startedAt_, @applicationSessionId_, @initialClientIpAddressId_, @sessionVariables_;
		END
	CLOSE _cursor_trgSessions_InsteadOfUpdate;
	DEALLOCATE _cursor_trgSessions_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgSessions_InsteadOfInsert
ON Sessions
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSessions_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@startedAt DATETIMEOFFSET,
		@applicationSessionId VARCHAR(100),
		@initialClientIpAddressId UNIQUEIDENTIFIER,
		@sessionVariables NVARCHAR(MAX);
	DECLARE _cursor_trgSessions_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[StartedAt],
				[ApplicationSessionId],
				[InitialClientIpAddressId],
				[SessionVariables]
			FROM inserted;
	OPEN _cursor_trgSessions_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgSessions_InsteadOfInsert INTO @id, @startedAt, @applicationSessionId, @initialClientIpAddressId, @sessionVariables;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO Sessions ([Id], [StartedAt], [ApplicationSessionId], [InitialClientIpAddressId], [SessionVariables])
					VALUES (@id, @startedAt, @applicationSessionId, @initialClientIpAddressId, @sessionVariables);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('StartedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startedAt)), 
					('ApplicationSessionId', 'VARCHAR(100)', 0, @applicationSessionId), 
					('InitialClientIpAddressId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @initialClientIpAddressId)), 
					('SessionVariables', 'NVARCHAR(MAX)', 1, @sessionVariables);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'Sessions', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgSessions_InsteadOfInsert INTO @id, @startedAt, @applicationSessionId, @initialClientIpAddressId, @sessionVariables;
		END
	CLOSE _cursor_trgSessions_InsteadOfInsert;
	DEALLOCATE _cursor_trgSessions_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgAbandonedSessions_InsteadOfDelete
ON AbandonedSessions
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgAbandonedSessions_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sessionId UNIQUEIDENTIFIER,
		@happenedAt DATETIMEOFFSET,
		@title NVARCHAR(200),
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgAbandonedSessions_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[SessionId],
				[HappenedAt],
				[Title],
				[AdditionalData]
			FROM deleted;
	OPEN _cursor_trgAbandonedSessions_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgAbandonedSessions_InsteadOfDelete INTO @id, @sessionId, @happenedAt, @title, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM AbandonedSessions WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'AbandonedSessions', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'AbandonedSessions', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('SessionId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sessionId), NULL), 
						('HappenedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @happenedAt), NULL), 
						('Title', 'NVARCHAR(200)', 0, @title, NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE AbandonedSessions WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'AbandonedSessions', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'AbandonedSessions', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgAbandonedSessions_InsteadOfDelete INTO @id, @sessionId, @happenedAt, @title, @additionalData;
		END
	CLOSE _cursor_trgAbandonedSessions_InsteadOfDelete;
	DEALLOCATE _cursor_trgAbandonedSessions_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgAbandonedSessions_InsteadOfUpdate
ON AbandonedSessions
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgAbandonedSessions_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sessionId UNIQUEIDENTIFIER,
		@happenedAt DATETIMEOFFSET,
		@title NVARCHAR(200),
		@additionalData NVARCHAR(MAX),
		@id_ UNIQUEIDENTIFIER,
		@sessionId_ UNIQUEIDENTIFIER,
		@happenedAt_ DATETIMEOFFSET,
		@title_ NVARCHAR(200),
		@additionalData_ NVARCHAR(MAX);
	DECLARE _cursor_trgAbandonedSessions_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[SessionId],
				D.[HappenedAt],
				D.[Title],
				D.[AdditionalData],
				I.[Id],
				I.[SessionId],
				I.[HappenedAt],
				I.[Title],
				I.[AdditionalData]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgAbandonedSessions_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgAbandonedSessions_InsteadOfUpdate INTO @id, @sessionId, @happenedAt, @title, @additionalData, @id_, @sessionId_, @happenedAt_, @title_, @additionalData_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('SessionId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sessionId), CONVERT(NVARCHAR(MAX), @sessionId_)), 
				('HappenedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @happenedAt), CONVERT(NVARCHAR(MAX), @happenedAt_)), 
				('Title', 'NVARCHAR(200)', 0, @title, @title_), 
				('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'AbandonedSessions', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgAbandonedSessions_InsteadOfUpdate INTO @id, @sessionId, @happenedAt, @title, @additionalData, @id_, @sessionId_, @happenedAt_, @title_, @additionalData_;
		END
	CLOSE _cursor_trgAbandonedSessions_InsteadOfUpdate;
	DEALLOCATE _cursor_trgAbandonedSessions_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgAbandonedSessions_InsteadOfInsert
ON AbandonedSessions
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgAbandonedSessions_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sessionId UNIQUEIDENTIFIER,
		@happenedAt DATETIMEOFFSET,
		@title NVARCHAR(200),
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgAbandonedSessions_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[SessionId],
				[HappenedAt],
				[Title],
				[AdditionalData]
			FROM inserted;
	OPEN _cursor_trgAbandonedSessions_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgAbandonedSessions_InsteadOfInsert INTO @id, @sessionId, @happenedAt, @title, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO AbandonedSessions ([Id], [SessionId], [HappenedAt], [Title], [AdditionalData])
					VALUES (@id, @sessionId, @happenedAt, @title, @additionalData);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 1, 'AbandonedSessions', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
					IF @@TRANCOUNT > 0
						COMMIT TRANSACTION;
				END TRY
				BEGIN CATCH
					IF @@TRANCOUNT > 0
						ROLLBACK TRANSACTION;
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('SessionId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sessionId)), 
					('HappenedAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @happenedAt)), 
					('Title', 'NVARCHAR(200)', 0, @title), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'AbandonedSessions', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgAbandonedSessions_InsteadOfInsert INTO @id, @sessionId, @happenedAt, @title, @additionalData;
		END
	CLOSE _cursor_trgAbandonedSessions_InsteadOfInsert;
	DEALLOCATE _cursor_trgAbandonedSessions_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgPageRequests_InsteadOfDelete
ON PageRequests
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgPageRequests_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sessionId UNIQUEIDENTIFIER,
		@requestDate DATETIMEOFFSET,
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
		@resultFailureReason_EnumerationValue INT,
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgPageRequests_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[SessionId],
				[RequestDate],
				[ClientIpAddressId],
				[Url],
				[Path],
				[QueryString],
				[UrlReferrer],
				[UserHostAddress],
				[UserHostName],
				[UserAgent],
				[UserLanguages],
				[IsSecureConnection],
				[HttpMethod],
				[ContentEncoding],
				[FilesCount],
				[TotalBytes],
				[RequestVariables],
				[FailureReason_EnumerationValue],
				[AdditionalData]
			FROM deleted;
	OPEN _cursor_trgPageRequests_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgPageRequests_InsteadOfDelete INTO @id, @sessionId, @requestDate, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM PageRequests WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'PageRequests', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'PageRequests', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('SessionId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sessionId), NULL), 
						('RequestDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @requestDate), NULL), 
						('ClientIpAddressId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @clientIpAddressId), NULL), 
						('Url', 'NVARCHAR(MAX)', 0, @url, NULL), 
						('Path', 'NVARCHAR(MAX)', 0, @path, NULL), 
						('QueryString', 'NVARCHAR(MAX)', 1, @queryString, NULL), 
						('UrlReferrer', 'NVARCHAR(MAX)', 1, @urlReferrer, NULL), 
						('UserHostAddress', 'NVARCHAR(50)', 0, @userHostAddress, NULL), 
						('UserHostName', 'NVARCHAR(MAX)', 1, @userHostName, NULL), 
						('UserAgent', 'NVARCHAR(MAX)', 1, @userAgent, NULL), 
						('UserLanguages', 'NVARCHAR(MAX)', 1, @userLanguages, NULL), 
						('IsSecureConnection', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isSecureConnection), NULL), 
						('HttpMethod', 'VARCHAR(50)', 0, @httpMethod, NULL), 
						('ContentEncoding', 'VARCHAR(100)', 1, @contentEncoding, NULL), 
						('FilesCount', 'INT', 0, CONVERT(NVARCHAR(MAX), @filesCount), NULL), 
						('TotalBytes', 'BIGINT', 0, CONVERT(NVARCHAR(MAX), @totalBytes), NULL), 
						('RequestVariables', 'NVARCHAR(MAX)', 1, @requestVariables, NULL), 
						('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE PageRequests WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'PageRequests', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'PageRequests', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgPageRequests_InsteadOfDelete INTO @id, @sessionId, @requestDate, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData;
		END
	CLOSE _cursor_trgPageRequests_InsteadOfDelete;
	DEALLOCATE _cursor_trgPageRequests_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgPageRequests_InsteadOfUpdate
ON PageRequests
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgPageRequests_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sessionId UNIQUEIDENTIFIER,
		@requestDate DATETIMEOFFSET,
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
		@resultFailureReason_EnumerationValue INT,
		@additionalData NVARCHAR(MAX),
		@id_ UNIQUEIDENTIFIER,
		@sessionId_ UNIQUEIDENTIFIER,
		@requestDate_ DATETIMEOFFSET,
		@clientIpAddressId_ UNIQUEIDENTIFIER,
		@url_ NVARCHAR(MAX),
		@path_ NVARCHAR(MAX),
		@queryString_ NVARCHAR(MAX),
		@urlReferrer_ NVARCHAR(MAX),
		@userHostAddress_ NVARCHAR(50),
		@userHostName_ NVARCHAR(MAX),
		@userAgent_ NVARCHAR(MAX),
		@userLanguages_ NVARCHAR(MAX),
		@isSecureConnection_ BIT,
		@httpMethod_ VARCHAR(50),
		@contentEncoding_ VARCHAR(100),
		@filesCount_ INT,
		@totalBytes_ BIGINT,
		@requestVariables_ NVARCHAR(MAX),
		@resultFailureReason_EnumerationValue_ INT,
		@additionalData_ NVARCHAR(MAX);
	DECLARE _cursor_trgPageRequests_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[SessionId],
				D.[RequestDate],
				D.[ClientIpAddressId],
				D.[Url],
				D.[Path],
				D.[QueryString],
				D.[UrlReferrer],
				D.[UserHostAddress],
				D.[UserHostName],
				D.[UserAgent],
				D.[UserLanguages],
				D.[IsSecureConnection],
				D.[HttpMethod],
				D.[ContentEncoding],
				D.[FilesCount],
				D.[TotalBytes],
				D.[RequestVariables],
				D.[FailureReason_EnumerationValue],
				D.[AdditionalData],
				I.[Id],
				I.[SessionId],
				I.[RequestDate],
				I.[ClientIpAddressId],
				I.[Url],
				I.[Path],
				I.[QueryString],
				I.[UrlReferrer],
				I.[UserHostAddress],
				I.[UserHostName],
				I.[UserAgent],
				I.[UserLanguages],
				I.[IsSecureConnection],
				I.[HttpMethod],
				I.[ContentEncoding],
				I.[FilesCount],
				I.[TotalBytes],
				I.[RequestVariables],
				I.[FailureReason_EnumerationValue],
				I.[AdditionalData]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgPageRequests_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgPageRequests_InsteadOfUpdate INTO @id, @sessionId, @requestDate, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData, @id_, @sessionId_, @requestDate_, @clientIpAddressId_, @url_, @path_, @queryString_, @urlReferrer_, @userHostAddress_, @userHostName_, @userAgent_, @userLanguages_, @isSecureConnection_, @httpMethod_, @contentEncoding_, @filesCount_, @totalBytes_, @requestVariables_, @resultFailureReason_EnumerationValue_, @additionalData_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('SessionId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sessionId), CONVERT(NVARCHAR(MAX), @sessionId_)), 
				('RequestDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @requestDate), CONVERT(NVARCHAR(MAX), @requestDate_)), 
				('ClientIpAddressId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @clientIpAddressId), CONVERT(NVARCHAR(MAX), @clientIpAddressId_)), 
				('Url', 'NVARCHAR(MAX)', 0, @url, @url_), 
				('Path', 'NVARCHAR(MAX)', 0, @path, @path_), 
				('QueryString', 'NVARCHAR(MAX)', 1, @queryString, @queryString_), 
				('UrlReferrer', 'NVARCHAR(MAX)', 1, @urlReferrer, @urlReferrer_), 
				('UserHostAddress', 'NVARCHAR(50)', 0, @userHostAddress, @userHostAddress_), 
				('UserHostName', 'NVARCHAR(MAX)', 1, @userHostName, @userHostName_), 
				('UserAgent', 'NVARCHAR(MAX)', 1, @userAgent, @userAgent_), 
				('UserLanguages', 'NVARCHAR(MAX)', 1, @userLanguages, @userLanguages_), 
				('IsSecureConnection', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isSecureConnection), CONVERT(NVARCHAR(MAX), @isSecureConnection_)), 
				('HttpMethod', 'VARCHAR(50)', 0, @httpMethod, @httpMethod_), 
				('ContentEncoding', 'VARCHAR(100)', 1, @contentEncoding, @contentEncoding_), 
				('FilesCount', 'INT', 0, CONVERT(NVARCHAR(MAX), @filesCount), CONVERT(NVARCHAR(MAX), @filesCount_)), 
				('TotalBytes', 'BIGINT', 0, CONVERT(NVARCHAR(MAX), @totalBytes), CONVERT(NVARCHAR(MAX), @totalBytes_)), 
				('RequestVariables', 'NVARCHAR(MAX)', 1, @requestVariables, @requestVariables_), 
				('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue_)), 
				('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'PageRequests', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgPageRequests_InsteadOfUpdate INTO @id, @sessionId, @requestDate, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData, @id_, @sessionId_, @requestDate_, @clientIpAddressId_, @url_, @path_, @queryString_, @urlReferrer_, @userHostAddress_, @userHostName_, @userAgent_, @userLanguages_, @isSecureConnection_, @httpMethod_, @contentEncoding_, @filesCount_, @totalBytes_, @requestVariables_, @resultFailureReason_EnumerationValue_, @additionalData_;
		END
	CLOSE _cursor_trgPageRequests_InsteadOfUpdate;
	DEALLOCATE _cursor_trgPageRequests_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgPageRequests_InsteadOfInsert
ON PageRequests
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgPageRequests_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sessionId UNIQUEIDENTIFIER,
		@requestDate DATETIMEOFFSET,
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
		@resultFailureReason_EnumerationValue INT,
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgPageRequests_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[SessionId],
				[RequestDate],
				[ClientIpAddressId],
				[Url],
				[Path],
				[QueryString],
				[UrlReferrer],
				[UserHostAddress],
				[UserHostName],
				[UserAgent],
				[UserLanguages],
				[IsSecureConnection],
				[HttpMethod],
				[ContentEncoding],
				[FilesCount],
				[TotalBytes],
				[RequestVariables],
				[FailureReason_EnumerationValue],
				[AdditionalData]
			FROM inserted;
	OPEN _cursor_trgPageRequests_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgPageRequests_InsteadOfInsert INTO @id, @sessionId, @requestDate, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO PageRequests ([Id], [SessionId], [RequestDate], [ClientIpAddressId], [Url], [Path], [QueryString], [UrlReferrer], [UserHostAddress], [UserHostName], [UserAgent], [UserLanguages], [IsSecureConnection], [HttpMethod], [ContentEncoding], [FilesCount], [TotalBytes], [RequestVariables], [FailureReason_EnumerationValue], [AdditionalData])
					VALUES (@id, @sessionId, @requestDate, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('SessionId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sessionId)), 
					('RequestDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @requestDate)), 
					('ClientIpAddressId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @clientIpAddressId)), 
					('Url', 'NVARCHAR(MAX)', 0, @url), 
					('Path', 'NVARCHAR(MAX)', 0, @path), 
					('QueryString', 'NVARCHAR(MAX)', 1, @queryString), 
					('UrlReferrer', 'NVARCHAR(MAX)', 1, @urlReferrer), 
					('UserHostAddress', 'NVARCHAR(50)', 0, @userHostAddress), 
					('UserHostName', 'NVARCHAR(MAX)', 1, @userHostName), 
					('UserAgent', 'NVARCHAR(MAX)', 1, @userAgent), 
					('UserLanguages', 'NVARCHAR(MAX)', 1, @userLanguages), 
					('IsSecureConnection', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isSecureConnection)), 
					('HttpMethod', 'VARCHAR(50)', 0, @httpMethod), 
					('ContentEncoding', 'VARCHAR(100)', 1, @contentEncoding), 
					('FilesCount', 'INT', 0, CONVERT(NVARCHAR(MAX), @filesCount)), 
					('TotalBytes', 'BIGINT', 0, CONVERT(NVARCHAR(MAX), @totalBytes)), 
					('RequestVariables', 'NVARCHAR(MAX)', 1, @requestVariables), 
					('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue)), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'PageRequests', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgPageRequests_InsteadOfInsert INTO @id, @sessionId, @requestDate, @clientIpAddressId, @url, @path, @queryString, @urlReferrer, @userHostAddress, @userHostName, @userAgent, @userLanguages, @isSecureConnection, @httpMethod, @contentEncoding, @filesCount, @totalBytes, @requestVariables, @resultFailureReason_EnumerationValue, @additionalData;
		END
	CLOSE _cursor_trgPageRequests_InsteadOfInsert;
	DEALLOCATE _cursor_trgPageRequests_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgFormResults_InsteadOfDelete
ON FormResults
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgFormResults_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
		@formName NVARCHAR(200);
	DECLARE _cursor_trgFormResults_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[RecordDate],
				[FormName]
			FROM deleted;
	OPEN _cursor_trgFormResults_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgFormResults_InsteadOfDelete INTO @id, @pageRequestId, @recordDate, @formName;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM FormResults WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'FormResults', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'FormResults', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), NULL), 
						('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate), NULL), 
						('FormName', 'NVARCHAR(200)', 0, @formName, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE FormResults WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'FormResults', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'FormResults', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgFormResults_InsteadOfDelete INTO @id, @pageRequestId, @recordDate, @formName;
		END
	CLOSE _cursor_trgFormResults_InsteadOfDelete;
	DEALLOCATE _cursor_trgFormResults_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgFormResults_InsteadOfUpdate
ON FormResults
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgFormResults_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
		@formName NVARCHAR(200),
		@id_ UNIQUEIDENTIFIER,
		@pageRequestId_ UNIQUEIDENTIFIER,
		@recordDate_ DATETIMEOFFSET,
		@formName_ NVARCHAR(200);
	DECLARE _cursor_trgFormResults_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[PageRequestId],
				D.[RecordDate],
				D.[FormName],
				I.[Id],
				I.[PageRequestId],
				I.[RecordDate],
				I.[FormName]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgFormResults_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgFormResults_InsteadOfUpdate INTO @id, @pageRequestId, @recordDate, @formName, @id_, @pageRequestId_, @recordDate_, @formName_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), CONVERT(NVARCHAR(MAX), @pageRequestId_)), 
				('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate), CONVERT(NVARCHAR(MAX), @recordDate_)), 
				('FormName', 'NVARCHAR(200)', 0, @formName, @formName_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'FormResults', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgFormResults_InsteadOfUpdate INTO @id, @pageRequestId, @recordDate, @formName, @id_, @pageRequestId_, @recordDate_, @formName_;
		END
	CLOSE _cursor_trgFormResults_InsteadOfUpdate;
	DEALLOCATE _cursor_trgFormResults_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgFormResults_InsteadOfInsert
ON FormResults
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgFormResults_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
		@formName NVARCHAR(200);
	DECLARE _cursor_trgFormResults_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[RecordDate],
				[FormName]
			FROM inserted;
	OPEN _cursor_trgFormResults_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgFormResults_InsteadOfInsert INTO @id, @pageRequestId, @recordDate, @formName;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO FormResults ([Id], [PageRequestId], [RecordDate], [FormName])
					VALUES (@id, @pageRequestId, @recordDate, @formName);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId)), 
					('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate)), 
					('FormName', 'NVARCHAR(200)', 0, @formName);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'FormResults', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgFormResults_InsteadOfInsert INTO @id, @pageRequestId, @recordDate, @formName;
		END
	CLOSE _cursor_trgFormResults_InsteadOfInsert;
	DEALLOCATE _cursor_trgFormResults_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgFormResultComponents_InsteadOfDelete
ON FormResultComponents
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgFormResultComponents_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@formResultId UNIQUEIDENTIFIER,
		@order INT,
		@displayName NVARCHAR(200),
		@valueTypes_EnumerationValue TINYINT,
		@valueTypeName VARCHAR(300),
		@value_String NVARCHAR(MAX),
		@value_Binary VARBINARY(MAX);
	DECLARE _cursor_trgFormResultComponents_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[FormResultId],
				[Order],
				[DisplayName],
				[ValueTypes_EnumerationValue],
				[ValueTypeName],
				[Value_String],
				[Value_Binary]
			FROM deleted;
	OPEN _cursor_trgFormResultComponents_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgFormResultComponents_InsteadOfDelete INTO @id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM FormResultComponents WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'FormResultComponents', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'FormResultComponents', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL, NULL, NULL), 
						('FormResultId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @formResultId), NULL, NULL, NULL), 
						('Order', 'INT', 0, CONVERT(NVARCHAR(MAX), @order), NULL, NULL, NULL), 
						('DisplayName', 'NVARCHAR(200)', 0, @displayName, NULL, NULL, NULL), 
						('ValueTypes_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @valueTypes_EnumerationValue), NULL, NULL, NULL), 
						('ValueTypeName', 'VARCHAR(300)', 1, @valueTypeName, NULL, NULL, NULL), 
						('Value_String', 'NVARCHAR(MAX)', 1, @value_String, NULL, NULL, NULL), 
						('Value_Binary', 'VARBINARY(MAX)', 1, NULL, NULL, @value_Binary, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE FormResultComponents WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'FormResultComponents', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'FormResultComponents', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgFormResultComponents_InsteadOfDelete INTO @id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary;
		END
	CLOSE _cursor_trgFormResultComponents_InsteadOfDelete;
	DEALLOCATE _cursor_trgFormResultComponents_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgFormResultComponents_InsteadOfUpdate
ON FormResultComponents
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgFormResultComponents_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@formResultId UNIQUEIDENTIFIER,
		@order INT,
		@displayName NVARCHAR(200),
		@valueTypes_EnumerationValue TINYINT,
		@valueTypeName VARCHAR(300),
		@value_String NVARCHAR(MAX),
		@value_Binary VARBINARY(MAX),
		@id_ UNIQUEIDENTIFIER,
		@formResultId_ UNIQUEIDENTIFIER,
		@order_ INT,
		@displayName_ NVARCHAR(200),
		@valueTypes_EnumerationValue_ TINYINT,
		@valueTypeName_ VARCHAR(300),
		@value_String_ NVARCHAR(MAX),
		@value_Binary_ VARBINARY(MAX);
	DECLARE _cursor_trgFormResultComponents_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[FormResultId],
				D.[Order],
				D.[DisplayName],
				D.[ValueTypes_EnumerationValue],
				D.[ValueTypeName],
				D.[Value_String],
				D.[Value_Binary],
				I.[Id],
				I.[FormResultId],
				I.[Order],
				I.[DisplayName],
				I.[ValueTypes_EnumerationValue],
				I.[ValueTypeName],
				I.[Value_String],
				I.[Value_Binary]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgFormResultComponents_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgFormResultComponents_InsteadOfUpdate INTO @id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary, @id_, @formResultId_, @order_, @displayName_, @valueTypes_EnumerationValue_, @valueTypeName_, @value_String_, @value_Binary_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_), NULL, NULL), 
				('FormResultId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @formResultId), CONVERT(NVARCHAR(MAX), @formResultId_), NULL, NULL), 
				('Order', 'INT', 0, CONVERT(NVARCHAR(MAX), @order), CONVERT(NVARCHAR(MAX), @order_), NULL, NULL), 
				('DisplayName', 'NVARCHAR(200)', 0, @displayName, @displayName_, NULL, NULL), 
				('ValueTypes_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @valueTypes_EnumerationValue), CONVERT(NVARCHAR(MAX), @valueTypes_EnumerationValue_), NULL, NULL), 
				('ValueTypeName', 'VARCHAR(300)', 1, @valueTypeName, @valueTypeName_, NULL, NULL), 
				('Value_String', 'NVARCHAR(MAX)', 1, @value_String, @value_String_, NULL, NULL), 
				('Value_Binary', 'VARBINARY(MAX)', 1, NULL, NULL, @value_Binary, @value_Binary_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'FormResultComponents', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgFormResultComponents_InsteadOfUpdate INTO @id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary, @id_, @formResultId_, @order_, @displayName_, @valueTypes_EnumerationValue_, @valueTypeName_, @value_String_, @value_Binary_;
		END
	CLOSE _cursor_trgFormResultComponents_InsteadOfUpdate;
	DEALLOCATE _cursor_trgFormResultComponents_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgFormResultComponents_InsteadOfInsert
ON FormResultComponents
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgFormResultComponents_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@formResultId UNIQUEIDENTIFIER,
		@order INT,
		@displayName NVARCHAR(200),
		@valueTypes_EnumerationValue TINYINT,
		@valueTypeName VARCHAR(300),
		@value_String NVARCHAR(MAX),
		@value_Binary VARBINARY(MAX);
	DECLARE _cursor_trgFormResultComponents_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[FormResultId],
				[Order],
				[DisplayName],
				[ValueTypes_EnumerationValue],
				[ValueTypeName],
				[Value_String],
				[Value_Binary]
			FROM inserted;
	OPEN _cursor_trgFormResultComponents_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgFormResultComponents_InsteadOfInsert INTO @id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO FormResultComponents ([Id], [FormResultId], [Order], [DisplayName], [ValueTypes_EnumerationValue], [ValueTypeName], [Value_String], [Value_Binary])
					VALUES (@id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String], [NewValue_Binary]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
					('FormResultId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @formResultId), NULL), 
					('Order', 'INT', 0, CONVERT(NVARCHAR(MAX), @order), NULL), 
					('DisplayName', 'NVARCHAR(200)', 0, @displayName, NULL), 
					('ValueTypes_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @valueTypes_EnumerationValue), NULL), 
					('ValueTypeName', 'VARCHAR(300)', 1, @valueTypeName, NULL), 
					('Value_String', 'NVARCHAR(MAX)', 1, @value_String, NULL), 
					('Value_Binary', 'VARBINARY(MAX)', 1, NULL, @value_Binary);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'FormResultComponents', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgFormResultComponents_InsteadOfInsert INTO @id, @formResultId, @order, @displayName, @valueTypes_EnumerationValue, @valueTypeName, @value_String, @value_Binary;
		END
	CLOSE _cursor_trgFormResultComponents_InsteadOfInsert;
	DEALLOCATE _cursor_trgFormResultComponents_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgSentEmails_InsteadOfDelete
ON SentEmails
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSentEmails_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
		@toAddress VARCHAR(300),
		@fromDisplayName VARCHAR(100),
		@toDisplayName VARCHAR(100),
		@subject NVARCHAR(MAX),
		@message NVARCHAR(MAX),
		@encoding VARCHAR(10),
		@isMessageHtml BIT,
		@sentDate DATETIMEOFFSET,
		@fromAddress VARCHAR(300),
		@failureDate DATETIMEOFFSET,
		@resultFailureReasons_EnumerationValue INT,
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgSentEmails_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[RecordDate],
				[ToAddress],
				[FromDisplayName],
				[ToDisplayName],
				[Subject],
				[Message],
				[Encoding],
				[IsMessageHtml],
				[SentDate],
				[FromAddress],
				[FailureDate],
				[FailureReason_EnumerationValue],
				[AdditionalData]
			FROM deleted;
	OPEN _cursor_trgSentEmails_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfDelete INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM SentEmails WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate), NULL), 
						('ToAddress', 'VARCHAR(300)', 0, @toAddress, NULL), 
						('FromDisplayName', 'VARCHAR(100)', 1, @fromDisplayName, NULL), 
						('ToDisplayName', 'VARCHAR(100)', 1, @toDisplayName, NULL), 
						('Subject', 'NVARCHAR(MAX)', 1, @subject, NULL), 
						('Message', 'NVARCHAR(MAX)', 1, @message, NULL), 
						('Encoding', 'VARCHAR(10)', 1, @encoding, NULL), 
						('IsMessageHtml', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isMessageHtml), NULL), 
						('SentDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @sentDate), NULL), 
						('FromAddress', 'VARCHAR(300)', 1, @fromAddress, NULL), 
						('FailureDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @failureDate), NULL), 
						('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue), NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE SentEmails WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfDelete INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData;
		END
	CLOSE _cursor_trgSentEmails_InsteadOfDelete;
	DEALLOCATE _cursor_trgSentEmails_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgSentEmails_InsteadOfUpdate
ON SentEmails
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSentEmails_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
		@toAddress VARCHAR(300),
		@fromDisplayName VARCHAR(100),
		@toDisplayName VARCHAR(100),
		@subject NVARCHAR(MAX),
		@message NVARCHAR(MAX),
		@encoding VARCHAR(10),
		@isMessageHtml BIT,
		@sentDate DATETIMEOFFSET,
		@fromAddress VARCHAR(300),
		@failureDate DATETIMEOFFSET,
		@resultFailureReasons_EnumerationValue INT,
		@additionalData NVARCHAR(MAX),
		@id_ UNIQUEIDENTIFIER,
		@recordDate_ DATETIMEOFFSET,
		@toAddress_ VARCHAR(300),
		@fromDisplayName_ VARCHAR(100),
		@toDisplayName_ VARCHAR(100),
		@subject_ NVARCHAR(MAX),
		@message_ NVARCHAR(MAX),
		@encoding_ VARCHAR(10),
		@isMessageHtml_ BIT,
		@sentDate_ DATETIMEOFFSET,
		@fromAddress_ VARCHAR(300),
		@failureDate_ DATETIMEOFFSET,
		@resultFailureReasons_EnumerationValue_ INT,
		@additionalData_ NVARCHAR(MAX);
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND D.[RecordDate] = I.[RecordDate] AND D.[ToAddress] = I.[ToAddress] AND ((D.[FromDisplayName] IS NULL AND I.[FromDisplayName] IS NULL) OR D.[FromDisplayName] = I.[FromDisplayName]) AND ((D.[ToDisplayName] IS NULL AND I.[ToDisplayName] IS NULL) OR D.[ToDisplayName] = I.[ToDisplayName]) AND ((D.[Subject] IS NULL AND I.[Subject] IS NULL) OR D.[Subject] = I.[Subject]) AND ((D.[Message] IS NULL AND I.[Message] IS NULL) OR D.[Message] = I.[Message]) AND ((D.[Encoding] IS NULL AND I.[Encoding] IS NULL) OR D.[Encoding] = I.[Encoding]) AND D.[IsMessageHtml] = I.[IsMessageHtml]
		WHERE
			((D.[SentDate] IS NOT NULL OR I.[SentDate] IS NOT NULL) AND D.[SentDate] <> I.[SentDate]) OR ((D.[FromAddress] IS NOT NULL OR I.[FromAddress] IS NOT NULL) AND D.[FromAddress] <> I.[FromAddress]) OR ((D.[FailureDate] IS NOT NULL OR I.[FailureDate] IS NOT NULL) AND D.[FailureDate] <> I.[FailureDate]) OR ((D.[FailureReason_EnumerationValue] IS NOT NULL OR I.[FailureReason_EnumerationValue] IS NOT NULL) AND D.[FailureReason_EnumerationValue] <> I.[FailureReason_EnumerationValue]) OR ((D.[AdditionalData] IS NOT NULL OR I.[AdditionalData] IS NOT NULL) AND D.[AdditionalData] <> I.[AdditionalData]);
	DECLARE _cursor_trgSentEmails_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[RecordDate],
				D.[ToAddress],
				D.[FromDisplayName],
				D.[ToDisplayName],
				D.[Subject],
				D.[Message],
				D.[Encoding],
				D.[IsMessageHtml],
				D.[SentDate],
				D.[FromAddress],
				D.[FailureDate],
				D.[FailureReason_EnumerationValue],
				D.[AdditionalData],
				I.[Id],
				I.[RecordDate],
				I.[ToAddress],
				I.[FromDisplayName],
				I.[ToDisplayName],
				I.[Subject],
				I.[Message],
				I.[Encoding],
				I.[IsMessageHtml],
				I.[SentDate],
				I.[FromAddress],
				I.[FailureDate],
				I.[FailureReason_EnumerationValue],
				I.[AdditionalData]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgSentEmails_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfUpdate INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData, @id_, @recordDate_, @toAddress_, @fromDisplayName_, @toDisplayName_, @subject_, @message_, @encoding_, @isMessageHtml_, @sentDate_, @fromAddress_, @failureDate_, @resultFailureReasons_EnumerationValue_, @additionalData_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values1 [dbo].[typeIUDLogValue];
					INSERT INTO @values1 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate), CONVERT(NVARCHAR(MAX), @recordDate_)), 
						('ToAddress', 'VARCHAR(300)', 0, @toAddress, @toAddress_), 
						('FromDisplayName', 'VARCHAR(100)', 1, @fromDisplayName, @fromDisplayName_), 
						('ToDisplayName', 'VARCHAR(100)', 1, @toDisplayName, @toDisplayName_), 
						('Subject', 'NVARCHAR(MAX)', 1, @subject, @subject_), 
						('Message', 'NVARCHAR(MAX)', 1, @message, @message_), 
						('Encoding', 'VARCHAR(10)', 1, @encoding, @encoding_), 
						('IsMessageHtml', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isMessageHtml), CONVERT(NVARCHAR(MAX), @isMessageHtml_)), 
						('SentDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @sentDate), CONVERT(NVARCHAR(MAX), @sentDate_)), 
						('FromAddress', 'VARCHAR(300)', 1, @fromAddress, @fromAddress_), 
						('FailureDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @failureDate), CONVERT(NVARCHAR(MAX), @failureDate_)), 
						('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue_)), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_);
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values1, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfUpdate INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData, @id_, @recordDate_, @toAddress_, @fromDisplayName_, @toDisplayName_, @subject_, @message_, @encoding_, @isMessageHtml_, @sentDate_, @fromAddress_, @failureDate_, @resultFailureReasons_EnumerationValue_, @additionalData_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfUpdate INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData, @id_, @recordDate_, @toAddress_, @fromDisplayName_, @toDisplayName_, @subject_, @message_, @encoding_, @isMessageHtml_, @sentDate_, @fromAddress_, @failureDate_, @resultFailureReasons_EnumerationValue_, @additionalData_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM SentEmails WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							DECLARE @values2 [dbo].[typeIUDLogValue];
							INSERT INTO @values2 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
								('SentDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @sentDate), CONVERT(NVARCHAR(MAX), @sentDate_)), 
								('FromAddress', 'VARCHAR(300)', 1, @fromAddress, @fromAddress_), 
								('FailureDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @failureDate), CONVERT(NVARCHAR(MAX), @failureDate_)), 
								('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue_)), 
								('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_);
							BEGIN TRY
								EXEC [dbo].[Insert_IUDLogs] 2, 0, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values2, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							IF
								(@fromAddress_ IS NOT NULL AND @fromAddress IS NOT NULL AND @fromAddress_ <> @fromAddress) OR
								(@sentDate_ IS NOT NULL AND @sentDate IS NOT NULL AND @sentDate_ <> @sentDate) OR
								(@failureDate_ IS NOT NULL AND @failureDate IS NOT NULL AND @failureDate_ <> @failureDate) OR
								(@resultFailureReasons_EnumerationValue_ IS NOT NULL AND @resultFailureReasons_EnumerationValue IS NOT NULL AND @resultFailureReasons_EnumerationValue_ <> @resultFailureReasons_EnumerationValue) OR
								(@additionalData_ IS NOT NULL AND @additionalData IS NOT NULL AND @additionalData_ <> @additionalData)
								EXEC [dbo].[RaiseSpecificError] N'The values to be updated were not empty.';
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE SentEmails
									SET
										[SentDate] = @sentDate_,
										[FromAddress] = @fromAddress_,
										[FailureDate] = @failureDate_,
										[FailureReason_EnumerationValue] = @resultFailureReasons_EnumerationValue_,
										[AdditionalData] = @additionalData_
									WHERE [Id] = @id;
								IF @@TRANCOUNT > 0
									COMMIT TRANSACTION;
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								DECLARE @values3 [dbo].[typeIUDLogValue];
								INSERT INTO @values3 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
									('SentDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @sentDate), CONVERT(NVARCHAR(MAX), @sentDate_)), 
									('FromAddress', 'VARCHAR(300)', 1, @fromAddress, @fromAddress_), 
									('FailureDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @failureDate), CONVERT(NVARCHAR(MAX), @failureDate_)), 
									('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue_)), 
									('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 2, 0, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values3, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfUpdate INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData, @id_, @recordDate_, @toAddress_, @fromDisplayName_, @toDisplayName_, @subject_, @message_, @encoding_, @isMessageHtml_, @sentDate_, @fromAddress_, @failureDate_, @resultFailureReasons_EnumerationValue_, @additionalData_;
				END
		END
	CLOSE _cursor_trgSentEmails_InsteadOfUpdate;
	DEALLOCATE _cursor_trgSentEmails_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgSentEmails_InsteadOfInsert
ON SentEmails
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSentEmails_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@recordDate DATETIMEOFFSET,
		@toAddress VARCHAR(300),
		@fromDisplayName VARCHAR(100),
		@toDisplayName VARCHAR(100),
		@subject NVARCHAR(MAX),
		@message NVARCHAR(MAX),
		@encoding VARCHAR(10),
		@isMessageHtml BIT,
		@sentDate DATETIMEOFFSET,
		@fromAddress VARCHAR(300),
		@failureDate DATETIMEOFFSET,
		@resultFailureReasons_EnumerationValue INT,
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgSentEmails_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[RecordDate],
				[ToAddress],
				[FromDisplayName],
				[ToDisplayName],
				[Subject],
				[Message],
				[Encoding],
				[IsMessageHtml],
				[SentDate],
				[FromAddress],
				[FailureDate],
				[FailureReason_EnumerationValue],
				[AdditionalData]
			FROM inserted;
	OPEN _cursor_trgSentEmails_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfInsert INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @sentDate IS NOT NULL OR @fromAddress IS NOT NULL OR @failureDate IS NOT NULL OR @resultFailureReasons_EnumerationValue IS NOT NULL OR @additionalData IS NOT NULL
				EXEC [dbo].[RaiseSpecificError] N'Some values were not empty on instert unexpectedly.';
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO SentEmails ([Id], [RecordDate], [ToAddress], [FromDisplayName], [ToDisplayName], [Subject], [Message], [Encoding], [IsMessageHtml], [SentDate], [FromAddress], [FailureDate], [FailureReason_EnumerationValue], [AdditionalData])
					VALUES (@id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('RecordDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @recordDate)), 
					('ToAddress', 'VARCHAR(300)', 0, @toAddress), 
					('FromDisplayName', 'VARCHAR(100)', 1, @fromDisplayName), 
					('ToDisplayName', 'VARCHAR(100)', 1, @toDisplayName), 
					('Subject', 'NVARCHAR(MAX)', 1, @subject), 
					('Message', 'NVARCHAR(MAX)', 1, @message), 
					('Encoding', 'VARCHAR(10)', 1, @encoding), 
					('IsMessageHtml', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isMessageHtml)), 
					('SentDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @sentDate)), 
					('FromAddress', 'VARCHAR(300)', 1, @fromAddress), 
					('FailureDate', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @failureDate)), 
					('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReasons_EnumerationValue)), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'SentEmails', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgSentEmails_InsteadOfInsert INTO @id, @recordDate, @toAddress, @fromDisplayName, @toDisplayName, @subject, @message, @encoding, @isMessageHtml, @sentDate, @fromAddress, @failureDate, @resultFailureReasons_EnumerationValue, @additionalData;
		END
	CLOSE _cursor_trgSentEmails_InsteadOfInsert;
	DEALLOCATE _cursor_trgSentEmails_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgSentEmailAttachments_InsteadOfDelete
ON SentEmailAttachments
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSentEmailAttachments_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sentEmailId UNIQUEIDENTIFIER,
		@filePath NVARCHAR(MAX),
		@displayFileName NVARCHAR(200),
		@contentType NVARCHAR(100);
	DECLARE _cursor_trgSentEmailAttachments_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[SentEmailId],
				[FilePath],
				[DisplayFileName],
				[ContentType]
			FROM deleted;
	OPEN _cursor_trgSentEmailAttachments_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgSentEmailAttachments_InsteadOfDelete INTO @id, @sentEmailId, @filePath, @displayFileName, @contentType;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM SentEmailAttachments WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'SentEmailAttachments', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'SentEmailAttachments', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('SentEmailId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sentEmailId), NULL), 
						('FilePath', 'NVARCHAR(MAX)', 0, @filePath, NULL), 
						('DisplayFileName', 'NVARCHAR(200)', 0, @displayFileName, NULL), 
						('ContentType', 'NVARCHAR(100)', 1, @contentType, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE SentEmailAttachments WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'SentEmailAttachments', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'SentEmailAttachments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgSentEmailAttachments_InsteadOfDelete INTO @id, @sentEmailId, @filePath, @displayFileName, @contentType;
		END
	CLOSE _cursor_trgSentEmailAttachments_InsteadOfDelete;
	DEALLOCATE _cursor_trgSentEmailAttachments_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgSentEmailAttachments_InsteadOfUpdate
ON SentEmailAttachments
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSentEmailAttachments_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sentEmailId UNIQUEIDENTIFIER,
		@filePath NVARCHAR(MAX),
		@displayFileName NVARCHAR(200),
		@contentType NVARCHAR(100),
		@id_ UNIQUEIDENTIFIER,
		@sentEmailId_ UNIQUEIDENTIFIER,
		@filePath_ NVARCHAR(MAX),
		@displayFileName_ NVARCHAR(200),
		@contentType_ NVARCHAR(100);
	DECLARE _cursor_trgSentEmailAttachments_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[SentEmailId],
				D.[FilePath],
				D.[DisplayFileName],
				D.[ContentType],
				I.[Id],
				I.[SentEmailId],
				I.[FilePath],
				I.[DisplayFileName],
				I.[ContentType]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgSentEmailAttachments_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgSentEmailAttachments_InsteadOfUpdate INTO @id, @sentEmailId, @filePath, @displayFileName, @contentType, @id_, @sentEmailId_, @filePath_, @displayFileName_, @contentType_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('SentEmailId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sentEmailId), CONVERT(NVARCHAR(MAX), @sentEmailId_)), 
				('FilePath', 'NVARCHAR(MAX)', 0, @filePath, @filePath_), 
				('DisplayFileName', 'NVARCHAR(200)', 0, @displayFileName, @displayFileName_), 
				('ContentType', 'NVARCHAR(100)', 1, @contentType, @contentType_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'SentEmailAttachments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgSentEmailAttachments_InsteadOfUpdate INTO @id, @sentEmailId, @filePath, @displayFileName, @contentType, @id_, @sentEmailId_, @filePath_, @displayFileName_, @contentType_;
		END
	CLOSE _cursor_trgSentEmailAttachments_InsteadOfUpdate;
	DEALLOCATE _cursor_trgSentEmailAttachments_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgSentEmailAttachments_InsteadOfInsert
ON SentEmailAttachments
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgSentEmailAttachments_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@sentEmailId UNIQUEIDENTIFIER,
		@filePath NVARCHAR(MAX),
		@displayFileName NVARCHAR(200),
		@contentType NVARCHAR(100);
	DECLARE _cursor_trgSentEmailAttachments_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[SentEmailId],
				[FilePath],
				[DisplayFileName],
				[ContentType]
			FROM inserted;
	OPEN _cursor_trgSentEmailAttachments_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgSentEmailAttachments_InsteadOfInsert INTO @id, @sentEmailId, @filePath, @displayFileName, @contentType;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO SentEmailAttachments ([Id], [SentEmailId], [FilePath], [DisplayFileName], [ContentType])
					VALUES (@id, @sentEmailId, @filePath, @displayFileName, @contentType);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('SentEmailId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @sentEmailId)), 
					('FilePath', 'NVARCHAR(MAX)', 0, @filePath), 
					('DisplayFileName', 'NVARCHAR(200)', 0, @displayFileName), 
					('ContentType', 'NVARCHAR(100)', 1, @contentType);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'SentEmailAttachments', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgSentEmailAttachments_InsteadOfInsert INTO @id, @sentEmailId, @filePath, @displayFileName, @contentType;
		END
	CLOSE _cursor_trgSentEmailAttachments_InsteadOfInsert;
	DEALLOCATE _cursor_trgSentEmailAttachments_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgErrorLogs_InsteadOfDelete
ON ErrorLogs
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgErrorLogs_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@errorType NVARCHAR(200),
		@errorMessage NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@occuredAt DATETIMEOFFSET;
	DECLARE _cursor_trgErrorLogs_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[ErrorType],
				[ErrorMessage],
				[AdditionalData],
				[OccuredAt]
			FROM deleted;
	OPEN _cursor_trgErrorLogs_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgErrorLogs_InsteadOfDelete INTO @id, @pageRequestId, @errorType, @errorMessage, @additionalData, @occuredAt;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM ErrorLogs WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ErrorLogs', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ErrorLogs', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('PageRequestId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @pageRequestId), NULL), 
						('ErrorType', 'NVARCHAR(200)', 0, @errorType, NULL), 
						('ErrorMessage', 'NVARCHAR(MAX)', 1, @errorMessage, NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL), 
						('OccuredAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @occuredAt), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE ErrorLogs WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'ErrorLogs', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ErrorLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgErrorLogs_InsteadOfDelete INTO @id, @pageRequestId, @errorType, @errorMessage, @additionalData, @occuredAt;
		END
	CLOSE _cursor_trgErrorLogs_InsteadOfDelete;
	DEALLOCATE _cursor_trgErrorLogs_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgErrorLogs_InsteadOfUpdate
ON ErrorLogs
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgErrorLogs_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@errorType NVARCHAR(200),
		@errorMessage NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@occuredAt DATETIMEOFFSET,
		@id_ UNIQUEIDENTIFIER,
		@pageRequestId_ UNIQUEIDENTIFIER,
		@errorType_ NVARCHAR(200),
		@errorMessage_ NVARCHAR(MAX),
		@additionalData_ NVARCHAR(MAX),
		@occuredAt_ DATETIMEOFFSET;
	DECLARE _cursor_trgErrorLogs_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[PageRequestId],
				D.[ErrorType],
				D.[ErrorMessage],
				D.[AdditionalData],
				D.[OccuredAt],
				I.[Id],
				I.[PageRequestId],
				I.[ErrorType],
				I.[ErrorMessage],
				I.[AdditionalData],
				I.[OccuredAt]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgErrorLogs_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgErrorLogs_InsteadOfUpdate INTO @id, @pageRequestId, @errorType, @errorMessage, @additionalData, @occuredAt, @id_, @pageRequestId_, @errorType_, @errorMessage_, @additionalData_, @occuredAt_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('PageRequestId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @pageRequestId), CONVERT(NVARCHAR(MAX), @pageRequestId_)), 
				('ErrorType', 'NVARCHAR(200)', 0, @errorType, @errorType_), 
				('ErrorMessage', 'NVARCHAR(MAX)', 1, @errorMessage, @errorMessage_), 
				('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_), 
				('OccuredAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @occuredAt), CONVERT(NVARCHAR(MAX), @occuredAt_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'ErrorLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgErrorLogs_InsteadOfUpdate INTO @id, @pageRequestId, @errorType, @errorMessage, @additionalData, @occuredAt, @id_, @pageRequestId_, @errorType_, @errorMessage_, @additionalData_, @occuredAt_;
		END
	CLOSE _cursor_trgErrorLogs_InsteadOfUpdate;
	DEALLOCATE _cursor_trgErrorLogs_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgErrorLogs_InsteadOfInsert
ON ErrorLogs
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgErrorLogs_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@errorType NVARCHAR(200),
		@errorMessage NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@occuredAt DATETIMEOFFSET;
	DECLARE _cursor_trgErrorLogs_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[ErrorType],
				[ErrorMessage],
				[AdditionalData],
				[OccuredAt]
			FROM inserted;
	OPEN _cursor_trgErrorLogs_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgErrorLogs_InsteadOfInsert INTO @id, @pageRequestId, @errorType, @errorMessage, @additionalData, @occuredAt;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO ErrorLogs ([Id], [PageRequestId], [ErrorType], [ErrorMessage], [AdditionalData], [OccuredAt])
					VALUES (@id, @pageRequestId, @errorType, @errorMessage, @additionalData, @occuredAt);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 1, 'ErrorLogs', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
					IF @@TRANCOUNT > 0
						COMMIT TRANSACTION;
				END TRY
				BEGIN CATCH
					IF @@TRANCOUNT > 0
						ROLLBACK TRANSACTION;
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('PageRequestId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @pageRequestId)), 
					('ErrorType', 'NVARCHAR(200)', 0, @errorType), 
					('ErrorMessage', 'NVARCHAR(MAX)', 1, @errorMessage), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData), 
					('OccuredAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @occuredAt));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'ErrorLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgErrorLogs_InsteadOfInsert INTO @id, @pageRequestId, @errorType, @errorMessage, @additionalData, @occuredAt;
		END
	CLOSE _cursor_trgErrorLogs_InsteadOfInsert;
	DEALLOCATE _cursor_trgErrorLogs_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgUserGroups_InsteadOfDelete
ON UserGroups
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserGroups_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@supUserGroupId UNIQUEIDENTIFIER,
		@name NVARCHAR(100),
		@grants VARBINARY(MAX),
		@isEnabled BIT;
	DECLARE _cursor_trgUserGroups_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[SupUserGroupId],
				[Name],
				[Grants],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgUserGroups_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfDelete INTO @id, @supUserGroupId, @name, @grants, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM UserGroups WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL, NULL, NULL), 
						('SupUserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @supUserGroupId), NULL, NULL, NULL), 
						('Name', 'NVARCHAR(100)', 0, @name, NULL, NULL, NULL), 
						('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL, NULL, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE UserGroups WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_UserGroups] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfDelete INTO @id, @supUserGroupId, @name, @grants, @isEnabled;
		END
	CLOSE _cursor_trgUserGroups_InsteadOfDelete;
	DEALLOCATE _cursor_trgUserGroups_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserGroups_InsteadOfUpdate
ON UserGroups
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserGroups_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@supUserGroupId UNIQUEIDENTIFIER,
		@name NVARCHAR(100),
		@grants VARBINARY(MAX),
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@supUserGroupId_ UNIQUEIDENTIFIER,
		@name_ NVARCHAR(100),
		@grants_ VARBINARY(MAX),
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id]
		WHERE
			(D.[IsEnabled] <> I.[IsEnabled] AND ((D.[SupUserGroupId] IS NULL AND I.[SupUserGroupId] IS NULL) OR D.[SupUserGroupId] = I.[SupUserGroupId]) AND D.[Name] = I.[Name] AND D.[Grants] = I.[Grants]) OR
			(D.[IsEnabled] = 1 AND I.[IsEnabled] = 1 AND (((D.[SupUserGroupId] IS NOT NULL OR I.[SupUserGroupId] IS NOT NULL) AND D.[SupUserGroupId] <> I.[SupUserGroupId]) OR D.[Name] <> I.[Name] OR D.[Grants] <> I.[Grants]));
	DECLARE _cursor_trgUserGroups_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[SupUserGroupId],
				D.[Name],
				D.[Grants],
				D.[IsEnabled],
				I.[Id],
				I.[SupUserGroupId],
				I.[Name],
				I.[Grants],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgUserGroups_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfUpdate INTO @id, @supUserGroupId, @name, @grants, @isEnabled, @id_, @supUserGroupId_, @name_, @grants_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values1 [dbo].[typeIUDLogValue];
					INSERT INTO @values1 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_), NULL, NULL), 
						('SupUserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @supUserGroupId), CONVERT(NVARCHAR(MAX), @supUserGroupId_), NULL, NULL), 
						('Name', 'NVARCHAR(100)', 0, @name, @name_, NULL, NULL), 
						('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, @grants_), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_), NULL, NULL);
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values1, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfUpdate INTO @id, @supUserGroupId, @name, @grants, @isEnabled, @id_, @supUserGroupId_, @name_, @grants_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfUpdate INTO @id, @supUserGroupId, @name, @grants, @isEnabled, @id_, @supUserGroupId_, @name_, @grants_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM UserGroups WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = @isEnabled_
									BEGIN
										DECLARE @values2 [dbo].[typeIUDLogValue];
										INSERT INTO @values2 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
											('SupUserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @supUserGroupId), CONVERT(NVARCHAR(MAX), @supUserGroupId_), NULL, NULL), 
											('Name', 'NVARCHAR(100)', 0, @name, @name_, NULL, NULL), 
											('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, @grants_);
										EXEC [dbo].[Insert_IUDLogs] 2, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values2, @logId OUTPUT;
									END
								ELSE IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE UserGroups
									SET
										[SupUserGroupId] = @supUserGroupId_,
										[Name] = @name_,
										[Grants] = @grants_,
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = @isEnabled_
										BEGIN
											DECLARE @values3 [dbo].[typeIUDLogValue];
											INSERT INTO @values3 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
												('SupUserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @supUserGroupId), CONVERT(NVARCHAR(MAX), @supUserGroupId_), NULL, NULL), 
												('Name', 'NVARCHAR(100)', 0, @name, @name_, NULL, NULL), 
												('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, @grants_);
											EXEC [dbo].[Insert_IUDLogs] 2, 1, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values3, @logId OUTPUT;
										END
									ELSE IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = @isEnabled_
										BEGIN
											DECLARE @values4 [dbo].[typeIUDLogValue];
											INSERT INTO @values4 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
												('SupUserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @supUserGroupId), CONVERT(NVARCHAR(MAX), @supUserGroupId_), NULL, NULL), 
												('Name', 'NVARCHAR(100)', 0, @name, @name_, NULL, NULL), 
												('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, @grants_);
											EXEC [dbo].[Insert_IUDLogs] 2, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values4, @logId OUTPUT;
										END
									ELSE IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfUpdate INTO @id, @supUserGroupId, @name, @grants, @isEnabled, @id_, @supUserGroupId_, @name_, @grants_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgUserGroups_InsteadOfUpdate;
	DEALLOCATE _cursor_trgUserGroups_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserGroups_InsteadOfInsert
ON UserGroups
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserGroups_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@supUserGroupId UNIQUEIDENTIFIER,
		@name NVARCHAR(100),
		@grants VARBINARY(MAX),
		@isEnabled BIT;
	DECLARE _cursor_trgUserGroups_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[SupUserGroupId],
				[Name],
				[Grants],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgUserGroups_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfInsert INTO @id, @supUserGroupId, @name, @grants, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						INSERT INTO UserGroups ([Id], [SupUserGroupId], [Name], [Grants], [IsEnabled])
							VALUES (@id, @supUserGroupId, @name, @grants, @isEnabled);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 1, 1, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String], [NewValue_Binary]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
					('SupUserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @supUserGroupId), NULL), 
					('Name', 'NVARCHAR(100)', 0, @name, NULL), 
					('Grants', 'VARBINARY(MAX)', 0, NULL, @grants), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'UserGroups', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgUserGroups_InsteadOfInsert INTO @id, @supUserGroupId, @name, @grants, @isEnabled;
		END
	CLOSE _cursor_trgUserGroups_InsteadOfInsert;
	DEALLOCATE _cursor_trgUserGroups_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgUsers_InsteadOfDelete
ON Users
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUsers_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userGroupId UNIQUEIDENTIFIER,
		@emailAddress VARCHAR(300),
		@username VARCHAR(50),
		@password NVARCHAR(MAX),
		@firstName NVARCHAR(200),
		@lastName NVARCHAR(200),
		@is2FAActive BIT,
		@grants VARBINARY(MAX),
		@isEnabled BIT;
	DECLARE _cursor_trgUsers_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[UserGroupId],
				[EmailAddress],
				[Username],
				[Password],
				[FirstName],
				[LastName],
				[Is2FAActive],
				[Grants],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgUsers_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgUsers_InsteadOfDelete INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Users WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL, NULL, NULL), 
						('UserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userGroupId), NULL, NULL, NULL), 
						('EmailAddress', 'VARCHAR(300)', 0, @emailAddress, NULL, NULL, NULL), 
						('Username', 'VARCHAR(50)', 0, @username, NULL, NULL, NULL), 
						('Password', 'NVARCHAR(MAX)', 0, @password, NULL, NULL, NULL), 
						('FirstName', 'NVARCHAR(200)', 0, @firstName, NULL, NULL, NULL), 
						('LastName', 'NVARCHAR(200)', 0, @lastName, NULL, NULL, NULL), 
						('Is2FAActive', 'BIT', 0, CONVERT(NVARCHAR(MAX), @is2FAActive), NULL, NULL, NULL), 
						('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL, NULL, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE Users WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_Users] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgUsers_InsteadOfDelete INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled;
		END
	CLOSE _cursor_trgUsers_InsteadOfDelete;
	DEALLOCATE _cursor_trgUsers_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUsers_InsteadOfUpdate
ON Users
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUsers_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT = 0,
		@psw NVARCHAR(MAX),
		@pswX NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userGroupId UNIQUEIDENTIFIER,
		@emailAddress VARCHAR(300),
		@username VARCHAR(50),
		@password NVARCHAR(MAX),
		@firstName NVARCHAR(200),
		@lastName NVARCHAR(200),
		@is2FAActive BIT,
		@grants VARBINARY(MAX),
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@userGroupId_ UNIQUEIDENTIFIER,
		@emailAddress_ VARCHAR(300),
		@username_ VARCHAR(50),
		@password_ NVARCHAR(MAX),
		@firstName_ NVARCHAR(200),
		@lastName_ NVARCHAR(200),
		@is2FAActive_ BIT,
		@grants_ VARBINARY(MAX),
		@isEnabled_ BIT;	
	DECLARE _cursor_trgUsers_InsteadOfUpdate_0 CURSOR FOR
		SELECT
				D.[Id],
				D.[UserGroupId],
				D.[EmailAddress],
				D.[Username],
				D.[Password],
				D.[FirstName],
				D.[LastName],
				D.[Is2FAActive],
				D.[Grants],
				D.[IsEnabled],
				I.[Id],
				I.[UserGroupId],
				LOWER(LTRIM(RTRIM(I.[EmailAddress])) COLLATE SQL_Latin1_General_CP1253_CI_AI),
				LOWER(LTRIM(RTRIM(I.[Username])) COLLATE SQL_Latin1_General_CP1253_CI_AI),
				I.[Password],
				[dbo].[CapitalizeString](LTRIM(RTRIM(I.[FirstName]))),
				[dbo].[CapitalizeString](LTRIM(RTRIM(I.[LastName]))),
				I.[Is2FAActive],
				I.[Grants],
				I.[IsEnabled]
			FROM inserted I
			INNER JOIN deleted D
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgUsers_InsteadOfUpdate_0;
	FETCH NEXT FROM _cursor_trgUsers_InsteadOfUpdate_0 INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled, @id_, @userGroupId_, @emailAddress_, @username_, @password_, @firstName_, @lastName_, @is2FAActive_, @grants_, @isEnabled_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC [dbo].[DecryptUserPassword] @username, @password, @psw OUTPUT;
			IF
				(@isEnabled <> @isEnabled_ AND ((@userGroupId IS NULL AND @userGroupId_ IS NULL) OR @userGroupId = @userGroupId_) AND @emailAddress = @emailAddress_ AND @username = @username_ AND @psw = @password_ AND @firstName = @firstName_ AND @lastName = @lastName_ AND @is2FAActive = @is2FAActive_ AND @grants = @grants_) OR
				(@isEnabled = 1 AND @isEnabled_ = 1 AND (((@userGroupId IS NOT NULL OR @userGroupId_ IS NOT NULL) AND @userGroupId <> @userGroupId_) OR @emailAddress <> @emailAddress_ OR @username <> @username_ OR @password <> @psw OR @firstName <> @firstName_ OR @lastName <> @lastName_ OR @is2FAActive <> @is2FAActive_ OR @grants <> @grants_))
				SET @count = @count + 1;
			FETCH NEXT FROM _cursor_trgUsers_InsteadOfUpdate_0 INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled, @id_, @userGroupId_, @emailAddress_, @username_, @password_, @firstName_, @lastName_, @is2FAActive_, @grants_, @isEnabled_;
		END
	CLOSE _cursor_trgUsers_InsteadOfUpdate_0;
	DEALLOCATE _cursor_trgUsers_InsteadOfUpdate_0;
	DECLARE _cursor_trgUsers_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[UserGroupId],
				D.[EmailAddress],
				D.[Username],
				D.[Password],
				D.[FirstName],
				D.[LastName],
				D.[Is2FAActive],
				D.[Grants],
				D.[IsEnabled],
				I.[Id],
				I.[UserGroupId],
				LOWER(LTRIM(RTRIM(I.[EmailAddress])) COLLATE SQL_Latin1_General_CP1253_CI_AI),
				LOWER(LTRIM(RTRIM(I.[Username])) COLLATE SQL_Latin1_General_CP1253_CI_AI),
				I.[Password],
				[dbo].[CapitalizeString](LTRIM(RTRIM(I.[FirstName]))),
				[dbo].[CapitalizeString](LTRIM(RTRIM(I.[LastName]))),
				I.[Is2FAActive],
				I.[Grants],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgUsers_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgUsers_InsteadOfUpdate INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled, @id_, @userGroupId_, @emailAddress_, @username_, @password_, @firstName_, @lastName_, @is2FAActive_, @grants_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF @password = @password_
						SET @pswX = @password;
					ELSE
						EXEC [dbo].[EncryptUserPassword] @username_, @password_, @pswX OUTPUT;
					DECLARE @values1 [dbo].[typeIUDLogValue];
					INSERT INTO @values1 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_), NULL, NULL), 
						('UserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userGroupId), CONVERT(NVARCHAR(MAX), @userGroupId_), NULL, NULL), 
						('EmailAddress', 'VARCHAR(300)', 0, @emailAddress, @emailAddress_, NULL, NULL), 
						('Username', 'VARCHAR(50)', 0, @username, @username_, NULL, NULL), 
						('Password', 'NVARCHAR(MAX)', 0, @password, @pswX, NULL, NULL), 
						('FirstName', 'NVARCHAR(200)', 0, @firstName, @firstName_, NULL, NULL), 
						('LastName', 'NVARCHAR(200)', 0, @lastName, @lastName_, NULL, NULL), 
						('Is2FAActive', 'BIT', 0, CONVERT(NVARCHAR(MAX), @is2FAActive), CONVERT(NVARCHAR(MAX), @is2FAActive_), NULL, NULL), 
						('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, @grants_), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_), NULL, NULL);
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values1, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgUsers_InsteadOfUpdate INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled, @id_, @userGroupId_, @emailAddress_, @username_, @password_, @firstName_, @lastName_, @is2FAActive_, @grants_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgUsers_InsteadOfUpdate INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled, @id_, @userGroupId_, @emailAddress_, @username_, @password_, @firstName_, @lastName_, @is2FAActive_, @grants_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF @password = @password_
						SET @pswX = @password;
					ELSE
						EXEC [dbo].[EncryptUserPassword] @username_, @password_, @pswX OUTPUT;
					IF NOT EXISTS(SELECT 1 FROM Users WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = @isEnabled_
									BEGIN
										DECLARE @values2 [dbo].[typeIUDLogValue];
										INSERT INTO @values2 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
											('UserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userGroupId), CONVERT(NVARCHAR(MAX), @userGroupId_), NULL, NULL), 
											('EmailAddress', 'VARCHAR(300)', 0, @emailAddress, @emailAddress_, NULL, NULL), 
											('Username', 'VARCHAR(50)', 0, @username, @username_, NULL, NULL), 
											('Password', 'NVARCHAR(MAX)', 0, @password, @pswX, NULL, NULL), 
											('FirstName', 'NVARCHAR(200)', 0, @firstName, @firstName_, NULL, NULL), 
											('LastName', 'NVARCHAR(200)', 0, @lastName, @lastName_, NULL, NULL), 
											('Is2FAActive', 'BIT', 0, CONVERT(NVARCHAR(MAX), @is2FAActive), CONVERT(NVARCHAR(MAX), @is2FAActive_), NULL, NULL), 
											('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, @grants_);
										EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values2, @logId OUTPUT;
									END
								ELSE IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							DECLARE @isEmailVerified BIT;
							DECLARE @emailVerifiedUserId UNIQUEIDENTIFIER;
							IF @emailAddress <> @emailAddress_
								BEGIN
									EXEC [dbo].[IsVerifiedOrPendingEmailAddress] @emailAddress_, @isEmailVerified OUTPUT, @emailVerifiedUserId OUTPUT;
									IF @emailVerifiedUserId IS NOT NULL
										EXEC [dbo].[RaiseSpecificError] N'New value of ''EmailAddress'' was in use.';
								END
							BEGIN TRANSACTION;
							BEGIN TRY
								DECLARE @additionalData NVARCHAR(MAX) = NULL;
								IF @is2FAActive_ = 1
									BEGIN
										IF @isEnabled = 0
											BEGIN
												SET @additionalData = N'Altough new value of ''Is2FAActive'' was ''1'', it was recorded as ''0'' because ''IsEnabled'' was ''0''.';
												SET @is2FAActive_ = 0;
											END
										ELSE IF @emailAddress <> @emailAddress_
											BEGIN
												SET @additionalData = N'Altough new value of ''Is2FAActive'' was ''1'', it was recorded as ''0'' because ''EmailAddress'' was changed.';
												SET @is2FAActive_ = 0;
											END
										ELSE
											BEGIN
												EXEC [dbo].[IsVerifiedOrPendingEmailAddress] @emailAddress, @isEmailVerified OUTPUT, @emailVerifiedUserId OUTPUT;
												IF @isEmailVerified = 1 AND @emailVerifiedUserId = @id
													BEGIN
														UPDATE [Users]
															SET
																[Is2FAActive] = 0
															WHERE
																[EmailAddress] = @emailAddress AND
																[Id] <> @id;
														SET @additionalData = N'All ''Is2FAActive'' records were updated to ''0'' for the records which had the same ''EmailAddress'' value. (NO MATTER if others were ''IsEnabled = 0'')';
													END
												ELSE
													BEGIN
														SET @additionalData = N'Altough new value of ''Is2FAActive'' was ''1'', it was recorded as ''0'' because there was no verified ''EmailAddress''.';
														SET @is2FAActive_ = 0;
													END
											END
									END
								IF @emailAddress <> @emailAddress_
									UPDATE [UserVerificationTokens] SET [IsEnabled] = 0 WHERE [UserId] = @id AND [IsEnabled] = 1;
								ELSE
									UPDATE [UserVerificationTokens] SET [IsEnabled] = 0 WHERE [UserId] = @id AND [IsEnabled] = 1 AND [VerificationType_EnumerationValue] <> 1;
								SET @additionalData =
									(CASE WHEN @additionalData IS NULL THEN N'' ELSE (@additionalData + N' ||| ') END) +
									N'All UserVerificationTokens which belong to the User was set to IsEbabled = 0, due to the User has been updated.';
								UPDATE Users
									SET
										[UserGroupId] = @userGroupId_,
										[EmailAddress] = @emailAddress_,
										[Username] = @username_,
										[Password] = @pswX,
										[FirstName] = @firstName_,
										[LastName] = @lastName_,
										[Is2FAActive] = @is2FAActive_,
										[Grants] = @grants_,
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = @isEnabled_
										BEGIN
											DECLARE @values3 [dbo].[typeIUDLogValue];
											INSERT INTO @values3 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
												('UserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userGroupId), CONVERT(NVARCHAR(MAX), @userGroupId_), NULL, NULL), 
												('EmailAddress', 'VARCHAR(300)', 0, @emailAddress, @emailAddress_, NULL, NULL), 
												('Username', 'VARCHAR(50)', 0, @username, @username_, NULL, NULL), 
												('Password', 'NVARCHAR(MAX)', 0, @password, @pswX, NULL, NULL), 
												('FirstName', 'NVARCHAR(200)', 0, @firstName, @firstName_, NULL, NULL), 
												('LastName', 'NVARCHAR(200)', 0, @lastName, @lastName_, NULL, NULL), 
												('Is2FAActive', 'BIT', 0, CONVERT(NVARCHAR(MAX), @is2FAActive), CONVERT(NVARCHAR(MAX), @is2FAActive_), NULL, NULL), 
												('Grants', 'VARBINARY(MAX)', 0, NULL, NULL, @grants, @grants_);
											EXEC [dbo].[Insert_IUDLogs] 2, 1, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values3, @logId OUTPUT;
										END
									ELSE IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = @isEnabled_
										BEGIN
											DECLARE @values4 [dbo].[typeIUDLogValue];
											INSERT INTO @values4 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String], [OldValue_Binary], [NewValue_Binary]) VALUES
												('UserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userGroupId), CONVERT(NVARCHAR(MAX), @userGroupId_), NULL, NULL), 
												('EmailAddress', 'VARCHAR(300)', 0, @emailAddress, @emailAddress_, NULL, NULL), 
												('Username', 'VARCHAR(50)', 0, @username, @username_, NULL, NULL), 
												('Password', 'NVARCHAR(MAX)', 0, @password, @pswX, NULL, NULL), 
												('FirstName', 'NVARCHAR(200)', 0, @firstName, @firstName_, NULL, NULL), 
												('LastName', 'NVARCHAR(200)', 0, @lastName, @lastName_, NULL, NULL), 
												('Is2FAActive', 'BIT', 0, CONVERT(NVARCHAR(MAX), @is2FAActive), CONVERT(NVARCHAR(MAX), @is2FAActive_), NULL, NULL), 
												('Grants', 'BIGINT', 0, NULL, NULL, @grants, @grants_);
											EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values4, @logId OUTPUT;
										END
									ELSE IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgUsers_InsteadOfUpdate INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled, @id_, @userGroupId_, @emailAddress_, @username_, @password_, @firstName_, @lastName_, @is2FAActive_, @grants_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgUsers_InsteadOfUpdate;
	DEALLOCATE _cursor_trgUsers_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUsers_InsteadOfInsert
ON Users
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUsers_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@encpsw NVARCHAR(MAX),
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userGroupId UNIQUEIDENTIFIER,
		@emailAddress VARCHAR(300),
		@username VARCHAR(50),
		@password NVARCHAR(MAX),
		@firstName NVARCHAR(200),
		@lastName NVARCHAR(200),
		@is2FAActive BIT,
		@grants VARBINARY(MAX),
		@isEnabled BIT;
	DECLARE _cursor_trgUsers_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[UserGroupId],
				LTRIM(RTRIM(LOWER([EmailAddress] COLLATE SQL_Latin1_General_CP1253_CI_AI))),
				LTRIM(RTRIM(LOWER([Username] COLLATE SQL_Latin1_General_CP1253_CI_AI))),
				[Password],
				LTRIM(RTRIM([dbo].[CapitalizeString]([FirstName]))),
				LTRIM(RTRIM([dbo].[CapitalizeString]([LastName]))),
				[Is2FAActive],
				[Grants],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgUsers_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgUsers_InsteadOfInsert INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			EXEC [dbo].[EncryptUserPassword] @username, @password, @encpsw OUTPUT;
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE IF @is2FAActive = 1
					EXEC [dbo].[RaiseSpecificError] '2FA was enabled on sign-up.';
				ELSE
					BEGIN
						DECLARE @isVerifiedEmail BIT;
						DECLARE @userIdEmail UNIQUEIDENTIFIER;
						EXEC [dbo].[IsVerifiedOrPendingEmailAddress] @emailAddress, @isVerifiedEmail OUTPUT, @userIdEmail OUTPUT;
						IF @userIdEmail IS NOT NULL
							EXEC [dbo].[RaiseSpecificError] 'Email Address was in use.';
						ELSE
							BEGIN
								INSERT INTO Users ([Id], [UserGroupId], [EmailAddress], [Username], [Password], [FirstName], [LastName], [Is2FAActive], [Grants], [IsEnabled])
									VALUES (@id, @userGroupId, @emailAddress, @username, @encpsw, @firstName, @lastName, @is2FAActive, @grants, @isEnabled);
								DECLARE @verificationToken VARCHAR(50);
								DECLARE @verificationTokenValidUntil DATETIMEOFFSET;
								DECLARE @verificationTokenId UNIQUEIDENTIFIER;
								EXEC [dbo].[Insert_UserVerificationTokens] @id, @emailAddress, 1, @verificationToken OUTPUT, @verificationTokenValidUntil OUTPUT, @verificationTokenId OUTPUT;
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 1, 1, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String], [NewValue_Binary]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
					('UserGroupId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userGroupId), NULL), 
					('EmailAddress', 'VARCHAR(300)', 0, @emailAddress, NULL), 
					('Username', 'VARCHAR(50)', 0, @username, NULL), 
					('Password', 'NVARCHAR(MAX)', 0, @encpsw, NULL), 
					('FirstName', 'NVARCHAR(200)', 0, @firstName, NULL), 
					('LastName', 'NVARCHAR(200)', 0, @lastName, NULL), 
					('Is2FAActive', 'BIT', 0, CONVERT(NVARCHAR(MAX), @is2FAActive), NULL), 
					('Grants', 'VARBINARY(MAX)', 0, NULL, @grants), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'Users', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgUsers_InsteadOfInsert INTO @id, @userGroupId, @emailAddress, @username, @password, @firstName, @lastName, @is2FAActive, @grants, @isEnabled;
		END
	CLOSE _cursor_trgUsers_InsteadOfInsert;
	DEALLOCATE _cursor_trgUsers_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgUserVerificationTokens_InsteadOfDelete
ON UserVerificationTokens
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserVerificationTokens_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userId UNIQUEIDENTIFIER,
		@additionalData NVARCHAR(MAX),
		@verificationType_EnumerationValue TINYINT,
		@verificationToken VARCHAR(50),
		@verificationTokenValidUntil DATETIMEOFFSET,
		@isEnabled BIT;
	DECLARE _cursor_trgUserVerificationTokens_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[UserId],
				[AdditionalData],
				[VerificationType_EnumerationValue],
				[VerificationToken],
				[VerificationTokenValidUntil],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgUserVerificationTokens_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfDelete INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM UserVerificationTokens WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('UserId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userId), NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL), 
						('VerificationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue), NULL), 
						('VerificationToken', 'VARCHAR(50)', 0, @verificationToken, NULL), 
						('VerificationTokenValidUntil', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @verificationTokenValidUntil), NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE UserVerificationTokens WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_UserVerificationTokens] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfDelete INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled;
		END
	CLOSE _cursor_trgUserVerificationTokens_InsteadOfDelete;
	DEALLOCATE _cursor_trgUserVerificationTokens_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserVerificationTokens_InsteadOfUpdate
ON UserVerificationTokens
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserVerificationTokens_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userId UNIQUEIDENTIFIER,
		@additionalData NVARCHAR(MAX),
		@verificationType_EnumerationValue TINYINT,
		@verificationToken VARCHAR(50),
		@verificationTokenValidUntil DATETIMEOFFSET,
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@userId_ UNIQUEIDENTIFIER,
		@additionalData_ NVARCHAR(MAX),
		@verificationType_EnumerationValue_ TINYINT,
		@verificationToken_ VARCHAR(50),
		@verificationTokenValidUntil_ DATETIMEOFFSET,
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND D.[UserId] = I.[UserId] AND ((D.[AdditionalData] IS NULL AND I.[AdditionalData] IS NULL) OR D.[AdditionalData] = I.[AdditionalData]) AND D.[VerificationType_EnumerationValue] = I.[VerificationType_EnumerationValue] AND D.[VerificationToken] = I.[VerificationToken] AND D.[VerificationTokenValidUntil] = I.[VerificationTokenValidUntil]
		WHERE
			D.[IsEnabled] <> I.[IsEnabled];
	DECLARE _cursor_trgUserVerificationTokens_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[UserId],
				D.[AdditionalData],
				D.[VerificationType_EnumerationValue],
				D.[VerificationToken],
				D.[VerificationTokenValidUntil],
				D.[IsEnabled],
				I.[Id],
				I.[UserId],
				I.[AdditionalData],
				I.[VerificationType_EnumerationValue],
				I.[VerificationToken],
				I.[VerificationTokenValidUntil],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgUserVerificationTokens_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfUpdate INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled, @id_, @userId_, @additionalData_, @verificationType_EnumerationValue_, @verificationToken_, @verificationTokenValidUntil_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('UserId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userId), CONVERT(NVARCHAR(MAX), @userId_)), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_), 
						('VerificationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue), CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue_)), 
						('VerificationToken', 'VARCHAR(50)', 0, @verificationToken, @verificationToken_), 
						('VerificationTokenValidUntil', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @verificationTokenValidUntil), CONVERT(NVARCHAR(MAX), @verificationTokenValidUntil_)), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_));
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfUpdate INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled, @id_, @userId_, @additionalData_, @verificationType_EnumerationValue_, @verificationToken_, @verificationTokenValidUntil_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfUpdate INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled, @id_, @userId_, @additionalData_, @verificationType_EnumerationValue_, @verificationToken_, @verificationTokenValidUntil_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM UserVerificationTokens WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE UserVerificationTokens
									SET
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfUpdate INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled, @id_, @userId_, @additionalData_, @verificationType_EnumerationValue_, @verificationToken_, @verificationTokenValidUntil_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgUserVerificationTokens_InsteadOfUpdate;
	DEALLOCATE _cursor_trgUserVerificationTokens_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserVerificationTokens_InsteadOfInsert
ON UserVerificationTokens
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserVerificationTokens_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userId UNIQUEIDENTIFIER,
		@additionalData NVARCHAR(MAX),
		@verificationType_EnumerationValue TINYINT,
		@verificationToken VARCHAR(50),
		@verificationTokenValidUntil DATETIMEOFFSET,
		@isEnabled BIT;
	DECLARE _cursor_trgUserVerificationTokens_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[UserId],
				[AdditionalData],
				[VerificationType_EnumerationValue],
				[VerificationToken],
				[VerificationTokenValidUntil],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgUserVerificationTokens_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfInsert INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						DECLARE @isVerified BIT;
						DECLARE @emailOwnerId UNIQUEIDENTIFIER;
						IF @verificationType_EnumerationValue <> 1 -- if it is not a verification request of an e-mail address
							BEGIN
								DECLARE @emailAddress VARCHAR(100);
								SELECT @emailAddress = [EmailAddress] FROM [Users] WHERE [Id] = @userId;
								EXEC [dbo].[IsVerifiedOrPendingEmailAddress] @emailAddress, @isVerified OUTPUT, @emailOwnerId OUTPUT;
								IF @isVerified = 0 OR @userId <> @emailOwnerId
									EXEC [dbo].[RaiseSpecificError] N'There was no verified e-mail address.';
							END
						ELSE
							BEGIN
								EXEC [dbo].[IsVerifiedOrPendingEmailAddress] @additionalData, @isVerified OUTPUT, @emailOwnerId OUTPUT;
								IF @isVerified = 1 AND @userId = @emailOwnerId
									EXEC [dbo].[RaiseSpecificError] N'E-mail address has already been verified by the same user.';
							END
						DECLARE @exists BIT;
						DECLARE @userGroupId UNIQUEIDENTIFIER;
						SELECT
								@exists = 1,
								@userGroupId = [UserGroupId]
							FROM [Users]
							WHERE
								[Id] = @userId AND
								[IsEnabled] = 1;
						IF @exists <> 1
							EXEC [dbo].[RaiseSpecificError] N'User was disabled.';
						DECLARE @isBanished BIT;
						EXEC [dbo].[IsBanishedUser] @userId, @isBanished OUTPUT;
						IF @isBanished <> 0
							EXEC [dbo].[RaiseSpecificError] N'User was banished.';
						IF @userGroupId IS NOT NULL
							BEGIN
								EXEC [dbo].[IsBanishedUserGroup] @userGroupId, @isBanished OUTPUT;
								IF @isBanished <> 0
									EXEC [dbo].[RaiseSpecificError] N'User Group was banished.';
							END
						UPDATE UserVerificationTokens
						SET
							[IsEnabled] = 0
						FROM UserVerificationTokens t1
						LEFT JOIN UserVerificationAttempts t2
							ON t2.[ResultUserVerificationTokenId] = t1.[Id]
						WHERE
							t1.[UserId] = @userId AND
							t1.[IsEnabled] = 1 AND
							(t2.[Id] IS NULL OR t1.[VerificationType_EnumerationValue] = @verificationType_EnumerationValue);
						INSERT INTO UserVerificationTokens ([Id], [UserId], [AdditionalData], [VerificationType_EnumerationValue], [VerificationToken], [VerificationTokenValidUntil], [IsEnabled])
							VALUES (@id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled);
						IF @@TRANCOUNT > 0
							COMMIT TRANSACTION;
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('UserId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userId)), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData), 
					('VerificationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue)), 
					('VerificationToken', 'VARCHAR(50)', 0, @verificationToken), 
					('VerificationTokenValidUntil', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @verificationTokenValidUntil)), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'UserVerificationTokens', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgUserVerificationTokens_InsteadOfInsert INTO @id, @userId, @additionalData, @verificationType_EnumerationValue, @verificationToken, @verificationTokenValidUntil, @isEnabled;
		END
	CLOSE _cursor_trgUserVerificationTokens_InsteadOfInsert;
	DEALLOCATE _cursor_trgUserVerificationTokens_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgUserVerificationAttempts_InsteadOfDelete
ON UserVerificationAttempts
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserVerificationAttempts_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@attemptDate DATETIMEOFFSET,
		@verificationType_EnumerationValue TINYINT,
		@userId UNIQUEIDENTIFIER,
		@verificationToken NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@resultUserVerificationTokenId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT;
	DECLARE _cursor_trgUserVerificationAttempts_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[AttemptDate],
				[VerificationType_EnumerationValue],
				[UserId],
				[VerificationToken],
				[AdditionalData],
				[ResultUserVerificationTokenId],
				[ResultFailureReason_EnumerationValue]
			FROM deleted;
	OPEN _cursor_trgUserVerificationAttempts_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgUserVerificationAttempts_InsteadOfDelete INTO @id, @pageRequestId, @attemptDate, @verificationType_EnumerationValue, @userId, @verificationToken, @additionalData, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM UserVerificationAttempts WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserVerificationAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserVerificationAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), NULL), 
						('AttemptDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @attemptDate), NULL), 
						('VerificationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue), NULL), 
						('UserId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userId), NULL), 
						('VerificationToken', 'NVARCHAR(MAX)', 1, @verificationToken, NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL), 
						('ResultUserVerificationTokenId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId), NULL), 
						('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE UserVerificationAttempts WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'UserVerificationAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserVerificationAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgUserVerificationAttempts_InsteadOfDelete INTO @id, @pageRequestId, @attemptDate, @verificationType_EnumerationValue, @userId, @verificationToken, @additionalData, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
		END
	CLOSE _cursor_trgUserVerificationAttempts_InsteadOfDelete;
	DEALLOCATE _cursor_trgUserVerificationAttempts_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserVerificationAttempts_InsteadOfUpdate
ON UserVerificationAttempts
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserVerificationAttempts_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@attemptDate DATETIMEOFFSET,
		@verificationType_EnumerationValue TINYINT,
		@userId UNIQUEIDENTIFIER,
		@verificationToken NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@resultUserVerificationTokenId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT,
		@id_ UNIQUEIDENTIFIER,
		@pageRequestId_ UNIQUEIDENTIFIER,
		@attemptDate_ DATETIMEOFFSET,
		@verificationType_EnumerationValue_ TINYINT,
		@userId_ UNIQUEIDENTIFIER,
		@verificationToken_ NVARCHAR(MAX),
		@additionalData_ NVARCHAR(MAX),
		@resultUserVerificationTokenId_ UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue_ INT;
	DECLARE _cursor_trgUserVerificationAttempts_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[PageRequestId],
				D.[AttemptDate],
				D.[VerificationType_EnumerationValue],
				D.[UserId],
				D.[VerificationToken],
				D.[AdditionalData],
				D.[ResultUserVerificationTokenId],
				D.[ResultFailureReason_EnumerationValue],
				I.[Id],
				I.[PageRequestId],
				I.[AttemptDate],
				I.[VerificationType_EnumerationValue],
				I.[UserId],
				I.[VerificationToken],
				I.[AdditionalData],
				I.[ResultUserVerificationTokenId],
				I.[ResultFailureReason_EnumerationValue]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgUserVerificationAttempts_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgUserVerificationAttempts_InsteadOfUpdate INTO @id, @pageRequestId, @attemptDate, @verificationType_EnumerationValue, @userId, @verificationToken, @additionalData, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue, @id_, @pageRequestId_, @attemptDate_, @verificationType_EnumerationValue_, @userId_, @verificationToken_, @additionalData_, @resultUserVerificationTokenId_, @resultFailureReason_EnumerationValue_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), CONVERT(NVARCHAR(MAX), @pageRequestId_)), 
				('AttemptDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @attemptDate), CONVERT(NVARCHAR(MAX), @attemptDate_)), 
				('VerificationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue), CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue_)), 
				('UserId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userId), CONVERT(NVARCHAR(MAX), @userId_)), 
				('VerificationToken', 'NVARCHAR(MAX)', 1, @verificationToken, @verificationToken_), 
				('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_), 
				('ResultUserVerificationTokenId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId), CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId_)), 
				('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'UserVerificationAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgUserVerificationAttempts_InsteadOfUpdate INTO @id, @pageRequestId, @attemptDate, @verificationType_EnumerationValue, @userId, @verificationToken, @additionalData, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue, @id_, @pageRequestId_, @attemptDate_, @verificationType_EnumerationValue_, @userId_, @verificationToken_, @additionalData_, @resultUserVerificationTokenId_, @resultFailureReason_EnumerationValue_;
		END
	CLOSE _cursor_trgUserVerificationAttempts_InsteadOfUpdate;
	DEALLOCATE _cursor_trgUserVerificationAttempts_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserVerificationAttempts_InsteadOfInsert
ON UserVerificationAttempts
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserVerificationAttempts_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@attemptDate DATETIMEOFFSET,
		@verificationType_EnumerationValue TINYINT,
		@userId UNIQUEIDENTIFIER,
		@verificationToken NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@resultUserVerificationTokenId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT;
	DECLARE _cursor_trgUserVerificationAttempts_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[AttemptDate],
				[VerificationType_EnumerationValue],
				[UserId],
				[VerificationToken],
				[AdditionalData],
				[ResultUserVerificationTokenId],
				[ResultFailureReason_EnumerationValue]
			FROM inserted;
	OPEN _cursor_trgUserVerificationAttempts_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgUserVerificationAttempts_InsteadOfInsert INTO @id, @pageRequestId, @attemptDate, @verificationType_EnumerationValue, @userId, @verificationToken, @additionalData, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @resultUserVerificationTokenId IS NOT NULL
					EXEC [dbo].[RaiseSpecificError] N'''ResultUserVerificationTokenId'' was not null on insert.';
				IF @resultFailureReason_EnumerationValue IS NOT NULL
					EXEC [dbo].[RaiseSpecificError] N'''ResultFailureReason_EnumerationValue'' was not null on insert.';
				DECLARE @failure INT = 0;
				DECLARE @ipAddress VARCHAR(15);
				DECLARE @requestFailure INT;
				SELECT
						@requestFailure = t1.[FailureReason_EnumerationValue],
						@ipAddress = t2.[IpAddressV4]
					FROM [PageRequests] t1
					INNER JOIN [ClientIpAddresses] t2 ON
						t2.[Id] = t1.[ClientIpAddressId]
					WHERE t1.[Id] = @pageRequestId;
				IF @ipAddress IS NULL OR @requestFailure IS NOT NULL
					SET @failure = @failure | 1; -- PageRequestFailure
				DECLARE @userIsEnabled BIT;
				DECLARE @tokenIsEnabled BIT;
				DECLARE @outOfDate BIT;
				DECLARE @additionalDataIsValid BIT;
				DECLARE @verificationTokenIsCorrect BIT;
				DECLARE @userGroupId UNIQUEIDENTIFIER;
				SELECT TOP 1
						@userIsEnabled = t2.[IsEnabled],
						@tokenIsEnabled = t1.[IsEnabled],
						@outOfDate =
							CASE
								WHEN t1.[VerificationTokenValidUntil] < SYSDATETIMEOFFSET()
									THEN 1
								ELSE 0
							END,
						@additionalDataIsValid =
							CASE
								WHEN t1.[VerificationType_EnumerationValue] <> 1 OR LOWER(t1.[AdditionalData] COLLATE SQL_Latin1_General_CP1253_CI_AI) = t2.[EmailAddress]
									THEN 1
								ELSE 0
							END,
						@verificationTokenIsCorrect =
							CASE
								WHEN t1.[VerificationToken] = @verificationToken
									THEN 1
								ELSE 0
							END,
						@resultUserVerificationTokenId = t1.[Id],
						@userGroupId = t2.[UserGroupId]
					FROM [UserVerificationTokens] t1
					INNER JOIN [Users] t2
						ON t2.[Id] = t1.[UserId]
					WHERE
						t1.[UserId] = @userId AND
						t1.[VerificationType_EnumerationValue] = @verificationType_EnumerationValue AND
						((@additionalData IS NULL AND t1.[AdditionalData] IS NULL) OR (@additionalData IS NOT NULL AND t1.[AdditionalData] IS NOT NULL AND t1.[AdditionalData] = @additionalData))
					ORDER BY
						t1.[VerificationTokenValidUntil] DESC;
				IF @userIsEnabled IS NULL
					SET @failure = @failure | 4; -- IncorrectQuery
				ELSE
					BEGIN
						IF @userIsEnabled = 0
							SET @failure = @failure | 128; -- DisabledUser
						IF @tokenIsEnabled = 0
							SET @failure = @failure | 64; -- DisabledVerificationToken
						IF @outOfDate = 1
							SET @failure = @failure | 16; -- OutOfDate
						IF @additionalDataIsValid = 0
							SET @failure = @failure | 32; -- IncorrectAdditionalData
						IF @verificationTokenIsCorrect = 0
							SET @failure = @failure | 8; -- IncorrectToken
						DECLARE @isBanishedUser BIT;
						EXEC [dbo].[IsBanishedUser] @userId, @isBanishedUser OUTPUT;
						IF @isBanishedUser = 1
							SET @failure = @failure | 512; -- BanishedUser
						IF @userGroupId IS NOT NULL
							BEGIN
								DECLARE @isBanishedUserGroup BIT;
								EXEC [dbo].[IsBanishedUserGroup] @userGroupId, @isBanishedUserGroup OUTPUT;
								IF @isBanishedUserGroup = 1
									SET @failure = @failure | 256; -- BanishedUserGroup
							END
						DECLARE @whitelistState TINYINT;
						EXEC [dbo].GetWhitelistState @userId, @ipAddress, @whitelistState OUTPUT;
						IF @whitelistState = 1
							SET @failure = @failure | 1024; -- NotInWhitelist_User
						ELSE IF @whitelistState = 2
							SET @failure = @failure | 2; -- NotInWhitelist
					END
				IF @resultUserVerificationTokenId IS NOT NULL
					BEGIN
						IF EXISTS(
							SELECT
									1
								FROM [UserVerificationAttempts]
								WHERE
									[ResultUserVerificationTokenId] = @resultUserVerificationTokenId AND
									[ResultFailureReason_EnumerationValue] IS NULL)
							SET @failure = @failure | 2048; -- AlreadyVerified
						DECLARE @latestTokenId UNIQUEIDENTIFIER;
						SELECT TOP 1
								@latestTokenId = [Id]
							FROM [UserVerificationTokens]
							WHERE
								[UserId] = @userId AND
								[VerificationType_EnumerationValue] = @verificationType_EnumerationValue
							ORDER BY
								[VerificationTokenValidUntil] DESC;
						IF @latestTokenId <> @resultUserVerificationTokenId
							BEGIN
								SET @failure = @failure | 4096; -- OldToken
								UPDATE [UserVerificationTokens]
									SET
										[IsEnabled] = 0
									WHERE
										[Id] = @resultUserVerificationTokenId AND
										[IsEnabled] = 1;
							END
					END
				IF @failure = 0
					BEGIN
						IF @verificationType_EnumerationValue = 1
							BEGIN
								UPDATE [UserVerificationTokens]
									SET
										[IsEnabled] = 0
									WHERE
										[IsEnabled] = 1 AND
										[VerificationType_EnumerationValue] = 1 AND
										[UserId] = @userId AND
										[Id] <> @resultUserVerificationTokenId;
								DECLARE @additionalData_ NVARCHAR(MAX) = UPPER(@additionalData);
								DECLARE @checksumAdditionalData INT = CHECKSUM(@additionalData_);
								UPDATE [UserVerificationTokens]
									SET
										[IsEnabled] = 0
									WHERE
										[IsEnabled] = 1 AND
										[VerificationType_EnumerationValue] = 1 AND
										[UserId] <> @userId AND
										[AdditionalData_Checksum] = @checksumAdditionalData AND
										[AdditionalData] IS NOT NULL AND
										UPPER([AdditionalData]) = @additionalData_;
							END
						ELSE
							UPDATE [UserVerificationTokens]
								SET
									[IsEnabled] = 0
								WHERE
									[IsEnabled] = 1 AND
									[UserId] = @userId AND
									[VerificationType_EnumerationValue] = @verificationType_EnumerationValue;
					END
				INSERT INTO UserVerificationAttempts
				(
					[Id],
					[PageRequestId],
					[AttemptDate],
					[VerificationType_EnumerationValue],
					[UserId],
					[VerificationToken],
					[AdditionalData],
					[ResultUserVerificationTokenId],
					[ResultFailureReason_EnumerationValue]
				)
				VALUES
				(
					@id,
					@pageRequestId,
					@attemptDate,
					@verificationType_EnumerationValue,
					@userId,
					@verificationToken,
					@additionalData,
					@resultUserVerificationTokenId,
					CASE
						WHEN @failure = 0
							THEN NULL
						ELSE @failure
					END
				);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId)), 
					('AttemptDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @attemptDate)), 
					('VerificationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @verificationType_EnumerationValue)), 
					('UserId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userId)), 
					('VerificationToken', 'NVARCHAR(MAX)', 1, @verificationToken), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData), 
					('ResultUserVerificationTokenId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId)), 
					('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'UserVerificationAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgUserVerificationAttempts_InsteadOfInsert INTO @id, @pageRequestId, @attemptDate, @verificationType_EnumerationValue, @userId, @verificationToken, @additionalData, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
		END
	CLOSE _cursor_trgUserVerificationAttempts_InsteadOfInsert;
	DEALLOCATE _cursor_trgUserVerificationAttempts_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgUserLoginAttempts_InsteadOfDelete
ON UserLoginAttempts
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserLoginAttempts_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@attemptDate DATETIMEOFFSET,
		@username NVARCHAR(MAX),
		@password NVARCHAR(MAX),
		@resultUserId UNIQUEIDENTIFIER,
		@resultUserVerificationTokenId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT;
	DECLARE _cursor_trgUserLoginAttempts_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[AttemptDate],
				[Username],
				[Password],
				[ResultUserId],
				[ResultUserVerificationTokenId],
				[ResultFailureReason_EnumerationValue]
			FROM deleted;
	OPEN _cursor_trgUserLoginAttempts_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgUserLoginAttempts_InsteadOfDelete INTO @id, @pageRequestId, @attemptDate, @username, @password, @resultUserId, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM UserLoginAttempts WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserLoginAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserLoginAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), NULL), 
						('AttemptDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @attemptDate), NULL), 
						('Username', 'NVARCHAR(MAX)', 0, @username, NULL), 
						('Password', 'NVARCHAR(MAX)', 0, @password, NULL), 
						('ResultUserId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserId), NULL), 
						('ResultUserVerificationTokenId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId), NULL), 
						('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE UserLoginAttempts WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'UserLoginAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserLoginAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgUserLoginAttempts_InsteadOfDelete INTO @id, @pageRequestId, @attemptDate, @username, @password, @resultUserId, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
		END
	CLOSE _cursor_trgUserLoginAttempts_InsteadOfDelete;
	DEALLOCATE _cursor_trgUserLoginAttempts_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserLoginAttempts_InsteadOfUpdate
ON UserLoginAttempts
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserLoginAttempts_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@attemptDate DATETIMEOFFSET,
		@username NVARCHAR(MAX),
		@password NVARCHAR(MAX),
		@resultUserId UNIQUEIDENTIFIER,
		@resultUserVerificationTokenId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT,
		@id_ UNIQUEIDENTIFIER,
		@pageRequestId_ UNIQUEIDENTIFIER,
		@attemptDate_ DATETIMEOFFSET,
		@username_ NVARCHAR(MAX),
		@password_ NVARCHAR(MAX),
		@resultUserId_ UNIQUEIDENTIFIER,
		@resultUserVerificationTokenId_ UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue_ INT;
	DECLARE _cursor_trgUserLoginAttempts_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[PageRequestId],
				D.[AttemptDate],
				D.[Username],
				D.[Password],
				D.[ResultUserId],
				D.[ResultUserVerificationTokenId],
				D.[ResultFailureReason_EnumerationValue],
				I.[Id],
				I.[PageRequestId],
				I.[AttemptDate],
				I.[Username],
				I.[Password],
				I.[ResultUserId],
				I.[ResultUserVerificationTokenId],
				I.[ResultFailureReason_EnumerationValue]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgUserLoginAttempts_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgUserLoginAttempts_InsteadOfUpdate INTO @id, @pageRequestId, @attemptDate, @username, @password, @resultUserId, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue, @id_, @pageRequestId_, @attemptDate_, @username_, @password_, @resultUserId_, @resultUserVerificationTokenId_, @resultFailureReason_EnumerationValue_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), CONVERT(NVARCHAR(MAX), @pageRequestId_)), 
				('AttemptDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @attemptDate), CONVERT(NVARCHAR(MAX), @attemptDate_)), 
				('Username', 'NVARCHAR(MAX)', 0, @username, @username_), 
				('Password', 'NVARCHAR(MAX)', 0, @password, @password_), 
				('ResultUserId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserId), CONVERT(NVARCHAR(MAX), @resultUserId_)), 
				('ResultUserVerificationTokenId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId), CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId_)), 
				('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'UserLoginAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgUserLoginAttempts_InsteadOfUpdate INTO @id, @pageRequestId, @attemptDate, @username, @password, @resultUserId, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue, @id_, @pageRequestId_, @attemptDate_, @username_, @password_, @resultUserId_, @resultUserVerificationTokenId_, @resultFailureReason_EnumerationValue_;
		END
	CLOSE _cursor_trgUserLoginAttempts_InsteadOfUpdate;
	DEALLOCATE _cursor_trgUserLoginAttempts_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserLoginAttempts_InsteadOfInsert
ON UserLoginAttempts
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserLoginAttempts_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@now DATETIMEOFFSET = SYSDATETIMEOFFSET(),
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@attemptDate DATETIMEOFFSET,
		@username NVARCHAR(MAX),
		@password NVARCHAR(MAX),
		@resultUserId UNIQUEIDENTIFIER,
		@resultUserVerificationTokenId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT;
	DECLARE _cursor_trgUserLoginAttempts_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[AttemptDate],
				[Username],
				[Password],
				[ResultUserId],
				[ResultUserVerificationTokenId],
				[ResultFailureReason_EnumerationValue]
			FROM inserted;
	OPEN _cursor_trgUserLoginAttempts_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgUserLoginAttempts_InsteadOfInsert INTO @id, @pageRequestId, @attemptDate, @username, @password, @resultUserId, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @enpsw NVARCHAR(MAX);
			EXEC [dbo].[EncryptUserPassword] @username, @password, @enpsw OUTPUT;
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @resultUserId IS NOT NULL
					EXEC [dbo].[RaiseSpecificError] N'''ResultUserId'' was not null on insert.';
				IF @resultUserVerificationTokenId IS NOT NULL
					EXEC [dbo].[RaiseSpecificError] N'''ResultUserVerificationTokenId'' was not null on insert.';
				IF @resultFailureReason_EnumerationValue IS NOT NULL
					EXEC [dbo].[RaiseSpecificError] N'''ResultFailureReason_EnumerationValue'' was not null on insert.';
				DECLARE @failure INT = 0;
				DECLARE @ipAddress VARCHAR(15);
				DECLARE @requestFailure INT;
				SELECT
						@requestFailure = t1.[FailureReason_EnumerationValue],
						@ipAddress = t2.[IpAddressV4]
					FROM [PageRequests] t1
					INNER JOIN [ClientIpAddresses] t2 ON
						t2.[Id] = t1.[ClientIpAddressId]
					WHERE t1.[Id] = @pageRequestId;
				IF @ipAddress IS NULL OR @requestFailure IS NOT NULL
					SET @failure = @failure | 1; -- PageRequestFailure
				DECLARE @isEnabled BIT;
				DECLARE @is2FAActive BIT;
				DECLARE @psw NVARCHAR(MAX);
				DECLARE @emailAddress VARCHAR(100);
				DECLARE @userGroupId UNIQUEIDENTIFIER;
				SELECT
						@userGroupId = t1.[UserGroupId],
						@isEnabled = t1.[IsEnabled],
						@resultUserId = t1.[Id],
						@psw = t1.[Password],
						@emailAddress = t1.[EmailAddress],
						@is2FAActive = t1.[Is2FAActive]
					FROM [Users] t1
					WHERE
						t1.[Username] = @username;
				IF @resultUserId IS NULL
					SET @failure = @failure | 4; -- IncorrectUsername
				ELSE
					BEGIN
						IF @isEnabled = 0
							SET @failure = @failure | 64; -- DisabledUser
						DECLARE @isBanishedUser BIT;
						EXEC [dbo].[IsBanishedUser] @resultUserId, @isBanishedUser OUTPUT;
						IF @isBanishedUser = 1
							SET @failure = @failure | 256; -- BanishedUser
						IF @userGroupId IS NOT NULL
							BEGIN
								DECLARE @isBanishedUserGroup BIT;
								EXEC [dbo].[IsBanishedUserGroup] @userGroupId, @isBanishedUserGroup OUTPUT;
								IF @isBanishedUserGroup = 1
									SET @failure = @failure | 128; -- BanishedUserGroup
							END
						DECLARE @psw_ NVARCHAR(MAX);
						EXEC [dbo].[DecryptUserPassword] @username, @psw, @psw_ OUTPUT;
						IF (@psw_ <> @password)
							SET @failure = @failure | 8; -- IncorrectPassword
						ELSE
							BEGIN
								DECLARE @isVeryFirstPassword BIT;
								EXEC [dbo].[IsVeryFirstPassword] @resultUserId, @isVeryFirstPassword OUTPUT;
								IF @isVeryFirstPassword = 1
									SET @failure = @failure | 1024; -- VeryFirstPassword
							END
					END
				DECLARE @isVerified BIT;
				DECLARE @aid UNIQUEIDENTIFIER;
				EXEC [dbo].[IsVerifiedOrPendingEmailAddress] @emailAddress, @isVerified OUTPUT, @aid OUTPUT;
				IF @isVerified = 0
					SET @failure = @failure | 16; -- NoVerifiedEmailAddress
				ELSE IF @aid <> @resultUserId
					SET @failure = @failure | 32; -- EmailAddressVerifiedForAnotherUser
				DECLARE @whitelistState TINYINT;
				EXEC [dbo].GetWhitelistState @resultUserId, @ipAddress, @whitelistState OUTPUT;
				IF @whitelistState = 1
					SET @failure = @failure | 512; -- NotInWhitelist_User
				ELSE IF @whitelistState = 2
					SET @failure = @failure | 2; -- NotInWhitelist
				IF @failure <> 0
					SET @resultFailureReason_EnumerationValue = @failure;
				ELSE IF @is2FAActive = 1
					BEGIN
						DECLARE @verificationToken VARCHAR(50);
						DECLARE @verificationTokenValidUntil DATETIMEOFFSET;
						EXEC [dbo].[Insert_UserVerificationTokens] @resultUserId, NULL, 3, @verificationToken, @verificationTokenValidUntil OUTPUT, @resultUserVerificationTokenId OUTPUT;
					END
				INSERT INTO UserLoginAttempts ([Id], [PageRequestId], [AttemptDate], [Username], [Password], [ResultUserId], [ResultUserVerificationTokenId], [ResultFailureReason_EnumerationValue])
					VALUES (@id, @pageRequestId, @attemptDate, @username, @enpsw, @resultUserId, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId)), 
					('AttemptDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @attemptDate)), 
					('Username', 'NVARCHAR(MAX)', 0, @username), 
					('Password', 'NVARCHAR(MAX)', 0, @enpsw), 
					('ResultUserId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserId)), 
					('ResultUserVerificationTokenId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @resultUserVerificationTokenId)), 
					('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'UserLoginAttempts', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgUserLoginAttempts_InsteadOfInsert INTO @id, @pageRequestId, @attemptDate, @username, @password, @resultUserId, @resultUserVerificationTokenId, @resultFailureReason_EnumerationValue;
		END
	CLOSE _cursor_trgUserLoginAttempts_InsteadOfInsert;
	DEALLOCATE _cursor_trgUserLoginAttempts_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgUserLogins_InsteadOfDelete
ON UserLogins
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserLogins_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userLoginAttemptId UNIQUEIDENTIFIER,
		@userVerificationAttemptId UNIQUEIDENTIFIER,
		@isEnabled BIT;
	DECLARE _cursor_trgUserLogins_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[UserLoginAttemptId],
				[UserVerificationAttemptId],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgUserLogins_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfDelete INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM UserLogins WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('UserLoginAttemptId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userLoginAttemptId), NULL), 
						('UserVerificationAttemptId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userVerificationAttemptId), NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE UserLogins WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_UserLogins] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfDelete INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled;
		END
	CLOSE _cursor_trgUserLogins_InsteadOfDelete;
	DEALLOCATE _cursor_trgUserLogins_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserLogins_InsteadOfUpdate
ON UserLogins
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserLogins_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userLoginAttemptId UNIQUEIDENTIFIER,
		@userVerificationAttemptId UNIQUEIDENTIFIER,
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@userLoginAttemptId_ UNIQUEIDENTIFIER,
		@userVerificationAttemptId_ UNIQUEIDENTIFIER,
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND D.[UserLoginAttemptId] = I.[UserLoginAttemptId] AND ((D.[UserVerificationAttemptId] IS NULL AND I.[UserVerificationAttemptId] IS NULL) OR D.[UserVerificationAttemptId] = I.[UserVerificationAttemptId])
		WHERE
			D.[IsEnabled] <> I.[IsEnabled];
	DECLARE _cursor_trgUserLogins_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[UserLoginAttemptId],
				D.[UserVerificationAttemptId],
				D.[IsEnabled],
				I.[Id],
				I.[UserLoginAttemptId],
				I.[UserVerificationAttemptId],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgUserLogins_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfUpdate INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled, @id_, @userLoginAttemptId_, @userVerificationAttemptId_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('UserLoginAttemptId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userLoginAttemptId), CONVERT(NVARCHAR(MAX), @userLoginAttemptId_)), 
						('UserVerificationAttemptId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userVerificationAttemptId), CONVERT(NVARCHAR(MAX), @userVerificationAttemptId_)), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_));
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfUpdate INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled, @id_, @userLoginAttemptId_, @userVerificationAttemptId_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfUpdate INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled, @id_, @userLoginAttemptId_, @userVerificationAttemptId_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM UserLogins WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE UserLogins
									SET
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfUpdate INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled, @id_, @userLoginAttemptId_, @userVerificationAttemptId_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgUserLogins_InsteadOfUpdate;
	DEALLOCATE _cursor_trgUserLogins_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgUserLogins_InsteadOfInsert
ON UserLogins
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgUserLogins_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userLoginAttemptId UNIQUEIDENTIFIER,
		@userVerificationAttemptId UNIQUEIDENTIFIER,
		@isEnabled BIT;
	DECLARE _cursor_trgUserLogins_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[UserLoginAttemptId],
				[UserVerificationAttemptId],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgUserLogins_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfInsert INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						DECLARE @userId UNIQUEIDENTIFIER;
						SELECT
								@userId = t2.[ResultUserId]
							FROM [UserLogins] t1
							INNER JOIN [UserLoginAttempts] t2
								ON t2.[Id] = t1.[UserLoginAttemptId]
							WHERE
								t1.[Id] = @id;
						UPDATE t1
							SET
								t1.[IsEnabled] = 0
							FROM [UserLogins] t1
							INNER JOIN [UserLoginAttempts] t2
								ON t2.[Id] = t1.[UserLoginAttemptId]
							LEFT JOIN [Users] t3
								ON t3.[Id] = t2.[ResultUserId]
							WHERE
								t1.[Id] <> @id AND
								t1.[IsEnabled] = 1 AND
								t2.[ResultUserId] IS NOT NULL AND
								t3.[Id] = @userId;
						INSERT INTO UserLogins ([Id], [UserLoginAttemptId], [UserVerificationAttemptId], [IsEnabled])
							VALUES (@id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled);
						IF @@TRANCOUNT > 0
							COMMIT TRANSACTION;
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('UserLoginAttemptId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userLoginAttemptId)), 
					('UserVerificationAttemptId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userVerificationAttemptId)), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'UserLogins', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgUserLogins_InsteadOfInsert INTO @id, @userLoginAttemptId, @userVerificationAttemptId, @isEnabled;
		END
	CLOSE _cursor_trgUserLogins_InsteadOfInsert;
	DEALLOCATE _cursor_trgUserLogins_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgWhitelist_InsteadOfDelete
ON Whitelist
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgWhitelist_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userId UNIQUEIDENTIFIER,
		@ipAddressV4 VARCHAR(15),
		@startsAt DATETIMEOFFSET,
		@endsAt DATETIMEOFFSET,
		@isEnabled BIT;
	DECLARE _cursor_trgWhitelist_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[UserId],
				[IpAddressV4],
				[StartsAt],
				[EndsAt],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgWhitelist_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfDelete INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Whitelist WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('UserId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userId), NULL), 
						('IpAddressV4', 'VARCHAR(15)', 0, @ipAddressV4, NULL), 
						('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt), NULL), 
						('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt), NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE Whitelist WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_Whitelist] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfDelete INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled;
		END
	CLOSE _cursor_trgWhitelist_InsteadOfDelete;
	DEALLOCATE _cursor_trgWhitelist_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgWhitelist_InsteadOfUpdate
ON Whitelist
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgWhitelist_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userId UNIQUEIDENTIFIER,
		@ipAddressV4 VARCHAR(15),
		@startsAt DATETIMEOFFSET,
		@endsAt DATETIMEOFFSET,
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@userId_ UNIQUEIDENTIFIER,
		@ipAddressV4_ VARCHAR(15),
		@startsAt_ DATETIMEOFFSET,
		@endsAt_ DATETIMEOFFSET,
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND ((D.[UserId] IS NULL AND I.[UserId] IS NULL) OR D.[UserId] = I.[UserId]) AND D.[IpAddressV4] = I.[IpAddressV4]
		WHERE
			(D.[IsEnabled] <> I.[IsEnabled] AND D.[StartsAt] = I.[StartsAt] AND ((D.[EndsAt] IS NULL AND I.[EndsAt] IS NULL) OR D.[EndsAt] = I.[EndsAt])) OR
			(D.[IsEnabled] = 1 AND I.[IsEnabled] = 1 AND (D.[StartsAt] <> I.[StartsAt] OR ((D.[EndsAt] IS NOT NULL OR I.[EndsAt] IS NOT NULL) AND D.[EndsAt] <> I.[EndsAt])));
	DECLARE _cursor_trgWhitelist_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[UserId],
				D.[IpAddressV4],
				D.[StartsAt],
				D.[EndsAt],
				D.[IsEnabled],
				I.[Id],
				I.[UserId],
				I.[IpAddressV4],
				I.[StartsAt],
				I.[EndsAt],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgWhitelist_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfUpdate INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled, @id_, @userId_, @ipAddressV4_, @startsAt_, @endsAt_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values1 [dbo].[typeIUDLogValue];
					INSERT INTO @values1 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('UserId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userId), CONVERT(NVARCHAR(MAX), @userId_)), 
						('IpAddressV4', 'VARCHAR(15)', 0, @ipAddressV4, @ipAddressV4_), 
						('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt), CONVERT(NVARCHAR(MAX), @startsAt_)), 
						('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt), CONVERT(NVARCHAR(MAX), @endsAt_)), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_));
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values1, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfUpdate INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled, @id_, @userId_, @ipAddressV4_, @startsAt_, @endsAt_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfUpdate INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled, @id_, @userId_, @ipAddressV4_, @startsAt_, @endsAt_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM Whitelist WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = @isEnabled_
									BEGIN
										DECLARE @values2 [dbo].[typeIUDLogValue];
										INSERT INTO @values2 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
											('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt), CONVERT(NVARCHAR(MAX), @startsAt_)), 
											('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt), CONVERT(NVARCHAR(MAX), @endsAt_));
										EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values2, @logId OUTPUT;
									END
								ELSE IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE Whitelist
									SET
										[StartsAt] = @startsAt_,
										[EndsAt] = @endsAt_,
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = @isEnabled_
										BEGIN
											DECLARE @values3 [dbo].[typeIUDLogValue];
											INSERT INTO @values3 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
												('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt), CONVERT(NVARCHAR(MAX), @startsAt_)), 
												('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt), CONVERT(NVARCHAR(MAX), @endsAt_));
											EXEC [dbo].[Insert_IUDLogs] 2, 1, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values3, @logId OUTPUT;
										END
									ELSE IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = @isEnabled_
										BEGIN
											DECLARE @values4 [dbo].[typeIUDLogValue];
											INSERT INTO @values4 ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
												('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt), CONVERT(NVARCHAR(MAX), @startsAt_)), 
												('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt), CONVERT(NVARCHAR(MAX), @endsAt_));
											EXEC [dbo].[Insert_IUDLogs] 2, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values4, @logId OUTPUT;
										END
									ELSE IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfUpdate INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled, @id_, @userId_, @ipAddressV4_, @startsAt_, @endsAt_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgWhitelist_InsteadOfUpdate;
	DEALLOCATE _cursor_trgWhitelist_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgWhitelist_InsteadOfInsert
ON Whitelist
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgWhitelist_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@userId UNIQUEIDENTIFIER,
		@ipAddressV4 VARCHAR(15),
		@startsAt DATETIMEOFFSET,
		@endsAt DATETIMEOFFSET,
		@isEnabled BIT;
	DECLARE _cursor_trgWhitelist_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[UserId],
				[IpAddressV4],
				[StartsAt],
				[EndsAt],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgWhitelist_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfInsert INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						INSERT INTO Whitelist ([Id], [UserId], [IpAddressV4], [StartsAt], [EndsAt], [IsEnabled])
							VALUES (@id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 1, 1, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('UserId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @userId)), 
					('IpAddressV4', 'VARCHAR(15)', 0, @ipAddressV4), 
					('StartsAt', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @startsAt)), 
					('EndsAt', 'DATETIMEOFFSET', 1, CONVERT(NVARCHAR(MAX), @endsAt)), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'Whitelist', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgWhitelist_InsteadOfInsert INTO @id, @userId, @ipAddressV4, @startsAt, @endsAt, @isEnabled;
		END
	CLOSE _cursor_trgWhitelist_InsteadOfInsert;
	DEALLOCATE _cursor_trgWhitelist_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgOperationLogs_InsteadOfDelete
ON OperationLogs
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgOperationLogs_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@operationDate DATETIMEOFFSET,
		@operationType_EnumerationValue BIGINT,
		@pageRequestId UNIQUEIDENTIFIER,
		@userLoginId UNIQUEIDENTIFIER,
		@targetContextName VARCHAR(50),
		@targetEntityName VARCHAR(100),
		@iUDLogId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT,
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgOperationLogs_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[OperationDate],
				[OperationType_EnumerationValue],
				[PageRequestId],
				[UserLoginId],
				[TargetContextName],
				[TargetEntityName],
				[IUDLogId],
				[FailureReason_EnumerationValue],
				[AdditionalData]
			FROM deleted;
	OPEN _cursor_trgOperationLogs_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgOperationLogs_InsteadOfDelete INTO @id, @operationDate, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iUDLogId, @resultFailureReason_EnumerationValue, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM OperationLogs WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'OperationLogs', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'OperationLogs', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('OperationDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @operationDate), NULL), 
						('OperationType_EnumerationValue', 'BIGINT', 0, CONVERT(NVARCHAR(MAX), @operationType_EnumerationValue), NULL), 
						('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), NULL), 
						('UserLoginId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userLoginId), NULL), 
						('TargetContextName', 'VARCHAR(50)', 1, @targetContextName, NULL), 
						('TargetEntityName', 'VARCHAR(100)', 1, @targetEntityName, NULL), 
						('IUDLogId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @iUDLogId), NULL), 
						('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), NULL), 
						('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE OperationLogs WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'OperationLogs', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'OperationLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgOperationLogs_InsteadOfDelete INTO @id, @operationDate, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iUDLogId, @resultFailureReason_EnumerationValue, @additionalData;
		END
	CLOSE _cursor_trgOperationLogs_InsteadOfDelete;
	DEALLOCATE _cursor_trgOperationLogs_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgOperationLogs_InsteadOfUpdate
ON OperationLogs
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgOperationLogs_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@operationDate DATETIMEOFFSET,
		@operationType_EnumerationValue BIGINT,
		@pageRequestId UNIQUEIDENTIFIER,
		@userLoginId UNIQUEIDENTIFIER,
		@targetContextName VARCHAR(50),
		@targetEntityName VARCHAR(100),
		@iUDLogId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT,
		@additionalData NVARCHAR(MAX),
		@id_ UNIQUEIDENTIFIER,
		@operationDate_ DATETIMEOFFSET,
		@operationType_EnumerationValue_ BIGINT,
		@pageRequestId_ UNIQUEIDENTIFIER,
		@userLoginId_ UNIQUEIDENTIFIER,
		@targetContextName_ VARCHAR(50),
		@targetEntityName_ VARCHAR(100),
		@iUDLogId_ UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue_ INT,
		@additionalData_ NVARCHAR(MAX);
	DECLARE _cursor_trgOperationLogs_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[OperationDate],
				D.[OperationType_EnumerationValue],
				D.[PageRequestId],
				D.[UserLoginId],
				D.[TargetContextName],
				D.[TargetEntityName],
				D.[IUDLogId],
				D.[FailureReason_EnumerationValue],
				D.[AdditionalData],
				I.[Id],
				I.[OperationDate],
				I.[OperationType_EnumerationValue],
				I.[PageRequestId],
				I.[UserLoginId],
				I.[TargetContextName],
				I.[TargetEntityName],
				I.[IUDLogId],
				I.[FailureReason_EnumerationValue],
				I.[AdditionalData]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgOperationLogs_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgOperationLogs_InsteadOfUpdate INTO @id, @operationDate, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iUDLogId, @resultFailureReason_EnumerationValue, @additionalData, @id_, @operationDate_, @operationType_EnumerationValue_, @pageRequestId_, @userLoginId_, @targetContextName_, @targetEntityName_, @iUDLogId_, @resultFailureReason_EnumerationValue_, @additionalData_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('OperationDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @operationDate), CONVERT(NVARCHAR(MAX), @operationDate_)), 
				('OperationType_EnumerationValue', 'BIGINT', 0, CONVERT(NVARCHAR(MAX), @operationType_EnumerationValue), CONVERT(NVARCHAR(MAX), @operationType_EnumerationValue_)), 
				('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), CONVERT(NVARCHAR(MAX), @pageRequestId_)), 
				('UserLoginId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userLoginId), CONVERT(NVARCHAR(MAX), @userLoginId_)), 
				('TargetContextName', 'VARCHAR(50)', 1, @targetContextName, @targetContextName_), 
				('TargetEntityName', 'VARCHAR(100)', 1, @targetEntityName, @targetEntityName_), 
				('IUDLogId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @iUDLogId), CONVERT(NVARCHAR(MAX), @iUDLogId_)), 
				('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue_)), 
				('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData, @additionalData_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'OperationLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgOperationLogs_InsteadOfUpdate INTO @id, @operationDate, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iUDLogId, @resultFailureReason_EnumerationValue, @additionalData, @id_, @operationDate_, @operationType_EnumerationValue_, @pageRequestId_, @userLoginId_, @targetContextName_, @targetEntityName_, @iUDLogId_, @resultFailureReason_EnumerationValue_, @additionalData_;
		END
	CLOSE _cursor_trgOperationLogs_InsteadOfUpdate;
	DEALLOCATE _cursor_trgOperationLogs_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgOperationLogs_InsteadOfInsert
ON OperationLogs
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgOperationLogs_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@operationDate DATETIMEOFFSET,
		@operationType_EnumerationValue BIGINT,
		@pageRequestId UNIQUEIDENTIFIER,
		@userLoginId UNIQUEIDENTIFIER,
		@targetContextName VARCHAR(50),
		@targetEntityName VARCHAR(100),
		@iUDLogId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT,
		@additionalData NVARCHAR(MAX);
	DECLARE _cursor_trgOperationLogs_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[OperationDate],
				[OperationType_EnumerationValue],
				[PageRequestId],
				[UserLoginId],
				[TargetContextName],
				[TargetEntityName],
				[IUDLogId],
				[FailureReason_EnumerationValue],
				[AdditionalData]
			FROM inserted;
	OPEN _cursor_trgOperationLogs_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgOperationLogs_InsteadOfInsert INTO @id, @operationDate, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iUDLogId, @resultFailureReason_EnumerationValue, @additionalData;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO OperationLogs ([Id], [OperationDate], [OperationType_EnumerationValue], [PageRequestId], [UserLoginId], [TargetContextName], [TargetEntityName], [IUDLogId], [FailureReason_EnumerationValue], [AdditionalData])
					VALUES (@id, @operationDate, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iUDLogId, @resultFailureReason_EnumerationValue, @additionalData);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('OperationDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @operationDate)), 
					('OperationType_EnumerationValue', 'BIGINT', 0, CONVERT(NVARCHAR(MAX), @operationType_EnumerationValue)), 
					('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId)), 
					('UserLoginId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @userLoginId)), 
					('TargetContextName', 'VARCHAR(50)', 1, @targetContextName), 
					('TargetEntityName', 'VARCHAR(100)', 1, @targetEntityName), 
					('IUDLogId', 'UNIQUEIDENTIFIER', 1, CONVERT(NVARCHAR(MAX), @iUDLogId)), 
					('FailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue)), 
					('AdditionalData', 'NVARCHAR(MAX)', 1, @additionalData);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'OperationLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgOperationLogs_InsteadOfInsert INTO @id, @operationDate, @operationType_EnumerationValue, @pageRequestId, @userLoginId, @targetContextName, @targetEntityName, @iUDLogId, @resultFailureReason_EnumerationValue, @additionalData;
		END
	CLOSE _cursor_trgOperationLogs_InsteadOfInsert;
	DEALLOCATE _cursor_trgOperationLogs_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgApiConsumers_InsteadOfDelete
ON ApiConsumers
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumers_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@name NVARCHAR(100),
		@privateKey NVARCHAR(300),
		@apiTypes_EnumerationValue INT,
		@isEnabled BIT;
	DECLARE _cursor_trgApiConsumers_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[Name],
				[PrivateKey],
				[Grants_EnumerationValue],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgApiConsumers_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfDelete INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM ApiConsumers WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('Name', 'NVARCHAR(100)', 0, CONVERT(NVARCHAR(MAX), @name), NULL), 
						('PrivateKey', 'NVARCHAR(300)', 0, CONVERT(NVARCHAR(MAX), @privateKey), NULL), 
						('Grants_EnumerationValue', 'INT', 0, CONVERT(NVARCHAR(MAX), @apiTypes_EnumerationValue), NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE ApiConsumers WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_ApiConsumers] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfDelete INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled;
		END
	CLOSE _cursor_trgApiConsumers_InsteadOfDelete;
	DEALLOCATE _cursor_trgApiConsumers_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumers_InsteadOfUpdate
ON ApiConsumers
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumers_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@name NVARCHAR(100),
		@privateKey NVARCHAR(300),
		@apiTypes_EnumerationValue INT,
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@name_ NVARCHAR(100),
		@privateKey_ NVARCHAR(300),
		@apiTypes_EnumerationValue_ INT,
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND D.[Name] = I.[Name] AND D.[PrivateKey] = I.[PrivateKey] AND D.[Grants_EnumerationValue] = I.[Grants_EnumerationValue]
		WHERE
			D.[IsEnabled] <> I.[IsEnabled];
	DECLARE _cursor_trgApiConsumers_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[Name],
				D.[PrivateKey],
				D.[Grants_EnumerationValue],
				D.[IsEnabled],
				I.[Id],
				I.[Name],
				I.[PrivateKey],
				I.[Grants_EnumerationValue],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgApiConsumers_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfUpdate INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled, @id_, @name_, @privateKey_, @apiTypes_EnumerationValue_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('Name', 'NVARCHAR(100)', 0, CONVERT(NVARCHAR(MAX), @name), CONVERT(NVARCHAR(MAX), @name_)), 
						('PrivateKey', 'NVARCHAR(300)', 0, CONVERT(NVARCHAR(MAX), @privateKey), CONVERT(NVARCHAR(MAX), @privateKey_)), 
						('Grants_EnumerationValue', 'INT', 0, CONVERT(NVARCHAR(MAX), @apiTypes_EnumerationValue), CONVERT(NVARCHAR(MAX), @apiTypes_EnumerationValue_)), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_));
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfUpdate INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled, @id_, @name_, @privateKey_, @apiTypes_EnumerationValue_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfUpdate INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled, @id_, @name_, @privateKey_, @apiTypes_EnumerationValue_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM ApiConsumers WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE ApiConsumers
									SET
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfUpdate INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled, @id_, @name_, @privateKey_, @apiTypes_EnumerationValue_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgApiConsumers_InsteadOfUpdate;
	DEALLOCATE _cursor_trgApiConsumers_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumers_InsteadOfInsert
ON ApiConsumers
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumers_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@name NVARCHAR(100),
		@privateKey NVARCHAR(300),
		@apiTypes_EnumerationValue INT,
		@isEnabled BIT;
	DECLARE _cursor_trgApiConsumers_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[Name],
				[PrivateKey],
				[Grants_EnumerationValue],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgApiConsumers_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfInsert INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @encrypted NVARCHAR(300);
			EXEC [dbo].[EncryptApiConsumerPrivateKey] @name, @privateKey, @encrypted OUTPUT;
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						DECLARE @id_ UNIQUEIDENTIFIER;
						EXEC [dbo].[GetApiConsumerId] @privateKey, @id_ OUTPUT;
						IF @id_ IS NOT NULL
							EXEC [dbo].[RaiseSpecificError] N'The same ''PrivateKey'' vale has already been existing.';
						ELSE
							BEGIN
								INSERT INTO ApiConsumers ([Id], [Name], [PrivateKey], [Grants_EnumerationValue], [IsEnabled])
									VALUES (@id, @name, @encrypted, @apiTypes_EnumerationValue, @isEnabled);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 1, 1, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('Name', 'NVARCHAR(100)', 0, CONVERT(NVARCHAR(MAX), @name)), 
					('PrivateKey', 'NVARCHAR(300)', 0, CONVERT(NVARCHAR(MAX), @encrypted)), 
					('Grants_EnumerationValue', 'INT', 0, CONVERT(NVARCHAR(MAX), @apiTypes_EnumerationValue)), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'ApiConsumers', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumers_InsteadOfInsert INTO @id, @name, @privateKey, @apiTypes_EnumerationValue, @isEnabled;
		END
	CLOSE _cursor_trgApiConsumers_InsteadOfInsert;
	DEALLOCATE _cursor_trgApiConsumers_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgApiConsumerIpAddresses_InsteadOfDelete
ON ApiConsumerIpAddresses
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerIpAddresses_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerId UNIQUEIDENTIFIER,
		@ipAddressV4 VARCHAR(15),
		@isEnabled BIT;
	DECLARE _cursor_trgApiConsumerIpAddresses_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[ApiConsumerId],
				[IpAddressV4],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgApiConsumerIpAddresses_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfDelete INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM ApiConsumerIpAddresses WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('ApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerId), NULL), 
						('IpAddressV4', 'VARCHAR(15)', 0, CONVERT(NVARCHAR(MAX), @ipAddressV4), NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE ApiConsumerIpAddresses WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_ApiConsumerIpAddresses] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfDelete INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled;
		END
	CLOSE _cursor_trgApiConsumerIpAddresses_InsteadOfDelete;
	DEALLOCATE _cursor_trgApiConsumerIpAddresses_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumerIpAddresses_InsteadOfUpdate
ON ApiConsumerIpAddresses
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerIpAddresses_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerId UNIQUEIDENTIFIER,
		@ipAddressV4 VARCHAR(15),
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@apiConsumerId_ UNIQUEIDENTIFIER,
		@ipAddressV4_ VARCHAR(15),
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND D.[ApiConsumerId] = I.[ApiConsumerId] AND D.[IpAddressV4] = I.[IpAddressV4]
		WHERE
			D.[IsEnabled] <> I.[IsEnabled];
	DECLARE _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[ApiConsumerId],
				D.[IpAddressV4],
				D.[IsEnabled],
				I.[Id],
				I.[ApiConsumerId],
				I.[IpAddressV4],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled, @id_, @apiConsumerId_, @ipAddressV4_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('ApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerId), CONVERT(NVARCHAR(MAX), @apiConsumerId_)), 
						('IpAddressV4', 'VARCHAR(15)', 0, CONVERT(NVARCHAR(MAX), @ipAddressV4), CONVERT(NVARCHAR(MAX), @ipAddressV4_)), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_));
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled, @id_, @apiConsumerId_, @ipAddressV4_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled, @id_, @apiConsumerId_, @ipAddressV4_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM ApiConsumerIpAddresses WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE ApiConsumerIpAddresses
									SET
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled, @id_, @apiConsumerId_, @ipAddressV4_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate;
	DEALLOCATE _cursor_trgApiConsumerIpAddresses_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumerIpAddresses_InsteadOfInsert
ON ApiConsumerIpAddresses
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerIpAddresses_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerId UNIQUEIDENTIFIER,
		@ipAddressV4 VARCHAR(15),
		@isEnabled BIT;
	DECLARE _cursor_trgApiConsumerIpAddresses_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[ApiConsumerId],
				[IpAddressV4],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgApiConsumerIpAddresses_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfInsert INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						INSERT INTO ApiConsumerIpAddresses ([Id], [ApiConsumerId], [IpAddressV4], [IsEnabled])
							VALUES (@id, @apiConsumerId, @ipAddressV4, @isEnabled);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 1, 1, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('ApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerId)), 
					('IpAddressV4', 'VARCHAR(15)', 0, CONVERT(NVARCHAR(MAX), @ipAddressV4)), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'ApiConsumerIpAddresses', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumerIpAddresses_InsteadOfInsert INTO @id, @apiConsumerId, @ipAddressV4, @isEnabled;
		END
	CLOSE _cursor_trgApiConsumerIpAddresses_InsteadOfInsert;
	DEALLOCATE _cursor_trgApiConsumerIpAddresses_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgApiConsumerMachines_InsteadOfDelete
ON ApiConsumerMachines
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerMachines_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerId UNIQUEIDENTIFIER,
		@isEnabled BIT;
	DECLARE _cursor_trgApiConsumerMachines_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[ApiConsumerId],
				[IsEnabled]
			FROM deleted;
	OPEN _cursor_trgApiConsumerMachines_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfDelete INTO @id, @apiConsumerId, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM ApiConsumerMachines WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('ApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerId), NULL), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE ApiConsumerMachines WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						IF @isEnabled = 1
							BEGIN
								BEGIN TRY
									EXEC [dbo].[Disable_ApiConsumerMachines] @id;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
						ELSE
							BEGIN
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfDelete INTO @id, @apiConsumerId, @isEnabled;
		END
	CLOSE _cursor_trgApiConsumerMachines_InsteadOfDelete;
	DEALLOCATE _cursor_trgApiConsumerMachines_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumerMachines_InsteadOfUpdate
ON ApiConsumerMachines
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerMachines_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@count INT,
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerId UNIQUEIDENTIFIER,
		@isEnabled BIT,
		@id_ UNIQUEIDENTIFIER,
		@apiConsumerId_ UNIQUEIDENTIFIER,
		@isEnabled_ BIT;
	SELECT
			@count = COUNT(*)
		FROM inserted I
		INNER JOIN deleted D
			ON D.[Id] = I.[Id] AND D.[ApiConsumerId] = I.[ApiConsumerId]
		WHERE
			D.[IsEnabled] <> I.[IsEnabled];
	DECLARE _cursor_trgApiConsumerMachines_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[ApiConsumerId],
				D.[IsEnabled],
				I.[Id],
				I.[ApiConsumerId],
				I.[IsEnabled]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	OPEN _cursor_trgApiConsumerMachines_InsteadOfUpdate;
	IF @count <> (SELECT COUNT(*) FROM deleted)
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseUpdatingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfUpdate INTO @id, @apiConsumerId, @isEnabled, @id_, @apiConsumerId_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
						('ApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerId), CONVERT(NVARCHAR(MAX), @apiConsumerId_)), 
						('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled), CONVERT(NVARCHAR(MAX), @isEnabled_));
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 2, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfUpdate INTO @id, @apiConsumerId, @isEnabled, @id_, @apiConsumerId_, @isEnabled_;
				END
		END
	ELSE
		BEGIN
			FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfUpdate INTO @id, @apiConsumerId, @isEnabled, @id_, @apiConsumerId_, @isEnabled_;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					IF NOT EXISTS(SELECT 1 FROM ApiConsumerMachines WHERE [Id] = @id)
						BEGIN
							BEGIN TRY
								EXEC [dbo].[RaiseUpdatingError];
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
							BEGIN TRY
								IF @isEnabled = 0
									EXEC [dbo].[Insert_IUDLogs] 3, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
								ELSE
									EXEC [dbo].[Insert_IUDLogs] 4, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
							END TRY
							BEGIN CATCH
								INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
							END CATCH
						END
					ELSE
						BEGIN
							BEGIN TRANSACTION;
							BEGIN TRY
								UPDATE ApiConsumerMachines
									SET
										[IsEnabled] = @isEnabled_
									WHERE [Id] = @id;
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 1, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 1, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
									IF @@TRANCOUNT > 0
										COMMIT TRANSACTION;
								END TRY
								BEGIN CATCH
									IF @@TRANCOUNT > 0
										ROLLBACK TRANSACTION;
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END TRY
							BEGIN CATCH
								IF @@TRANCOUNT > 0
									ROLLBACK TRANSACTION;
								SET @error = ERROR_MESSAGE();
								INSERT INTO @errorMessages VALUES (@error);
								BEGIN TRY
									IF @isEnabled = 0
										EXEC [dbo].[Insert_IUDLogs] 3, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
									ELSE
										EXEC [dbo].[Insert_IUDLogs] 4, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
								END TRY
								BEGIN CATCH
									INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
								END CATCH
							END CATCH
						END
					FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfUpdate INTO @id, @apiConsumerId, @isEnabled, @id_, @apiConsumerId_, @isEnabled_;
				END
		END
	CLOSE _cursor_trgApiConsumerMachines_InsteadOfUpdate;
	DEALLOCATE _cursor_trgApiConsumerMachines_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumerMachines_InsteadOfInsert
ON ApiConsumerMachines
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerMachines_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerId UNIQUEIDENTIFIER,
		@isEnabled BIT;
	DECLARE _cursor_trgApiConsumerMachines_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[ApiConsumerId],
				[IsEnabled]
			FROM inserted;
	OPEN _cursor_trgApiConsumerMachines_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfInsert INTO @id, @apiConsumerId, @isEnabled;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @isEnabled = 0
					EXEC [dbo].[RaiseInsertingError];
				ELSE
					BEGIN
						INSERT INTO ApiConsumerMachines ([Id], [ApiConsumerId], [IsEnabled])
							VALUES (@id, @apiConsumerId, @isEnabled);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 1, 1, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('ApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerId)), 
					('IsEnabled', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isEnabled));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'ApiConsumerMachines', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumerMachines_InsteadOfInsert INTO @id, @apiConsumerId, @isEnabled;
		END
	CLOSE _cursor_trgApiConsumerMachines_InsteadOfInsert;
	DEALLOCATE _cursor_trgApiConsumerMachines_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgApiConsumerMachineVariables_InsteadOfDelete
ON ApiConsumerMachineVariables
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerMachineVariables_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerMachineId UNIQUEIDENTIFIER,
		@name NVARCHAR(300),
		@value NVARCHAR(MAX);
	DECLARE _cursor_trgApiConsumerMachineVariables_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[ApiConsumerMachineId],
				[Name],
				[Value]
			FROM deleted;
	OPEN _cursor_trgApiConsumerMachineVariables_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgApiConsumerMachineVariables_InsteadOfDelete INTO @id, @apiConsumerMachineId, @name, @value;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM ApiConsumerMachineVariables WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumerMachineVariables', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumerMachineVariables', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('ApiConsumerMachineId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerMachineId), NULL), 
						('Name', 'NVARCHAR(300)', 0, CONVERT(NVARCHAR(MAX), @name), NULL), 
						('Value', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @value), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE ApiConsumerMachineVariables WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'ApiConsumerMachineVariables', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiConsumerMachineVariables', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgApiConsumerMachineVariables_InsteadOfDelete INTO @id, @apiConsumerMachineId, @name, @value;
		END
	CLOSE _cursor_trgApiConsumerMachineVariables_InsteadOfDelete;
	DEALLOCATE _cursor_trgApiConsumerMachineVariables_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumerMachineVariables_InsteadOfUpdate
ON ApiConsumerMachineVariables
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerMachineVariables_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerMachineId UNIQUEIDENTIFIER,
		@name NVARCHAR(300),
		@value NVARCHAR(MAX),
		@id_ UNIQUEIDENTIFIER,
		@apiConsumerMachineId_ UNIQUEIDENTIFIER,
		@name_ NVARCHAR(300),
		@value_ NVARCHAR(MAX);
	DECLARE _cursor_trgApiConsumerMachineVariables_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[ApiConsumerMachineId],
				D.[Name],
				D.[Value],
				I.[Id],
				I.[ApiConsumerMachineId],
				I.[Name],
				I.[Value]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgApiConsumerMachineVariables_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgApiConsumerMachineVariables_InsteadOfUpdate INTO @id, @apiConsumerMachineId, @name, @value, @id_, @apiConsumerMachineId_, @name_, @value_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('ApiConsumerMachineId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerMachineId), CONVERT(NVARCHAR(MAX), @apiConsumerMachineId_)), 
				('Name', 'NVARCHAR(300)', 0, CONVERT(NVARCHAR(MAX), @name), CONVERT(NVARCHAR(MAX), @name_)), 
				('Value', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @value), CONVERT(NVARCHAR(MAX), @value_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'ApiConsumerMachineVariables', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumerMachineVariables_InsteadOfUpdate INTO @id, @apiConsumerMachineId, @name, @value, @id_, @apiConsumerMachineId_, @name_, @value_;
		END
	CLOSE _cursor_trgApiConsumerMachineVariables_InsteadOfUpdate;
	DEALLOCATE _cursor_trgApiConsumerMachineVariables_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiConsumerMachineVariables_InsteadOfInsert
ON ApiConsumerMachineVariables
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiConsumerMachineVariables_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@apiConsumerMachineId UNIQUEIDENTIFIER,
		@name NVARCHAR(300),
		@value NVARCHAR(MAX);
	DECLARE _cursor_trgApiConsumerMachineVariables_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[ApiConsumerMachineId],
				[Name],
				[Value]
			FROM inserted;
	OPEN _cursor_trgApiConsumerMachineVariables_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgApiConsumerMachineVariables_InsteadOfInsert INTO @id, @apiConsumerMachineId, @name, @value;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO ApiConsumerMachineVariables ([Id], [ApiConsumerMachineId], [Name], [Value])
					VALUES (@id, @apiConsumerMachineId, @name, @value);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 1, 'ApiConsumerMachineVariables', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values_, @logId OUTPUT;
					IF @@TRANCOUNT > 0
						COMMIT TRANSACTION;
				END TRY
				BEGIN CATCH
					IF @@TRANCOUNT > 0
						ROLLBACK TRANSACTION;
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('ApiConsumerMachineId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @apiConsumerMachineId)), 
					('Name', 'NVARCHAR(300)', 0, CONVERT(NVARCHAR(MAX), @name)), 
					('Value', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @value));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'ApiConsumerMachineVariables', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgApiConsumerMachineVariables_InsteadOfInsert INTO @id, @apiConsumerMachineId, @name, @value;
		END
	CLOSE _cursor_trgApiConsumerMachineVariables_InsteadOfInsert;
	DEALLOCATE _cursor_trgApiConsumerMachineVariables_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgApiRequests_InsteadOfDelete
ON ApiRequests
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiRequests_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@requestDate DATETIMEOFFSET,
		@randomToken NVARCHAR(MAX),
		@privateKey NVARCHAR(MAX),
		@machineVariablesJson NVARCHAR(MAX),
		@resultApiConsumerId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT;
	DECLARE _cursor_trgApiRequests_InsteadOfDelete CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[RequestDate],
				[RandomToken],
				[PrivateKey],
				[MachineVariablesJson],
				[ResultApiConsumerId],
				[ResultFailureReason_EnumerationValue]
			FROM deleted;
	OPEN _cursor_trgApiRequests_InsteadOfDelete;
	FETCH NEXT FROM _cursor_trgApiRequests_InsteadOfDelete INTO @id, @pageRequestId, @requestDate, @randomToken, @privateKey, @machineVariablesJson, @resultApiConsumerId, @resultFailureReason_EnumerationValue;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM ApiRequests WHERE [Id] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiRequests', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was not found.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE IF 1 = 0 --EXISTS(SELECT 1 FROM {{tblFKContainer}} WHERE [{{colFK}}] = @id)
				BEGIN
					BEGIN TRY
						EXEC [dbo].[RaiseDeletingError];
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiRequests', 'Id', 'UNIQUEIDENTIFIER', @id, N'Specified PK-value was referenced by another table''s record.', @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
					END CATCH
				END
			ELSE
				BEGIN
					DECLARE @values [dbo].[typeIUDLogValue];
					INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
						('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL), 
						('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), NULL), 
						('RequestDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @requestDate), NULL), 
						('RandomToken', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @randomToken), NULL), 
						('PrivateKey', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @privateKey), NULL), 
						('MachineVariablesJson', 'NVARCHAR(MAX)', 0, CONVERT(NVARCHAR(MAX), @machineVariablesJson), NULL), 
						('ResultApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @resultApiConsumerId), NULL), 
						('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), NULL);
					BEGIN TRANSACTION;
					BEGIN TRY
						DELETE ApiRequests WHERE [Id] = @id;
						BEGIN TRY										
							EXEC [dbo].[Insert_IUDLogs] 5, 1, 'ApiRequests', 'Id', 'UNIQUEIDENTIFIER', @id, NULL, @values, @logId OUTPUT;
							IF @@TRANCOUNT > 0
								COMMIT TRANSACTION;
						END TRY
						BEGIN CATCH
							IF @@TRANCOUNT > 0
								ROLLBACK TRANSACTION;
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END TRY
					BEGIN CATCH
						IF @@TRANCOUNT > 0
							ROLLBACK TRANSACTION;
						SET @error = ERROR_MESSAGE();
						INSERT INTO @errorMessages VALUES (@error);
						BEGIN TRY
							EXEC [dbo].[Insert_IUDLogs] 5, 0, 'ApiRequests', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
						END TRY
						BEGIN CATCH
							INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						END CATCH
					END CATCH
				END
			FETCH NEXT FROM _cursor_trgApiRequests_InsteadOfDelete INTO @id, @pageRequestId, @requestDate, @randomToken, @privateKey, @machineVariablesJson, @resultApiConsumerId, @resultFailureReason_EnumerationValue;
		END
	CLOSE _cursor_trgApiRequests_InsteadOfDelete;
	DEALLOCATE _cursor_trgApiRequests_InsteadOfDelete;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiRequests_InsteadOfUpdate
ON ApiRequests
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiRequests_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@requestDate DATETIMEOFFSET,
		@randomToken NVARCHAR(MAX),
		@privateKey NVARCHAR(MAX),
		@machineVariablesJson NVARCHAR(MAX),
		@resultApiConsumerId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT,
		@id_ UNIQUEIDENTIFIER,
		@pageRequestId_ UNIQUEIDENTIFIER,
		@requestDate_ DATETIMEOFFSET,
		@randomToken_ NVARCHAR(MAX),
		@privateKey_ NVARCHAR(MAX),
		@machineVariablesJson_ NVARCHAR(MAX),
		@resultApiConsumerId_ UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue_ INT;
	DECLARE _cursor_trgApiRequests_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[PageRequestId],
				D.[RequestDate],
				D.[RandomToken],
				D.[PrivateKey],
				D.[MachineVariablesJson],
				D.[ResultApiConsumerId],
				D.[ResultFailureReason_EnumerationValue],
				I.[Id],
				I.[PageRequestId],
				I.[RequestDate],
				I.[RandomToken],
				I.[PrivateKey],
				I.[MachineVariablesJson],
				I.[ResultApiConsumerId],
				I.[ResultFailureReason_EnumerationValue]
			FROM deleted D
			FULL OUTER JOIN inserted I
				ON D.[Id] = I.[Id];
	BEGIN TRY
		EXEC [dbo].[RaiseUpdatingError];
	END TRY
	BEGIN CATCH
		SET @error = ERROR_MESSAGE();
		INSERT INTO @errorMessages VALUES (@error);
	END CATCH
	OPEN _cursor_trgApiRequests_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgApiRequests_InsteadOfUpdate INTO @id, @pageRequestId, @requestDate, @randomToken, @privateKey, @machineVariablesJson, @resultApiConsumerId, @resultFailureReason_EnumerationValue, @id_, @pageRequestId_, @requestDate_, @randomToken_, @privateKey_, @machineVariablesJson_, @resultApiConsumerId_, @resultFailureReason_EnumerationValue_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('PageRequestId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @pageRequestId), CONVERT(NVARCHAR(MAX), @pageRequestId_)), 
				('RequestDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @requestDate), CONVERT(NVARCHAR(MAX), @requestDate_)), 
				('RandomToken', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @randomToken), CONVERT(NVARCHAR(MAX), @randomToken_)), 
				('PrivateKey', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @privateKey), CONVERT(NVARCHAR(MAX), @privateKey_)), 
				('MachineVariablesJson', 'NVARCHAR(MAX)', 0, CONVERT(NVARCHAR(MAX), @machineVariablesJson), CONVERT(NVARCHAR(MAX), @machineVariablesJson_)), 
				('ResultApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @resultApiConsumerId), CONVERT(NVARCHAR(MAX), @resultApiConsumerId_)), 
				('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue), CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'ApiRequests', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgApiRequests_InsteadOfUpdate INTO @id, @pageRequestId, @requestDate, @randomToken, @privateKey, @machineVariablesJson, @resultApiConsumerId, @resultFailureReason_EnumerationValue, @id_, @pageRequestId_, @requestDate_, @randomToken_, @privateKey_, @machineVariablesJson_, @resultApiConsumerId_, @resultFailureReason_EnumerationValue_;
		END
	CLOSE _cursor_trgApiRequests_InsteadOfUpdate;
	DEALLOCATE _cursor_trgApiRequests_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgApiRequests_InsteadOfInsert
ON ApiRequests
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgApiRequests_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@pageRequestId UNIQUEIDENTIFIER,
		@requestDate DATETIMEOFFSET,
		@randomToken NVARCHAR(MAX),
		@privateKey NVARCHAR(MAX),
		@machineVariablesJson NVARCHAR(MAX),
		@resultApiConsumerId UNIQUEIDENTIFIER,
		@resultFailureReason_EnumerationValue INT;
	DECLARE _cursor_trgApiRequests_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[PageRequestId],
				[RequestDate],
				[RandomToken],
				[PrivateKey],
				[MachineVariablesJson],
				[ResultApiConsumerId],
				[ResultFailureReason_EnumerationValue]
			FROM inserted;
	OPEN _cursor_trgApiRequests_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgApiRequests_InsteadOfInsert INTO @id, @pageRequestId, @requestDate, @randomToken, @privateKey, @machineVariablesJson, @resultApiConsumerId, @resultFailureReason_EnumerationValue;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @strPageRequestId NVARCHAR(MAX) = CONVERT(NVARCHAR(MAX), @pageRequestId);
			DECLARE @encrypted NVARCHAR(300) = NULL;
			IF @privateKey IS NOT NULL
				EXEC [dbo].[EncryptApiConsumerPrivateKey] @strPageRequestId, @privateKey, @encrypted OUTPUT;			
			BEGIN TRANSACTION;
			BEGIN TRY
				IF @resultApiConsumerId IS NOT NULL
					EXEC [dbo].[RaiseSpecificError] N'''ResultApiConsumerId'' was not null on insert.';
				IF @resultFailureReason_EnumerationValue IS NOT NULL
					EXEC [dbo].[RaiseSpecificError] N'''ResultFailureReason_EnumerationValue'' was not null on insert.';			
				DECLARE @prFailure INT;
				DECLARE @ipAddress VARCHAR(15);
				SELECT
						@prFailure = t1.[FailureReason_EnumerationValue],
						@ipAddress = t2.[IpAddressV4]
					FROM [PageRequests] t1
					INNER JOIN [ClientIpAddresses] t2 ON
						t2.[Id] = t1.[ClientIpAddressId]
					WHERE
						t1.[Id] = @pageRequestId
				DECLARE @failure INT = 0;
				IF EXISTS(SELECT 1 FROM [ApiRequests] WHERE [PageRequestId] = @pageRequestId)
					SET @failure = @failure | 256; -- Used Page Request
				IF @prFailure IS NOT NULL
					SET @failure = @failure | 1; -- Page Request Failure
				IF @randomToken IS NULL
					SET @failure = @failure | 2; -- No Random Token
				ELSE IF EXISTS(SELECT 1 FROM [ApiRequests] WHERE [RandomToken] = @randomToken)
					SET @failure = @failure | 4; -- Unexpected Random Token
				IF @privateKey IS NULL
					SET @failure = @failure | 8; -- No Private Key
				ELSE
					BEGIN
						EXEC [dbo].[GetApiConsumerId] @privateKey, @resultApiConsumerId OUTPUT;
						IF @resultApiConsumerId IS NULL
							SET @failure = @failure | 16; -- Incorrect Private Key
					END
				IF @resultApiConsumerId IS NOT NULL 
					BEGIN
						IF EXISTS(SELECT 1 FROM [ApiConsumers] WHERE [Id] = @resultApiConsumerId AND [IsEnabled] = 0)
							SET @failure = @failure | 128; -- Disabled API Consumer
						IF
							EXISTS(SELECT 1 FROM [ApiConsumerIpAddresses] WHERE [ApiConsumerId] = @resultApiConsumerId AND [IsEnabled] = 1) AND
							NOT EXISTS(SELECT 1 FROM [ApiConsumerIpAddresses] WHERE [ApiConsumerId] = @resultApiConsumerId AND [IpAddressV4] = @ipAddress AND [IsEnabled] = 1)
							SET @failure = @failure | 32; -- Unexpected IP Address
						IF EXISTS(SELECT 1 FROM [ApiConsumerMachines] WHERE [ApiConsumerId] = @resultApiConsumerId AND [IsEnabled] = 1)
							BEGIN
								DECLARE @machineFound BIT = 0;
								IF @machineVariablesJson IS NOT NULL AND ISJSON(@machineVariablesJson) > 0
									BEGIN
										DECLARE @machineId UNIQUEIDENTIFIER;
										DECLARE @variableName NVARCHAR(300);
										DECLARE @variableValue NVARCHAR(MAX);
										DECLARE _cursor_ApiConsumerMachines CURSOR FOR
											SELECT
													[Id]
												FROM [ApiConsumerMachines]
												WHERE
													[ApiConsumerId] = @resultApiConsumerId AND
													[IsEnabled] = 1;
										OPEN _cursor_ApiConsumerMachines
										FETCH NEXT FROM _cursor_ApiConsumerMachines INTO @machineId;
										WHILE @@FETCH_STATUS = 0
											BEGIN
												DECLARE @allValuesMatched BIT = 1;
												DECLARE _cursor_ApiConsumerMachineVariables CURSOR FOR
													SELECT
															[Name],
															[Value]
														FROM [ApiConsumerMachineVariables]
														WHERE
															[ApiConsumerMachineId] = @machineId;
												OPEN _cursor_ApiConsumerMachineVariables;
												FETCH NEXT FROM _cursor_ApiConsumerMachineVariables INTO @variableName, @variableValue;
												WHILE @@FETCH_STATUS = 0
													BEGIN
														DECLARE @jsonValue NVARCHAR(MAX) = JSON_VALUE(@machineVariablesJson, N'$."' + @variableName + '"');
														IF NOT ((@variableValue IS NULL AND @jsonValue IS NULL) OR (@variableValue IS NOT NULL AND @jsonValue IS NOT NULL AND @variableValue = @jsonValue))
															BEGIN
																SET @allValuesMatched = 0;
																BREAK;
															END
														FETCH NEXT FROM _cursor_ApiConsumerMachineVariables INTO @variableName, @variableValue;
													END
												CLOSE _cursor_ApiConsumerMachineVariables;
												DEALLOCATE _cursor_ApiConsumerMachineVariables;
												IF @allValuesMatched = 1
													BEGIN
														SET @machineFound = 1;
														BREAK;
													END
												FETCH NEXT FROM _cursor_ApiConsumerMachines INTO @machineId;
											END
										CLOSE _cursor_ApiConsumerMachines;
										DEALLOCATE _cursor_ApiConsumerMachines;
										IF @machineFound = 0
											SET @failure = @failure | 64; -- Unexpected Machine Variables
									END
							END
					END
				IF @failure <> 0
					SET @resultFailureReason_EnumerationValue = @failure;
				INSERT INTO ApiRequests ([Id], [PageRequestId], [RequestDate], [RandomToken], [PrivateKey], [MachineVariablesJson], [ResultApiConsumerId], [ResultFailureReason_EnumerationValue])
					VALUES (@id, @pageRequestId, @requestDate, @randomToken, @encrypted, @machineVariablesJson, @resultApiConsumerId, @resultFailureReason_EnumerationValue);
				IF @@TRANCOUNT > 0
					COMMIT TRANSACTION;
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				DECLARE @values [dbo].[typeIUDLogValue];
				INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [NewValue_String]) VALUES
					('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id)), 
					('PageRequestId', 'UNIQUEIDENTIFIER', 0, @strPageRequestId), 
					('RequestDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @requestDate)), 
					('RandomToken', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @randomToken)), 
					('PrivateKey', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @encrypted)), 
					('MachineVariablesJson', 'NVARCHAR(MAX)', 0, CONVERT(NVARCHAR(MAX), @machineVariablesJson)), 
					('ResultApiConsumerId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @resultApiConsumerId)), 
					('ResultFailureReason_EnumerationValue', 'INT', 1, CONVERT(NVARCHAR(MAX), @resultFailureReason_EnumerationValue));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'ApiRequests', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgApiRequests_InsteadOfInsert INTO @id, @pageRequestId, @requestDate, @randomToken, @privateKey, @machineVariablesJson, @resultApiConsumerId, @resultFailureReason_EnumerationValue;
		END
	CLOSE _cursor_trgApiRequests_InsteadOfInsert;
	DEALLOCATE _cursor_trgApiRequests_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO