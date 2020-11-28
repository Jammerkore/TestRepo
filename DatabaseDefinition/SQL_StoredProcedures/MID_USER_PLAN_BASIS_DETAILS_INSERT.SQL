--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Inserts a user plan basis detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_PLAN_BASIS_DETAILS_INSERT]
	@USER_RID int, 
	@BASIS_RID int,
	@SEQ_ID int, 
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int, 
	@WEIGHT float, 
	@IS_INCLUDED_IND char
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO USER_PLAN_BASIS_DETAILS
		(
		USER_RID, 
		BASIS_RID, 
		SEQ_ID, 
		HN_RID, 
		FV_RID, 
		CDR_RID, 
		WEIGHT, 
		IS_INCLUDED_IND
		)
	VALUES
		(
		@USER_RID, 
		@BASIS_RID, 
		@SEQ_ID, 
		@HN_RID, 
		@FV_RID, 
		@CDR_RID, 
		@WEIGHT, 
		@IS_INCLUDED_IND
		)

	SELECT @@ROWCOUNT;

END
GO





