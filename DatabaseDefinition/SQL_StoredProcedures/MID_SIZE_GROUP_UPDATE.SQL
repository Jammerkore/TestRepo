--dv =============================================
--dv Create date:	1/29/2014
--dv Description:	Updates a size group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SIZE_GROUP_UPDATE]
	@SIZE_GROUP_RID int,
	@SIZE_GROUP_NAME varchar(50),
	@SIZE_GROUP_DESCRIPTION varchar(250),
	@WIDTH_ACROSS_IND char
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SIZE_GROUP 
	SET 
		SIZE_GROUP_NAME = @SIZE_GROUP_NAME,
		SIZE_GROUP_DESCRIPTION = @SIZE_GROUP_DESCRIPTION,
		WIDTH_ACROSS_IND = @WIDTH_ACROSS_IND
	WHERE SIZE_GROUP_RID = @SIZE_GROUP_RID

	SELECT @@ROWCOUNT;
END
GO
