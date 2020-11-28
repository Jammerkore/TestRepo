--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Inserts or updates a row for chain plan set percentages for a user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_SET_PERCENT_USER_UPSERT]
	@USER_RID int,
	@SG_RID int,
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT SG_RID FROM CHAIN_SET_PERCENT_USER WHERE USER_RID = @USER_RID) 
		INSERT INTO CHAIN_SET_PERCENT_USER(USER_RID, SG_RID, CDR_RID) VALUES(@USER_RID, @SG_RID, @CDR_RID)
    ELSE 
		UPDATE CHAIN_SET_PERCENT_USER WITH (ROWLOCK)
		SET 
			SG_RID = @SG_RID,
			CDR_RID = @CDR_RID 
		WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;

END
GO
