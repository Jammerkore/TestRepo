alter table "METHOD_SIZE_CURVE_MRCH_BAS_DET" 
	add constraint "MTHD_SZ_CRV_MRCH_BAS_COLL_FK1" foreign key ("CUSTOM_OLL_RID")
	 references "OVERRIDE_LL_MODEL_HEADER" ("OLL_RID")  	 

go
