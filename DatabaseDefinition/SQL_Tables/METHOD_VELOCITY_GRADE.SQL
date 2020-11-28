--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#505 -AGallagher -Velocity - Apply Min/Max (#58)
--dv =============================================  
create table [dbo].[METHOD_VELOCITY_GRADE] ( 
	"METHOD_RID" int not null,
	"BOUNDARY" int not null,
	"GRADE_CODE" varchar(20) null,
	"MINIMUM_STOCK" int null,
	"MAXIMUM_STOCK" int null,
	"MINIMUM_AD" int null)

go
