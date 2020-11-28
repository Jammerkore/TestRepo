alter table "MID_ENQUEUE"
	add constraint "PROD_HIER_MID_ENQUEUE_FK1" foreign key (
		"PH_RID")
	 references "PRODUCT_HIERARCHY" (
		"PH_RID")  

go
