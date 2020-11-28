--dv =============================================
--dv Create date:	11/17/2020
--dv Description:	Inserts a plan view format
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_FORMAT_INSERT] 
	@VIEW_RID int,
    @PLAN_BASIS_TYPE int,
    @VARIABLE_NUMBER int,
    @QUANTITY_VARIABLE_KEY int,
    @TIME_PERIOD_TYPE int,
	@WIDTH int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PLAN_VIEW_FORMAT
		(
		VIEW_RID,
        PLAN_BASIS_TYPE,
        VARIABLE_NUMBER,
        QUANTITY_VARIABLE_KEY,
        TIME_PERIOD_TYPE,
		WIDTH
		)
	VALUES
		(
		@VIEW_RID,
        @PLAN_BASIS_TYPE,
        @VARIABLE_NUMBER,
        @QUANTITY_VARIABLE_KEY,
        @TIME_PERIOD_TYPE,
		@WIDTH
		)

	SELECT @@ROWCOUNT;

END
GO
