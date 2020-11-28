/* Begin TT#228 - RBeck - Hierarchy Reclass message incorrectly displayed */
-- Add foreign key constraints to table HIER_RECLASS_INFO.
alter table [dbo].HIER_RECLASS_INFO
	add constraint PROC_HDR_HIER_RECLASS_INFO_FK1 foreign key (PROCESS_RID)
	references [dbo].PROC_HDR (PROCESS_RID);	
GO
