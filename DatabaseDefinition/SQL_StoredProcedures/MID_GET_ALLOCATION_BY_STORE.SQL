-- =============================================
-- Description:	Gets total allocation units by store, header status, and header type
-- =============================================
CREATE PROCEDURE [dbo].[MID_GET_ALLOCATION_BY_STORE] 
@ST_RID int,
--Header Types
@Include_Receipt		 BIT,
@Include_ASN			 BIT,
@Include_Dummy			 BIT,
@Include_DropShip		 BIT,
@Include_MultiHeader	 BIT,
@Include_Reserve		 BIT,
@Include_WorkupTotalBuy  BIT,
@Include_PurchaseOrder   BIT,
@Include_Assortment		 BIT,
@Include_Placeholder	 BIT,
@Include_IMO             BIT,
@Include_Master          BIT = 1,    /*  TT#1966-MD - JSmith - DC Fulfillment */
--Header Statuses
@Include_ReceivedOutOfBalance   	BIT,
@Include_ReceivedInBalance      	BIT,
@Include_InUseByMultiHeader     	BIT,
@Include_PartialSizeOutOfBalance	BIT,
@Include_PartialSizeInBalance   	BIT,
@Include_AllocatedOutOfBalance  	BIT,
@Include_AllocatedInBalance     	BIT,
@Include_SizesOutOfBalance      	BIT,
@Include_AllInBalance           	BIT,
@Include_Released               	BIT,
@Include_ReleaseApproved        	BIT,
@Include_AllocationStarted      	BIT
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
	SELECT ta.HDR_RID, 
		   HDR_ID, 
           COALESCE(ta.UNITS_ALLOCATED,0) as UNITS_ALLOCATED,
           COALESCE(ta.UNITS_SHIPPED,0) as UNITS_SHIPPED,
           COALESCE(ta.ITEM_UNITS_ALLOCATED,UNITS_ALLOCATED) AS ITEM_UNITS_ALLOCATED,
		   DISPLAY_STATUS,
		   DISPLAY_TYPE,
		   DISPLAY_INTRANSIT,
		   DISPLAY_SHIP_STATUS,
		   [dbo].[UDF_HIERARCHY_BUILD_BASE_NODE_DISPLAY_ID]( (SELECT PHL_DISPLAY_OPTION_ID FROM PRODUCT_HIERARCHY_LEVELS phl WITH (NOLOCK) WHERE phl.PHL_SEQUENCE=hn.HOME_LEVEL), bn.BN_ID, bn.BN_NAME, bn.BN_DESCRIPTION ) AS STYLE_ID,
		   [dbo].[UDF_HIERARCHY_BUILD_BASE_NODE_DISPLAY_ID]( 
			   (SELECT PHL_DISPLAY_OPTION_ID FROM PRODUCT_HIERARCHY_LEVELS phl WITH (NOLOCK) INNER JOIN [dbo].[HIERARCHY_NODE] hn2 WITH (NOLOCK) ON phl.PHL_SEQUENCE=hn2.HOME_LEVEL WHERE hn2.HN_RID=hnj.PARENT_HN_RID ), 
			   (SELECT BN_ID FROM [dbo].[BASE_NODE] bn2 WITH (NOLOCK) WHERE bn2.HN_RID=hnj.PARENT_HN_RID), 
			   (SELECT BN_NAME FROM [dbo].[BASE_NODE] bn2 WITH (NOLOCK) WHERE bn2.HN_RID=hnj.PARENT_HN_RID),
			   (SELECT BN_DESCRIPTION FROM [dbo].[BASE_NODE] bn2 WITH (NOLOCK) WHERE bn2.HN_RID=hnj.PARENT_HN_RID)
		   ) AS PARENT_ID
      FROM TOTAL_ALLOCATION ta WITH (NOLOCK)
	  INNER JOIN [dbo].[VW_GET_HEADERS] hd WITH (NOLOCK) on hd.HDR_RID=ta.HDR_RID
	  INNER JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) on hn.HN_RID=hd.STYLE_HNRID
	  INNER JOIN [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK) on hnj.HN_RID=hn.HN_RID AND hn.HOME_PH_RID=hnj.PH_RID
	  INNER JOIN [dbo].[BASE_NODE] bn WITH (NOLOCK) on bn.HN_RID=hd.STYLE_HNRID
	  WHERE ta.ST_RID=@ST_RID AND hd.HDR_RID = ta.HDR_RID AND COALESCE(ta.UNITS_ALLOCATED,0) > 0
	   /*  Begin TT#1966-MD - JSmith - DC Fulfillment */
	  --AND DISPLAY_TYPE in 
	  AND
	  (
		  DISPLAY_TYPE in 
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
		 @Master = 800742 AND hd.HDR_RID IN (SELECT MASTER_HDR_RID FROM MASTER_HEADER WITH (NOLOCK)) 
		 )
	  )
	  /*  End TT#1966-MD - JSmith - DC Fulfillment */
	  AND DISPLAY_STATUS in 
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
	  ORDER BY DISPLAY_TYPE, EARLIEST_SHIP_DAY, RELEASE_DATETIME, HDR_ID
END
GO