--dv =============================================
--dv Modified date: 8/12/2014
--dv History:		TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
--dv =============================================
ALTER TABLE "METHOD_GLOBAL_LOCK"
	ADD CONSTRAINT "STR_FLTR_MTHD_GLBL_LK_FK1" FOREIGN KEY (
		"STORE_FILTER_RID")
	 REFERENCES "FILTER" (
		"FILTER_RID") ON UPDATE NO ACTION ON DELETE NO ACTION 
GO
