/* Add foreign key constraints to table "METHOD_GLOBAL_UNLOCK".                               */
alter table "METHOD_GLOBAL_UNLOCK"
	add constraint "MTHD_MTHD_GLBL_UNLK_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
