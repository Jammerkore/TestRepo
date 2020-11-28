--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].HIER_RECLASS_INFO (	 
	[PROCESS_RID]           int  not null,
	[HEIR_TRANS_WRITTEN]    int null,
	[ADDCHG_TRANS_WRITTEN]  int null,
	[DELETE_TRANS_WRITTEN]  int null ,
	[MOVE_TRANS_WRITTEN]    int null,
	[TRANS_REJECTED]        int null,
constraint HIER_RECLASS_INFO_PK primary key clustered (PROCESS_RID) 
); 
GO
