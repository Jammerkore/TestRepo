--dv =============================================
--dv Create date: 2/9/2018
--dv Description:	Write Hierarchy Data
--dv =============================================
CREATE PROCEDURE [dbo].[MID_EXTRACT_PLANNING_CONTROL_READ]
	@HN_RID int,
	@FV_RID int,
	@PLAN_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [HN_RID]
      ,[FV_RID]
      ,[TIME_ID]
      ,[PLAN_TYPE]
      ,[UPDATE_DATE]
      ,[EXTRACT_DATE]
  FROM [dbo].[EXTRACT_PLANNING_CONTROL]
	WHERE [HN_RID] = @HN_RID
		  AND [FV_RID] = @FV_RID
		  AND [PLAN_TYPE] = @PLAN_TYPE

END
GO


















