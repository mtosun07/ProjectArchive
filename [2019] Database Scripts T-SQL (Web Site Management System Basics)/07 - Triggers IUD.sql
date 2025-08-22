USE WebSiteBaseDB
GO



CREATE TRIGGER trgIUDLogs_InsteadOfDelete
ON IUDLogs
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgIUDLogs_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@logId_ UNIQUEIDENTIFIER = NULL,
		@id UNIQUEIDENTIFIER,
		@operationType_EnumerationValue TINYINT,
		@isSuccess BIT,
		@tableName VARCHAR(128),
		@additionalData NVARCHAR(MAX),
		@operationDate DATETIMEOFFSET;
	SELECT TOP(1)
			@logId = [Id],
			@operationType_EnumerationValue = [OperationType_EnumerationValue],
			@isSuccess = [IsSuccess],
			@tableName = [TableName],
			@logId_ = CAST([PrimaryKeyValue] AS UNIQUEIDENTIFIER),
			@additionalData = UPPER([AdditionalData]),
			@operationDate = [OperationDate]
		FROM IUDLogs
		ORDER BY
			[OperationDate] DESC;
	IF @operationType_EnumerationValue = 0 AND @isSuccess = 1 AND @tableName IS NULL AND @additionalData LIKE '%EXPORTED AS XML%'
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				DELETE FROM IUDLogValues;
				SELECT TOP(1)
						@logId = [Id],
						@operationType_EnumerationValue = [OperationType_EnumerationValue],
						@isSuccess = [IsSuccess],
						@tableName = [TableName],
						@logId_ = CAST([PrimaryKeyValue] AS UNIQUEIDENTIFIER),
						@additionalData = UPPER([AdditionalData])
					FROM IUDLogs
					ORDER BY
						[OperationDate] DESC;
				IF @operationType_EnumerationValue = 0 AND @isSuccess = 1 AND @tableName = 'IUDLogValues' AND @logId_ IS NOT NULL AND @additionalData LIKE '%CLEANED UP%' AND EXISTS(SELECT 1 FROM IUDLogs WHERE [Id] = @logId_)
					BEGIN
						DELETE FROM IUDLogs WHERE [Id] <> @logId AND [Id] <> @logId_ AND [OperationDate] < @operationDate;
						INSERT INTO IUDLogs ([OperationType_EnumerationValue], [IsSuccess], [TableName], [AdditionalData])
							VALUES (0, 1, 'IUDLogs', CONCAT(N'Table was cleaned up. (WHERE: [OperationDate] <', CONVERT(NVARCHAR, @operationDate), N')'));
						IF @@TRANCOUNT > 0
							COMMIT TRANSACTION;
					END
				ELSE
					EXEC [dbo].[RaiseDeletingError];
			END TRY
			BEGIN CATCH
				IF @@TRANCOUNT > 0
					ROLLBACK TRANSACTION;
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 0, 0, 'IUDLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
		END
	ELSE
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseDeletingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			DECLARE _cursor_trgIUDLogs_InsteadOfDelete CURSOR FOR
				SELECT [Id] FROM deleted;
			OPEN _cursor_trgIUDLogs_InsteadOfDelete;
			FETCH NEXT FROM _cursor_trgIUDLogs_InsteadOfDelete INTO @id;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'IUDLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						BREAK;
					END CATCH
					FETCH NEXT FROM _cursor_trgIUDLogs_InsteadOfDelete INTO @id;
				END
			CLOSE _cursor_trgIUDLogs_InsteadOfDelete;
			DEALLOCATE _cursor_trgIUDLogs_InsteadOfDelete;
		END
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgIUDLogs_InsteadOfUpdate
ON IUDLogs
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgIUDLogs_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@operationType_EnumerationValue TINYINT,
		@isSuccess BIT,
		@tableName VARCHAR(128),
		@primaryKeyColumnName VARCHAR(128),
		@primaryKeyDataType VARCHAR(128),
		@primaryKeyValue NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@operationDate DATETIMEOFFSET,
		@id_ UNIQUEIDENTIFIER,
		@operationType_EnumerationValue_ TINYINT,
		@isSuccess_ BIT,
		@tableName_ VARCHAR(128),
		@primaryKeyColumnName_ VARCHAR(128),
		@primaryKeyDataType_ VARCHAR(128),
		@primaryKeyValue_ NVARCHAR(MAX),
		@additionalData_ NVARCHAR(MAX),
		@operationDate_ DATETIMEOFFSET;
	DECLARE _cursor_trgIUDLogs_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[OperationType_EnumerationValue],
				D.[IsSuccess],
				D.[TableName],
				D.[PrimaryKeyColumnName],
				D.[PrimaryKeyDataType],
				D.[PrimaryKeyValue],
				D.[AdditionalData],
				D.[OperationDate],
				I.[Id],
				I.[OperationType_EnumerationValue],
				I.[IsSuccess],
				I.[TableName],
				I.[PrimaryKeyColumnName],
				I.[PrimaryKeyDataType],
				I.[PrimaryKeyValue],
				I.[AdditionalData],
				I.[OperationDate]
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
	OPEN _cursor_trgIUDLogs_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgIUDLogs_InsteadOfUpdate INTO @id, @operationType_EnumerationValue, @isSuccess, @tableName, @primaryKeyColumnName, @primaryKeyDataType, @primaryKeyValue, @additionalData, @operationDate, @id_, @operationType_EnumerationValue_, @isSuccess_, @tableName_, @primaryKeyColumnName_, @primaryKeyDataType_, @primaryKeyValue_, @additionalData_, @operationDate_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [NewValue_String]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), CONVERT(NVARCHAR(MAX), @id_)), 
				('OperationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @operationType_EnumerationValue), CONVERT(NVARCHAR(MAX), @operationType_EnumerationValue_)), 
				('IsSuccess', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isSuccess), CONVERT(NVARCHAR(MAX), @isSuccess_)), 
				('TableName', 'VARCHAR(128)', 1, CONVERT(NVARCHAR(MAX), @tableName), CONVERT(NVARCHAR(MAX), @tableName_)), 
				('PrimaryKeyColumnName', 'VARCHAR(128)', 1, CONVERT(NVARCHAR(MAX), @primaryKeyColumnName), CONVERT(NVARCHAR(MAX), @primaryKeyColumnName_)), 
				('PrimaryKeyDataType', 'VARCHAR(128)', 1, CONVERT(NVARCHAR(MAX), @primaryKeyDataType), CONVERT(NVARCHAR(MAX), @primaryKeyDataType_)), 
				('PrimaryKeyValue', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @primaryKeyValue), CONVERT(NVARCHAR(MAX), @primaryKeyValue_)),
				('AdditionalData', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @additionalData), CONVERT(NVARCHAR(MAX), @additionalData_)),
				('OperationDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @operationDate), CONVERT(NVARCHAR(MAX), @operationDate_));
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'IUDLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgIUDLogs_InsteadOfUpdate INTO @id, @operationType_EnumerationValue, @isSuccess, @tableName, @primaryKeyColumnName, @primaryKeyDataType, @primaryKeyValue, @additionalData, @operationDate, @id_, @operationType_EnumerationValue_, @isSuccess_, @tableName_, @primaryKeyColumnName_, @primaryKeyDataType_, @primaryKeyValue_, @additionalData_, @operationDate_;
		END
	CLOSE _cursor_trgIUDLogs_InsteadOfUpdate;
	DEALLOCATE _cursor_trgIUDLogs_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgIUDLogs_InsteadOfInsert
ON IUDLogs
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgIUDLogs_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@operationType_EnumerationValue TINYINT,
		@isSuccess BIT,
		@tableName VARCHAR(128),
		@primaryKeyColumnName VARCHAR(128),
		@primaryKeyDataType VARCHAR(128),
		@primaryKeyValue NVARCHAR(MAX),
		@additionalData NVARCHAR(MAX),
		@operationDate DATETIMEOFFSET;
	DECLARE _cursor_trgIUDLogs_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[OperationType_EnumerationValue],
				[IsSuccess],
				[TableName],
				[PrimaryKeyColumnName],
				[PrimaryKeyDataType],
				[PrimaryKeyValue],
				[AdditionalData],
				[OperationDate]
			FROM inserted;
	OPEN _cursor_trgIUDLogs_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgIUDLogs_InsteadOfInsert INTO @id, @operationType_EnumerationValue, @isSuccess, @tableName, @primaryKeyColumnName, @primaryKeyDataType, @primaryKeyValue, @additionalData, @operationDate;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO IUDLogs ([Id], [OperationType_EnumerationValue], [IsSuccess], [TableName], [PrimaryKeyColumnName], [PrimaryKeyDataType], [PrimaryKeyValue], [AdditionalData], [OperationDate])
					VALUES (@id, @operationType_EnumerationValue, @isSuccess, @tableName, @primaryKeyColumnName, @primaryKeyDataType, @primaryKeyValue, @additionalData, @operationDate);
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
					('OperationType_EnumerationValue', 'TINYINT', 0, CONVERT(NVARCHAR(MAX), @operationType_EnumerationValue)), 
					('IsSuccess', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isSuccess)), 
					('TableName', 'VARCHAR(128)', 1, CONVERT(NVARCHAR(MAX), @tableName)), 
					('PrimaryKeyColumnName', 'VARCHAR(128)', 1, CONVERT(NVARCHAR(MAX), @primaryKeyColumnName)), 
					('PrimaryKeyDataType', 'VARCHAR(128)', 1, CONVERT(NVARCHAR(MAX), @primaryKeyDataType)), 
					('PrimaryKeyValue', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @primaryKeyValue)),
					('AdditionalData', 'NVARCHAR(MAX)', 1, CONVERT(NVARCHAR(MAX), @additionalData)),
					('OperationDate', 'DATETIMEOFFSET', 0, CONVERT(NVARCHAR(MAX), @operationDate));
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'IUDLogs', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgIUDLogs_InsteadOfInsert INTO @id, @operationType_EnumerationValue, @isSuccess, @tableName, @primaryKeyColumnName, @primaryKeyDataType, @primaryKeyValue, @additionalData, @operationDate;
		END
	CLOSE _cursor_trgIUDLogs_InsteadOfInsert;
	DEALLOCATE _cursor_trgIUDLogs_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO



CREATE TRIGGER trgIUDLogValues_InsteadOfDelete
ON IUDLogValues
INSTEAD OF DELETE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgIUDLogValues_InsteadOfDelete')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@case BIT = NULL,
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@operationType_EnumerationValue TINYINT,
		@isSuccess BIT,
		@additionalData NVARCHAR(MAX),
		@operationDate DATETIMEOFFSET;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgIUDLogs_InsteadOfDelete')) = 1
		BEGIN
			SELECT TOP(1)
					@logId = [Id],
					@operationType_EnumerationValue = [OperationType_EnumerationValue],
					@isSuccess = [IsSuccess],
					@additionalData = UPPER([AdditionalData]),
					@operationDate = [OperationDate]
				FROM IUDLogs
				ORDER BY
					[OperationDate] DESC;
			SET @case = CASE WHEN @operationType_EnumerationValue = 0 AND @isSuccess = 1 AND @additionalData LIKE '%EXPORTED AS XML%' THEN 1 ELSE 0 END;
			IF @case = 1
				BEGIN
					DELETE IUDLogValues
						FROM IUDLogValues t1
						INNER JOIN IUDLogs t2
							ON t2.[Id] = t1.[IUDLogId]
						WHERE
							t2.[OperationDate] < @operationDate;
					INSERT INTO IUDLogs ([OperationType_EnumerationValue], [IsSuccess], [TableName], [PrimaryKeyValue], [AdditionalData])
						VALUES (0, 1, 'IUDLogValues', @logId, CONCAT(N'Table was cleaned up. (WHERE: [OperationDate] <', CONVERT(NVARCHAR, @operationDate), N')'));
				END
		END
	IF @case <> 1
		BEGIN
			BEGIN TRY
				EXEC [dbo].[RaiseDeletingError];
			END TRY
			BEGIN CATCH
				SET @error = ERROR_MESSAGE();
				INSERT INTO @errorMessages VALUES (@error);
			END CATCH
			DECLARE _cursor_trgIUDLogValues_InsteadOfDelete CURSOR FOR
				SELECT [Id] FROM deleted;
			OPEN _cursor_trgIUDLogValues_InsteadOfDelete;
			FETCH NEXT FROM _cursor_trgIUDLogValues_InsteadOfDelete INTO @id;
			WHILE @@FETCH_STATUS = 0
				BEGIN
					BEGIN TRY
						EXEC [dbo].[Insert_IUDLogs] 5, 0, 'IUDLogValues', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values_, @logId OUTPUT;
					END TRY
					BEGIN CATCH
						INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
						BREAK;
					END CATCH
					FETCH NEXT FROM _cursor_trgIUDLogValues_InsteadOfDelete INTO @id;
				END
			CLOSE _cursor_trgIUDLogValues_InsteadOfDelete;
			DEALLOCATE _cursor_trgIUDLogValues_InsteadOfDelete;
		END
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgIUDLogValues_InsteadOfUpdate
ON IUDLogValues
INSTEAD OF UPDATE
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgIUDLogValues_InsteadOfUpdate')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@values_ [dbo].[typeIUDLogValue],
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@iUDLogId UNIQUEIDENTIFIER,
		@columnName VARCHAR(128),
		@columnDataType VARCHAR(128),
		@isNullable BIT,
		@oldValue_String NVARCHAR(MAX),
		@oldValue_Binary VARBINARY(MAX),
		@newValue_String NVARCHAR(MAX),
		@newValue_Binary VARBINARY(MAX),
		@id_ UNIQUEIDENTIFIER,
		@iUDLogId_ UNIQUEIDENTIFIER,
		@columnName_ VARCHAR(128),
		@columnDataType_ VARCHAR(128),
		@isNullable_ BIT,
		@oldValue_String_ NVARCHAR(MAX),
		@oldValue_Binary_ VARBINARY(MAX),
		@newValue_String_ NVARCHAR(MAX),
		@newValue_Binary_ VARBINARY(MAX);
	DECLARE _cursor_trgIUDLogValues_InsteadOfUpdate CURSOR FOR
		SELECT
				D.[Id],
				D.[IUDLogId],
				D.[ColumnName],
				D.[ColumnDataType],
				D.[IsNullable],
				D.[OldValue_String],
				D.[OldValue_Binary],
				D.[NewValue_String],
				D.[NewValue_Binary],
				I.[Id],
				I.[IUDLogId],
				I.[ColumnName],
				I.[ColumnDataType],
				I.[IsNullable],
				I.[OldValue_String],
				I.[OldValue_Binary],
				I.[NewValue_String],
				I.[NewValue_Binary]
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
	OPEN _cursor_trgIUDLogValues_InsteadOfUpdate;	
	FETCH NEXT FROM _cursor_trgIUDLogValues_InsteadOfUpdate INTO @id, @iUDLogId, @columnName, @columnDataType, @isNullable, @oldValue_String, @oldValue_Binary, @newValue_String, @newValue_Binary, @id_, @iUDLogId_, @columnName_, @columnDataType_, @isNullable_, @oldValue_String_, @oldValue_Binary_, @newValue_String_, @newValue_Binary_;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @values [dbo].[typeIUDLogValue];
			INSERT INTO @values ([ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [OldValue_Binary], [NewValue_String], [NewValue_Binary]) VALUES
				('Id', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @id), NULL, CONVERT(NVARCHAR(MAX), @id_), NULL), 
				('IUDLogId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @iUDLogId), NULL, CONVERT(NVARCHAR(MAX), @iUDLogId_), NULL), 
				('ColumnName', 'VARCHAR(128)', 0, CONVERT(NVARCHAR(MAX), @columnName), NULL, CONVERT(NVARCHAR(MAX), @columnName_), NULL), 
				('ColumnDataType', 'VARCHAR(128)', 0, CONVERT(NVARCHAR(MAX), @columnDataType), NULL, CONVERT(NVARCHAR(MAX), @columnDataType_), NULL), 
				('IsNullable', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isNullable), NULL, CONVERT(NVARCHAR(MAX), @isNullable_), NULL), 
				('OldValue_String', 'NVARCHAR(MAX)', 1, @oldValue_String, NULL, @oldValue_String_, NULL), 
				('OldValue_Binary', 'VARBINARY(MAX)', 1, NULL, @oldValue_Binary, NULL, @oldValue_Binary_), 
				('NewValue_String', 'NVARCHAR(MAX)', 1, @newValue_String, NULL, @newValue_String_, NULL), 
				('NewValue_Binary', 'VARBINARY(MAX)', 1, NULL, @newValue_Binary, NULL, @newValue_Binary_);
			BEGIN TRY
				EXEC [dbo].[Insert_IUDLogs] 2, 0, 'IUDLogValues', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
			END TRY
			BEGIN CATCH
				INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
			END CATCH
			FETCH NEXT FROM _cursor_trgIUDLogValues_InsteadOfUpdate INTO @id, @iUDLogId, @columnName, @columnDataType, @isNullable, @oldValue_String, @oldValue_Binary, @newValue_String, @newValue_Binary, @id_, @iUDLogId_, @columnName_, @columnDataType_, @isNullable_, @oldValue_String_, @oldValue_Binary_, @newValue_String_, @newValue_Binary_;
		END
	CLOSE _cursor_trgIUDLogValues_InsteadOfUpdate;
	DEALLOCATE _cursor_trgIUDLogValues_InsteadOfUpdate;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO

CREATE TRIGGER trgIUDLogValues_InsteadOfInsert
ON IUDLogValues
INSTEAD OF INSERT
AS
	SET NOCOUNT ON;
	IF TRIGGER_NESTLEVEL(OBJECT_ID('dbo.trgIUDLogValues_InsteadOfInsert')) > 1
		RETURN;
	DECLARE @errorMessages TABLE (ErrorMessage NVARCHAR(MAX));
	DECLARE
		@error NVARCHAR(MAX),
		@logId UNIQUEIDENTIFIER,
		@id UNIQUEIDENTIFIER,
		@iUDLogId UNIQUEIDENTIFIER,
		@columnName VARCHAR(128),
		@columnDataType VARCHAR(128),
		@isNullable BIT,
		@oldValue_String NVARCHAR(MAX),
		@oldValue_Binary VARBINARY(MAX),
		@newValue_String NVARCHAR(MAX),
		@newValue_Binary VARBINARY(MAX);
	DECLARE _cursor_trgIUDLogValues_InsteadOfInsert CURSOR FOR
		SELECT
				[Id],
				[IUDLogId],
				[ColumnName],
				[ColumnDataType],
				[IsNullable],
				[OldValue_String],
				[OldValue_Binary],
				[NewValue_String],
				[NewValue_Binary]
			FROM inserted;
	OPEN _cursor_trgIUDLogValues_InsteadOfInsert;
	FETCH NEXT FROM _cursor_trgIUDLogValues_InsteadOfInsert INTO @id, @iUDLogId, @columnName, @columnDataType, @isNullable, @oldValue_String, @oldValue_Binary, @newValue_String, @newValue_Binary;
	WHILE @@FETCH_STATUS = 0
		BEGIN
			BEGIN TRANSACTION;
			BEGIN TRY
				INSERT INTO IUDLogValues ([Id], [IUDLogId], [ColumnName], [ColumnDataType], [IsNullable], [OldValue_String], [OldValue_Binary], [NewValue_String], [NewValue_Binary])
					VALUES (@id, @iUDLogId, @columnName, @columnDataType, @isNullable, @oldValue_String, @oldValue_Binary, @newValue_String, @newValue_Binary);
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
					('IUDLogId', 'UNIQUEIDENTIFIER', 0, CONVERT(NVARCHAR(MAX), @iUDLogId), NULL), 
					('ColumnName', 'VARCHAR(128)', 0, CONVERT(NVARCHAR(MAX), @columnName), NULL), 
					('ColumnDataType', 'VARCHAR(128)', 0, CONVERT(NVARCHAR(MAX), @columnDataType), NULL), 
					('IsNullable', 'BIT', 0, CONVERT(NVARCHAR(MAX), @isNullable), NULL), 
					('OldValue_String', 'NVARCHAR(MAX)', 1, @oldValue_String, NULL), 
					('OldValue_Binary', 'VARBINARY(MAX)', 1, NULL, @oldValue_Binary), 
					('NewValue_String', 'NVARCHAR(MAX)', 1, @newValue_String, NULL), 
					('NewValue_Binary', 'VARBINARY(MAX)', 1, NULL, @newValue_Binary);
				BEGIN TRY
					EXEC [dbo].[Insert_IUDLogs] 1, 0, 'IUDLogValues', 'Id', 'UNIQUEIDENTIFIER', @id, @error, @values, @logId OUTPUT;
				END TRY
				BEGIN CATCH
					INSERT INTO @errorMessages VALUES (ERROR_MESSAGE());
				END CATCH
			END CATCH
			FETCH NEXT FROM _cursor_trgIUDLogValues_InsteadOfInsert INTO @id, @iUDLogId, @columnName, @columnDataType, @isNullable, @oldValue_String, @oldValue_Binary, @newValue_String, @newValue_Binary;
		END
	CLOSE _cursor_trgIUDLogValues_InsteadOfInsert;
	DEALLOCATE _cursor_trgIUDLogValues_InsteadOfInsert;
	SELECT
			@error = STRING_AGG(t.[ErrorMessage], N' ||| ')
		FROM (SELECT DISTINCT [ErrorMessage] FROM @errorMessages) t
		WHERE
			t.[ErrorMessage] IS NOT NULL AND
			[dbo].[IsEmptyString_N](t.[ErrorMessage]) = 0;
	IF [dbo].[IsEmptyString_N](@error) = 0
		RAISERROR(@error, 16, 1);
GO