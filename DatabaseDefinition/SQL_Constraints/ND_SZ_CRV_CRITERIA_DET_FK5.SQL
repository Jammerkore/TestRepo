alter table "NODE_SIZE_CURVE_CRITERIA_DETAIL"
	add constraint "ND_SZ_CRV_CRITERIA_DET_FK5" foreign key ("SIZE_GROUP_RID")
	 references "SIZE_GROUP" ("SIZE_GROUP_RID")  

go
