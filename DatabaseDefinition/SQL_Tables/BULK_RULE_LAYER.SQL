--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1085 - AGallagher - Add Velocity "Reconcile Layers" option without Balance
--dv =============================================  
create table [dbo].[BULK_RULE_LAYER] ( 
	"HDR_RID" int not null,
	"LAYER_ID" int not null,
	"METHOD_RID" int not null, 
	"RECONCILE_IND" char(1) null)

go
