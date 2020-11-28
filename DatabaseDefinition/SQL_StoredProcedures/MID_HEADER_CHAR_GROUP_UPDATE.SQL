--dv =============================================
--dv Create date:	3/23/2015
--dv Description:	Updates a header characteristic group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_CHAR_GROUP_UPDATE] 
	@HCG_RID INT,
	@HCG_ID VARCHAR(50),
	@HCG_TYPE INT,
	@HCG_LIST_IND CHAR,
	@HCG_PROTECT_IND CHAR
AS
BEGIN
	SET NOCOUNT ON

	UPDATE HEADER_CHAR_GROUP SET
		HCG_ID = @HCG_ID,
		HCG_TYPE = @HCG_TYPE,
		HCG_LIST_IND = @HCG_LIST_IND,
		HCG_PROTECT_IND = @HCG_PROTECT_IND
	WHERE HCG_RID = @HCG_RID

	SELECT @@ROWCOUNT;
END
GO


