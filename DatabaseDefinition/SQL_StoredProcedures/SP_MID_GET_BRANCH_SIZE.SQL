CREATE PROCEDURE [dbo].[SP_MID_GET_BRANCH_SIZE] 
	@HN_RID INT,
	@HOME_HIERARCHY_ONLY CHAR,
	@BRANCH_SIZE INT OUTPUT
AS
SET NOCOUNT ON

DECLARE @Loop INT,
	@LoopCount INT,
	@NextLoopCount INT,
	@OrgLevels INT,
	@HighestGuestLevel INT,
	@HomePathLength INT,
	@RC INT

EXEC @RC = [dbo].[SP_MID_GET_HIGHEST_GUEST_LEVEL] @HN_RID,
	@HighestGuestLevel OUTPUT,
	@HomePathLength OUTPUT

IF @HighestGuestLevel = - 1
BEGIN
	SET @OrgLevels = 0
	SET @HighestGuestLevel = 0
END
ELSE
BEGIN
	SELECT @OrgLevels = count(*)
	FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] WITH (NOLOCK)

	SET @HighestGuestLevel = @HighestGuestLevel - 1
END

IF @HOME_HIERARCHY_ONLY = '1'
BEGIN
	SET @BRANCH_SIZE = @HomePathLength + 1
END
ELSE
BEGIN
	SET @BRANCH_SIZE = @HomePathLength + @OrgLevels - @HighestGuestLevel
END
GO


