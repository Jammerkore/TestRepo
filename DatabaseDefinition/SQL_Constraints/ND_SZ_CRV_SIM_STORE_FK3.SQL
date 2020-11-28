alter table "NODE_SIZE_CURVE_SIMILAR_STORE" 
	add constraint "ND_SZ_CRV_SIM_STORE_FK3" foreign key ("SS_RID")
	 references "STORES" ("ST_RID")   

go
