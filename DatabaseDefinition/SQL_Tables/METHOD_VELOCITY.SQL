--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#91 -stodd
--dv History: TT#313 -JSmith -balance does not remain checked
--dv History: TT#505 -AGallagher -Velocity - Apply Min/Max (#58)
--dv History: TT#1085 -AGallagher -Add Velocity "Reconcile Layers" option without Balance
--dv History: TT#1287 -AGallagher -Inventory Min/Max
--dv History: TT#855-MD -jsobek -Velocity Enhancements
--dv =============================================   
create table [dbo].[METHOD_VELOCITY] ( 
	"METHOD_RID" int not null,
	"SG_RID" int null,
	"OTS_PLAN_HN_RID" int null,
	"OTS_PLAN_PH_RID" int null,
	"OTS_PLAN_PHL_SEQUENCE" int null,
	"SIM_STORE_IND" char(1) null,
	"AVG_USING_CHAIN_IND" char(1) null,
	"SHIP_USING_BASIS_IND" char(1) null,
	"TREND_PERCENT" char(1) null,
	"OTS_BEGIN_CDR_RID" int null,
	"OTS_SHIP_TO_CDR_RID" int null,
	"BALANCE_IND" char(1) null,
	"APPLY_MIN_MAX_IND" char(1) null, 
	"RECONCILE_IND" char(1) null,
	"INVENTORY_IND" char(1) null,
	"MERCH_TYPE" int default 3 not null,
	"MERCH_HN_RID" int null,
	"MERCH_PH_RID" int null,
	"MERCH_PHL_SEQUENCE" int null,
	"GRADE_VARIABLE_TYPE" int null, 
	"BALANCE_TO_HEADER_IND" char(1) null 
	)
	
go
