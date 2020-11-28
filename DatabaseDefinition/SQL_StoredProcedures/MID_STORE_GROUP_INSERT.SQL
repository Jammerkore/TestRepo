--dv =============================================
--dv Create date:	2/9/2015
--dv Description:	Inserts a store group
--dv History:		02/09/2015 TT#1414-MD -jsobek -Attribute Set Filter
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_GROUP_INSERT] 
	@SG_ID VARCHAR(50),
	@SG_DYNAMIC_GROUP_IND CHAR(1),
	@USER_RID INT,
	@FILTER_RID INT,
	@SG_VERSION INT,
	@SG_RID INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO STORE_GROUP (
		SG_ID,
		SG_DYNAMIC_GROUP_IND,
		USER_RID,
		FILTER_RID,
		IS_ACTIVE,
		SG_VERSION
		)
	VALUES (
		@SG_ID,
		@SG_DYNAMIC_GROUP_IND,
		@USER_RID,
		@FILTER_RID,
		1,
		@SG_VERSION
		)

	SELECT @SG_RID = SCOPE_IDENTITY()
END
GO


