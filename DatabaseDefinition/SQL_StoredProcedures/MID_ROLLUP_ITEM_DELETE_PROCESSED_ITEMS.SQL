--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes rollup items that have been processed
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_DELETE_PROCESSED_ITEMS]
	@ALTERNATES_ONLY char
AS
BEGIN
     DECLARE @rowcount INT = 1
	 DECLARE @totalrowcount INT = 0

     WHILE @rowcount > 0
     BEGIN
	  SET ROWCOUNT 5000
	  DELETE 
	  FROM ROLLUP_ITEM 
	  WHERE ITEM_PROCESSED IS NOT NULL 
		  AND (ALTERNATES_ONLY = @ALTERNATES_ONLY OR ALTERNATES_ONLY IS NULL)

      SELECT @rowcount = @@RowCount
	  SET @totalrowcount = @totalrowcount + @rowcount
    END

	SELECT @totalrowcount;
END
GO