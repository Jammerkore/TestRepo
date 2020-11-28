CREATE PROCEDURE [dbo].[MID_GET_ALLOCATION_METHOD_TYPES_USED]
@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	--hard code the forecast method types so they can be excluded
	--DECLARE @UnAssigned				INT
	DECLARE @OTSPlan				INT
	DECLARE @ForecastBalance		INT
	DECLARE @ForecastSpread			INT
	DECLARE @CopyChainForecast		INT
	DECLARE @CopyStoreForecast		INT
	DECLARE @ForecastModifySales	INT
	DECLARE @Export					INT
	DECLARE @GlobalUnlock       	INT
	DECLARE @GlobalLock         	INT
	DECLARE @Rollup             	INT

	--SET @UnAssigned					= 0
	SET @OTSPlan					= 802100
	SET @ForecastBalance			= 802101
	SET @ForecastSpread		        = 802128
	SET @CopyChainForecast			= 802129
	SET @CopyStoreForecast			= 802130
	SET @ForecastModifySales		= 802132
	SET @Export						= 802136
    SET @GlobalUnlock               = 802137
    SET @GlobalLock                 = 802138
    SET @Rollup                     = 802139
 
		SELECT TEXT_VALUE AS METHOD_NAME, TEXT_CODE AS METHOD_TYPE_ID FROM APPLICATION_TEXT 
		WHERE TEXT_CODE NOT IN (
		--@UnAssigned,
		@OTSPlan,			
		@ForecastBalance,
		@ForecastSpread,		
		@CopyChainForecast,	
		@CopyStoreForecast,	
		@ForecastModifySales,
		@Export,				
		@GlobalUnlock,       
		@GlobalLock,         
		@Rollup             
		)
		AND TEXT_CODE IN
		(
			SELECT DISTINCT METHOD_TYPE_ID FROM METHOD M
			INNER JOIN APPLICATION_USER AU ON AU.USER_RID=M.USER_RID WHERE AU.USER_NAME='global' OR AU.USER_RID=@USER_RID
		)
		ORDER BY TEXT_CODE
END

GO
