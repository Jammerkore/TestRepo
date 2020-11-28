alter table "METHOD_SIZE_CURVE_MRCH_BAS_DET" 
	add constraint "MTHD_SZ_CRV_MRCH_BAS_HN_FK1" foreign key ("HN_RID")
	 references "HIERARCHY_NODE" ("HN_RID")  

go
