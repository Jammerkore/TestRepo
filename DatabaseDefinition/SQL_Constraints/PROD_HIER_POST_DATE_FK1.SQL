/* Add foreign key constraints to table "POSTING_DATE".                                       */
alter table "POSTING_DATE"
	add constraint "PROD_HIER_POST_DATE_FK1" foreign key (
		"PH_RID")
	 references "PRODUCT_HIERARCHY" (
		"PH_RID")  

go
