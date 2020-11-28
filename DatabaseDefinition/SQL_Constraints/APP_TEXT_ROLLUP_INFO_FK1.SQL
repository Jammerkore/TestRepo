alter table "ROLLUP_PROCESS"
	add constraint "APP_TEXT_ROLLUP_INFO_FK1" foreign key (
		"STATUS_CODE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
