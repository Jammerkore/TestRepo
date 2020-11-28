alter table "TASK_SIZE_CURVE_GENERATE"
	add constraint "METHOD_TASK_SZ_CRV_GEN_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD" (
		"METHOD_RID")  

go
