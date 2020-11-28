/* Add foreign key constraints to table "PRODUCT_HIERARCHY".                                  */
alter table "PRODUCT_HIERARCHY"
	add constraint "APP_TEXT_PROD_HIER_FK1" foreign key (
		"OTS_PLANLEVEL_TYPE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
