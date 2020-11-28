--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#637 - AGallagher - Velocity - Spread Average (#7)
--dv =============================================   
create table [dbo].[METHOD_VELOCITY_GROUP_LEVEL] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"NO_ONHAND_RULE" int null,
	"NO_ONHAND_QUANTITY" float null,
	"MATRIX_MODE_IND" char(1) null,
	"MATRIX_MODE_AVERAGE_RULE" int null, 
	"MATRIX_MODE_AVERAGE_QUANITY" float null,
	"MATRIX_SPREAD_IND" char(1) null) 	 

go
