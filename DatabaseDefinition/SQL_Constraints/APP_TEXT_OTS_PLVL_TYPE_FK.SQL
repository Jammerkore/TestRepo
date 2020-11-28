alter table "PRODUCT_HIERARCHY_LEVELS"
	add constraint "APP_TEXT_OTS_PLVL_TYPE_FK" foreign key (
		"OTS_PLANLEVEL_TYPE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
