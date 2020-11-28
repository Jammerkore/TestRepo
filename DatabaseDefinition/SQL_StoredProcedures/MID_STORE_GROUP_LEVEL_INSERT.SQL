--dv =============================================
--dv Create date:	1/21/2015
--dv Description:	Inserts a store group level.
--dv History:		4/15/2015 TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_LEVEL_INSERT] 
	@SGL_SEQUENCE INT,
	@SG_RID INT,
	@SGL_ID VARCHAR(50),
	@IS_ACTIVE BIT,
	@SGL_VERSION INT,
	@LEVEL_TYPE INT,
	@SGL_RID INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO STORE_GROUP_LEVEL (
		SGL_SEQUENCE,
		SG_RID,
		SGL_ID,
		IS_ACTIVE,
		SGL_VERSION,
		LEVEL_TYPE
		)
	VALUES (
		@SGL_SEQUENCE,
		@SG_RID,	
		@SGL_ID,
		@IS_ACTIVE,
		@SGL_VERSION,
		@LEVEL_TYPE
		)

	SELECT @SGL_RID = SCOPE_IDENTITY();
END
GO


