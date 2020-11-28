--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 10/16/2014 TT#1313-MD -jsobek -Header Filters
--dv History: 01/08/2015 TT#1477-MD -jsobek -Header Filter Sort on Workspace
--dv =============================================  
create table [dbo].[GRID_VIEW] ( 
	VIEW_RID int identity not null,
	USER_RID int not null,
	LAYOUT_ID int not null,
	VIEW_ID varchar(50) not null,
	SHOW_DETAILS char(1) not null,  
	GROUP_BY int null,
	GROUP_BY_SECONDARY int null,
	IS_SEQUENTIAL char(1) null,
	WORKSPACE_FILTER_RID int null,
	USE_FILTER_SORTING bit null
	)  

go
