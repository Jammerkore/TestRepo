-- =============================================
-- Description:	Gets header audit information by styleRID, dates, header status, and header type
-- =============================================
CREATE PROCEDURE [dbo].[MID_GET_ALLOCATION_ANALYSIS] 
@RestrictToStyle_HN_RID  INT = -1,
@UseDateRange BIT = 0,
@StartDate DATETIME,
@EndDate DATETIME,
@ResultLimit INT=-1,

@RestrictMethods BIT = 0,
@MethodRIDsToInclude VARCHAR(MAX),

@RestrictUsers BIT = 0,
@UserRIDsToInclude VARCHAR(MAX),

@RestrictHeaders BIT = 0,
@HeaderRIDsToInclude VARCHAR(MAX),

--Header Types
@Include_Receipt		 BIT = 1,
@Include_ASN			 BIT = 1,
@Include_Dummy			 BIT = 1,
@Include_DropShip		 BIT = 1,
@Include_MultiHeader	 BIT = 1,
@Include_Reserve		 BIT = 1,
@Include_WorkupTotalBuy  BIT = 1,
@Include_PurchaseOrder   BIT = 1,
@Include_Assortment		 BIT = 1,
@Include_Placeholder	 BIT = 1,
@Include_IMO             BIT = 1,
@Include_Master          BIT = 1,    /*  TT#1966-MD - JSmith - DC Fulfillment */
--Header Statuses
@Include_ReceivedOutOfBalance   	BIT = 1,
@Include_ReceivedInBalance      	BIT = 1,
@Include_InUseByMultiHeader     	BIT = 1,
@Include_PartialSizeOutOfBalance	BIT = 1,
@Include_PartialSizeInBalance   	BIT = 1,
@Include_AllocatedOutOfBalance  	BIT = 1,
@Include_AllocatedInBalance     	BIT = 1,
@Include_SizesOutOfBalance      	BIT = 1,
@Include_AllInBalance           	BIT = 1,
@Include_Released               	BIT = 1,
@Include_ReleaseApproved        	BIT = 1,
@Include_AllocationStarted      	BIT = 1,
--Action Types
@IncludeActionType_ActionUnassigned							BIT = 1,
@IncludeActionType_ApplyAPI_Workflow						BIT = 1,
@IncludeActionType_BackoutAllocation						BIT = 1,
@IncludeActionType_BackoutDetailPackAllocation				BIT = 1,
@IncludeActionType_BackoutSizeAllocation					BIT = 1,
@IncludeActionType_BackoutSizeIntransit						BIT = 1,
@IncludeActionType_BackoutStyleIntransit					BIT = 1,
@IncludeActionType_BalanceSizeBilaterally					BIT = 1,
@IncludeActionType_BalanceSizeNoSubs						BIT = 1,
@IncludeActionType_BalanceSizeWithConstraints				BIT = 1,
@IncludeActionType_BalanceSizeWithSubs						BIT = 1,
@IncludeActionType_BalanceStyleProportional					BIT = 1,
@IncludeActionType_BalanceToDC								BIT = 1,
@IncludeActionType_BreakoutSizesAsReceived					BIT = 1,
@IncludeActionType_BreakoutSizesAsReceivedWithConstraints	BIT = 1,
@IncludeActionType_ChargeIntransit							BIT = 1,
@IncludeActionType_ChargeSizeIntransit						BIT = 1,
@IncludeActionType_DeleteHeader								BIT = 1,
@IncludeActionType_ReapplyTotalAllocation					BIT = 1,
@IncludeActionType_Release									BIT = 1,
@IncludeActionType_RemoveAPI_Workflow						BIT = 1,
@IncludeActionType_Reset									BIT = 1,
@IncludeActionType_StyleNeed								BIT = 1,
@IncludeActionType_BalanceToVSW								BIT = 1		/* TT#1334-MD - stodd - Balance to VSW Action */
AS 
BEGIN 
	--Declare header types
	DECLARE @Receipt		 INT;
	DECLARE @ASN			 INT;
	DECLARE @Dummy			 INT;
	DECLARE @DropShip		 INT;
	DECLARE @MultiHeader	 INT;
	DECLARE @Reserve		 INT;
	DECLARE @WorkupTotalBuy  INT;
	DECLARE @PurchaseOrder   INT;
	DECLARE @Assortment		 INT;
	DECLARE @Placeholder	 INT;
	DECLARE @IMO             INT;
	DECLARE @Master          INT;     /*  TT#1966-MD - JSmith - DC Fulfillment */
	--Default header types to negative one to exclude them
	SET @Receipt		= -1;
	SET @ASN			= -1;
	SET @Dummy			= -1;
	SET @DropShip		= -1;
	SET @MultiHeader	= -1;
	SET @Reserve		= -1;
	SET @WorkupTotalBuy	= -1;
	SET @PurchaseOrder 	= -1;
	SET @Assortment		= -1;
	SET @Placeholder	= -1;
	SET @IMO           	= -1;
	SET @Master         = -1;     /*  TT#1966-MD - JSmith - DC Fulfillment */
	--Conditionally set the header types based on the selected options
	IF @Include_Receipt=1			BEGIN SET @Receipt		  = 800730; END
	IF @Include_ASN=1				BEGIN SET @ASN			  = 800731; END
	IF @Include_Dummy=1				BEGIN SET @Dummy		  = 800732; END
	IF @Include_DropShip=1			BEGIN SET @DropShip		  = 800733; END
	IF @Include_MultiHeader=1		BEGIN SET @MultiHeader	  = 800734; END
	IF @Include_Reserve=1			BEGIN SET @Reserve		  = 800735; END
	IF @Include_WorkupTotalBuy=1	BEGIN SET @WorkupTotalBuy = 800737; END
	IF @Include_PurchaseOrder=1		BEGIN SET @PurchaseOrder  = 800738; END
	IF @Include_Assortment=1		BEGIN SET @Assortment	  = 800739; END
	IF @Include_Placeholder=1		BEGIN SET @Placeholder	  = 800740; END
	IF @Include_IMO=1				BEGIN SET @IMO            = 800741; END
	IF @Include_Master=1			BEGIN SET @Master         = 800742; END    /*  TT#1966-MD - JSmith - DC Fulfillment */
	--Declare header statuses
	DECLARE @ReceivedOutOfBalance   	INT;
	DECLARE @ReceivedInBalance      	INT;
	DECLARE @InUseByMultiHeader     	INT;
	DECLARE @PartialSizeOutOfBalance	INT;
	DECLARE @PartialSizeInBalance   	INT;
	DECLARE @AllocatedOutOfBalance  	INT;
	DECLARE @AllocatedInBalance     	INT;
	DECLARE @SizesOutOfBalance      	INT;
	DECLARE @AllInBalance           	INT;
	DECLARE @Released               	INT;
	DECLARE @ReleaseApproved        	INT;
	DECLARE @AllocationStarted      	INT;
	--Default header statuses to negative one to exclude them
	SET @ReceivedOutOfBalance    = -1;
	SET @ReceivedInBalance       = -1;
	SET @InUseByMultiHeader      = -1;
	SET @PartialSizeOutOfBalance = -1;
	SET @PartialSizeInBalance    = -1;
	SET @AllocatedOutOfBalance   = -1;
	SET @AllocatedInBalance      = -1;
	SET @SizesOutOfBalance       = -1;
	SET @AllInBalance            = -1;
	SET @Released                = -1;
	SET @ReleaseApproved         = -1;
	SET @AllocationStarted       = -1;
	--Conditionally set the header statuses based on the selected options
	IF @Include_ReceivedOutOfBalance = 1	BEGIN SET @ReceivedOutOfBalance    = 802700; END
	IF @Include_ReceivedInBalance = 1		BEGIN SET @ReceivedInBalance       = 802701; END
	IF @Include_InUseByMultiHeader = 1		BEGIN SET @InUseByMultiHeader      = 802702; END
	IF @Include_PartialSizeOutOfBalance = 1 BEGIN SET @PartialSizeOutOfBalance = 802703; END
	IF @Include_PartialSizeInBalance = 1	BEGIN SET @PartialSizeInBalance    = 802704; END
	IF @Include_AllocatedOutOfBalance = 1	BEGIN SET @AllocatedOutOfBalance   = 802705; END
	IF @Include_AllocatedInBalance = 1		BEGIN SET @AllocatedInBalance      = 802706; END
	IF @Include_SizesOutOfBalance = 1		BEGIN SET @SizesOutOfBalance       = 802707; END
	IF @Include_AllInBalance = 1			BEGIN SET @AllInBalance            = 802708; END
	IF @Include_Released  = 1				BEGIN SET @Released                = 802709; END
	IF @Include_ReleaseApproved = 1			BEGIN SET @ReleaseApproved         = 802710; END
	IF @Include_AllocationStarted  = 1		BEGIN SET @AllocationStarted       = 802711; END
	--Declare Action Types
	DECLARE @ActionUnassigned						INT;
	DECLARE @ApplyAPI_Workflow						INT;
	DECLARE @BackoutAllocation						INT;
	DECLARE @BackoutDetailPackAllocation			INT;	
	DECLARE @BackoutSizeAllocation					INT;
	DECLARE @BackoutSizeIntransit					INT;	
	DECLARE @BackoutStyleIntransit					INT;
	DECLARE @BalanceSizeBilaterally					INT;
	DECLARE @BalanceSizeNoSubs						INT;
	DECLARE @BalanceSizeWithConstraints				INT;
	DECLARE @BalanceSizeWithSubs					INT;	
	DECLARE @BalanceStyleProportional				INT;	
	DECLARE @BalanceToDC							INT;	
	DECLARE @BreakoutSizesAsReceived				INT;	
	DECLARE @BreakoutSizesAsReceivedWithConstraints	INT;
	DECLARE @ChargeIntransit						INT;	
	DECLARE @ChargeSizeIntransit					INT;	
	DECLARE @DeleteHeader							INT;	
	DECLARE @ReapplyTotalAllocation					INT;
	DECLARE @Release								INT;	
	DECLARE @RemoveAPI_Workflow						INT;
	DECLARE @Reset									INT;
	DECLARE @StyleNeed								INT;
	DECLARE @BalanceToVSW							INT;	/* TT#1334-MD - stodd - Balance to VSW Action */
	--Default action types to negative one to exclude them
	SET @ActionUnassigned						= -1;
	SET @ApplyAPI_Workflow						= -1;
	SET @BackoutAllocation						= -1;
	SET @BackoutDetailPackAllocation			= -1;	
	SET @BackoutSizeAllocation					= -1;
	SET @BackoutSizeIntransit					= -1;	
	SET @BackoutStyleIntransit					= -1;
	SET @BalanceSizeBilaterally					= -1;
	SET @BalanceSizeNoSubs						= -1;
	SET @BalanceSizeWithConstraints				= -1;
	SET @BalanceSizeWithSubs					= -1;	
	SET @BalanceStyleProportional				= -1;	
	SET @BalanceToDC							= -1;	
	SET @BreakoutSizesAsReceived				= -1;	
	SET @BreakoutSizesAsReceivedWithConstraints	= -1;
	SET @ChargeIntransit						= -1;	
	SET @ChargeSizeIntransit					= -1;	
	SET @DeleteHeader							= -1;	
	SET @ReapplyTotalAllocation					= -1;
	SET @Release								= -1;	
	SET @RemoveAPI_Workflow						= -1;
	SET @Reset									= -1;
	SET @StyleNeed								= -1;
	SET @BalanceToVSW							= -1;	/* TT#1334-MD - stodd - Balance to VSW Action */
	--Conditionally set the action types based on the selected options
	IF @IncludeActionType_ActionUnassigned=1						BEGIN SET @ActionUnassigned							= 0;	  END
	IF @IncludeActionType_ApplyAPI_Workflow=1						BEGIN SET @ApplyAPI_Workflow						= 802135; END
	IF @IncludeActionType_BackoutAllocation=1						BEGIN SET @BackoutAllocation						= 802123; END
	IF @IncludeActionType_BackoutDetailPackAllocation=1				BEGIN SET @BackoutDetailPackAllocation				= 802126; END
	IF @IncludeActionType_BackoutSizeAllocation=1					BEGIN SET @BackoutSizeAllocation					= 802122; END
	IF @IncludeActionType_BackoutSizeIntransit=1					BEGIN SET @BackoutSizeIntransit						= 802120; END
	IF @IncludeActionType_BackoutStyleIntransit=1					BEGIN SET @BackoutStyleIntransit					= 802121; END
	IF @IncludeActionType_BalanceSizeBilaterally=1					BEGIN SET @BalanceSizeBilaterally					= 802144; END
	IF @IncludeActionType_BalanceSizeNoSubs=1						BEGIN SET @BalanceSizeNoSubs						= 802116; END
	IF @IncludeActionType_BalanceSizeWithConstraints=1				BEGIN SET @BalanceSizeWithConstraints				= 802143; END
	IF @IncludeActionType_BalanceSizeWithSubs=1						BEGIN SET @BalanceSizeWithSubs						= 802115; END
	IF @IncludeActionType_BalanceStyleProportional=1				BEGIN SET @BalanceStyleProportional					= 802113; END
	IF @IncludeActionType_BalanceToDC=1								BEGIN SET @BalanceToDC								= 802114; END
	IF @IncludeActionType_BreakoutSizesAsReceived=1					BEGIN SET @BreakoutSizesAsReceived					= 802112; END
	IF @IncludeActionType_BreakoutSizesAsReceivedWithConstraints=1	BEGIN SET @BreakoutSizesAsReceivedWithConstraints	= 802145; END
	IF @IncludeActionType_ChargeIntransit=1							BEGIN SET @ChargeIntransit							= 802117; END
	IF @IncludeActionType_ChargeSizeIntransit=1						BEGIN SET @ChargeSizeIntransit						= 802127; END
	IF @IncludeActionType_DeleteHeader=1							BEGIN SET @DeleteHeader								= 802124; END
	IF @IncludeActionType_ReapplyTotalAllocation=1					BEGIN SET @ReapplyTotalAllocation					= 802142; END
	IF @IncludeActionType_Release=1									BEGIN SET @Release									= 802118; END
	IF @IncludeActionType_RemoveAPI_Workflow=1						BEGIN SET @RemoveAPI_Workflow						= 802134; END
	IF @IncludeActionType_Reset=1									BEGIN SET @Reset									= 802119; END
	IF @IncludeActionType_StyleNeed=1								BEGIN SET @StyleNeed								= 802111; END
	IF @IncludeActionType_BalanceToVSW=1							BEGIN SET @BalanceToVSW								= 802147; END	/* TT#1334-MD - stodd - Balance to VSW Action */												
	
	
	--DECLARE @HasStartDate  	BIT;				
	--IF (@StartDate IS NULL)
	--	SET @HasStartDate = 0;
	--ELSE
	--	SET @HasStartDate =1

	--DECLARE @HasEndDate  	BIT;
	--IF (@EndDate IS NULL)
	--	SET @HasEndDate = 0;
	--ELSE
	--	SET @HasEndDate =1

	 
	 IF (@RestrictToStyle_HN_RID IS NULL)
		SET @RestrictToStyle_HN_RID = -1

	DECLARE @METHOD_RIDS_TO_INCLUDE TABLE 
	(  
		METHOD_RID int
	)

	IF (@RestrictMethods = 1)
	BEGIN
	    INSERT INTO @METHOD_RIDS_TO_INCLUDE SELECT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@MethodRIDsToInclude, ',') 
	END

	DECLARE @USER_RIDS_TO_INCLUDE TABLE 
	(  
		USER_RID int
	)
	IF (@RestrictUsers = 1)
	BEGIN
	    INSERT INTO @USER_RIDS_TO_INCLUDE SELECT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@UserRIDsToInclude, ',') 
	END

	DECLARE @HEADER_RIDS_TO_INCLUDE TABLE 
	(  
		HDR_RID int
	)
	IF (@RestrictHeaders = 1)
	BEGIN
	    INSERT INTO @HEADER_RIDS_TO_INCLUDE SELECT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@HeaderRIDsToInclude, ',') 
	END

	DECLARE @HN_RIDS_TO_INCLUDE TABLE 
	(  
		HN_RID int
		PRIMARY KEY (HN_RID)
	)
	IF NOT (@RestrictToStyle_HN_RID IS NULL OR @RestrictToStyle_HN_RID = -1)
	BEGIN
		INSERT INTO @HN_RIDS_TO_INCLUDE SELECT HN_RID FROM [dbo].[UDF_HIERARCHY_GET_ALL_DESCENDANT_NODES] (@RestrictToStyle_HN_RID)
	END


	 SELECT * INTO #MyTable FROM
	 (
	 SELECT
		AH.PROCESS_DATE_TIME AS AUDIT_DATETIME,
		(SELECT AU.USER_NAME FROM [dbo].[APPLICATION_USER] AU WITH (NOLOCK) WHERE AU.USER_RID=AH.USER_RID) AS USER_ID,
		VH.HDR_ID AS HEADER_ID,
		(SELECT TEXT_VALUE FROM APPLICATION_TEXT AT WITH (NOLOCK) WHERE AT.TEXT_CODE=VH.DISPLAY_TYPE) AS HEADER_TYPE,
		(SELECT TEXT_VALUE FROM APPLICATION_TEXT AT WITH (NOLOCK) WHERE AT.TEXT_CODE=VH.DISPLAY_STATUS) AS HEADER_STATUS,

		(SELECT [dbo].[UDF_HIERARCHY_BUILD_BASE_NODE_DISPLAY_ID](phl.PHL_DISPLAY_OPTION_ID, bn.BN_ID, bn.BN_NAME, bn.BN_DESCRIPTION) AS NODE_DISPLAY_ID 
			FROM [dbo].[BASE_NODE] bn WITH (NOLOCK) 
			INNER JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = bn.HN_RID
			INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK) on phl.PHL_SEQUENCE=hn.HOME_LEVEL
			WHERE bn.HN_RID=VH.STYLE_HNRID) AS STYLE,
	
	
		(SELECT TEXT_VALUE FROM APPLICATION_TEXT AT WITH (NOLOCK) WHERE AT.TEXT_CODE=AH.ACTION_TYPE) AS ACTION,
		(SELECT M.METHOD_NAME FROM METHOD M WITH (NOLOCK) WHERE M.METHOD_RID=AH.METHOD_RID) AS METHOD,
		--CASE WHEN AH.METHOD_NAME IS NOT NULL AND AH.METHOD_NAME <> '' THEN AH.METHOD_NAME ELSE (SELECT METHOD_NAME FROM METHOD M (NOLOCK) WHERE M.METHOD_RID=AH.METHOD_RID) END AS METHOD,
		COALESCE(AH.UNITS_ALLOCATED_BY_PROCESS, 0) AS ALLOCATED_BY_PROCESS,
		COALESCE(AH.STORE_COUNT, 0) AS STORES_BY_PROCESS,
		COALESCE(VH.UNITS_RECEIVED, 0) AS AVAILABLE_UNITS,
		COALESCE(VH.ALLOCATED_UNITS, 0) AS TOTAL_ALLOCATED,
		COALESCE(VH.STORES_WITH_ALOCTN_COUNT, 0) AS TOTAL_STORES,
		COALESCE(VH.RSV_ALLOCATED_UNITS, 0) AS RESERVE_UNITS,

		(SELECT TOP 1 W.WORKFLOW_NAME FROM WORKFLOW W WITH (NOLOCK) INNER JOIN WORKFLOW_HISTORY WH (NOLOCK) ON W.WORKFLOW_RID= WH.WORKFLOW_RID WHERE WH.HDR_RID=AH.HDR_RID) AS WORKFLOW,
		(SELECT null) AS TASKLIST

	FROM [dbo].[AUDIT_HEADER] AH WITH (NOLOCK)
	INNER JOIN [dbo].[VW_GET_HEADERS] VH WITH (NOLOCK) ON VH.HDR_RID=AH.HDR_RID


	WHERE 
	(
		@RestrictToStyle_HN_RID = -1 
		OR VH.STYLE_HNRID IN 
		(
			SELECT HN_RID FROM @HN_RIDS_TO_INCLUDE
		)
	) 
	AND
	(   /*  Begin TT#1966-MD - JSmith - DC Fulfillment */
		VH.DISPLAY_TYPE IN 
		(
			@Receipt,		 
			@ASN,			 
			@Dummy,		 
			@DropShip,		 
			@MultiHeader,	 
			@Reserve,		 
			@WorkupTotalBuy,
			@PurchaseOrder, 
			@Assortment,	 
			@Placeholder,	 
			@IMO                
		)
		OR 
		(
		@Master = 800742 AND VH.HDR_RID IN (SELECT MASTER_HDR_RID FROM MASTER_HEADER WITH (NOLOCK))
		)
	)
	/*  End TT#1966-MD - JSmith - DC Fulfillment */
	AND VH.DISPLAY_STATUS IN 
	(
		@ReceivedOutOfBalance,   
		@ReceivedInBalance,      
		@InUseByMultiHeader,     
		@PartialSizeOutOfBalance,
		@PartialSizeInBalance,   
		@AllocatedOutOfBalance,  
		@AllocatedInBalance,     
		@SizesOutOfBalance,      
		@AllInBalance,           
		@Released,               
		@ReleaseApproved,        
		@AllocationStarted      
	)
	AND AH.ACTION_TYPE IN
	(
	    @ActionUnassigned,
		@ApplyAPI_Workflow,					
		@BackoutAllocation,					
		@BackoutDetailPackAllocation,
		@BackoutSizeAllocation,			
		@BackoutSizeIntransit,					
		@BackoutStyleIntransit,					
		@BalanceSizeBilaterally,					
		@BalanceSizeNoSubs,						
		@BalanceSizeWithConstraints,				
		@BalanceSizeWithSubs,					
		@BalanceStyleProportional,				
		@BalanceToDC,							
		@BreakoutSizesAsReceived,
		@BreakoutSizesAsReceivedWithConstraints,
		@ChargeIntransit,
		@ChargeSizeIntransit,
		@DeleteHeader,							
		@ReapplyTotalAllocation,
		@Release,								
		@RemoveAPI_Workflow,
		@Reset,					
		@StyleNeed,
		@BalanceToVSW	/* TT#1334-MD - stodd - Balance to VSW Action */						
	)
	AND
	(
		@UseDateRange=0 OR (AH.PROCESS_DATE_TIME >= @StartDate)
	)
	AND
	(
		@UseDateRange=0 OR (AH.PROCESS_DATE_TIME <= @EndDate)
	)
	AND
	(
		@RestrictMethods=0 OR (AH.METHOD_RID IN (SELECT METHOD_RID FROM @METHOD_RIDS_TO_INCLUDE))
	)
	AND
	(
		@RestrictUsers=0 OR (AH.USER_RID IN (SELECT USER_RID FROM @USER_RIDS_TO_INCLUDE))
	)
	AND
	(
		@RestrictHeaders=0 OR (AH.HDR_RID IN (SELECT HDR_RID FROM @HEADER_RIDS_TO_INCLUDE))
	)
	) AS T
	
	IF (@ResultLimit IS NULL OR @ResultLimit=-1)
		SELECT * FROM #MyTable ORDER BY AUDIT_DATETIME
	ELSE
		SELECT TOP (@ResultLimit) * FROM #MyTable ORDER BY AUDIT_DATETIME
	
END
GO
