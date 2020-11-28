--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1312 - JScott - Alternate Hierarchy Reclass
--dv ============================================= 
CREATE TABLE [dbo].[HIERARCHY_RECLASS_TRANS](
	TRANS_PROCESS_ID int NULL,
	TRANS_SEQUENCE int NULL,
	TRANS_ORIGINAL varchar(5000) NULL,
	TRANS_HIERARCHY_ID varchar(50) NULL,
	TRANS_PARENT_ID varchar(100) NULL,
	TRANS_PRODUCT_ID varchar(50) NULL,
	TRANS_PRODUCT_NAME varchar(50) NULL,
	TRANS_PRODUCT_DESC varchar(250) NULL)

GO
