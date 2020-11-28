alter table "METHOD_GENERAL_ALLOCATION"
	add constraint "PROD_HL_METH_GEN_AL_FK1" foreign key (
		"MERCH_PH_RID",
		"MERCH_PHL_SEQUENCE")
	 references "PRODUCT_HIERARCHY_LEVELS" (
		"PH_RID",
		"PHL_SEQUENCE")  

go
