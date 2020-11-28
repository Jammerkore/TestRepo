--dv =============================================
--dv Create date:	3/19/2015
--dv Description:	Updates a store characteristic group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_CHAR_GROUP_UPDATE] 
	@SCG_RID INT,
	@SCG_ID VARCHAR(50),
	@SCG_TYPE INT,
	@SCG_LIST_IND CHAR
AS
BEGIN
	SET NOCOUNT ON

	UPDATE STORE_CHAR_GROUP SET
		SCG_ID = @SCG_ID,
		SCG_TYPE = @SCG_TYPE,
		SCG_LIST_IND = @SCG_LIST_IND
	WHERE SCG_RID = @SCG_RID

	SELECT @@ROWCOUNT;
END
GO


