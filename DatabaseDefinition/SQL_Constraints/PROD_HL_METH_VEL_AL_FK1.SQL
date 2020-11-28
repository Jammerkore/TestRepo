alter table "METHOD_VELOCITY"
	add constraint "PROD_HL_METH_VEL_AL_FK1" foreign key (
		"MERCH_PH_RID",
		"MERCH_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  
go		
