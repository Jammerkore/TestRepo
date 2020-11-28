--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ADAPTER
--dv Description:	Inserts a calendar model
--dv History: TT#3256 - JSmith - Replace @@IDENTITY with SCOPE_IDENTITY()
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_CALENDARMODEL_INSERT] 
	@CM_ID VARCHAR(50),
	@START_DATE DATETIME,
	@FISCAL_YEAR INT,
	@CM_RID INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO CALENDAR_MODEL (
		CM_ID,
		START_DATE,
		FISCAL_YEAR
		)
	VALUES (
		@CM_ID,
		@START_DATE,
		@FISCAL_YEAR
		)

	SELECT @CM_RID = SCOPE_IDENTITY()
END
GO


