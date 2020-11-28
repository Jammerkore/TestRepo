alter table "NODE_SIZE_CURVE_CRITERIA_DEFAULT" 
	add constraint "ND_SZ_CRV_CRITERIA_DEF_FK2" foreign key ("NSCCD_RID")
	 references "NODE_SIZE_CURVE_CRITERIA_DETAIL" ("NSCCD_RID")  

go
