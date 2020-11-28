--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[USER_ASSORTMENT_STORE_GRADE] ( 
	"USER_RID" int not null,
	"STORE_GRADE_SEQ" int not null,
	"BOUNDARY_UNITS" int not null,	
	"BOUNDARY_INDEX" int not null,	
	"GRADE_CODE" varchar(20) not null)  

go
