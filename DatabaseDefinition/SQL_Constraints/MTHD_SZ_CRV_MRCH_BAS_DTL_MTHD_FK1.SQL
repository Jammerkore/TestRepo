alter table "METHOD_SIZE_CURVE_MRCH_BAS_DET" 
	add constraint "MTHD_SZ_CRV_MRCH_BAS_DTL_MTHD_FK1" foreign key ("METHOD_RID")
	 references "METHOD_SIZE_CURVE" ("METHOD_RID")  

go
