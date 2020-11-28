--dv =============================================
--dv Modified date: 8/13/2014
--dv Notes:	
--dv History:  RMatelic - Add CREATE table SQL
--dv ============================================= 
create table [dbo].[METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY] ( 
	"METHOD_RID" int not null,
	"BOUNDARY" int not null,
	"GRADE_CODE" varchar(20) null)

go