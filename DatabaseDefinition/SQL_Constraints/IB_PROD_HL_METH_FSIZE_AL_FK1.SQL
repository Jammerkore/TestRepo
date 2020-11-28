alter table "METHOD_FILL_SIZE_HOLES"
	add constraint "IB_PROD_HL_METH_FSIZE_AL_FK1" foreign key (
		"IB_MERCH_PH_RID",
		"IB_MERCH_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  
go	
