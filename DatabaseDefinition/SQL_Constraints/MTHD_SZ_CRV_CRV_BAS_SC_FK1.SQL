alter table "METHOD_SIZE_CURVE_CRVE_BAS_DET"
	add constraint "MTHD_SZ_CRV_CRV_BAS_SC_FK1" foreign key ("SIZE_CURVE_GROUP_RID")
	 references "SIZE_CURVE_GROUP" ("SIZE_CURVE_GROUP_RID")  

go
