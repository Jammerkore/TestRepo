--dv =============================================
--dv Description:	Upserts data for chain set percent
--dv History:		5/13/2015 -TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_CHAIN_SET_PERCENT_SET_INSERT] 
	@YEAR_WEEK INT,
	@HN_RID INT,
	@STORE_ATTRIBUTE VARCHAR(250),
	@STORE_ATTRIBUTE_SET VARCHAR(250),
	@PERCENT DECIMAL(6, 3),
	@SG_VERSION INT,   -- TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
	@RETURN INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	--Get the CDR_RID
	DECLARE @CDR_RID INT

	--Get the YEAR_WEEK
	SELECT @CDR_RID = FIRST_DAY_OF_WEEK
	FROM FISCAL_WEEKS
	WHERE FISCAL_WEEK = @YEAR_WEEK

	DECLARE @SG_RID AS INT = (
			SELECT SG_RID
			FROM STORE_GROUP
			WHERE SG_ID = @STORE_ATTRIBUTE
				AND USER_RID = 4
			)
	DECLARE @SGL_RID AS INT = (
			SELECT SGL_RID
			FROM STORE_GROUP_JOIN
			WHERE SG_RID = @SG_RID
				AND SGL_OVERRIDE_ID = @STORE_ATTRIBUTE_SET
				AND SG_VERSION = @SG_VERSION  -- TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
			)

	SET @RETURN = 0

	IF @HN_RID IS NULL
		SET @RETURN = 1
	ELSE IF @CDR_RID IS NULL
		SET @RETURN = 2
	ELSE IF @SG_RID IS NULL
		SET @RETURN = 3
	ELSE IF @SGL_RID IS NULL
		SET @RETURN = 4

	IF @RETURN <> 0
		RETURN (
				SELECT @RETURN
				)
	ELSE
		--UPSERT DATA TO THE TABLES
		UPDATE CHAIN_SET_PERCENT_SET
		SET PERCENTAGE = @PERCENT
		WHERE TIME_ID = @CDR_RID
			AND SGL_RID = @SGL_RID
			AND HN_RID = @HN_RID

	IF @@ROWCOUNT = 0
		INSERT INTO CHAIN_SET_PERCENT_SET (
			HN_RID,
			SGL_RID,
			TIME_ID,
			PERCENTAGE
			)
		VALUES (
			@HN_RID,
			@SGL_RID,
			@CDR_RID,
			@PERCENT
			)

	SELECT @@ROWCOUNT;
END
GO


