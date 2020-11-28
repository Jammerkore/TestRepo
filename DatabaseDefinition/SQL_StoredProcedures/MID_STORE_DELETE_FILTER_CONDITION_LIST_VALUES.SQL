--dv =============================================
--dv Create date: 10/31/2014
--dv Description: During a store delete, removes store values from store filters
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_DELETE_FILTER_CONDITION_LIST_VALUES] 
	@BATCH_SIZE INT,
	@RC INT OUTPUT
AS
BEGIN
	DECLARE @cmd NVARCHAR(500)
	DECLARE @parmDefinition NVARCHAR(500)
	DECLARE @count INT
	DECLARE @stop INT,
		@rowCount INT,
		@storeId VARCHAR(100)

	SET @RC = 0
	SET @parmDefinition = N'@countOUT int OUTPUT'
	SET @count = 0
	SET @stop = 0
	SET @rowCount = 0

	WHILE (1 = 1)
	BEGIN
		BEGIN TRANSACTION

		DELETE TOP (@BATCH_SIZE)
		FROM FILTER_CONDITION_LIST_VALUES
		WHERE LIST_VALUE_TYPE_INDEX = 9 --Store List
			AND LIST_VALUE_INDEX IN (
				SELECT ST_RID
				FROM STORES
				WHERE STORE_DELETE_IND = '1'
				)

		SET @rowCount = @@ROWCOUNT

		COMMIT

		IF @rowCount = 0
			BREAK
		ELSE
		BEGIN
			SET @RC = @RC + @rowCount

			CONTINUE
		END
	END

	RETURN @RC
END
GO


