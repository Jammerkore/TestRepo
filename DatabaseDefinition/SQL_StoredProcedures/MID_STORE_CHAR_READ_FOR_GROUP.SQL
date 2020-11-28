--dv =============================================
--dv Create date: 6/2/2014
--dv Description: Reads all possible store characteristic values from a store characteristic group
--dv Example:  Store characteristic group=Climate
--dv		   Store characteristic values=Hot, Cold
--dv				public enum eStoreCharType
--dv				{
--dv					text=0,
--dv					date=1,
--dv					number=2,
--dv					dollar=3,
--dv					unknown=4,
--dv					list = 5, --not supported
--dv					boolean = 6 --not supported
--dv				}
--dv Modify date: 1/26/2017
--dv Description: TT1925-MD - JSmith - Store Char Values (Attribure Sets) not sorting in Ascending or Descending order for Dollar, Number and Date.   They do sort for Fields.  Should these be consistent.
--dv              Restructured to allow SQL to order the values by type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_READ_FOR_GROUP]
	@SCG_RID INT,
	@SORT_STRING VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @STORE_CHAR_GROUP_VALUE_TYPE INT;

	SELECT @STORE_CHAR_GROUP_VALUE_TYPE = SCG_TYPE FROM STORE_CHAR_GROUP WHERE SCG_RID=@SCG_RID

	--SELECT 

	--	CASE 		
	--		WHEN @STORE_CHAR_GROUP_VALUE_TYPE=1 THEN CONVERT(varchar(50), SC.DATE_VALUE, 101)
	--		WHEN @STORE_CHAR_GROUP_VALUE_TYPE=2 THEN CONVERT(varchar(50), SC.NUMBER_VALUE)
	--		WHEN @STORE_CHAR_GROUP_VALUE_TYPE=3 THEN CONVERT(varchar(50), SC.DOLLAR_VALUE)
	--		ELSE SC.TEXT_VALUE
	--	END AS CHAR_VALUE,
	--	SC.SC_RID 
	--FROM STORE_CHAR SC WITH (NOLOCK)
	--WHERE SC.SCG_RID=@SCG_RID
	--ORDER BY CHAR_VALUE

	DECLARE @CMD NVARCHAR(500)

	IF @STORE_CHAR_GROUP_VALUE_TYPE=1
	BEGIN
	     SET @CMD = 'SELECT 
		   CONVERT(varchar(50), SC.DATE_VALUE, 101) AS CHAR_VALUE,
		   SC.SC_RID 
	     FROM STORE_CHAR SC WITH (NOLOCK)
	     WHERE SC.SCG_RID=' + convert(NVARCHAR,@SCG_RID)
	     + ' ORDER BY DATE_VALUE ' +  @SORT_STRING
	END
	ELSE IF @STORE_CHAR_GROUP_VALUE_TYPE=2
	BEGIN
	     SET @CMD = 'SELECT 
		   CONVERT(varchar(50), SC.NUMBER_VALUE) AS CHAR_VALUE,
		   SC.SC_RID 
	     FROM STORE_CHAR SC WITH (NOLOCK)
	     WHERE SC.SCG_RID=' + convert(NVARCHAR,@SCG_RID)
	     + ' ORDER BY NUMBER_VALUE ' +  @SORT_STRING
	END
	ELSE IF @STORE_CHAR_GROUP_VALUE_TYPE=3
	BEGIN
	     SET @CMD = 'SELECT 
		   CONVERT(varchar(50), SC.DOLLAR_VALUE) AS CHAR_VALUE,
		   SC.SC_RID 
	     FROM STORE_CHAR SC WITH (NOLOCK)
	     WHERE SC.SCG_RID=' + convert(NVARCHAR,@SCG_RID)
	     + ' ORDER BY DOLLAR_VALUE ' +  @SORT_STRING
	END
	ELSE 
	BEGIN
	     SET @CMD = 'SELECT 
		   SC.TEXT_VALUE AS CHAR_VALUE,
		   SC.SC_RID 
	     FROM STORE_CHAR SC WITH (NOLOCK)
	     WHERE SC.SCG_RID=' + convert(NVARCHAR,@SCG_RID)
	     + ' ORDER BY TEXT_VALUE ' +  @SORT_STRING
	END

	EXEC (@CMD)
END

GO


