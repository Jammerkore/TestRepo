alter table "PRODUCT_HIERARCHY_LEVELS"
	add constraint "APP_TEXT_OTS_LVL_TYPE_FK" foreign key (
		"PHL_TYPE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
