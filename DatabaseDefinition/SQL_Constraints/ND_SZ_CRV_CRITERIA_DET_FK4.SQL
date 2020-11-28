alter table "NODE_SIZE_CURVE_CRITERIA_DETAIL" 
	add constraint "ND_SZ_CRV_CRITERIA_DET_FK4" foreign key ("CUSTOM_OLL_RID")
	 references "OVERRIDE_LL_MODEL_HEADER" ("OLL_RID")  	 

go
