alter table "METHOD_SIZE_CURVE"
	add constraint "MTHD_SZ_CRV_MTHD_FK1" foreign key ("METHOD_RID")
	 references "METHOD" ("METHOD_RID")  

go
