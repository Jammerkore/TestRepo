alter table "NODE_SIZE_CURVE_SIMILAR_STORE" 
	add constraint "ND_SZ_CRV_SIM_STORE_FK2" foreign key ("ST_RID")
	 references "STORES" ("ST_RID")   

go
