USE WebSiteBaseDB
GO



CREATE TYPE [typeIUDLogValue] AS TABLE
(
	[ColumnName] VARCHAR(128) NOT NULL,
	[ColumnDataType] VARCHAR(128) NOT NULL,
	[IsNullable] BIT NOT NULL,
	[OldValue_String] NVARCHAR(MAX) NULL,
	[OldValue_Binary] VARBINARY(MAX) NULL,
	[NewValue_String] NVARCHAR(MAX) NULL,
	[NewValue_Binary] VARBINARY(MAX) NULL
)
GO


CREATE VIEW [vwRandomIntegers]
AS
	SELECT CHECKSUM(NEWID()) AS [Value]
GO