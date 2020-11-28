--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes user allocation tables for the user rid passed in, then inserts a row into the user allocation table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_ALLOCATION_DELETE_AND_INSERT]
	@USER_RID int,
	@VIEW_TYPE int,
	@SG_RID int,
	@FILTER_RID int,
	@GROUP_BY_ID int,
	@VIEW_RID int,
	@INCLUDE_INELIGIBLE_STORES char,
	@BEGIN_CDR_RID int,
	@END_CDR_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_ALLOCATION_BASIS
    WHERE USER_RID = @USER_RID

	DELETE FROM USER_ALLOCATION_HEADERS
	WHERE USER_RID = @USER_RID

	DELETE FROM USER_ALLOCATION
	WHERE USER_RID = @USER_RID

	INSERT INTO USER_ALLOCATION
		(
		USER_RID, 
		VIEW_TYPE, 
		SG_RID, 
		FILTER_RID, 
		GROUP_BY_ID,
		VIEW_RID, 
		INCLUDE_INELIGIBLE_STORES,
		BEGIN_CDR_RID, 
		END_CDR_RID, 
		HN_RID
		) 
	VALUES
		(
		@USER_RID, 
		@VIEW_TYPE, 
		@SG_RID, 
		@FILTER_RID, 
		@GROUP_BY_ID,
		@VIEW_RID, 
		@INCLUDE_INELIGIBLE_STORES,
		@BEGIN_CDR_RID, 
		@END_CDR_RID, 
		@HN_RID
		) 

	SELECT @@ROWCOUNT;

END
GO
