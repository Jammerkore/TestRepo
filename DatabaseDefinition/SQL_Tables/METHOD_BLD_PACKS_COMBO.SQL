--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	This table is the parent table of METHOD_BLD_PACKS_COMBO_PATTERN.  It defines the name of a pack combination and any other general information about a combination
--dv Associated Tables:  METHOD, METHOD_BLD_PACKS, METHOD_BLD_PACKS_COMBO_PATTERN, METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN
--dv Fields: COMBO_NAME:  This is the name of a pack combination.
--dv Fields: PATTERN_SELECTED_IND:  "Y" indicates this pack combination has be selected as an option to use in generating the Work Up Buy
--dv History: 
--dv ============================================= 
create table [dbo].[METHOD_BLD_PACKS_COMBO] ( 
	"METHOD_RID" int not null,
	"COMBO_RID" int identity(100,1) not null,	
    "COMBO_NAME" varchar(50) not null,
    "COMBO_SELECTED_IND" char(1) null)
go
